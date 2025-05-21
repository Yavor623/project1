using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AccountManagement.Models
{
    public class Train
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public int Line { get; set; }
        public int AmountOfPassagers { get; set; }
        public bool IsItBusy { get; set; } = false;
        public bool IsItCurrentlyUsed { get; set; } = false;
        [AllowNull]
        public IEnumerable<Schedule>? Schedules { get; set; }
        public TypeOfTrain TypeOfTrain { get; set; }
        [ForeignKey("TypeOfTrainId")]
        public int TypeOfTrainId { get; set; }
        [AllowNull]
        public string? RatingScore { get; set; } = "Unrated";
        [AllowNull]
        public IEnumerable<Assesment>? Ratings { get; set; }
    }
}
