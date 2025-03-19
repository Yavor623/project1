using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccountManagement.Models;
using Microsoft.AspNetCore.Authorization;

namespace AccountManagement.Controllers
{
    
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public IActionResult Index()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Create() 
        {
            return View(new CreateRoleViewModel());
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var roleExist =  await _roleManager.RoleExistsAsync(model.RoleName);
                if (!roleExist&&await _userManager.IsInRoleAsync(user,"Admin"))
                {
                     await _roleManager.CreateAsync(new IdentityRole(model.RoleName));
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Ролята вече съществува");
            }
            return View(model);
        }

    }
}
