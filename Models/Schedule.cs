using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public string FromWhere { get; set; }
        public string ToWhere { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime StartsAtStation { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime ArrivesAtDestination { get; set; }
        [ForeignKey("TrainId")]
        public int TrainId { get; set; }
        public Train Train { get; set; }
        [ForeignKey("TrainStationId")]
        public int TrainStationId { get; set; }
        public TrainStation TrainStation { get; set; }
    }
}
