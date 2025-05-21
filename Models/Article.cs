using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime TimeItWasAdded { get; set; }
        public string Content { get; set; }
    }
}
