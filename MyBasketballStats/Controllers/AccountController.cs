using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBasketballStats.Context;
using MyBasketballStats.Models;
using MyBasketballStats.Models.Identity;
using MyBasketballStats.Models.ManyToMany;
using MyBasketballStats.Models.PersonBehavior;
using MyBasketballStats.ViewModels;
using MyBasketballStats.ViewModels.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
    
namespace MyBasketballStats.Controllers
{
    public class AccountController : Controller
    {
        private SignInManager<User> signInManager;
        private UserManager<User> userManager;
        private ApplicationContext context;
        private DateTime Date;
        private static string[] months = { "January", "Febrary", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        private static double pagCount = 4;

        public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, ApplicationContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.context = context;
            Date = DateTime.Now.AddDays(2);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModel { ReturnUrl = "" });
        }
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false).Result;
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    return View(model);
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterModel model, string latitude, string longitude,string city,string country)
        {
            if (ModelState.IsValid)
            {
                Random rand = new Random();
                User user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.Name,
                    Email = model.Email
                };
                Person person = new Person
                {
                    Id = user.Id,
                    Name = model.Name,
                    Surname = model.Surname,
                    Patronymic = model.Patronymic,
                    Email = model.Email
                };
                Geoposition geoposition = new Geoposition
                {
                    Id = Guid.NewGuid().ToString(),
                    //Longitude = double.Parse(longitude.Replace('.',',')),
                    //Latitude = double.Parse(latitude.Replace('.',','))
                    Longitude = rand.Next(1, 100),
                    Latitude = rand.Next(1, 100),
                    City = city,
                    Country = country
                };
                person.Geoposition = geoposition;
                var result = userManager.CreateAsync(user, model.Password).Result;
                if (result.Succeeded)
                {
                    //if(user.UserName == "root")
                    //    userManager.AddToRoleAsync(user, "Admin");
                    userManager.AddToRoleAsync(user,"User");
                    context.People.Add(person);
                    context.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
                else
                    return View(model);
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Profile()
        {
            User user = userManager.GetUserAsync(HttpContext.User).Result;
            Person person = context.People.Include(p => p.GamePerfomances).FirstOrDefault(p => p.Id == user.Id);
            if (person == null)
                return View("Error.cshtml");
            ProfileViewModel model = new ProfileViewModel { Email = user.Email, Person = person };
            model.Begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month + 1));
            model.Duration = months[DateTime.Now.Month - 1] + " " + DateTime.Now.Year;
            model.AccessType = Enums.ProfileAccessType.Owner;
            return View(model);
        }
        [HttpPost]
        public IActionResult Profile(string id, string email, string begin, string end, string accessType)
        {
            Person person = context.People.Include(p => p.GamePerfomances).FirstOrDefault(p => p.Id == id);
            if (person == null)
                return View("Error.cshtml");
            string[] beginSep = begin.Split('.');
            string[] endSep = end.Split('.');
            DateTime beginDate = new DateTime(int.Parse(beginSep[2]), int.Parse(beginSep[1]), int.Parse(beginSep[0]));
            DateTime endDate = new DateTime(int.Parse(endSep[2]), int.Parse(endSep[1]), int.Parse(endSep[0]));
            ProfileViewModel model = new ProfileViewModel
            {
                Email = email,
                Person = person,
                Begin = beginDate,
                End = endDate
            };
            if (accessType == "Owner")
                model.AccessType = Enums.ProfileAccessType.Owner;
            else
                model.AccessType = Enums.ProfileAccessType.Another;

            if (beginDate.Year != endDate.Year)
            {
                for (int i = beginDate.Year; i <= endDate.Year; i++)
                {
                    model.Duration += i.ToString() + " ";
                }
            }
            else
            {
                for (int i = beginDate.Month; i <= endDate.Month; i++)
                {
                    model.Duration += months[i - 1] + " ";
                }
                model.Duration += DateTime.Now.Year.ToString();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AddPerfomance()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPerfomance(BasketballGamePerfomance perfomance)
        {
            if (ModelState.IsValid)
            {
                //Something went wrong with perfomance.
                if (perfomance == null)
                    return View("Error.cshtml");
                Person person = context.People
                    .FirstOrDefault(p => p.Id == userManager.GetUserAsync(HttpContext.User).Result.Id);
                //Person was not found
                if (person == null)
                    return View("Error.cshtml");
                perfomance.Id = Guid.NewGuid().ToString();
                Random rund = new Random();
                Date = Date.AddMonths(rund.Next(0, 1)).AddDays(rund.Next(1, 3)).AddHours(1);
                perfomance.Date = Date;
                person.GamePerfomances.Add(perfomance);
                StatsCounter.CountStats(person, perfomance);
                context.Entry(person).State = EntityState.Modified;
                context.SaveChanges();
            }
            return RedirectToAction("Profile", "Account");
        }
        /// <summary>
        /// Shit long code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult SendFriendRequest(int id)
        {
            string currrentUserId = userManager.GetUserId(HttpContext.User);
            //ViewBag.CurrentUser = currrentUserId;
            Person current = context.People.Include(p => p.Friends).Include(p=>p.Geoposition).FirstOrDefault(p => p.Id == currrentUserId);
            if (current == null)
                return View("Error.cshtml");
            List<Person> noFrineds = new();
            foreach (var item in context.People.Include(p => p.Geoposition).Include(p=>p.Friends).ToList())
            {
                if (!IsFriends(currrentUserId, item.Id))
                    noFrineds.Add(item);
            }
            noFrineds.Remove(current);
            ViewBag.Count = Math.Ceiling((noFrineds.Where(p=>p.Geoposition.City == current.Geoposition.City).ToList().Count) / pagCount);
            ViewBag.Current = id;
            return View(noFrineds.Skip(id * (int)pagCount).Take((int)pagCount).ToList());
        }
        [HttpPost]
        public IActionResult SendFriendRequest(string friendEmail)
        {
            if (ModelState.IsValid)
            {
                Person current = context.People.FirstOrDefault(p => p.Id == userManager.GetUserId(HttpContext.User));
                Person receiver = context.People.Include(p => p.Notifications).FirstOrDefault(p => p.Email == friendEmail);
                if (current == null || receiver == null)
                    return View("Error.cshtml");
                Notification notification = new Notification();
                notification.Id = Guid.NewGuid().ToString();
                notification.Message = $"Friend request from {current.Surname} {current.Name}";
                notification.SenderId = current.Id;
                notification.Date = DateTime.Now;
                notification.Type = Enums.NotificationType.FriendRequest;
                receiver.Notifications.Add(notification);
                context.Entry(receiver).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult AcceptFriendRequest(string id)
        {
            Person friendAccount = context.People.Include(p => p.Notifications).FirstOrDefault(p => p.Id == id);
            if (friendAccount == null)
            {
                ModelState.AddModelError("", $"User not found");
                return RedirectToAction("Index", "Home");
            }
            string currentUserId = userManager.GetUserId(HttpContext.User);
            if (String.IsNullOrEmpty(currentUserId))
            {
                ModelState.AddModelError("", $"There are some trubles");
                return RedirectToAction("Index", "Home");
            }
            Person current = context.People.Include(p => p.Notifications).FirstOrDefault(p => p.Id == currentUserId);
            if (current == null)
            {
                ModelState.AddModelError("", $"Current user not found");
                return RedirectToAction("Index", "Home");
            }
            if (IsFriends(currentUserId, friendAccount.Id))
            {
                ModelState.AddModelError("", $"You are friends");
                return RedirectToAction("Index", "Home");
            }

            Friend friend = new Friend
            {
                Id = Guid.NewGuid().ToString(),
                FirstFriendId = currentUserId,
                SecondFriendId = friendAccount.Id
            };
            context.Friends.Add(friend);

            Notification acceptNotification = new Notification
            {
                Id = Guid.NewGuid().ToString(),
                Message = $"{current.Surname} {current.Name} accept your friend request.",
                Date = DateTime.Now,
                Type = Enums.NotificationType.JustText,
                SenderId = current.Id
            };
            friendAccount.Notifications.Add(acceptNotification);
            context.Entry(friendAccount).State = EntityState.Modified;


            current.Notifications
                   .Where(n => n.SenderId == friendAccount.Id && n.Type == Enums.NotificationType.FriendRequest)
                   .ToList().ForEach(n => n.IsChecked = true);
            context.Entry(current).State = EntityState.Modified;
           
            context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        public IActionResult RefuseFriendRequest(string id)
        {
            try
            {
                Person sender = context.People.Include(p => p.Notifications).FirstOrDefault(p => p.Id == id);
                if (sender == null)
                {
                    ModelState.AddModelError("", $"User not found");
                    return View();
                }
                Person current = context.People.Include(p => p.Notifications).FirstOrDefault(p => p.Id == userManager.GetUserId(HttpContext.User));
                if (current == null)
                {
                    ModelState.AddModelError("", $"User not found");
                    return View();
                }

                current.Notifications
                    .Where(n => n.SenderId == id && n.Type == Enums.NotificationType.FriendRequest)
                    .ToList().ForEach(n => n.IsChecked = true);
                Notification refuseNotification = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = $"{current.Surname} {current.Name} refuse your friend request.",
                    Date = DateTime.Now,
                    Type = Enums.NotificationType.JustText,
                    SenderId = current.Id
                };
                sender.Notifications.Add(refuseNotification);
                context.Entry(sender).State = EntityState.Modified;
                context.Entry(current).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return NotFound();
            }
        }

        public IActionResult MarkAsChecked(string notificationId)
        {
            Person current = context.People.Include(p => p.Notifications).FirstOrDefault(p => p.Id == userManager.GetUserId(HttpContext.User));
            if (current == null)
                return NotFound();
            Notification notification = current.Notifications.FirstOrDefault(n => n.Id == notificationId);
            if (notification != null)
            {
                notification.IsChecked = true;
                context.Entry(current).State = EntityState.Modified;
                context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult MarkAllAsChecked()
        {
            Person current = context.People.Include(p => p.Notifications).FirstOrDefault(p => p.Id == userManager.GetUserId(HttpContext.User));
            if (current == null)
                return NotFound();
            current.Notifications.ForEach(n => n.IsChecked = true);
            context.Entry(current).State = EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("Index","Home");
        }


        /// <summary>
        /// Ultra shit code!!!!!!!!!
        /// </summary>
        /// <returns></returns>
        public IActionResult ShowFriends(string id)
        {
            string currentUserId = id;
            Person person = context.People.FirstOrDefault(p => p.Id == id);
            List<Person> friends = new();
            foreach (var item in context.People.Include(p => p.Friends).ToList())
            {
                if (IsFriends(currentUserId, item.Id))
                    friends.Add(item);
            }
            if (friends.Count == 0)
            {
                ModelState.AddModelError("", "U r my friend a a");
                return RedirectToAction("Index", "Home");
            }
            return View(friends);
        }
        public IActionResult ShowAnotherProfile(string id)
        {
            Person person = context.People.Include(p => p.GamePerfomances).FirstOrDefault(p => p.Id == id);
            ProfileViewModel model = new ProfileViewModel { Email = person.Email, Person = person };
            model.Begin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            model.End = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month + 1));
            model.Duration = months[DateTime.Now.Month - 1] + " " + DateTime.Now.Year;
            model.AccessType = Enums.ProfileAccessType.Another;
            return View("Profile", model);
        }


       

        public bool IsFriends(string firstPersonId, string secondPersonId)
        {
            Friend friend1 = context.Friends.FirstOrDefault(p => p.FirstFriendId == firstPersonId && p.SecondFriendId == secondPersonId);
            Friend friend2 = context.Friends.FirstOrDefault(p => p.FirstFriendId == secondPersonId && p.SecondFriendId == firstPersonId);
            if (friend1 == null && friend2 == null)
                return false;
            return true;
        }
    }
}