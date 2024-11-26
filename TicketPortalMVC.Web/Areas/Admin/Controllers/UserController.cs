using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketPortalMVC.Application.Services.Interface;
using TicketPortalMVC.Application.ViewModels;
using TicketPortalMVC.Domain.Entities;

namespace TicketPortalMVC.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class UserController : Controller
    {

        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IAccountService accountService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _accountService = accountService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            // Načteme všechny uživatele
            var users = await _accountService.GetUsersAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                // Načteme role pro uživatele
                var roles = await _userManager.GetRolesAsync(user);

                userViewModels.Add(new UserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    City = user.City,
                    Country = user.Country,
                    PostalCode = user.PostalCode,
                    PhoneNumber = user.PhoneNumber,
                    Role = roles.FirstOrDefault()

                });
            }

            return View(userViewModels);



        }

        public async Task<IActionResult> UserDetail(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                PostalCode = user.PostalCode,
                PhoneNumber = user.PhoneNumber,
                Role = roles.FirstOrDefault()
            };

            return View(model);
        }

        public async Task<IActionResult> UserEdit(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user == null)
            {
                return NotFound();
            }



            var roles = await _roleManager.Roles.Select(role => role.Name).ToListAsync();
            var userRole = await _userManager.GetRolesAsync(user);

            var model = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                City = user.City,
                Country = user.Country,
                PostalCode = user.PostalCode,
                PhoneNumber = user.PhoneNumber,
                Role = userRole.FirstOrDefault(),
                AllRoles = roles
            };
            
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> UserEdit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null)
                {
                    return NotFound();
                }

                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.City = model.City;
                user.Country = model.Country;
                user.PostalCode = model.PostalCode;
                user.PhoneNumber = model.PhoneNumber;
                

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    var currentRoles = await _userManager.GetRolesAsync(user);
                    var rolesToAdd = model.Role == null ? new List<string>() : new List<string> { model.Role };
                    var rolesToRemove = currentRoles.Except(rolesToAdd).ToList();

                    foreach (var role in rolesToAdd)
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }

                    foreach (var role in rolesToRemove)
                    {
                        await _userManager.RemoveFromRoleAsync(user, role);
                    }

                    return RedirectToAction("UserList");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            var roles = _roleManager.Roles.Select(role => role.Name).ToList();
            model.AllRoles = roles;
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> UserDelete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("UserList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Uživatel nebyl nalezen.");
            }

            return View(user);
        }
        
        
        public async Task<IActionResult> UserCreate()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            var model = new UserCreateViewModel
            {
                Roles = roles
            };
            
            
            
            return View(model);
            
        }
        [HttpPost]
        public async Task<IActionResult> UserCreate(UserCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {

                    Email = model.Email,
                    UserName = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    City = model.City,
                    Country = model.Country,
                    PostalCode = model.PostalCode,
                    PhoneNumber = model.PhoneNumber
                    

                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("UserList");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            
            model.Roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(model);
        }

        
        
        public IActionResult RoleCreate()
        {
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            return View(roles);
        }
        
        [HttpPost]
        public async Task<IActionResult> RoleCreate(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                ModelState.AddModelError(string.Empty, "Název role je povinný.");
                return View();
            }

            var role = new IdentityRole(roleName);
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("RoleCreate");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> RoleDelete(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RoleCreate");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Role nebyla nalezena.");
            }
    
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> RoleEdit(string roleName, string newRole)
        {
            if (string.IsNullOrWhiteSpace(newRole))
            {
                ModelState.AddModelError("", "Název role není platný.");
                return RedirectToAction("RoleCreate");
            }

            var roleToEdit = await _roleManager.FindByNameAsync(roleName);
            if (roleToEdit == null)
            {
                ModelState.AddModelError("", "Role nebyla nalezena.");
                return RedirectToAction("RoleCreate");
            }

            // Debug log
            Console.WriteLine($"Role nalezena: {roleToEdit.Name}");

            roleToEdit.Name = newRole; // Zde by mělo být nové jméno, pokud chcete změnit název
            var result = await _roleManager.UpdateAsync(roleToEdit);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("RoleCreate");
            }

            return RedirectToAction("RoleCreate");
        }





    }
}
