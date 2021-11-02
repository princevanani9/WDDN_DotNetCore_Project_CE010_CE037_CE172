using ChattingApplication.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ChattingApplication.Controllers
{
   
    public class UserController:Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment hostingEnviroment;
        const string SessionName = "_uname";
        public UserController(IUserRepository userRepository, IWebHostEnvironment hostingEnviroment)
        {
            _userRepository = userRepository;
            this.hostingEnviroment = hostingEnviroment;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(UserRegisterViewModel model)
        {
            if(_userRepository.checkUser(model.Username)!=null)
            {
                ModelState.AddModelError("Username","Please Check username it is already exist");
            }
            if(ModelState.IsValid)
            {
                string uniqueFileName = null;
                if(model.Photo!=null)
                {
                    string uploadsFolder = Path.Combine(hostingEnviroment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));

                }
                User newuser = new User {
                    Username = model.Username,
                    Email=model.Email,
                    Password=model.Password,
                    ConfirmPassword=model.Password,
                    Mobile=model.Mobile,
                    Photopath=uniqueFileName
                   
                };

                 _userRepository.Add(newuser);
                return RedirectToAction("Login");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login(User user)
        {
            return View();
        }
        public IActionResult Submit(string Username, string Password) 
        {
            User existuser = _userRepository.hasUser(Username, Password);
            if (existuser == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
               HttpContext.Session.SetString(SessionName, Username);
                return RedirectToAction("Home");
            }
        }
        public IActionResult Home()
        {
              //  var model = _userRepository.GetAllUsers();
                ViewBag.user = HttpContext.Session.GetString(SessionName);
                return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionName);
            return RedirectToAction("Login");
        }
        public IActionResult Profile(int Id)
        {
           // User model = _userRepository.GetUser(Id);
            return View();
        }
    }
}
