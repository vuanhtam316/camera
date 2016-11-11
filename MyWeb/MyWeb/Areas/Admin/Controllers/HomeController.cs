using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MyWeb.Business;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Controllers
{
    public class HomeController : ProviderControllerBase
    {
        //
        // GET: /Admin/Home/
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            return View();
        }






        //#region Login
        //public ActionResult Login()
        //{
        //    if (Session["erro"] != null)
        //    {
        //        ViewData["Message"] = Session["erro"];
        //        Session["erro"] = null;
        //    }
        //    return View();
        //}

        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult Login(string username, string password, string returnUrl = null)
        //{
        //    ViewBag.MessageCommand = "";
        //    var messageModel = new MesageModel.MessagesModel
        //    {
        //        MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
        //        MessageType = MesageModel.MessageType.Error
        //    };
        //    ViewBag.MessageCommand = messageModel;
        //    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
        //    {
        //        var pass = Encryptor.Encrypt(password);
        //        var us = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME.ToLower().Equals(username) && x.PASSWORDS.Equals(pass));
        //        if (us != null)
        //        {
        //            if (us.USER_ACTIVE != 1)
        //            {
        //                messageModel.MessageContent = "Tài khoản đã bị khóa, vui lòng liên hệ quản trị viên";
        //                messageModel.MessageType = MesageModel.MessageType.Warning;
        //                Session["erro"] = "Tài khoản đã bị khóa, vui lòng liên hệ quản trị viên";
        //                return View("Login");
        //            }
        //            FormsAuthentication.SetAuthCookie(us.USER_NAME, true);
        //            if (Request.IsAuthenticated && User.IsInRole(IsAdmin))
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //            return Redirect("/");
        //        }
        //        messageModel.MessageContent = "Tài khoản hoặc mật khẩu không chính xác.";
        //        messageModel.MessageType = MesageModel.MessageType.Warning;
        //        Session["erro"] = "Tài khoản hoặc mật khẩu không chính xác.";
        //        return View("Login");
        //    }
        //    messageModel.MessageContent = "Hãy nhập đầy đủ thông tin";
        //    messageModel.MessageType = MesageModel.MessageType.Warning;
        //    Session["erro"] = "Hãy nhập đầy đủ thông tin";
        //    return View("Login");
        //}
        //#endregion
        //[Authorize]
        //public ActionResult Logout()
        //{
        //    FormsAuthentication.SignOut();
        //    return RedirectToAction("Login", "Home");
        //}
        //#region ChangePassword

        //[Authorize(Roles = "Administrator,Custommer")]
        //public PartialViewResult ChangePassword(string username)
        //{
        //    var model = new AccountModel { UserName = username };
        //    return PartialView("_ptvChangePassword", model);
        //}
        //[Authorize(Roles = "Administrator,Custommer")]
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult ChangePassword(AccountModel model)
        //{
        //    ViewBag.MessageCommand = "";
        //    var messageModel = new MesageModel.MessagesModel
        //    {
        //        MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
        //        MessageType = MesageModel.MessageType.Error
        //    };
        //    ViewBag.MessageCommand = messageModel;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(model.UserName))
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var user = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME == model.UserName);
        //                if (user != null)
        //                {
        //                    if (user.PASSWORDS != Encryptor.Encrypt(model.Passwords))
        //                    {
        //                        messageModel.MessageContent = "Mật khẩu cũ không chính xác";
        //                        messageModel.MessageType = MesageModel.MessageType.Warning;
        //                        return PartialView("_Message", ViewBag.MessageCommand);
        //                    }
        //                    user.PASSWORDS = Encryptor.Encrypt(model.NewPasswords);
        //                    GetContext().SaveChanges();
        //                    messageModel.MessageContent = "Thay đổi mật khẩu thành công";
        //                    messageModel.MessageType = MesageModel.MessageType.Success;
        //                    FormsAuthentication.SignOut();
        //                    return JavaScript("window.location ='/Admin/Home/Login'");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(String.Format("An error occurred in the process ChangePassword item in master admin with erro: {0}", ex));
        //        return PartialView("_Message", ViewBag.MessageCommand);
        //    }
        //    return PartialView("_Message", ViewBag.MessageCommand);
        //}
        //#endregion
    }
}
