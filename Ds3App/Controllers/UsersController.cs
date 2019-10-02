using Ds3App.EmailerService;
using Ds3App.Models;
using Ds3App.Repository.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Ds3App.Controllers
{
    public class UsersController : AdminController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly EmailSender sender = new EmailSender();

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public async Task<ActionResult> Edit(string id)
        {
            var user = new RegisterViewModelEdit();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var update = context.Users.Find(id);
                var roleId = update.Roles.FirstOrDefault().RoleId;
                user.UserId = id;
                user.FirstName = update.FirstName;
                user.LastName = update.LastName;
                user.Contact = update.Contact;
                user.Email = update.Email;
                user.Role = roleManager.Roles.Where(r => r.Id == roleId).FirstOrDefault().Name;
            }
            ViewBag.Roles = new SelectList(await Roles(), "Id", "Name", user.Role);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RegisterViewModelEdit register)
        {
            if (ModelState.IsValid)
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    try
                    {
                        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        var updateUser = userManager.FindById(register.UserId);
                        updateUser.FirstName = register.FirstName;
                        updateUser.LastName = register.LastName;
                        updateUser.Contact = register.Contact;
                        updateUser.Email = register.Email;
                        updateUser.UserName = register.Email;

                        await userManager.RemoveFromRoleAsync(updateUser.Id, updateUser.Roles.FirstOrDefault()?.RoleId);
                        await userManager.AddToRoleAsync(updateUser.Id, register.Role);
                        await userManager.UpdateAsync(updateUser);
                        return RedirectToAction("User");
                    }
                    catch
                    {
                        return RedirectToAction("BadRequest", "Home");
                    }
                }
            }
            ViewBag.Roles = new SelectList(await Roles(), "Id", "Name", register.Role);
            return View(register);
        }
        public new async Task<ActionResult> User()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                List<ApplicationUser> applicationUsers = new List<ApplicationUser>();
                foreach (var item in await context.Users.Where(u => !u.IsDeleted && u.Email != "admin@app.com").ToListAsync())
                {
                    applicationUsers.Add(new ApplicationUser()
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Contact = item.Contact,
                        Email = item.Email
                        
                    });
                }
                return View(applicationUsers);
            }

        }
        public async Task<ActionResult> Register()
        {
            ViewBag.Roles = new SelectList(await Roles(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Contact = model.Contact,
                    IsDeleted = false
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        userManager.AddToRole(user.Id, model.Role);
                    }

                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    try
                    {
                        await sender.ConfirmAccount(user.Email, Constant.MagicStringReplacer.DefaultPassword, callbackUrl);
                    }
                    catch
                    {

                    }
             
                    return RedirectToAction("User");
                }
                AddErrors(result);
            }
            ViewBag.Roles = new SelectList(await Roles(), "Id", "Name", model.Role);
            return View(model);
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private async Task<IEnumerable<SystemRoles>> Roles()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                List<SystemRoles> roles = new List<SystemRoles>();
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                await Task.Run(() =>
                {
                    foreach (var roleManagerRole in roleManager.Roles.Where(r => r.Name != "Administrator"))
                    {
                        roles.Add(new SystemRoles()
                        {
                            Id = roleManagerRole.Name,
                            Name = roleManagerRole.Name
                        });
                    }
                });

                return roles.AsEnumerable();
            }
        }

        public async Task<JsonResult> Delete(string id)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                await userManager.DeleteAsync(userManager.FindById(id));
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }

        internal class SystemRoles
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}