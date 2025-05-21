using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public string FromWhere { get; set; }
        public string ToWhere { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartsAtStation { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ArrivesAtDestination { get; set; }
        public int TrainId { get; set; }
        [ForeignKey("TrainId")]
        public Train Train { get; set; }

    }
}
