using GameApp.Gateway;
using GameApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GameApp.Controllers
{

    public class UsersController : Controller
    {
        UserGateway aUserGateway = new UserGateway();
        [Authorize(Roles = "Admin,Editor")]
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GetUser()
        {
            var userListDesc = aUserGateway.GetAllUser().OrderByDescending(u => u.UserId);

            return Json(userListDesc, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Login(User u)
        {

            try
            {
                string lowerUsername = u.UserName.ToLower();
                var users = aUserGateway.GetAllUser();
                var user = users.FirstOrDefault(x => x.UserName == lowerUsername && x.Password == u.Password && x.IsActive == true);
                if (user == null)
                {
                    u.Message = "0";
                    return Json(u, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(u.UserName, false);
                    user.Message = "1";
                    return Json(user, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [Authorize]
        public ActionResult ChangePassword()
        {
            return PartialView();
        }
        [Authorize]
        [HttpPost]
        public JsonResult ChangePassword(User u)
        {
            User user = new User();
            try
            {

                var users = aUserGateway.GetAllUser();
                string lowerUsername = u.UserName.ToLower();
                bool nameExist = users.ToList().Exists(model => model.UserName.Equals(u.UserName, StringComparison.CurrentCultureIgnoreCase));
                bool passExist = users.ToList().Exists(model => model.Password.Equals(u.OldPassword));
                if (nameExist == true) 
                {
                    if (passExist == true)
                    {
                        if (u.Password == u.ConfirmPassword)
                        {
                            User us = new User();
                            us = users.Where(x => x.UserName == u.UserName).FirstOrDefault();
                            u.UserId = us.UserId;
                            u.RoleName = us.RoleName;
                            u.IsActive = us.IsActive;
                            u.FullName = us.FullName;
                            u.Phone = us.Phone;
                            u.Email = us.Email;
                            u.UserName = us.UserName;
                            u.Password = u.Password;
                            aUserGateway.SaveUser(u);
                            u.Message = "Password Updated Successfully.";
                        }
                        else
                        {
                            u.Message = "Confirm Password not Matched.";
                        }
                    }
                    else
                    {
                        u.Message = "Old Password not Matched.";
                    }

                }
                else
                {
                    u.Message = "Username not Matched.";
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            user.Message = u.Message;
            return Json(user, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Edit()
        {
            return PartialView();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult Edit(User u)
        {
            User user = new User();
            try
            {
                if (u.UserId > 0)
                {
                    aUserGateway.SaveUser(u);
                    u.Message = "Permitted Successfully.";
                }
                else
                {
                    u.Message = "Invalid User.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            user.Message = u.Message;
            return Json(user, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save()
        {
            return PartialView();
        }
        [HttpPost]
        public JsonResult Save(User u)
        {
            User user = new User();
            try
            {
                var users = aUserGateway.GetAllUser();
                string lowerUsername = u.UserName.ToLower();
                if (u.Password == u.ConfirmPassword)
                {
                    u.UserName = lowerUsername;
                    bool result = users.ToList().Exists(model => model.UserName.Equals(u.UserName, StringComparison.CurrentCultureIgnoreCase));
                    if (result != true)
                    {
                        aUserGateway.SaveUser(u);
                        u.Message = "Registration Successfull.";
                    }
                    else
                    {
                        u.Message = "User Already Exist.";
                    }

                }
                else
                {
                    u.Message = "Confirm Password not Matched.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            user.Message = u.Message;
            return Json(user, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public JsonResult DeleteUser(int uId)
        {
            var isDeleted = aUserGateway.DeleteUser(uId);

            return Json(isDeleted, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

    }
}