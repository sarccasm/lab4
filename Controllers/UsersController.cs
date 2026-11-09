using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductsValidation.Models;
using ProductsValidation.Services;

namespace ProductsValidation.Controllers
{
    public class UsersController : Controller
    {
        private List<User> users;

        public UsersController(Data data)
        {
            users = data.Users;
        }
        
        public IActionResult Index()
        {
            return View(users);
        }

        [HttpGet]
        public IActionResult Create([FromQuery] User user)
        {
            return View(user);
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost([FromQuery] User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.Id = users.Count == 0 ? 1 : users.Max(existingUser => existingUser.Id) + 1;
            users.Add(user);
            return View("View", user);
        }
    }
}
