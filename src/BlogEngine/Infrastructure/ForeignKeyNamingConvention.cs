namespace BlogEngine.Infrastructure
{
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public class ForeignKeyNamingConvention : IStoreModelConvention<AssociationType>
    {

        public void Apply(AssociationType association, DbModel model)
        {
            // Identify ForeignKey properties (including IAs)  
            if (association.IsForeignKey)
            {
                // rename FK columns
                var constraint = association.Constraint;

                NormalizeForeignKeyProperties(constraint.FromProperties);
                NormalizeForeignKeyProperties(constraint.ToProperties);
            }
        }

        private void NormalizeForeignKeyProperties(ReadOnlyMetadataCollection<EdmProperty> properties)
        {
            for (int i = 0; i < properties.Count; ++i)
            {
                int underscoreIndex = properties[i].Name.IndexOf('_');
                if (underscoreIndex > 0)
                {
                    properties[i].Name = properties[i].Name.Remove(underscoreIndex, 1);
                }
            }
        }
    }
}