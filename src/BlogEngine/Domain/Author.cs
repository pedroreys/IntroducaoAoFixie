namespace BlogEngine.Domain
{
    using System.Collections.Generic;

    public class Author : Entity
    {
        public string Name { get; set; }

        public virtual List<BlogPost> Posts { get; set; } 
    }
}