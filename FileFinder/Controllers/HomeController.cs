using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using FileFinder.Models;
using FileFinder.ViewModels;
using FileFinder.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Session;
using System.Security.Authentication;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace FileFinder.Controllers
{
    public class HomeController : Controller
    {
        const string SessionName = "_Name";

        private FileFinderContext _context;

        public HomeController(FileFinderContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            //Check if user logged in:
            if (HttpContext.Session.GetString("Username") == null)
            {
                return Redirect("Login");
            }

            // For Search:
            HomeViewModel homeViewModel = new HomeViewModel();

            // For Daily Tasks:
            homeViewModel.ActionFiles = _context.Files
                                        .Where(f => f.Status == Status.Damaged || f.Status == Status.Full)
                                        .Include(f => f.Consumer)
                                        .Include(f => f.CaseManager)
                                        .Include(f => f.Room)
                                        .OrderBy(f => f.Consumer.LastName);

            homeViewModel.InactiveFiles = _context.Files
                                        .Where(f => f.ShredDate <= homeViewModel.Today)
                                        .Include(f => f.Consumer)
                                        .Include(f => f.CaseManager)
                                        .Include(f => f.Room)
                                        .OrderBy(f => f.Consumer.LastName);

            foreach (File file in homeViewModel.InactiveFiles)
            {
                file.Status = Models.Status.Shred;
                _context.Update(file);
                _context.SaveChangesAsync();
            }

            return View(homeViewModel);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginViewModel loginVM = new LoginViewModel();

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                //Find user
                if (_context.FileMembers.Any(x => x.Email == loginVM.Email))
                {
                    FileMember user = _context.FileMembers.Single(u => u.Email == loginVM.Email);

                    if (user.Password.Equals(Encryption.Hash(loginVM.Password)))
                    {
                        //start session
                        HttpContext.Session.Set(SessionName, System.Text.Encoding.UTF8.GetBytes(user.Email));
                        //string SessionNum = HttpContext.Session.GetHashCode().ToString();
                        //HttpContext.Response.Cookies.Append(SessionNum, user.Email.ToString());

                        HttpContext.Session.SetString("Username", user.Email);


                        return RedirectToAction(nameof(Index));
                    }
                    ViewBag.Error = "Incorrect Password.";
                    return View(loginVM);
                }
                ViewBag.Error = "User does not exist. Please register.";
                return View(loginVM);
            }
            ViewBag.Error = "Login failed. Please try again.";
            return View(loginVM);
        }

        public IActionResult Logout(int? id)
        {
            HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Delete(SessionName);
            return View();
        }

        public async Task<IActionResult> FileRefresh(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var file = await _context.Files.SingleOrDefaultAsync(m => m.ID == id);

            file.Status = Models.Status.OK;
            file.Quantity += 1;

            _context.Files.Update(file);
            await _context.SaveChangesAsync();

            return Redirect("/Home/Index");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterViewModel registerVM = new RegisterViewModel();

            return View(registerVM);
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerVM)
        {
            if(ModelState.IsValid)
            {
                //if user does not already exist:
                if (!_context.FileMembers.Any(u => u.Email == registerVM.Email))
                {
                    //if passwords match:
                    if (registerVM.Password.Equals(registerVM.Verify))
                    {
                        //make new model with VM data, hashing password
                        FileMember newUser = new FileMember
                        {
                            FirstName = registerVM.FirstName,
                            LastName = registerVM.LastName,
                            Email = registerVM.Email,
                            Password = Encryption.Hash(registerVM.Password)
                        };
                        
                        //save model to DB
                        _context.FileMembers.Add(newUser);
                        _context.SaveChanges();

                        //start session
                        HttpContext.Session.Set(SessionName, System.Text.Encoding.UTF8.GetBytes(newUser.Email));
                        string SessionNum = HttpContext.Session.GetHashCode().ToString();
                        HttpContext.Response.Cookies.Append(SessionNum, newUser.Email.ToString());

                        return RedirectToAction(nameof(Index));
                    }

                    ViewBag.Error = "Passwords must match.";
                    return View(registerVM);
                }

                ViewBag.Error = "User already exists. Please login.";
                return View(registerVM);
            }

            ViewBag.Error = "Register failed. Please try again.";
            return View(registerVM);
        }

// JSON Checks
        public JsonResult DoesEmailExist(string email)
        {
            return Json(!_context.FileMembers.Any(x => x.Email == email));
        }

        public JsonResult EmailNotRegistered(string email)
        {
            return Json(_context.FileMembers.Any(x => x.Email == email));
        }

        public JsonResult WrongPassword(string password)
        {
            return Json(!_context.FileMembers.Any(x => x.Password == password));
        }

        // <----------- PARTIAL VIEW EXPERIMENTS -------------->

        //public IActionResult Search()
        //{
        //    SearchViewModel searchViewModel = new SearchViewModel();

        //    return PartialView("_Search", searchViewModel);
        //}


        //public IActionResult DailyTasks()
        //{
        //    var query = from file in _context.Files.Include("Consumers")
        //                where file.Status != Status.OK || file.Status != null
        //                select file;

        //    return PartialView("_DailyTasks", query);
        //}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
