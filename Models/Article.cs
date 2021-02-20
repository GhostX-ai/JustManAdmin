namespace JustManAdmin.Models
{
    public class Article
    {
        public int Id { get; set; }
        public MainCategory MainCategory { get; set; }
        public string ImgPath { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}