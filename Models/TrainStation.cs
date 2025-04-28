using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models
{
    public class TrainStation
    {
        [Key]
        public int Id { get; set; }
        public string Location { get; set; }
        public IEnumerable<Schedule> Schedules { get; set; }
    }
}
