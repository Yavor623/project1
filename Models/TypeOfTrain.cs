using System.ComponentModel.DataAnnotations;

namespace AccountManagement.Models
{
    public class TypeOfTrain
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Train> Trains { get; set; }
    }
}
