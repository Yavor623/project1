using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Models
{
    public class Assesment
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal RatingScore { get; set; }
        public string Comment { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime TimeItWasAdded { get; set; }
        [NotMapped]
        public bool IsItEdited { get; set; } = false;
        [ForeignKey("TrainId")]
        public Train Train { get; set; }
        public int TrainId { get; set; }
        public ApplicationUser User { get; set; }
        [ForeignKey("UserId")]
        public string UserId { get; set; }
    }
}
