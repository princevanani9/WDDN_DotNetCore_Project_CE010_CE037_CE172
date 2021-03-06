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
        const string Reciverid = "_rid"; 
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
                var model = _userRepository.GetAllUsers();
                ViewBag.user = HttpContext.Session.GetString(SessionName);
                return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionName);
            return RedirectToAction("Login");
        }
        public IActionResult Profile(int Id)
        {
           User model = _userRepository.GetUser(Id);
            return View(model);
        }
        public IActionResult ViewProfile()
        {
            User model = _userRepository.GetUsername(HttpContext.Session.GetString(SessionName));
            return View(model);
        }

        public IActionResult UpdateProfile()
        {
            User model = _userRepository.GetUsername(HttpContext.Session.GetString(SessionName));
            return View(model);
        }
        public IActionResult SaveChanges(string Username, string Email, string Mobile, string Password, String ConfirmPassword)
        {

            User usr = _userRepository.GetUsername(HttpContext.Session.GetString(SessionName));
            usr.Username = Username;
            usr.Email = Email;
            usr.Mobile = Mobile;
            usr.Password = Password;
            usr.ConfirmPassword = ConfirmPassword;
            _userRepository.Update(usr);
            HttpContext.Session.SetString(SessionName, Username);
            return RedirectToAction("ViewProfile");
        }
        public IActionResult Message(int Id)
        {
            Response.Headers.Add("Refresh", "4");
            int sId = _userRepository.GetId(HttpContext.Session.GetString(SessionName));
            var model = _userRepository.GetAllChat();
            ViewBag.user = HttpContext.Session.GetString(SessionName);
            ViewBag.users = _userRepository.GetAllUsers();
            ViewBag.SenderId = sId;
            ViewBag.ReciverId = Id;
            HttpContext.Session.SetInt32(Reciverid, Id);
            return View(model);
        }
        public IActionResult Search(string search)
        {
           // int ?id = _userRepository.GetId(search);
                if (_userRepository.checkUser(search)!=null)
                {
                 int ? id = _userRepository.GetId(search);
                return RedirectToAction("Message", new { Id = id });
                }
                else
                {
                  
                    return RedirectToAction("Home");
            }
        }
        public IActionResult SaveChat(string message)
        {
            int sId = _userRepository.GetId(HttpContext.Session.GetString(SessionName));
            int rId = (int)HttpContext.Session.GetInt32(Reciverid);
              Chat newchat = new Chat
              {

                  Sender = sId,
                  Reciver=rId,
                  Message=message
              };
            _userRepository.AddChat(newchat);
            return RedirectToAction("Message", new { Id = rId });
        }

        public IActionResult ClearChat(string message)
        {
            int sId = _userRepository.GetId(HttpContext.Session.GetString(SessionName));
            int rId = (int)HttpContext.Session.GetInt32(Reciverid);
            _userRepository.Clear(sId, rId);
            return RedirectToAction("Message", new { Id = rId });
        }
    }
}
