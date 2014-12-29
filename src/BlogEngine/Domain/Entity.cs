namespace BlogEngine.Domain
{
    using System;

    public abstract class Entity
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
 
        public bool IsPersistent {get { return Id != default(int); }}
    }
}