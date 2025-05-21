using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models.Articles
{
    public class DeleteArticleViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public DateTime TimeItWasAdded { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
