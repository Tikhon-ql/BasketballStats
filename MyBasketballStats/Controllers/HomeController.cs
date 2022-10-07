using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyBasketballStats.Context;
using MyBasketballStats.Models;
using MyBasketballStats.Models.Identity;
using MyBasketballStats.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MyBasketballStats.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext context;
        /// <summary>
        /// We can work without it
        /// </summary>
        private UserManager<User> userManager;
        public HomeController(ApplicationContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var claims = User.FindFirst(ClaimTypes.NameIdentifier);
            if (claims != null)
            {
                ViewBag.CurrentId = claims.Value;
                Person person = context.People.Include(p => p.Notifications).FirstOrDefault(p => p.Id == claims.Value);
                return View(person.Notifications.Where(n => n.IsChecked == false).OrderBy(n=>n.Date).ToList());
            }
            return View(null);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
        public IActionResult AddPlayer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPlayer(Person person)
        {
            if (person == null)
                return View("Error.cshtml");
            person.Id = Guid.NewGuid().ToString();
            context.People.Add(person);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DeletePlayer()
        {
            return View();
        }
        //ошибка на пустой id и ненахождение person с данным id
        [HttpPost]
        public IActionResult DeletePlayer(string id)
        {
            if (String.IsNullOrEmpty(id))
                return View("Error.cshtml");
            Person person = context.People.FirstOrDefault(p => p.Id == id);
            if (person == null)
                return View("Error.cshtml");
            context.Entry(person).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            context.SaveChanges();
            return RedirectToAction();
        }

        public IActionResult ShowTrainings()
        {
            return View(context.Trainings.ToList());
        }
        public IActionResult GetDriblingTurotials()
        {
            return PartialView("Turotials", context.Trainings.Where(t => t.Type == Enums.TrainingType.Dribbling).ToList());
        }

        public IActionResult GetGameThrowingTurotials()
        {
            return PartialView("Turotials", context.Trainings.Where(t => t.Type == Enums.TrainingType.GameThrowing).ToList());
        }
        public IActionResult GetThreePointerTurotials()
        {
            return PartialView("Turotials", context.Trainings.Where(t => t.Type == Enums.TrainingType.ThreePointer).ToList());
        }
        public IActionResult GetPassingTurotials()
        {
            return PartialView("Turotials", context.Trainings.Where(t => t.Type == Enums.TrainingType.Passing).ToList());
        }
        public IActionResult GetWarmupTurotials()
        {
            return PartialView("Turotials", context.Trainings.Where(t => t.Type == Enums.TrainingType.Warmup).ToList());
        }
    }
}
