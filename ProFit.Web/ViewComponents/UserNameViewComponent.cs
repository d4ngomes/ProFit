using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProFit.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProFit.Web.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserNameViewComponent(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userEmail = user.Email;
            var userDb = await _db.Users.FirstOrDefaultAsync(a => a.Email == userEmail);
            return View(userDb);
        }
    }
}
