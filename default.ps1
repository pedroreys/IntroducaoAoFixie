$script:project_config = "Release"

properties {

  Framework '4.5.1'

  $solution_name = "IntroducaoAoFixie"

  if(-not $version)
  {
      $version = "0.0.0.1"
  }

  $date = Get-Date  
  


  $ReleaseNumber =  $version
  
  Write-Host "**********************************************************************"
  Write-Host "Release Number: $ReleaseNumber"
  Write-Host "**********************************************************************"
  

  $base_dir = resolve-path .
  $build_dir = "$base_dir\build"     
  $source_dir = "$base_dir\src"
  $test_dir = "$build_dir\test"
  $result_dir = "$build_dir\results"
  
  $test_assembly_patterns_unit = @("*Tests.dll")

  $nuget_exe = "$source_dir\.nuget\nuget.exe"

  $roundhouse_dir = "$base_dir\tools\roundhouse"
  $roundhouse_output_dir = "$roundhouse_dir\output"
  $roundhouse_exe_path = "$roundhouse_dir\rh.exe"
  
  $db_server = if ($env:db_server) { $env:db_server } else { "localhost" }
  $db_name = if ($env:db_name) { $env:db_name } else { "FixieBlogEngine" }
  $test_db_name = if ($env:test_db_name) { $env:test_db_name } else { "$db_name.Tests" }

  $dev_connection_string_name = "$solution_name.ConnectionString"
  $test_connection_string_name = "$solution_name.Tests.ConnectionString"

  $devConnectionString = if(test-path env:$dev_connection_string_name) { (get-item env:$dev_connection_string_name).Value } else { "Server=$db_server;Database=$db_name;Trusted_Connection=True;" }
  $testConnectionString = if(test-path env:$test_connection_string_name) { (get-item env:$test_connection_string_name).Value } else { "Server=$db_server;Database=$test_db_name;Trusted_Connection=True;" }
  
  $db_scripts_dir = "$source_dir\DatabaseMigration"  
  
}
   
#These are aliases for other build tasks. They typically are named after the camelcase letters (rd = Rebuild Databases)
#aliases should be all lowercase, conventionally
#please list all aliases in the help task
task default -depends Clean, Compile, RebuildAllDatabase, RunAllTests

task SetDebugBuild {
    $script:project_config = "Debug"
}

task SetReleaseBuild {
    $script:project_config = "Release"
}

task RebuildAllDatabase -depends RebuildDevDatabase, RebuildTestDatabase

task RebuildDevDatabase{
  deploy-database "Rebuild" $devConnectionString $db_scripts_dir "DEV"  
}

task RebuildTestDatabase {
      deploy-database "Rebuild" $testConnectionString $db_scripts_dir "TEST"
}

task UpdateDatabase {
    deploy-database "Update" $devConnectionString $db_scripts_dir "DEV"
}

task CopyAssembliesForTest -Depends Compile {
    copy_all_assemblies_for_test $test_dir
}

task RunAllTests -Depends CopyAssembliesForTest {
    $test_assembly_patterns_unit | %{ run_fixie_tests $_ }
}

task Compile -depends Clean { 
    exec { & $nuget_exe restore $source_dir\$solution_name.sln }
    exec { msbuild.exe /t:build /v:q /p:Configuration=$project_config /p:Platform="Any CPU" /nologo $source_dir\$solution_name.sln }
}


task Clean {    
    delete_directory $build_dir
    create_directory $test_dir 
    create_directory $result_dir
   
    exec { msbuild /t:clean /v:q /p:Configuration=$project_config /p:Platform="Any CPU" $source_dir\$solution_name.sln }
}


# -------------------------------------------------------------------------------------------------------------
# generalized functions 
# --------------------------------------------------------------------------------------------------------------
function deploy-database($action, $connectionString, $scripts_dir, $env) {
        write-host "action: $action"
    write-host "connectionString: $connectionString"    
    write-host "scripts_dir: $scripts_dir"
    write-host "env: $env"

    if (!$env) {
        $env = "LOCAL"
        Write-Host "RoundhousE environment variable is not specified... defaulting to 'LOCAL'"
    } else {
        Write-Host "Executing RoundhousE for environment:" $env
    }  
   
    # Run roundhouse commands on $scripts_dir
    if ($action -eq "Update"){
       exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 -f $scripts_dir --env $env --silent -o $roundhouse_output_dir --transaction }
       exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 -f $scripts_dir --env "NOTRANSACTION" --silent -o $roundhouse_output_dir }
    }
    if ($action -eq "Rebuild"){
       exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 --env $env --silent -drop -o $roundhouse_output_dir }
       exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 -f $scripts_dir -env $env --silent --simple -o $roundhouse_output_dir --transaction }
       exec { &$roundhouse_exe_path -cs "$connectionString" --commandtimeout=300 -f $scripts_dir -env "NOTRANSACTION" --silent --simple -o $roundhouse_output_dir }
    }
}

function run_fixie_tests([string]$pattern) {
    $items = Get-ChildItem -Path $test_dir $pattern    
    $items | %{ run_fixie $_.Name }
}


function global:delete_file($file) {
    if($file) { remove-item $file -force -ErrorAction SilentlyContinue | out-null } 
}

function global:delete_directory($directory_name) {
  rd $directory_name -recurse -force  -ErrorAction SilentlyContinue | out-null
}

function global:create_directory($directory_name) {
  mkdir $directory_name  -ErrorAction SilentlyContinue  | out-null
}

function global:run_fixie ($test_assembly) {
   $assembly_to_test = $test_dir + "\" + $test_assembly
   $results_output = $result_dir + "\" + $test_assembly + ".xml"
    write-host "Running Fixie Tests in: $test_assembly"
    exec { & tools\fixie\Fixie.Console.exe $assembly_to_test --NUnitXml $results_output --TeamCity off }
}

function global:Copy_and_flatten ($source,$include,$dest) {
   Get-ChildItem $source -include $include -r | cp -dest $dest
}

function global:copy_all_assemblies_for_test($destination){
   $bin_dir_match_pattern = "$source_dir\**\bin\$project_config"
   create_directory $destination
   Copy_and_flatten $bin_dir_match_pattern @("*.exe","*.dll","*.config","*.pdb","*.sql","*.xlsx","*.csv") $destination   
}