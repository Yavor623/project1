using System.ComponentModel.DataAnnotations.Schema;

namespace AccountManagement.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderName { get; set; }
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
