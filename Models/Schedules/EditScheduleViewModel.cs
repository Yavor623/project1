using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models.Schedules
{
    public class EditScheduleViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string FromWhere { get; set; }
        [Required]
        public string ToWhere { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartsAtStation { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime ArrivesAtDestination { get; set; }
        [Required]
        public int TrainId { get; set; }
    }
}
