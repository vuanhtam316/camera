using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MyWeb.Areas.Front.Models;
using MyWeb.Business;
using MyWeb.Models;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using MyWeb.Areas.Admin.Models;
using System.IO;
using MyWeb.Areas.Agency.Models;

namespace MyWeb.Controllers
{
    public class AccountController : ProviderControllerBase
    {
        //
        // GET: /Account/

        #region Login
        public ActionResult Login()
        {
            if (Session["erro"] != null)
            {
                //ViewData["Message"] = Session["erro"];
                ViewBag.thongbao = "<script>$(document).ready(function () {alert('" + Session["erro"] + "');});</script>";
                Session["erro"] = null;
                ViewBag.thongbao = "";
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(string username, string password, LoginUser user)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Bạn hãy nhập đầy đủ thông tin",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                if (ModelState.IsValid)
                {
                    var userName = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME == user.UserName);

                    if (userName != null)
                    {
                        var pass = Encryptor.Encrypt(user.Password);
                        var passWord = GetContext().TBL_USER.FirstOrDefault(x => x.PASSWORDS == pass);
                        if (passWord != null)
                        {
                            var us = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME.ToLower().Equals(username) && x.PASSWORDS.Equals(pass));
                            if (us != null)
                            {
                                if (us.USER_ACTIVE != 1)
                                {
                                    messageModel.MessageContent = "Tài khoản đã bị khóa hoặc chưa được kích hoạt, vui lòng liên hệ quản trị viên";
                                    messageModel.MessageType = MesageModel.MessageType.Warning;
                                    Session["erro"] = "Tài khoản đã bị khóa hoặc chưa được kích hoạt, vui lòng liên hệ quản trị viên";
                                    //ViewBag.thongbao = "<script>$(document).ready(function () {alert('Tài khoản đã bị khóa hoặc chưa được kích hoạt, vui lòng liên hệ quản trị viên');});</script>";
                                    return View("Login");
                                }
                                FormsAuthentication.SetAuthCookie(us.USER_NAME, true);
                                if (Request.IsAuthenticated && User.IsInRole(IsAgency))
                                {
                                    return RedirectToAction("Index", "HomeAgency");
                                }
                                else
                                {
                                    if (Request.IsAuthenticated && User.IsInRole(IsAdmin))
                                    {
                                        return RedirectToAction("Index", "Home");
                                    }
                                    else
                                    {
                                        return Redirect("/");
                                    }
                                }
                            }
                        }
                        else
                        {
                            messageModel.MessageContent = "Password chưa đúng, hãy kiểm tra lại";
                            messageModel.MessageType = MesageModel.MessageType.Error;
                        }
                    }
                    else
                    {
                        messageModel.MessageContent = "UserName chưa đúng, hãy kiểm tra lại";
                        messageModel.MessageType = MesageModel.MessageType.Error;
                    }
                }
            }
            //var passwork = GetContext().TBL_USER.FirstOrDefault(x => x.EMAIL == user.PASSWORDS);
            //var userName = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME == user.USER_NAME);
            //if(userName!= null)
            //{
            //    messageModel.MessageContent = "UserName chua dung";
            //    messageModel.MessageType = MesageModel.MessageType.Error;
            //}
            //else
            //{

            //}


            //if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            //{
            //    var pass = Encryptor.Encrypt(password);
            //    var us = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME.ToLower().Equals(username) && x.PASSWORDS.Equals(pass));
            //    //var userName = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME.ToLower().Equals(username));
            //    //var passWork = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME.ToLower().Equals(password));


            //    if (us != null)
            //    {
            //        if (us.USER_ACTIVE != 1)
            //        {
            //            messageModel.MessageContent = "Tài khoản đã bị khóa hoặc chưa được kích hoạt, vui lòng liên hệ quản trị viên";
            //            messageModel.MessageType = MesageModel.MessageType.Warning;
            //            Session["erro"] = "Tài khoản đã bị khóa, vui lòng liên hệ quản trị viên";
            //            ViewBag.thongbao = "<script>$(document).ready(function () {alert('Tài khoản đã bị khóa, vui lòng liên hệ quản trị viên');});</script>";
            //            return View("Login");
            //        }
            //        FormsAuthentication.SetAuthCookie(us.USER_NAME, true);
            //        if (Request.IsAuthenticated && (User.IsInRole(IsAdmin) || User.IsInRole(IsUser)))
            //        {
            //            return RedirectToAction("Index", "Home");
            //        }
            //        return Redirect("/");
            //    }

