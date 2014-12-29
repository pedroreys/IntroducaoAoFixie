namespace BlogEngine.Domain
{
    public class Comment : Entity
    {
        public virtual Author Author { get; set; }
        public string CommentContent { get; set; }
    }
}