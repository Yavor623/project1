using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata.Ecma335;
using AccountManagement.Models.Orders;

namespace AccountManagement.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection <Order> Orders { get; set; }
    }
}
