using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models.Rating
{
    public class DeleteRatingViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:N1}")]
        public decimal RatingScore { get; set; }
        [Required]
        public string Comment { get; set; }
        [Required]
        public int TrainId { get; set; }
    }
}
