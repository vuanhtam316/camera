using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MyWeb.Areas.Admin.Models;
using MyWeb.Areas.Front.Models;
using MyWeb.Business;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CameraController : ProviderControllerBase
    {
        //
        // GET: /Admin/Camera/

        #region Index
        public ActionResult Index(int id)
        {
            try
            {
                var model = new CameraAdminModel { CameraModels = GetCameraByUser(id), UserId = id, CustomerModel = GetInfoCustomer(id) };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get Camera in master admin");
            }
        }
        #endregion
        #region Create
        public ActionResult Create(int idUser)
        {
            try
            {
                var model = new CameraModel { USER_ID = idUser };
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera - create Camera page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process Camera - create Camera page");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CameraModel model)
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
                    var cam = new TBL_CAMERA
                    {
                        CAMERA_URL = model.CAMERA_URL,
                        CAMERA_NAME = model.CAMERA_NAME,
                        USER_ID = model.USER_ID,
                    };
                    GetContext().TBL_CAMERA.Add(cam);
                    GetContext().SaveChanges();
                    messageModel.MessageContent = "Thêm camera thành công";
                    messageModel.MessageType = MesageModel.MessageType.Success;
                }
                else
                {
                    Log.Info(String.Format("Model Camera is not valid"));
                }

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process insert Camera item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Update
        public PartialViewResult Edit(int idCamera)
        {
            try
            {
                var model = GetCamera().FirstOrDefault(x => x.CAMERA_ID == idCamera);
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
        public PartialViewResult Edit(CameraModel model)
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
                    var cam = GetContext().TBL_CAMERA.FirstOrDefault(x => x.CAMERA_ID == model.CAMERA_ID);
                    if (cam != null)
                    {
                        cam.CAMERA_NAME = model.CAMERA_NAME;
                        cam.CAMERA_URL = model.CAMERA_URL;
                        GetContext().SaveChanges();
                        messageModel.MessageContent = "Cập nhật camera thành công";
                        messageModel.MessageType = MesageModel.MessageType.Success;
                    }
                    else
                    {
                        Log.Error(String.Format("CameraID is null when update CameraItem in MA"));
                        messageModel.MessageContent = "Cập nhật thất bại vui lòng thử lại";
                        messageModel.MessageType = MesageModel.MessageType.Error;
                    }
                }
                else
                {
                    Log.Info(String.Format("Model Camera is not valid"));
                }

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process update Camera item in master admin with erro: {0}", ex));
            }
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

                var cam = GetContext().TBL_CAMERA.FirstOrDefault(x => x.CAMERA_ID == id);
                GetContext().TBL_CAMERA.Remove(cam);
                GetContext().SaveChanges();

                messageModel.MessageContent = "Xóa camera thành công";
                messageModel.MessageType = MesageModel.MessageType.Success;
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process delete Camera item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Search
        [HttpGet]
        public PartialViewResult SearchResult(int id)
        {
            IEnumerable<CameraModel> cam = GetCameraByUser(id);
            return PartialView("_ptvShow", cam);
        }
        #endregion
        #region Add from add fuction camera

        public PartialViewResult AddFunction(int idCamera)
        {
            var model = new AddFunctionModel { CameraId = idCamera, Functions = GetFunctionModel(idCamera) };
            return PartialView("_ptvAddFunction", model);
        }

        [HttpPost]
        public ActionResult CreateFunction(AddFunctionModel model)
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
                    foreach (var item in model.Functions)
                    {
                        var isExitCamStt = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.CAMERA_ID == model.CameraId && x.FUNCTION_ID == item.FunctionId);
                        if (isExitCamStt == null)//Nếu fuction chưa được add vào camera
                        {
                            if (item.Status == true)
                            {
                                var fc = new TBL_CAMERA_STATUS
                                {
                                    CAMERA_ID = model.CameraId,
                                    CAMERA_STATUS = 0,
                                    CAMERA_STATUS_DESCRIPTION = null,
                                    FUNCTION_ID = (short)item.FunctionId
                                };
                                GetContext().TBL_CAMERA_STATUS.Add(fc);
                                GetContext().SaveChanges();
                            }
                        }
                        else
                        {
                            if (item.Status == false)
                            {
                                GetContext().TBL_CAMERA_STATUS.Remove(isExitCamStt);
                                GetContext().SaveChanges();
                            }
                        }
                    }
                    messageModel.MessageContent = "Cập nhạt chức năng cho camera thành công";
                    messageModel.MessageType = MesageModel.MessageType.Success;
                }
                else
                {
                    Log.Info(String.Format("Model AddFunctionModel is not valid"));
                }

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process add function to camera in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Hepper
        private IEnumerable<CameraModel> GetCameraByUser(int userid)
        {
            try
            {
                var data = GetContext().TBL_CAMERA.Where(x => x.USER_ID == userid).ToList();
                return data.Select(item => new CameraModel
                {
                    CAMERA_ID = (int)item.CAMERA_ID,
                    //CAMERA_IP = item.CAMERA_IP,
                    CAMERA_NAME = item.CAMERA_NAME,
                    //CAMERA_NUMBER = item.CAMERA_NUMBER,
                    CAMERA_URL = item.CAMERA_URL,
                    //CAMERA_URL_STREAM
                    CAMERA_URL_STREAM = item.CAMERA_URL_STREAM,
                    USER_ID = userid
                }).ToList();

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera list with erro: {0}", ex));
                return null;
            }
        }
        private IEnumerable<CameraModel> GetCamera()
        {
            try
            {
                var data = GetContext().TBL_CAMERA.ToList();
                return data.Select(item => new CameraModel
                {
                    CAMERA_ID = (int)item.CAMERA_ID,
                    //CAMERA_IP = item.CAMERA_IP,
                    CAMERA_NAME = item.CAMERA_NAME,
                    //CAMERA_NUMBER = item.CAMERA_NUMBER,
                    CAMERA_URL = item.CAMERA_URL,
                    //CAMERA_URL_STREAM
                    CAMERA_URL_STREAM= item.CAMERA_URL_STREAM,

                    USER_ID = (int)item.USER_ID
                }).ToList();

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera list with erro: {0}", ex));
                return null;
            }
        }

        private CustomerModel GetInfoCustomer(int id)
        {
            try
            {
                var data = GetContext().TBL_PROFILE_USER.FirstOrDefault(x => x.USER_ID == id);
                var info = new CustomerModel();
                if (data == null) return info;
                info.Address = data.ADDRESS;
                info.FullName = data.FULL_NAME;
                info.CustomerNumber = data.ID_NUMBER;
                info.Email = data.TBL_USER.EMAIL;
                info.Phone = data.PHONE;
                info.Id = id;
                return info;

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Customer with erro: {0}", ex));
                return null;
            }
        }
        private List<SelectListItem> GetFunction()
        {
            try
            {
                var data = GetContext().TBL_FUNCTION.ToList();
                return data.Select(item => new SelectListItem
                {
                    Text = item.FUNCTION_NAME,
                    Value = item.FUNCTION_ID.ToString(CultureInfo.InvariantCulture)
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Function by with erro: {0}", ex));
                return null;
            }
        }

        private List<SelectListItem> GetFunctionByCamera(int cameraid)
        {
            try
            {
                var camsst = GetContext().TBL_CAMERA_STATUS.Where(x => x.CAMERA_ID == cameraid).Select(x => x.FUNCTION_ID).ToArray();
                var fc = GetContext().TBL_FUNCTION.Where(x => !camsst.Contains(x.FUNCTION_ID)).ToList();
                return fc.Select(item => new SelectListItem
                {
                    Text = item.FUNCTION_NAME,
                    Value = item.FUNCTION_ID.ToString(CultureInfo.InvariantCulture)
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get FunctionByCamera by with erro: {0}", ex));
                return null;
            }
        }
        private List<FunctionsModel> GetFunctionModel(int cameraid)
        {
            try
            {
                var camsst = GetContext().TBL_CAMERA_STATUS.Where(x => x.CAMERA_ID == cameraid).Select(x => x.FUNCTION_ID).ToArray();
                var fc = GetContext().TBL_FUNCTION.Where(x => !camsst.Contains(x.FUNCTION_ID)).OrderBy(x => x.FUNCTION_ID).ToList();
                var fc1 = GetContext().TBL_FUNCTION.Where(x => camsst.Contains(x.FUNCTION_ID)).OrderBy(x => x.FUNCTION_ID).ToList();
                var list = fc.Select(item1 => new FunctionsModel
                {
                    FunctionId = item1.FUNCTION_ID,
                    FunctionName = item1.FUNCTION_NAME,
                    Status = false
                }).ToList();
                list.AddRange(fc1.Select(item2 => new FunctionsModel
                {
                    FunctionId = item2.FUNCTION_ID,
                    FunctionName = item2.FUNCTION_NAME,
                    Status = true
                }));
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get FunctionModel with erro: {0}", ex));
                return null;
            }
        }
        #endregion

    }
}
