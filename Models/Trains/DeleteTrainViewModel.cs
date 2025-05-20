using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models.Trains
{
    public class DeleteTrainViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string SerialNumber { get; set; }
        [Required]
        public int Line { get; set; }
        [Required]
        public int AmountOfPassagers { get; set; }
        [Required]
        public int TypeOfTrainId { get; set; }
    }
}
