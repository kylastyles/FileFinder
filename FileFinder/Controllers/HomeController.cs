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


namespace FileFinder.Controllers
{
    public class HomeController : Controller
    {
        const string SessionName = "_Name";

        private readonly FileFinderContext _context;

        public HomeController(FileFinderContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            // For Search:
            HomeViewModel homeViewModel = new HomeViewModel();

            // For Daily Tasks:
            homeViewModel.ActionFiles = _context.Files
                                        .Where(f => f.Status == Status.Damaged || f.Status == Status.Full)
                                        .Include(f => f.Consumer)
                                        .Include(f => f.CaseManager)
                                        .Include(f => f.Room);

            homeViewModel.InactiveFiles = _context.Files
                                        .Where(f => f.ShredDate <= homeViewModel.Today)
                                        .Include(f => f.Consumer)
                                        .Include(f => f.CaseManager)
                                        .Include(f => f.Room);

            foreach (File file in homeViewModel.InactiveFiles)
            {
                file.Status = Models.Status.Shred;
                _context.Update(file);
                _context.SaveChangesAsync();
            }

            return View(homeViewModel);
        }


        public IActionResult Login()
        {
            LoginViewModel loginVM = new LoginViewModel();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                //Find user
                //System.InvalidOperationException: 'Sequence contains no elements'

                try
                {
                    FileMember user = _context.FileMembers.Single(u => u.Email == loginVM.Email);

                    if (user.Password == loginVM.Password)
                    {
                        //start session
                        HttpContext.Session.Set(SessionName, System.Text.Encoding.UTF8.GetBytes(user.Email));
                        string SessionNum = HttpContext.Session.GetHashCode().ToString();
                        HttpContext.Response.Cookies.Append(SessionNum, user.Email.ToString());
                        
                        RedirectToAction(nameof(Index));
                    }
                }
                catch (InvalidOperationException)
                {
                    ViewBag.Error = "User not found. Please register.";
                    return View(loginVM);
                }

            }

            //Login failed, return to login screen
            ViewBag.Error = "Login Failed. Tray Again.";
            return View(loginVM);
        }

        public IActionResult Logout(int? id)
        {
            //TODO: Remove Cookie from Session

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

            return Redirect("/");
        }

        public IActionResult Register()
        {
            RegisterViewModel registerVM = new RegisterViewModel();

            return View(registerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel registerVM)
        {
            if(ModelState.IsValid)
            {
                //if user does not already exist:
                //if(_context.FileMembers.Where(u => u.Email == registerVM.Email) == null)
                //{
                    //if passwords match:
                    if(registerVM.Password.Equals(registerVM.Verify))
                    {
                        //make new model with VM data, hashing data
                        FileMember newUser = new FileMember
                        {
                            FirstName = registerVM.FirstName,
                            LastName = registerVM.LastName,
                            Email = registerVM.Email,
                            Password = registerVM.Password
                        };

                        //save model to DB
                        _context.FileMembers.Add(newUser);
                        _context.SaveChangesAsync();

                        //start session
                        HttpContext.Session.Set(SessionName, System.Text.Encoding.UTF8.GetBytes(newUser.Email));
                        String CookieName = registerVM.Email;
                        HttpContext.Response.Cookies.Append(CookieName, registerVM.Email);

                    return RedirectToAction(nameof(Index));
                    }

                    ViewBag.Error = "Passwords must match.";
                    return View(registerVM);
                //}

                //ViewBag.Error = "User already exists. Please login.";
                //return View(registerVM);
            }

            ViewBag.Error = "Register failed. Please try again.";
            return View(registerVM);
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