            //messageModel.MessageContent = "Tài khoản hoặc mật khẩu không chính xác.";
            //messageModel.MessageType = MesageModel.MessageType.Warning;
            //Session["erro"] = "Tài khoản hoặc mật khẩu không chính xác.";
            //ViewBag.thongbao = "<script>$(document).ready(function () {alert('Tài khoản hoặc mật khẩu không chính xác.');});</script>";
            //return View("Login");
            //}
            //messageModel.MessageContent = "Hãy nhập đầy đủ thông tin";
            //messageModel.MessageType = MesageModel.MessageType.Warning;
            //Session["erro"] = "Hãy nhập đầy đủ thông tin";
            //ViewBag.thongbao = "<script>$(document).ready(function () {alert('Hãy nhập đầy đủ thông tin.');});</script>";
            return View("Login");
        }
        #endregion

        #region Register role
        public ActionResult RegisterRole()
        {
            if (Session["erro"] != null)
            {
                //ViewData["Message"] = Session["erro"];
                ViewBag.thongbao = "<script>$(document).ready(function () {alert('" + Session["erro"] + "');});</script>";
                Session["erro"] = null;
                ViewBag.thongbao = "";
            }
            return View();
        }
        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult Register_Role2()
        //{
        //    return View("RegisterRole");
        //}
        #endregion

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        #region ChangePassword

        public PartialViewResult ChangePassword(string username)
        {
            var model = new AccountModel { UserName = username };
            return PartialView("_ptvChangePassword", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(AccountModel model)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;
            try
            {
                if (!string.IsNullOrEmpty(model.UserName))
                {
                    if (ModelState.IsValid)
                    {
                        var user = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME == model.UserName);
                        if (user != null)
                        {
                            if (user.PASSWORDS != Encryptor.Encrypt(model.Passwords))
                            {
                                messageModel.MessageContent = "Mật khẩu cũ không chính xác";
                                messageModel.MessageType = MesageModel.MessageType.Warning;
                                return PartialView("_Message", ViewBag.MessageCommand);
                            }
                            user.PASSWORDS = Encryptor.Encrypt(model.NewPasswords);
                            GetContext().SaveChanges();
                            messageModel.MessageContent = "Thay đổi mật khẩu thành công";
                            messageModel.MessageType = MesageModel.MessageType.Success;
                            FormsAuthentication.SignOut();
                            return JavaScript("window.location ='/Account/Login'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process ChangePassword item in master admin with erro: {0}", ex));
                return PartialView("_Message", ViewBag.MessageCommand);
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region UpdateUser

        public PartialViewResult AddFormUpdateInfo(string username)
        {
            var model = new UpdateInfoModel();
            if (string.IsNullOrEmpty(username)) return PartialView("_ptvUpdateInfo", model);
            var user = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME == username);
            if (user == null) return PartialView("_ptvUpdateInfo", model);
            var userInfo = GetContext().TBL_PROFILE_USER.FirstOrDefault(x => x.USER_ID == user.USER_ID);
            if (userInfo == null) return PartialView("_ptvUpdateInfo", model);
            model.Email = user.EMAIL;
            model.FullName = userInfo.FULL_NAME;
            model.IdUser = (int)userInfo.TBL_USER.USER_ID;
            model.IdUserInfo = (int)userInfo.PROFILE_USER_ID;
            model.Phone = userInfo.PHONE;
            model.Address = userInfo.ADDRESS;
            return PartialView("_ptvUpdateInfo", model);
        }

        public ActionResult UpdateInfo(UpdateInfoModel model)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;
            try
            {
                if (ModelState.IsValid)
                {
                    var us = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);
                    if (us != null)
                    {
                        us.EMAIL = model.Email;
                        GetContext().SaveChanges();
                    }
                    var usDetail = GetContext().TBL_PROFILE_USER.FirstOrDefault(x => x.PROFILE_USER_ID == GetUserLogin.CurentCustomerId);
                    if (usDetail == null)
                    {
                        var detail = new TBL_PROFILE_USER();
                        detail.FULL_NAME = model.FullName;
                        detail.PHONE = model.Phone;
                        detail.ADDRESS = model.Address;
                        detail.USER_ID = GetUserLogin.CurentCustomerId;
                        GetContext().TBL_PROFILE_USER.Add(detail);
                        GetContext().SaveChanges();
                    }
                    else
                    {
                        usDetail.FULL_NAME = model.FullName;
                        usDetail.PHONE = model.Phone;
                        usDetail.ADDRESS = model.Address;
                        GetContext().SaveChanges();
                    }
                    var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                    if (update.STATUS_UPDATE == 0)
                    {
                        update.STATUS_UPDATE = 1;
                    }
                    messageModel.MessageContent = "Cập nhật thông tin thành công";
                    messageModel.MessageType = MesageModel.MessageType.Success;
                }
                else
                {
                    Log.Info(String.Format("Model UpdateInfoModel is not valid"));
                }

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process update UpdateInfoModel item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Register
        public ActionResult Register()
        {
            if (Session["erro"] != null)
            {
                //ViewData["Message"] = Session["erro"];
                ViewBag.thongbao = "<script>$(document).ready(function () {alert('" + Session["erro"] + "');});</script>";
                Session["erro"] = null;
                ViewBag.thongbao = "";
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult RegisterForm(NewCustomer user)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;
            try
            {
                //if (ModelState.IsValid)
                //{
                var mail = GetContext().TBL_USER.FirstOrDefault(x => x.EMAIL == user.EMAIL);
                var username = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME == user.USER_NAME);
                //if (user.NAME == null || user.EMAIL == null || user.USER_NAME == null || user.PASSWORDS == null || user.CONFIGPASSWORDS == null)
                if (mail != null || username != null)
                {
                    messageModel.MessageContent = "Email hoặc UserName đã tồi tại";
                    messageModel.MessageType = MesageModel.MessageType.Error;
                }
                else
                {
                    Entities1 en = new Entities1();
                    {
                        var encryptPass = Encryptor.Encrypt(user.PASSWORDS);

                        var newUser = new TBL_USER()
                        {

                            // add new user
                            NAME = user.NAME_USERS,
                            DISPLAY_NAME = user.NAME_USERS,
                            EMAIL = user.EMAIL,
                            USER_NAME = user.USER_NAME,
                            PASSWORDS = encryptPass,
                            CONFIGPASSWORDS = encryptPass,
                            USER_ACTIVE = 0,
                        };
                        //newUser.USER_ACTIVE = (short)(user.Active == true ? 1 : 0),

                        GetContext().TBL_USER.Add(newUser);
                        //GetContext().SaveChanges();
                        //en.TBL_USER.Add(newUser);

                        //add role

                        //var userID = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == newUser.USER_ID);

                        //var userName = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME == newUser.USER_NAME);
                        if (newUser.USER_ACTIVE == 0)
                        {
                            //if (user.NAME_ROLE == "Doanh nghiệp")
                            //{
                            var userRole = new TBL_USER_ROLE()
                            {

                                //ROLE_ID = (short)model.RoleId,
                                USER_NAME = newUser.USER_NAME,
                                ROLE_NAME = "Custommer",
                                USER_ROLE_DESCRIPTION = null,
                                USER_ID = newUser.USER_ID,
                                ROLE_ID = 3
                            };
                            GetContext().TBL_USER_ROLE.Add(userRole);
                        }
                        var callbackUrl = Url.Action(
                                               "ConfirmEmail", "Account",
                                               new { },
                                               protocol: Request.Url.Scheme);

                        string st = "Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. </br> Mời bạn click vào Đăng Nhập đề hoàn tất việc đăng kí: <a href=\""
                                               + callbackUrl + "\">Đăng Nhập</a>";
                        SendEmail(user.EMAIL, st);
                        GetContext().SaveChanges();

                    }
                    messageModel.MessageContent = "Vào mail để kích hoạt tài khoản";
                    messageModel.MessageType = MesageModel.MessageType.Success;

                }
                //}
                //else
                //{
                //    //Log.Info(String.Format(""));
                //}
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process update UpdateInfoModel item in master admin with erro: {0}", ex));
            }
            //return PartialView("_Message", ViewBag.MessageCommand);
            //return RedirectToAction("Register", "Account");
            return View("Register");
        }

        #endregion



        public bool SendEmail(string toMail, string body)
        {
            bool bRes = true;
            string host = "smtp.gmail.com";
            int port = 587;
            string userName = "iloveyougt1503@gmail.com";
            string password = "gianggiang";
            MailMessage mail = new MailMessage();
            String s = "Negatech NSC";
            mail.From = new MailAddress(userName, s);
            mail.To.Add(toMail);
            mail.Subject = "Email kích hoạt tài khoản";
            mail.Body = body;
            //mail.Body = "Mời bạn click vào link dưới đây để kích hoạt tài khoản. /n";
            //mail.Body= string.Format("Thank you for your registration, please click on the below link to comlete your registration: <a href=\"{++}></a>  ");
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient(host, port);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(userName, password);
            client.Host = host;
            client.Port = port;
            client.EnableSsl = true;
            try
            {
                client.Send(mail);
            }
            catch (Exception ex)
            {
                bRes = false;
            }
            return bRes;
        }
        public ActionResult ConfirmEmail(NewCustomer newUser)
        {

            var user = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ACTIVE == newUser.USER_ACTIVE);

            if (user != null)
            {

                //var newUser = new TBL_USER()

                //user.USER_ACTIVE = 1;

                user.USER_ACTIVE = 1;
                //GetContext().TBL_USER.Add(user);
                GetContext().SaveChanges();
                return RedirectToAction("Login", "Account");
                //return JavaScript("window.location ='/Account/Login'");
                //await TBL_USER
                //if (user.Email == Email)
                //{
                //    user.ConfirmedEmail = true;
                //    await UserManager.UpdateAsync(user);
                //    await SignInAsync(user, isPersistent: false);
                //}
                //else
                //{
                //return RedirectToAction("Confirm", "Account", new { Email = user.Email });
                //}
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
        #region ManageCamera
        public ActionResult ManageCamera()
        {

            return PartialView("ManageCamera");
        }
        #endregion
        #region Register customer
        public ActionResult RegisterCustomer()
        {
            if (Session["erro"] != null)
            {
                //ViewData["Message"] = Session["erro"];
                ViewBag.thongbao = "<script>$(document).ready(function () {alert('" + Session["erro"] + "');});</script>";
                Session["erro"] = null;
                ViewBag.thongbao = "";
            }
            return View();
        }
        [HttpPost]
        //[ActionName("RegisterCustomer")]
        [AllowAnonymous]
        public ActionResult RegisterCustomerForm(NewAgencyModel user, HttpPostedFileBase file)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;
            try
            {
                if (ModelState.IsValid)
                {
                    var mail = GetContext().TBL_USER.FirstOrDefault(x => x.EMAIL == user.Email);
                    if (mail != null)
                    {
                        messageModel.MessageContent = "Email đã tồi tại";
                        messageModel.MessageType = MesageModel.MessageType.Error;
                    }
                    else
                    {
                        Entities1 en = new Entities1();
                        {
                            var encryptPass = Encryptor.Encrypt(user.Password);
                            var encryptConfigPass = Encryptor.Encrypt(user.ConfigPassword);
                            string directory = @"E:\Avatar\";

                            if (file != null && file.ContentType.Contains("image"))
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                file.SaveAs(Path.Combine(directory, fileName));
                                var newUser = new TBL_USER()
                                {
                                    NAME = user.Name_Agency,
                                    DISPLAY_NAME = user.Name_Agency,
                                    TAX_CODE = user.Tax_Code,
                                    PHONE = user.Phone,
                                    REPRESENTATIVE = user.Representation,
                                    ADDRESS = user.Address,
                                    EMAIL = user.Email,
                                    IMAGE_URL = "Avatar/" + file.FileName,
                                    USER_NAME = user.Username,
                                    PASSWORDS = encryptPass,
                                    CONFIGPASSWORDS = encryptConfigPass,
                                    USER_ACTIVE = 0,
                                };
                                GetContext().TBL_USER.Add(newUser);
                                if (newUser.USER_ACTIVE == 0)
                                {
                                    var userRole = new TBL_USER_ROLE()
                                    {
                                        USER_NAME = newUser.USER_NAME,
                                        ROLE_NAME = "Agency",
                                        USER_ROLE_DESCRIPTION = null,
                                        USER_ID = newUser.USER_ID,
                                        ROLE_ID = 2
                                    };
                                    GetContext().TBL_USER_ROLE.Add(userRole);
                                }
                                var callbackUrl = Url.Action(
                                                       "ConfirmEmail", "Account",
                                                       new { },
                                                       protocol: Request.Url.Scheme);

                                string st = "Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. </br> Mời bạn click vào Đăng Nhập đề hoàn tất việc đăng kí: <a href=\""
                                                       + callbackUrl + "\">Đăng Nhập</a>";
                                GetContext().SaveChanges();
                                SendEmail(user.Email, st);
                                messageModel.MessageContent = "Vào mail để kích hoạt tài khoản";
                                messageModel.MessageType = MesageModel.MessageType.Success;
                            }
                            else
                            {
                                messageModel.MessageContent = "Kiểm tra lại logo";
                                messageModel.MessageType = MesageModel.MessageType.Error;
                            }
                        }
                    }
                }
                else
                {
                    //Log.Info(String.Format(""));
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process update UpdateInfoModel item in master admin with erro: {0}", ex));
            }
            return View("RegisterCustomer");
        }
        #endregion
        #region feedback

        public PartialViewResult Feedback()
        {
            return PartialView("_ptvFeedback");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(FeedbackModel model)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;
            var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);
            try
            {
                if (ModelState.IsValid)
                {
                    var feedback = new TBL_FEEDBACK();
                    feedback.TITLE = model.Title;
                    feedback.FEEDBACK_CONTENT = model.Content;
                    feedback.FEEDBACK_TIME = DateTime.Now;
                    feedback.USER_ID = GetUserLogin.CurentCustomerId;
                    if (data.AGENCY_ID != null)
                    {
                        feedback.AGENCY_ID = data.AGENCY_ID;
                    }
                    else
                    {
                        feedback.AGENCY_ID = null;
                    }
                    GetContext().TBL_FEEDBACK.Add(feedback);
                    GetContext().SaveChanges();
                    messageModel.MessageContent = "Gửi thành công";
                    messageModel.MessageType = MesageModel.MessageType.Success;
                    //ViewBag.MessageCommand = messageModel;
                }
                else
                {
                    Log.Info(String.Format(""));
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process insert Camera item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion





        //public ActionResult ConfirmEmailAgency(NewAgencyModel newUser)
        //{

        //    var agency = GetContext().TBL_AGENCY.FirstOrDefault(x => x.ACTIVE == newUser.Active);

        //    if (agency != null)
        //    {

        //        //var newUser = new TBL_USER()

        //        //user.USER_ACTIVE = 1;

        //        agency.ACTIVE = 1;
        //        //GetContext().TBL_USER.Add(user);
        //        GetContext().SaveChanges();
        //        return RedirectToAction("Login", "Account");
        //        //return JavaScript("window.location ='/Account/Login'");
        //        //await TBL_USER
        //        //if (user.Email == Email)
        //        //{
        //        //    user.ConfirmedEmail = true;
        //        //    await UserManager.UpdateAsync(user);
        //        //    await SignInAsync(user, isPersistent: false);
        //        //}
        //        //else
        //        //{
        //        //return RedirectToAction("Confirm", "Account", new { Email = user.Email });
        //        //}
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //}
    }
}
