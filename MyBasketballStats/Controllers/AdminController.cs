using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBasketballStats.Context;
using MyBasketballStats.Models;
using MyBasketballStats.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBasketballStats.Controllers
{
    public class AdminController : Controller
    {
        private ApplicationContext context;
        private UserManager<User> userManager;
        public AdminController(ApplicationContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }
        public IActionResult AdminPage()
        {
            return View();
        }
        public IActionResult ShowUsers()
        {
            return View(context.People.Include(p=>p.Geoposition).ToList());
        }
        public IActionResult DeleteUser(string id)
        {
            Person person = context.People.FirstOrDefault(p => p.Id == id);
            User user = userManager.Users.FirstOrDefault(u => u.Id == id);
            if(person != null && user != null)
            {
                context.People.Remove(person);
                var result = userManager.DeleteAsync(user).Result;
                if (result.Succeeded)
                    context.SaveChanges();
            }
            return RedirectToAction("ShowUsers","Admin");
        }
        [HttpGet]
        public IActionResult EditUser(string id)
        {
            if (String.IsNullOrEmpty(id))
                return RedirectToAction("ShowUsers", "Admin");
            Person person = context.People.Include(p=>p.Geoposition).FirstOrDefault(p => p.Id == id);
            if(person == null)
                return RedirectToAction("ShowUsers", "Admin");
            return View(person);
        }
        [HttpPost]
        public IActionResult EditUser(Person model)
        {
            if(ModelState.IsValid)
            {
                context.Entry(model).State = EntityState.Modified;
                User user = userManager.Users.FirstOrDefault(u => u.Id == model.Id);
                user.UserName = model.Name;
                user.Email = model.Email;
                var result =  userManager.UpdateAsync(user).Result;
                if(result.Succeeded)
                {
                    context.SaveChanges();
                    return RedirectToAction("ShowUsers", "Admin");
                }
                return View(model);
            }
            return View(model);
        }
    }
}
