using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AccountManagement.Models
{
    public class Train
    {
        [Key]
        public int Id { get; set; }
        public int Line { get; set; }
        public int AmountOfPassagers { get; set; }
        [AllowNull]
        public int? ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        public TypeOfTrain TypeOfTrain { get; set; }
        [ForeignKey("TypeOfTrainId")]
        public int TypeOfTrainId { get; set; }
    }
}
