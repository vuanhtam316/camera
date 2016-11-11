using MyWeb.Areas.Agency.Models;
using MyWeb.Business;
using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Agency.Controllers
{
    public class UserController : ProviderControllerBase
    {
        //
        // GET: /Agency/User/
        [Authorize(Roles = "Agency")]
        #region index
        public ActionResult Index()
        {
            try
            {
                var model = new UserAgencyModel { UserModel = GetListUserInAgency() };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }
        #endregion
        #region create user
        public ActionResult CreateUser()
        {
            try
            {
                var model = new UserOfAgency();
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get CustomerModel - create category page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process CustomerModel - create category page");
            }
        }
        [HttpPost]
        [ActionName("CreateUser")]
        //[ValidateAntiForgeryToken]
        public PartialViewResult CreateUser(UserOfAgency model)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Tạo mới khách hàng thành công",
                MessageType = MesageModel.MessageType.Success
            };

            try
            {
                var mail = GetContext().TBL_USER.FirstOrDefault(x => x.EMAIL == model.Email);
                var str = model.Email.Split('@');
                if (mail != null)
                {
                    messageModel.MessageContent = "Email đã tồi tại";
                    messageModel.MessageType = MesageModel.MessageType.Error;
                    ViewBag.MessageCommand = messageModel;
                }
                else
                {
                    var en = new Entities1();
                    {
                        var encryptPass = Encryptor.Encrypt("123456");
                        var newUser = new TBL_USER()
                        {
                            // add new user
                            NAME = model.FullName,
                            DISPLAY_NAME = model.FullName,
                            EMAIL = model.Email,
                            PHONE = Convert.ToInt32(model.Phone),
                            USER_NAME = str[0],
                            PASSWORDS = encryptPass,
                            AGENCY_ID = GetUserLogin.CurentCustomerId,
                            USER_ACTIVE = 1,
                            REVIEW = 2
                        };
                        GetContext().TBL_USER.Add(newUser);
                        var userRole = new TBL_USER_ROLE()
                        {
                            USER_NAME = newUser.USER_NAME,
                            ROLE_NAME = "Custommer",
                            USER_ROLE_DESCRIPTION = null,
                            USER_ID = newUser.USER_ID,
                            ROLE_ID = 3
                        };
                        GetContext().TBL_USER_ROLE.Add(userRole);
                        //var callbackUrl = Url.Action(
                        //                       "ConfirmEmail", "User",
                        //                       new { },
                        //                       protocol: Request.Url.Scheme);

                        string st = string.Format("Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. </br>Tài khoản của bạn là: " + str[0] + ". </br> Mật khẩu là: 123456. </br> Mời bạn click vào Đăng Nhập để hoàn tất: <a href='http://nsc.negatech.vn/Account/Login'>Đăng nhập</a> </br> CHÚ Ý: Hãy thay đổi mật khẩu ngay sau khi đăng nhập!");
                        SendEmail(model.Email, st);
                        GetContext().SaveChanges();
                        ViewBag.MessageCommand = messageModel;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return PartialView("CreateUser");
        }
        #endregion
        #region Delete user of agency
        public PartialViewResult _ptvDeleteUserOfAgency(int userId)
        {
            try
            {
                var model = new UserOfAgency();
                var user = GetUserOfAgency(userId).FirstOrDefault(x => x.UserId == userId);
                model = user;
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera - Update Camera page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process Camera - Update Camera page");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ptvDeleteUserOfAgency(UserOfAgency model)
        {
            //var messageModel = new MesageModel.MessagesModel
            //{
            //    MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
            //    MessageType = MesageModel.MessageType.Error
            //};
            //ViewBag.MessageCommand = messageModel;
            try
            {
                var user = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == model.UserId);
                if (user != null)
                {
                    var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                    if (update.STATUS_UPDATE == 0)
                    {
                        update.STATUS_UPDATE = 1;
                    }
                    GetContext().TBL_USER.Remove(user);
                    GetContext().SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process update Camera item in master admin with erro: {0}", ex));
            }
            var userOfAgency = new UserAgencyModel { UserModel = GetListUserInAgency() };
            return PartialView("Index", userOfAgency);
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
        #region GetList User In Agency
        private IEnumerable<UserOfAgency> GetListUserInAgency()
        {
            try
            {
                var data = GetContext().TBL_USER.Where(x => x.AGENCY_ID == GetUserLogin.CurentCustomerId).ToList();
                var list = (from item in data
                            //let active = item.AGENCY_ID == GetUserLogin.CurentCustomerId
                            select new UserOfAgency
                            {
                                UserId = Convert.ToInt32(item.USER_ID),
                                FullName = item.NAME,
                                Username = item.USER_NAME,
                                Email = item.EMAIL,
                                Phone = "0" + Convert.ToString(item.PHONE),
                                TotalCamera = item.TBL_CAMERA.Count

                                //Active = active,
                                //Address = item.ADDRESS,
                                //CustomerNumber = item.ID_NUMBER,
                                //DisplayName = item.TBL_USER.DISPLAY_NAME,
                                //Email = item.TBL_USER.EMAIL,
                                //Password = item.TBL_USER.PASSWORDS,
                                //UserName = item.TBL_USER.USER_NAME,
                                //Id = item.TBL_USER.USER_ID,
                                //Phone = item.PHONE,
                                //Status = item.TBL_USER.USER_STATUS,
                                //FullName = item.FULL_NAME,
                                //Name = item.TBL_USER.NAME,
                                //TotalCamera = item.TBL_USER.TBL_CAMERA.Count
                            }).ToList();
                return list;

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Customer list with erro: {0}", ex));
                return null;
            }
        }
        #endregion
        #region get user of agency
        public List<UserOfAgency> GetUserOfAgency(int userid)
        {
            try
            {
                var data = GetContext().TBL_USER.Where(x => x.USER_ID == userid).ToList();
                var list = new List<UserOfAgency>();
                foreach (var item in data)
                {
                    var user = new UserOfAgency();
                    user.FullName = item.NAME;
                    user.UserId = Convert.ToInt32(item.USER_ID);
                    user.Email = item.EMAIL;
                    user.Username = item.USER_NAME;
                    list.Add(user);
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera list with erro: {0}", ex));
                return null;
            }
        }
        #endregion
        #region create agency
        public ActionResult CreateAgency()
        {
            try
            {
                var model = new AgencyModel();
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get CustomerModel - create category page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process CustomerModel - create category page");
            }
        }
        [HttpPost]
        [ActionName("CreateAgency")]
        //[ValidateAntiForgeryToken]
        public PartialViewResult CreateAgency(AgencyModel model)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Tạo mới đại lý thành công",
                MessageType = MesageModel.MessageType.Success
            };

            try
            {
                var mail = GetContext().TBL_USER.FirstOrDefault(x => x.EMAIL == model.Email);
                var str = model.Email.Split('@');
                if (mail != null)
                {
                    messageModel.MessageContent = "Email đã tồi tại";
                    messageModel.MessageType = MesageModel.MessageType.Error;
                    ViewBag.MessageCommand = messageModel;
                }
                else
                {
                    var en = new Entities1();
                    {
                        var encryptPass = Encryptor.Encrypt("123456");
                        var newUser = new TBL_USER()
                        {
                            // add new user
                            NAME = model.Agency_Name,
                            DISPLAY_NAME = model.Agency_Name,
                            EMAIL = model.Email,
                            PHONE = Convert.ToInt32(model.Phone),
                            REPRESENTATIVE = model.Representation,
                            ADDRESS = model.Address,
                            TAX_CODE = model.Tax_Code,
                            USER_NAME = str[0],
                            PASSWORDS = encryptPass,
                            AGENCY_ID = GetUserLogin.CurentCustomerId,
                            USER_ACTIVE = 1,
                            REVIEW = 2
                        };
                        GetContext().TBL_USER.Add(newUser);
                        var userRole = new TBL_USER_ROLE()
                        {
                            USER_NAME = newUser.USER_NAME,
                            ROLE_NAME = "Agency",
                            USER_ROLE_DESCRIPTION = null,
                            USER_ID = newUser.USER_ID,
                            ROLE_ID = 2
                        };
                        GetContext().TBL_USER_ROLE.Add(userRole);
                        //var callbackUrl = Url.Action(
                        //                       "ConfirmEmail", "User",
                        //                       new { },
                        //                       protocol: Request.Url.Scheme);

                        string st = string.Format("Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. </br>Tài khoản của bạn là: " + str[0] + ". </br> Mật khẩu là: 123456. </br> Mời bạn click vào Đăng Nhập để hoàn tất: <a href='http://nsc.negatech.vn/Account/Login'>Đăng nhập</a> </br> CHÚ Ý: Hãy thay đổi mật khẩu ngay sau khi đăng nhập!");
                        SendEmail(model.Email, st);
                        GetContext().SaveChanges();
                        ViewBag.MessageCommand = messageModel;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return PartialView("CreateAgency");
        }
        #endregion

    }
}
