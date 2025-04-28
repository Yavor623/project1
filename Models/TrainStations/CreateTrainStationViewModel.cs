using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models.TrainStations
{
    public class CreateTrainStationViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Location { get; set; }
    }
}
