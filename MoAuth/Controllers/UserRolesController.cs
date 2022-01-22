using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TakweenTemplate.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserRolesController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserRolesController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index(string userId)
        {

            var userRolesVM = new List<UserRolesViewModel>();
            var user = await _userManager.FindByIdAsync(userId);

            foreach (var role in _roleManager.Roles)
            {
                var userRoleVM = new UserRolesViewModel()
                {
                    RoleName = role.Name,
                };

                if (await _userManager.IsInRoleAsync(user, userRoleVM.RoleName))
                {
                    userRoleVM.Selected = true;
                }
                else
                {
                    userRoleVM.Selected = false;
                }

                userRolesVM.Add(userRoleVM);
            }

            var model = new ManageUserRoleViewModel()
            {
                UserId = userId,
                UserRoles = userRolesVM
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string userId, ManageUserRoleViewModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.RemoveFromRolesAsync(user, roles);
            result = await _userManager.AddToRolesAsync(user, model.UserRoles.Where(a => a.Selected == true).Select(x => x.RoleName));

            var currentUser = await _userManager.GetUserAsync(User);

            await _signInManager.RefreshSignInAsync(currentUser);
            await DefaultUsers.SeedAdminUsersAsync(_userManager, _roleManager);

            return RedirectToAction(nameof(Index), new { userId = userId });


        }
    }
}
