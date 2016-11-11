using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyWeb.Areas.Front.Models;
using MyWeb.Business;
using MyWeb.Models;
using System;
using MyWeb.Areas.Agency.Models;

namespace MyWeb.Areas.Front.Controllers
{
    [Authorize(Roles = "Custommer")]
    public class PartialViewController : ProviderControllerBase
    {
        //
        // GET: /Front/PartialView/
        private const int keyId = 11012253;

        public PartialViewResult TabListVideo()
        {
            List<CameraModel> model = GetListCamera().Take(4).ToList();
            return PartialView(model);
        }
        public PartialViewResult TabLiveStream()
        {
            return PartialView();
        }

        public PartialViewResult TabReport()
        {
            var total = GetTotalFileOfCamera();
            return PartialView(total);
        }

        public PartialViewResult TabHistoryCamera()
        {
            var model = new HistoryModel
            {
                ListCameras = GetListCamera(),
            };
            return PartialView(model);
        }
        public PartialViewResult TabConfig()
        {
            var model = GetListCamera();
            return PartialView(model);
        }
        public PartialViewResult TabAppStore()
        {
            List<FunctionsModel> model = GetListFunction().ToList();
            return PartialView(model);
        }
        public PartialViewResult TabManageCamera()
        {
            var model = GetListCamera();
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult GetModalLiveView(string Imageurl)
        {
            var model = new ImagesHistory { ImageUrl = Imageurl };
            return PartialView("_ptvViewVideo", model);
        }
        public ActionResult GetModalLiveStream(string url_stream, int camid)
        {
            var model = new CameraModel { CAMERA_ID = camid + keyId, CAMERA_URL_STREAM = url_stream };
            return PartialView("_ptvViewVideo", model);
        }

        public ActionResult AgencyReView()
        {
            var model = new GetUser { Getuser = GetUser() };
            return PartialView("SetUp", model);
        }
        private List<UserModel> GetUser()
        {
            try
            {
                var list = new List<UserModel>();
                var data = GetContext().TBL_USER.Where(x => x.USER_ID == GetUserLogin.CurentCustomerId).ToList();
                foreach (var item in data)
                {

                    var user = new UserModel();
                    user.REVIEW = Convert.ToInt32(item.REVIEW);
                    //user.FullName = item.NAME;
                    //user.Title = i.TITLE;
                    //user.Content = i.FEEDBACK_CONTENT;
                    //user.Time = i.FEEDBACK_TIME;
                    list.Add(user);
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Customer list with erro: {0}", ex));
                return null;
            }
        }

        #region Search
        [HttpGet]
        public PartialViewResult SearchResult()
        {
            IEnumerable<CameraModel> cam = GetListCamera();
            return PartialView("_ptvShow", cam);
        }
        #endregion
        #region Search2
        [HttpGet]
        public PartialViewResult ListCamera()
        {
            IEnumerable<CameraModel> cam = GetListCamera();
            return PartialView("_ptvShowCamera", cam);
        }
        #endregion
        #region Hepper
        [HttpPost]
        public ActionResult SelectMode(string values)
        {
            var model = new List<CameraModel>();
            if (!string.IsNullOrEmpty(values))
            {
                model = GetListCamera().Take(int.Parse(values)).ToList();
            }
            return PartialView("ptvShowLiveCam", model);
        }
        #endregion


        #region list function
        [HttpGet]
        public PartialViewResult ListFunction()
        {
            IEnumerable<FunctionsModel> func = GetListFunction();
            return PartialView("_ptvShowFunc", func);
        }
        #endregion
        public ActionResult OnSetUp()
        {
            var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);
            if (data != null)
            {
                data.REVIEW = 2;
                GetContext().SaveChanges();
            }
            var model = new GetUser { Getuser = GetUser() };
            return PartialView("SetUp", model);
        }
        public ActionResult OffSetUp()
        {
            var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);
            if (data != null)
            {
                data.REVIEW = 1;
                GetContext().SaveChanges();
            }
            var model = new GetUser { Getuser = GetUser() };
            return PartialView("SetUp", model);
        }
        public ActionResult LogoOfAgency()
        {
            try
            {
                //var model = new GetAgencyModel { getAgency = GetAgency() };
                var model = new AgencyModel();
                var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);
                if (data.AGENCY_ID != null)
                {
                    var agency = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == data.AGENCY_ID);
                    if (agency.IMAGE_URL != null)
                    {
                        model.avatar = "http://nsc3.negatech.vn/" + agency.IMAGE_URL;
                    }
                    else
                    {
                        model.avatar = "http://nsc3.negatech.vn/Avatar/logo.png";
                    }
                    model.Agency_Name = agency.NAME;
                    return PartialView("_ptvLogoOfAgency", model);
                }
                else
                    return PartialView("_ptvLogoOfAgency", model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }


        //public ActionResult ManageCamera()
        // {
        //     return PartialView("TabManageCamera");
        // }


        //    public ActionResult AddCamera(AddCameraModel cam)
        //    {
        //        ViewBag.MessageCommand = "";
        //        var messageModel = new MesageModel.MessagesModel
        //        {
        //            MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
        //            MessageType = MesageModel.MessageType.Error
        //        };
        //        ViewBag.MessageCommand = messageModel;
        //        try
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                var userid = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);

        //                if (userid != null)
        //                {
        //                    var newCamera = new TBL_CAMERA()
        //                    {

        //                        // add new camera
        //                        CAMERA_NAME = cam.CAMERA_NAME,
        //                        CAMERA_NUMBER = cam.CAMERA_NUMBER,
        //                        CAMERA_IP = cam.CAMERA_IP,
        //                        CAMERA_URL = cam.CAMERA_URL,
        //                        CAMERA_URL_STREAM = cam.CAMERA_URL_STREAM,
        //                        USER_ID = GetUserLogin.CurentCustomerId

        //                    };
        //                    GetContext().TBL_CAMERA.Add(newCamera);
        //                    GetContext().SaveChanges();
        //                }
        //                messageModel.MessageContent = "Thêm camera thành công";
        //                messageModel.MessageType = MesageModel.MessageType.Success;
        //            }
        //            else
        //            {
        //                //Log.Info(String.Format(""));
        //            }
        //        }
        //        catch
        //        {

        //        }
        //        var model = GetListCamera();
        //        return PartialView("TabManageCamera", model);
        //        //return View("_ptvShowCamera", model);
        //    }


    }
}
