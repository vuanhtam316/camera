using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

using MyWeb.Areas.Admin.Models;
using MyWeb.Business;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CustomerController : ProviderControllerBase
    {
        //
        // GET: /Admin/Customer/

        #region Index
        public ActionResult Index()
        {
            try
            {
                var model = new CustomerAdminModel { CustomerModel = GetListCustomer() };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }
        #endregion
        #region Create
        public ActionResult Create()
        {
            try
            {
                var model = new CustomerModel();
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get CustomerModel - create category page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process CustomerModel - create category page");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult Create(CustomerModel model)
        {
            //using (var transaction = new TransactionScope())
            //{

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
                    var isExitUsername = GetContext().TBL_USER.FirstOrDefault(x => x.USER_NAME == model.UserName);
                    var isExitCustomNumber = GetContext().TBL_PROFILE_USER.FirstOrDefault(x => x.ID_NUMBER == model.CustomerNumber);
                    if (isExitUsername != null || isExitCustomNumber != null)
                    {
                        messageModel.MessageContent = "Kiểm tra username hoặc mã khách hàng đã tồn tại";
                        messageModel.MessageType = MesageModel.MessageType.Warning;
                        return PartialView("_Message", ViewBag.MessageCommand);
                    }
                    var useinfo=new GetUserLogin();
                    var pass = Encryptor.Encrypt(model.Password);
                    var us = new TBL_USER()
                    {
                        CREATE_BY = useinfo.CurrentUserName,
                        DISPLAY_NAME = model.DisplayName,
                        EMAIL = model.Email,
                        NAME = model.Name,
                        USER_NAME = model.UserName,
                        PASSWORDS = pass,
                        USER_STATUS = model.Status,
                        USER_ACTIVE = (short)(model.Active == true ? 1 : 0),
                    };

                    GetContext().TBL_USER.Add(us);
                    GetContext().SaveChanges();

                    //if (insertUs == null)
                    //{
                    //    //transaction.Dispose();
                    //    messageModel.MessageContent = "Không thể tạo user";
                    //    messageModel.MessageType = MesageModel.MessageType.Error;
                    //    Log.Info(String.Format("Could not create User"));
                    //    return PartialView("_Message", ViewBag.MessageCommand);
                    //}


                    //insert user detail
                    var usDetail = new TBL_PROFILE_USER
                    {
                        FULL_NAME = model.FullName,
                        ADDRESS = model.Address,
                        CITY = null,
                        DISTRICT = null,
                        BIRTHDAY = null,
                        SEX = (short)(model.Sex == true ? 1 : 0),
                        PHONE = model.Phone,
                        MOBILE = null,
                        ID_NUMBER = model.CustomerNumber,
                        USER_ID = us.USER_ID
                    };


                    GetContext().TBL_PROFILE_USER.Add(usDetail);
                    GetContext().SaveChanges();
                    messageModel.MessageContent = "Thêm user thành công";
                    messageModel.MessageType = MesageModel.MessageType.Success;
                }
                else
                {
                    Log.Info(String.Format("Model CustomerModel is not valid"));
                }

            }
            catch (Exception ex)
            {
                //transaction.Dispose();
                Log.Error(String.Format("An error occurred in the process insert Customer item in master admin with erro: {0}", ex));
            }
            //}
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Delete

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delelte(int id)
        {
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;
            try
            {
                var us = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == id);
                GetContext().TBL_USER.Remove(us);
                GetContext().SaveChanges();

                messageModel.MessageContent = "Xóa khách hàng thành công";
                messageModel.MessageType = MesageModel.MessageType.Success;
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process delete Customer item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Search
        [HttpGet]
        public PartialViewResult SearchResult(CustomerAdminModel model)
        {
            IEnumerable<CustomerModel> cus = GetListCustomer();
            model.CustomerModel = cus;
            return PartialView("_ptvShow", model.CustomerModel);
        }
        #endregion
        #region Hepper
        private IEnumerable<CustomerModel> GetListCustomer()
        {
            try
            {
                var data = GetContext().TBL_PROFILE_USER.ToList();
                var list = (from item in data
                            let active = item.TBL_USER.USER_ACTIVE == 1
                            select new CustomerModel
                            {
                                Active = active,
                                Address = item.ADDRESS,
                                CustomerNumber = item.ID_NUMBER,
                                DisplayName = item.TBL_USER.DISPLAY_NAME,
                                Email = item.TBL_USER.EMAIL,
                                Password = item.TBL_USER.PASSWORDS,
                                UserName = item.TBL_USER.USER_NAME,
                                Id = item.TBL_USER.USER_ID,
                                Phone = item.PHONE,
                                Status = item.TBL_USER.USER_STATUS,
                                FullName = item.FULL_NAME,
                                Name = item.TBL_USER.NAME,
                                TotalCamera = item.TBL_USER.TBL_CAMERA.Count
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

    }
}
