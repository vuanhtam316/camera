using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using MyWeb.Areas.Front.Models;
using MyWeb.Business;
using MyWeb.Models;
using System.Net;
using System.Web.UI;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace MyWeb.Areas.Front.Controllers
{
    [Authorize(Roles = "Custommer")]
    public class HomeController : ProviderControllerBase
    {
        //
        // GET: /Front/Index/
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetDetailImage(int id)
        {
            var src = "";
            var url = GetContext().TBL_HISTORY_IMAGES.FirstOrDefault(x => x.HISTORYIMAGES_ID == id);
            if (url != null)
            {
                src = url.HISTORYIMAGES_URL;
            }
            return Json(src);
        }
        [HttpPost]
        public ActionResult GetDetailCameraImage(int id)
        {
            int total = 0;
            var model1 = GetHistoyImageModels(id, null, null, null, out total, 1, RowPerPage);
            ViewBag.total = total;
            var model2 = new Function { GetListItems = GetFunction(id) };
            var model3 = GetListCamera().FirstOrDefault(x => x.CAMERA_ID == id);
            var model4 = GetDateTime(id, null);
            var listImage = RenderRazorViewToString(this.ControllerContext, "_ptvImageByCamera", model1);
            var listFunction = RenderRazorViewToString(this.ControllerContext, "_ptvFunction", model2);
            var listVideo = RenderRazorViewToString(this.ControllerContext, "_ptvVideoByImage", model3);
            var listDatetime = RenderRazorViewToString(this.ControllerContext, "_ptvDateTime", model4);
            var json = Json(new { listImage, listFunction, listVideo, listDatetime });
            //return PartialView("_ptvImageByCamera",model);
            return json;
        }

        [HttpPost]
        public ActionResult GetCameraDetail(int idCamera, int viewmode)
        {
            var model1 = GetImageHistory(idCamera);
            var model2 = new Function { GetListItems = GetFunction(idCamera) };
            var model3 = GetListCamera().Where(x => x.CAMERA_ID == idCamera);
            var listImage = RenderRazorViewToString(this.ControllerContext, "_ptvImageByCamera", model1);
            var listFunction = RenderRazorViewToString(this.ControllerContext, "_ptvFunction", model2);
            var listVideo = RenderRazorViewToString(this.ControllerContext, "_ptvVideoByCamera", model3);
            var json = viewmode == 1 ? Json(new { listImage, listFunction }) : Json(new { listVideo, listFunction });
            //return PartialView("_ptvImageByCamera",model);
            return json;
        }
        [HttpPost]
        public ActionResult GetDetail(int idHistoryImg)
        {
            string srcImg = "";
            string srcVideo = "";
            double paraS = 0;
            double paraE = 0;
            var tblHistoryImages = GetContext().TBL_HISTORY_IMAGES.FirstOrDefault(x => x.HISTORYIMAGES_ID == idHistoryImg && x.USER_ID == GetUserLogin.CurentCustomerId);
            if (tblHistoryImages != null)
            {
                srcImg = "http://nsc3.negatech.vn/" + tblHistoryImages.HISTORYIMAGES_URL;
                //var data = GetContext().TBL_HISTORY_VIDEO.FirstOrDefault(x => x.HISTORYVIDEO_TIMESTART <= tblHistoryImages.HISTORYIMAGES_TIMESTART && x.HISTORYVIDEO_TIMEEND >= tblHistoryImages.HISTORYIMAGES_TIMESTART);
                string videoUrl = @"select * from TBL_HISTORY_VIDEO video, TBL_HISTORY_IMAGES img
                                where img.HISTORYIMAGES_TIMESTART>=video.HISTORYVIDEO_TIMESTART
                                and img.HISTORYIMAGES_TIMESTART<=video.HISTORYVIDEO_TIMEEND
                                and img.USER_ID=video.USER_ID
                                and img.CAMERA_ID=video.CAMERA_ID and img.HISTORYIMAGES_ID=:1";
                var ivideo =
                    GetContext()
                        .TBL_HISTORY_VIDEO.Where(
                            x => x.CAMERA_ID == tblHistoryImages.CAMERA_ID && x.USER_ID == tblHistoryImages.USER_ID &&
                                 x.HISTORYVIDEO_TIMESTART <= tblHistoryImages.HISTORYIMAGES_TIMESTART &&
                                 tblHistoryImages.HISTORYIMAGES_TIMESTART < x.HISTORYVIDEO_TIMEEND).SingleOrDefault();
                if (ivideo != null)
                {
                    srcVideo = "http://nsc3.negatech.vn/" + ivideo.HISTORYVIDEO_URL;
                    DateTime infoVideo_S;
                    DateTime infoVideo_E;
                    DateTime infoVideo_detect_S;
                    DateTime infoVideo_detect_E;

                    if (ivideo.HISTORYVIDEO_TIMESTART != null && ivideo.HISTORYVIDEO_TIMEEND != null)
                    {
                        infoVideo_S = (DateTime)ivideo.HISTORYVIDEO_TIMESTART;
                        infoVideo_E = (DateTime)ivideo.HISTORYVIDEO_TIMEEND;
                        infoVideo_detect_S = (DateTime)tblHistoryImages.HISTORYIMAGES_TIMESTART;
                        infoVideo_detect_E = (DateTime)tblHistoryImages.HISTORYIMAGES_TIMESTART;
                        paraS = (infoVideo_detect_S - infoVideo_S).TotalSeconds /
                                    (infoVideo_E - infoVideo_S).TotalSeconds;
                        paraE = (infoVideo_detect_E - infoVideo_S).TotalSeconds /
                                     (infoVideo_E - infoVideo_S).TotalSeconds;
                    }

                }
                //var video = GetContext().Database.SqlQuery<string>(videoUrl, idHistoryImg).FirstOrDefault();
                //srcVideo = "../Uploads/Video/2016/03/03/quatnv/QuocTuGiam01/2016030314000173.mp4";
                //if (video != null)
                //    srcVideo = "http://nsc3.negatech.vn/" + video;


            }
            var result = new { ImageSrc = srcImg, VideoSrc = srcVideo, paras = paraS, parae = paraE };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Filter
        [HttpGet]
        public ActionResult Search(int id, int? day, int? month, int? functionid, int page = 1, int rowperpage = RowPerPage)//
        {
            int total = 0;
            var model = GetHistoyImageModels(id, day, month, functionid, out total, page, rowperpage);//
            //ViewData["page"] = page;
            ViewBag.total = total;
            return PartialView("_ptvShowImages", model);
        }

        public string convertString(string s)
        {
            //string st;
            if (s == "0")
            {
                return (s = ("0" + s));
            }
            if (s == "1")
            {
                return (s = ("0" + s));
            }
            if (s == "2")
            {
                return (s = ("0" + s));
            }
            if (s == "3")
            {
                return (s = ("0" + s));
            }
            if (s == "4")
            {
                return (s = ("0" + s));
            }
            if (s == "5")
            {
                return (s = ("0" + s));
            }
            if (s == "6")
            {
                return (s = ("0" + s));
            }
            if (s == "7")
            {
                return (s = ("0" + s));
            }
            if (s == "8")
            {
                return (s = ("0" + s));
            }
            if (s == "9")
            {
                return (s = ("0" + s));
            }
            return s;
        }




        [HttpGet]
        public ActionResult SearchFromTo(int id, string fromTime, string toTime, string fromDate, int? functionid, int page = 1, int rowperpage = RowPerPage)//
        {
            int total = 0;
            //DateTime? refromDate = null;
            //DateTime? retoDate = null;
            //DateTime? todate = null;


            DateTime? refromTime = null;
            DateTime? retoTime = null;


            //fromtime = Convert.ToDateTime(fromtime);
            //DateTime fromtime = Convert.ToDateTime(fromTime);


            //DateTime d1 = Convert.ToDateTime(fromDate);

            //DateTime dt2 = DateTime.ParseExact(fromtime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            //dt2.ToString("dd-MMM-yy HH.mm");
            //string DT2 = Convert.ToString(dt2);
            //dt2.ToString();
            //var totime = fromDate + toTime + ":59";
            //DateTime dt3 = DateTime.ParseExact(totime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            //dt3.ToString("dd-MMM-yy HH.mm");
            //retoTime = dt3.AddMinutes(59);
            //dt3.ToString("HH");
            //string DT3 = Convert.ToString(dt3);

            if (!string.IsNullOrEmpty(fromDate))
            {
                //if (!string.IsNullOrEmpty(fromTime))
                //{
                var fromtime = fromDate + convertString(fromTime) + ":00";
                refromTime = DateTime.ParseExact(fromtime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                //refromTime = DateTime.ParseExact(DT2, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                //}
                //if (!string.IsNullOrEmpty(toTime))
                //{

                if (toTime == "24")
                {
                    var totime = fromDate + "23:59:59";
                    retoTime = DateTime.ParseExact(totime, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                }
                else
                {
                    var totime = fromDate + convertString(toTime) + ":00";
                    retoTime = DateTime.ParseExact(totime, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                }
                var model = SearchImageModels(id, refromTime, retoTime, functionid, out total, page, rowperpage);//, page, rowperpage,
                //ViewData["page"] = page;
                ViewBag.total = total;
                return PartialView("_ptvShowImages", model);
                //refromTime = DateTime.ParseExact(DT2, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                //}
            }
            else
            {
                var model = GetHistoyImageModels(id, null, null, null, out total, page, rowperpage);//
                //ViewData["page"] = page;
                ViewBag.total = total;
                return PartialView("_ptvShowImages", model);
            }
            ////refromTime.ToString("HH:mm");
            //if (!string.IsNullOrEmpty(toTime))
            //    retoTime = DateTime.ParseExact(DT3, "dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
            //retoTime.
            //if (!string.IsNullOrEmpty(fromDate))
            //    refromDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy ", System.Globalization.CultureInfo.InvariantCulture);
            //if (!string.IsNullOrEmpty(fromDate))
            //    retoDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy ", System.Globalization.CultureInfo.InvariantCulture);
            //todate = retoDate.Value.AddDays(1);

            //var model = SearchImageModels(id, refromDate, retoDate, functionid, page, rowperpage, out total);


        }
        [HttpGet]
        public ActionResult SearchAndDateTime(int id, int? day, int? month, int? functionid, int page = 1, int rowperpage = RowPerPage)//
        {
            int total = 0;
            var model = GetHistoyImageModels(id, day, month, functionid, out total, page, rowperpage);//
            var modelDate = GetDateTime(id, functionid);
            //ViewData["page"] = page;
            ViewBag.total = total;
            var listImage = RenderRazorViewToString(this.ControllerContext, "_ptvShowImages", model);
            var listDatetime = RenderRazorViewToString(this.ControllerContext, "_ptvDateTime", modelDate);
            var json = Json(new { listImage, listDatetime });
            return json;
        }
        #endregion
        string name = RandomString();
        #region add camera

        public ActionResult AddCamera()
        {
            try
            {
                var model = new AddCameraModel();
                return View("AddNewCamera", model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera - create Camera page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process Camera - create Camera page");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> AddCamera(AddCameraModel model)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;
            string s1 = model.Cameramodel.MODEL_CAMERA;
            string url = "";
            switch (s1)
            {
                case "Foscam":
                    url = "rtsp://" + model.Cameramodel.USERNAME_CAMERA + ":" + model.Cameramodel.PASSWORD_CAMERA + "@" + model.Cameramodel.IP_CAMERA + ":" + model.Cameramodel.PORT_RTSP + "/videoMain";
                    await AddStream(name, url);
                    break;
                case "Tiandy":
                    url = "rtsp://" + model.Cameramodel.USERNAME_CAMERA + ":" + model.Cameramodel.PASSWORD_CAMERA + "@" + model.Cameramodel.IP_CAMERA + ":" + model.Cameramodel.PORT_RTSP + "/1/m.c.str.(normal)";
                    await AddStream(name, url);
                    break;
                case "Vantech":
                    url = "rtsp://" + model.Cameramodel.IP_CAMERA + ":" + model.Cameramodel.PORT_RTSP + "/user=" + model.Cameramodel.USERNAME_CAMERA + "_password=" + model.Cameramodel.PASSWORD_CAMERA + "_channel=1_stream=0.sdp";
                    await AddStream(name, url);
                    break;
                case "DaHua":
                    url = "rtsp://" + model.Cameramodel.USERNAME_CAMERA + ":" + model.Cameramodel.PASSWORD_CAMERA + "@" + model.Cameramodel.IP_CAMERA + ":" + model.Cameramodel.PORT_RTSP + "/live";
                    await AddStream(name, url);
                    break;
                case "Hikvision":
                    url = "rtsp://" + model.Cameramodel.USERNAME_CAMERA + ":" + model.Cameramodel.PASSWORD_CAMERA + "@" + model.Cameramodel.IP_CAMERA + ":" + model.Cameramodel.PORT_RTSP + "/Streaming/Channels/1";
                    await AddStream(name, url);
                    break;
                default:
                    break;
            }
            ViewBag.Url = url;
            try
            {
                if (ModelState.IsValid)
                {
                    var userid = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);
                    if (userid != null)
                    {
                        var camera = GetContext().TBL_CAMERA.FirstOrDefault(x => x.CAMERA_NAME == model.Cameramodel.CAMERA_NAME);
                        if (camera == null)
                        {
                            string s = "http://10.62.1.152:9999/viewstatus?name=" + name;
                            var client = new HttpClient();
                            HttpResponseMessage response = await client.GetAsync(s);
                            response.EnsureSuccessStatusCode();
                            var result = await response.Content.ReadAsStringAsync();
                            if (result == "isConnected:False")
                            {
                                for (int i = 0; i <= 30; i++)
                                {
                                    HttpResponseMessage resdata = await client.GetAsync(s);
                                    resdata.EnsureSuccessStatusCode();
                                    var status = await resdata.Content.ReadAsStringAsync();
                                    if (status == "isConnected:True")
                                    {
                                        var cam = new TBL_CAMERA();
                                        cam.CAMERA_NAME = model.Cameramodel.CAMERA_NAME;
                                        cam.CAMERA_URL = url;
                                        cam.CAMERA_URL_STREAM = name;
                                        cam.USER_ID = GetUserLogin.CurentCustomerId;
                                        GetContext().TBL_CAMERA.Add(cam);
                                        GetContext().SaveChanges();

                                        messageModel.MessageContent = "Thêm camera thành công";
                                        messageModel.MessageType = MesageModel.MessageType.Success;
                                        break;
                                    }
                                    else
                                    {
                                        messageModel.MessageContent = "Đang kết nối.";
                                        messageModel.MessageType = MesageModel.MessageType.Error;
                                    }
                                }
                            }
                            else
                            {
                                var cam = new TBL_CAMERA();
                                cam.CAMERA_NAME = model.Cameramodel.CAMERA_NAME;
                                cam.CAMERA_URL = url;
                                cam.CAMERA_URL_STREAM = name;
                                cam.USER_ID = GetUserLogin.CurentCustomerId;
                                GetContext().TBL_CAMERA.Add(cam);
                                GetContext().SaveChanges();
                                messageModel.MessageContent = "Thêm camera thành công";
                                messageModel.MessageType = MesageModel.MessageType.Success;
                            }
                        }
                        else
                        {
                            messageModel.MessageContent = "Camera đã tồn tại";
                            messageModel.MessageType = MesageModel.MessageType.Info;
                            await DeleteStream(name);
                        }
                    }
                }
                else
                {
                    Log.Info(String.Format("Model Camera is not valid"));
                }
                if (messageModel.MessageContent == "Đang kết nối.")
                {
                    messageModel.MessageContent = "Kiểm tra lại thông tin của camera.";
                    messageModel.MessageType = MesageModel.MessageType.Error;
                    await DeleteStream(name);
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process insert Camera item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region delete camera
        public PartialViewResult DeleteCamera(int idCamera)
        {
            try
            {
                var model = new DeleteCameraModel();
                var camera = GetListCamera().FirstOrDefault(x => x.CAMERA_ID == idCamera);
                model.CameraModel = camera;
                //model.FunctionsModel = GetFunctionByCamera(idCamera);
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
        public async Task<PartialViewResult> DeleteCamera(DeleteCameraModel model)
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
                //ModelState["CameraModel.CAMERA_URL"].Errors.Clear();
                //if (ModelState.IsValid)
                //{
                string s = "http://10.62.1.152:9999/onstream?name=" + model.CameraModel.CAMERA_URL_STREAM;
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(s);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();

                var cam = GetContext().TBL_CAMERA.FirstOrDefault(x => x.CAMERA_ID == model.CameraModel.CAMERA_ID);
                if (cam != null && result != null)
                {
                    await DeleteStream(model.CameraModel.CAMERA_URL_STREAM);
                    var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                    if (update.STATUS_UPDATE == 0)
                    {
                        update.STATUS_UPDATE = 1;
                    }
                    GetContext().TBL_CAMERA.Remove(cam);
                    GetContext().SaveChanges();
                }
                else
                {

                    GetContext().TBL_CAMERA.Remove(cam);
                    GetContext().SaveChanges();

                    //Log.Error(String.Format(""));
                    //messageModel.MessageContent = "";
                    //messageModel.MessageType = MesageModel.MessageType.Error;
                }
                //}
                //else
                //{
                //    Log.Info(String.Format("Model Camera is not valid"));
                //}
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process update Camera item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        //[AcceptVerbs(HttpVerbs.Post)]
        //public ActionResult DeleteCamera(int idCamera)
        //{
        //    var model = new DeleteCameraModel();
        //    var camera = GetListCamera().FirstOrDefault(x => x.CAMERA_ID == idCamera);
        //    model.CameraModel = camera;

        //    var camid = GetContext().TBL_CAMERA.FirstOrDefault(x => x.CAMERA_ID == model.CameraModel.CAMERA_ID && x.USER_ID == GetUserLogin.CurentCustomerId);
        //    if (camid != null)

        //        GetContext().TBL_CAMERA.Remove(camid);
        //    GetContext().SaveChanges();
        //    return View("_ptvShowCamera", camera);
        //}

        #endregion
        #region update
        public PartialViewResult Edit(int idCamera)
        {
            try
            {
                var model = new UpdateCameraModel();
                var camera = GetListCamera().FirstOrDefault(x => x.CAMERA_ID == idCamera);
                model.CameraModel = camera;
                model.FunctionsModel = GetFunctionByCamera(idCamera);
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
        public async Task<PartialViewResult> Edit(UpdateCameraModel model)
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
                //ModelState["CameraModel.CAMERA_URL"].Errors.Clear();
                //if (ModelState.IsValid)
                //{
                var cam = GetContext().TBL_CAMERA.FirstOrDefault(x => x.CAMERA_ID == model.CameraModel.CAMERA_ID && x.USER_ID == GetUserLogin.CurentCustomerId);
                if (cam != null)
                {
                    //string s = "rtsp://" + model.CameraModel.USERNAME_CAMERA + ":" + model.CameraModel.PASSWORD_CAMERA + "@" + model.CameraModel.IP_CAMERA + ":" + model.CameraModel.PORT_RTSP + "/videoMain";
                    //string updatestream = await UpdateStream(model.CameraModel.CAMERA_URL_STREAM, s);
                    //string checkBandwidth = await CheckBandwidth(model.CameraModel.CAMERA_URL_STREAM);
                    //if (checkBandwidth != "0")
                    //{
                    cam.CAMERA_NAME = model.CameraModel.CAMERA_NAME;
                    GetContext().SaveChanges();
                    //update camera status
                    if (model.FunctionsModel != null && model.FunctionsModel.Any())
                    {
                        foreach (var item in model.FunctionsModel)
                        {
                            var stt = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.CAMERA_ID == model.CameraModel.CAMERA_ID && x.FUNCTION_ID == item.FunctionId);
                            if (stt != null)
                            {
                                stt.CAMERA_STATUS = (short?)(item.Status == true ? 1 : 0);

                                //    stt.CAMERA_STATUS = 1;
                                //stt.CAMERA_STATUS_TIME_START = item.Start;
                                //stt.CAMERA_STATUS_TIME_END = item.End;
                                if (item.Start != null) stt.CAMERA_STATUS_TIME_START = item.Start;
                                if (item.End != null) stt.CAMERA_STATUS_TIME_END = item.End;
                                var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                                if (update.STATUS_UPDATE == 0)
                                {
                                    update.STATUS_UPDATE = 1;
                                }
                                //stt.UPDATE_STATUS = 1;
                            }
                            GetContext().SaveChanges();
                        }
                    }
                    messageModel.MessageContent = "Cập nhật tên camera thành công";
                    messageModel.MessageType = MesageModel.MessageType.Success;
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
        #region Hepper
        private IEnumerable<ImagesHistory> GetImageHistory(int cameraid)
        {
            try
            {
                var data = GetContext().TBL_HISTORY_IMAGES.Where(x => x.USER_ID == GetUserLogin.CurentCustomerId && x.CAMERA_ID == cameraid).ToList();
                return data.Select(item => new ImagesHistory
                {
                    CameraId = (int)item.CAMERA_ID,
                    ImageName = item.HISTORYIMAGES_INFO,
                    ImageUrl = HostData + item.HISTORYIMAGES_URL,
                    ImageId = (int)item.HISTORYIMAGES_ID,
                    TimeStart = item.HISTORYIMAGES_TIMESTART,
                    TimeEnd = item.HISTORYIMAGES_TIMEEND,
                    UserId = GetUserLogin.CurentCustomerId
                }).ToList();

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera list with erro: {0}", ex));
                return null;
            }
        }

        private List<FunctionsModel> GetFunctionByCamera(int cameraId)
        {
            try
            {
                var data = GetContext().TBL_CAMERA_STATUS.Where(x => x.CAMERA_ID == cameraId && x.FUNCTION_REGIGES == 1).ToList();
                ViewBag.url = "http://nsc3.negatech.vn/" + getImgFunction(cameraId);
                ViewBag.cameraid = cameraId;
                return data.Select(item => new FunctionsModel
                {
                    FunctionId = item.FUNCTION_ID,
                    FunctionName = item.TBL_FUNCTION.FUNCTION_NAME,
                    Status = item.CAMERA_STATUS == 1,
                    Start = item.CAMERA_STATUS_TIME_START,
                    End = item.CAMERA_STATUS_TIME_END,
                    FunctionImage = ViewBag.url
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Function by cameraId with erro: {0}", ex));
                return null;
            }
        }
        private List<SelectListItem> GetFunction(int cameraId)
        {
            try
            {
                var data = GetContext().TBL_CAMERA_STATUS.Where(x => x.CAMERA_ID == cameraId && x.FUNCTION_REGIGES == 1 && x.TBL_FUNCTION.PROCESS == 1).ToList();
                return data.Select(item => new SelectListItem
                {
                    Text = item.TBL_FUNCTION.FUNCTION_NAME,
                    Value = item.FUNCTION_ID.ToString(CultureInfo.InvariantCulture)
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Function by cameraId with erro: {0}", ex));
                return null;
            }
        }

        private DateTimeModel GetDateTime(int cameraId, int? functionId)
        {
            //var dicNgay = new Dictionary<string, string>();
            var dateTimeModel = new DateTimeModel();
            var listDate = new List<DataDate>();
            var listDay = new List<SelectDay>();
            var data = GetImagesHistory(cameraId, functionId);
            var imagesHistories = data as List<HistoryImageModel> ?? data.ToList();


            for (var i = 1; i <= MonthConst; i++)
            {
                var item = new DataDate();
                if (imagesHistories.Count(x => x.MONTHTIMESTART == i) <= 0) continue;
                item.ExitData = true;
                item.Month = i;
                for (var j = 1; j <= DayConst; j++)
                {
                    if (imagesHistories.Count(x => x.DAYTIMESTART == j && x.MONTHTIMESTART == i) > 0)
                    {
                        listDay.Add(new SelectDay { Days = j, TextDay = "Ngày " + j + " tháng " + i });
                    }
                }
                item.Day = listDay;
                listDay = new List<SelectDay>();
                listDate.Add(item);
            }
            dateTimeModel.DataDates = listDate;
            return dateTimeModel;
        }

        private void UpdateHistoryImg()
        {
            var data = GetContext().TBL_HISTORY_IMAGES.ToList();
            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.HISTORYIMAGES_URL))
                {
                    var img = item.HISTORYIMAGES_URL.Substring(22);
                    item.HISTORYIMAGES_URL = img;
                    GetContext().SaveChanges();
                }
            }
        }
        #region Pagging

        private List<ImagesHistory> GetHistoryImagePagging(int cameraid, int numPerPage, int page, out int total)
        {
            try
            {
                var data = GetContext().TBL_HISTORY_IMAGES.Where(x => x.USER_ID == GetUserLogin.CurentCustomerId && x.CAMERA_ID == cameraid);
                total = data.Count();
                data = data.OrderBy(p => p.HISTORYIMAGES_ID).Skip(page * numPerPage - numPerPage).Take(numPerPage);
                return data.Select(item => new ImagesHistory
                {
                    CameraId = (int)item.CAMERA_ID,
                    ImageName = item.HISTORYIMAGES_INFO,
                    ImageUrl = HostData + item.HISTORYIMAGES_URL,
                    ImageId = (int)item.HISTORYIMAGES_ID,
                    TimeStart = item.HISTORYIMAGES_TIMESTART,
                    TimeEnd = item.HISTORYIMAGES_TIMEEND,
                    UserId = GetUserLogin.CurentCustomerId
                }).ToList();

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera pagging list with erro: {0}", ex));
                total = 0;
                return null;
            }
        }
        #endregion
        #region show function description
        public PartialViewResult ShowFuncDescription(int idFunction)
        {
            try
            {
                var model = new FunctionModel();
                var func = GetListFunction().FirstOrDefault(x => x.FunctionId == idFunction);
                //var cam=GetContext().TBL_CAMERA
                model.Function = func;
                return PartialView("_ptvFunctionDescription", model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera - Update Camera page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process Camera - Update Camera page");
            }
        }
        #endregion

        #region  function register
        public PartialViewResult _ptvFunctionRegister(int idfunction)
        {
            var listcam = GetListViewModel(idfunction);
            ViewBag.IdFunction = idfunction;
            return PartialView(listcam);
        }

        [HttpPost]
        [ActionName("_ptvFunctionRegister")]
        //[ValidateAntiForgeryToken]
        public PartialViewResult _ptvFunctionRegister(int idCamera, short idFunction)
        {
            ViewBag.IdCamera = idCamera;
            ViewBag.IdFunction = idFunction;
            try
            {
                //var camID = GetContext().TBL_CAMERA.FirstOrDefault(x => x.CAMERA_ID == idcamera);
                var cam = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.CAMERA_ID == idCamera && x.FUNCTION_ID == idFunction);
                if (cam == null)
                {
                    var camStatus = new TBL_CAMERA_STATUS()
                    {
                        FUNCTION_REGIGES = 0,
                        FUNCTION_ID = idFunction,
                        CAMERA_ID = idCamera,
                        //UPDATE_STATUS = 0
                    };
                    GetContext().TBL_CAMERA_STATUS.Add(camStatus);
                    GetContext().SaveChanges();
                    //    messageModel.MessageContent = "Đăng ký thành công rồi nha.";
                    //  messageModel.MessageType = MesageModel.MessageType.Success;
                }
                else
                {
                    //messageModel.MessageContent = "Chức năng này đã được đăng ký rồi";
                    //messageModel.MessageType = MesageModel.MessageType.Warning;
                    //Session["erro"] = "Camera đã được cài đặt chức năng này.";
                    //ViewBag.thongbao = "<script>$(document).ready(function () {alert('Camera đã được cài đặt chức năng này.');});</script>";
                    //var listcam = GetListCamera();
                    //return PartialView("_ptvFunctionRegister", listcam);
                    //return PartialView("_RegisterFunction", listcam);
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera - Update Camera page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process Camera - Update Camera page");
            }
            var listcam = GetListViewModel(idFunction);
            //var c = new CameraModel();
            //IEnumerable<ViewModel> model = GetListViewModel(idFuction);
            return PartialView("_RegisterFunction", listcam);
            //return PartialView("_ptvFunctionRegister", c);
        }
        #endregion
        //#region load viewModel
        //[HttpGet]
        //public PartialViewResult OnLoadViewModel(short idfuction)
        //{
        //    IEnumerable<ViewModel> model = GetListViewModel(idfuction);
        //    return PartialView(model);
        //}

        //#endregion
        #region regis
        public PartialViewResult _ptvValidate(int idCamera, int idFuction)
        {
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Hãy nhập mã kích hoạt để sử dụng chức năng!",
                MessageType = MesageModel.MessageType.Info
            };

            ViewBag.MessageCommand = messageModel;
            try
            {
                var model = new UpdateCameraStatus();
                //var idFuction = ViewBag.IdFunction;
                var regis_status = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.CAMERA_ID == idCamera && x.FUNCTION_ID == idFuction);
                if (regis_status != null)
                {
                    var regis_status_update = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.CAMERA_ID == idCamera && x.FUNCTION_ID == idFuction && x.FUNCTION_REGIGES == 0);
                    if (regis_status_update != null)
                    {
                        var camStatus = GetListCameraStatus().FirstOrDefault(x => x.Camera_Id == idCamera && x.Function_Id == idFuction);
                        model.Camerastatus = camStatus;
                        return PartialView(model);
                    }
                    else
                    {
                        messageModel.MessageContent = "Chức năng này đã được kích hoạt rồi.";
                        messageModel.MessageType = MesageModel.MessageType.Warning;
                        return PartialView("_Message", ViewBag.MessageCommand);
                    }

                }
                else
                {
                    messageModel.MessageContent = "Chức năng này chưa được đăng ký. Xin hãy đăng ký trước.";
                    messageModel.MessageType = MesageModel.MessageType.Warning;
                    //ViewBag.thongbao = "<script>$(document).ready(function () {alert('Tài khoản đã bị khóa hoặc chưa được kích hoạt, vui lòng liên hệ quản trị viên');});</script>";
                    //ViewBag
                    return PartialView("_Message", ViewBag.MessageCommand);
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera - Update Camera page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process Camera - Update Camera page");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult _ptvValidate(UpdateCameraStatus regis)
        {
            string text = "123456";

            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel();
            ViewBag.MessageCommand = messageModel;
            var function_regis = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.CAMERA_ID == regis.Camerastatus.Camera_Id && x.FUNCTION_ID == regis.Camerastatus.Function_Id);
            if (text == regis.Camerastatus.text)
            {
                function_regis.FUNCTION_REGIGES = 1;
                function_regis.CAMERA_STATUS = 1;
                function_regis.CAMERA_STATUS_TIME_START = "00 : 00 : AM";
                function_regis.CAMERA_STATUS_TIME_END = "11 : 59 : PM";
                //function_regis.UPDATE_STATUS = 0;
                var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                if (update.STATUS_UPDATE == 0)
                {
                    update.STATUS_UPDATE = 1;
                }
                GetContext().SaveChanges();
                messageModel.MessageContent = "Kích hoạt thành công";
                messageModel.MessageType = MesageModel.MessageType.Info;
            }
            else
            {
                messageModel.MessageContent = "Nhập lại mã số để kích hoạt";
                messageModel.MessageType = MesageModel.MessageType.Error;
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region SelectItemCamera
        public ActionResult GetSelectListCamera()
        {
            var model = new SelectListCamera { GetListCameras = GetCamera() };
            var listcam = RenderRazorViewToString(this.ControllerContext, "_ptvSelectListCamera", model);
            var json = Json(new { listcam });
            return json;
        }

        private List<SelectListItem> GetCamera()
        {
            try
            {
                var data = GetContext().TBL_CAMERA.Where(x => x.USER_ID == GetUserLogin.CurentCustomerId).ToList();
                return data.Select(item => new SelectListItem
                {
                    Text = item.CAMERA_NAME,
                    Value = item.CAMERA_ID.ToString(CultureInfo.InvariantCulture)
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Function by cameraId with erro: {0}", ex));
                return null;
            }
        }
        #endregion

        [HttpPost]
        public ActionResult _ptvChartMonth(int cameraid)
        {
            ViewBag.idcam = cameraid;
            var model = GetTotalFileOfMonth(cameraid);
            var listMonth = RenderRazorViewToString(this.ControllerContext, "_ptvChartMonth", model);
            var json = Json(new { listMonth });
            return json;
        }

        [HttpPost]
        public ActionResult _ptvLineChart(int cameraid, int month)
        {
            var model = GetTotalFileOfDay(cameraid, month);
            var listDay = RenderRazorViewToString(this.ControllerContext, "_ptvLineChart", model);
            var json = Json(new { listDay });
            return json;
        }
        public ActionResult GetNotificationContacts()
        {
            var notificationRegisterTime = Session["LastUpdated"] != null ? Convert.ToDateTime(Session["LastUpdated"]) : DateTime.Now;
            NotificationComponent NC = new NotificationComponent();
            var list = NC.GetInfoImages(notificationRegisterTime);
            var listNotify = new List<object>();

            foreach (var item in list)
            {
                Entities1 ent = new Entities1();
                string img = "http://nsc3.negatech.vn/" + item.HISTORYIMAGES_URL;
                string camera_Name = ent.TBL_CAMERA.Where(x => x.CAMERA_ID == item.CAMERA_ID).Select(a => a.CAMERA_NAME).Single();
                string function = ent.TBL_FUNCTION.Where(x => x.FUNCTION_ID == item.FUNCTION_ID).Select(a => a.FUNCTION_NAME).Single();
                listNotify.Add(new { Name = camera_Name, Function = function, Time = item.HISTORYIMAGES_TIMESTART.ToString(), Image = img, url = img });
            }

            //update session here for get only new added contacts (notification)
            Session["LastUpdate"] = DateTime.Now;
            return Json(listNotify, JsonRequestBehavior.AllowGet);
        }

        #region
        public ActionResult GetPolygon(int idfunction, int idcamera)
        {
            try
            {
                var model = new Camera_Function();
                //var camera = GetListCamera().FirstOrDefault(x => x.CAMERA_ID == idCamera);
                //model.CameraModel = camera;
                model.polygon = GetCoordinate(idfunction, idcamera, GetUserLogin.CurentCustomerId);
                model.functionId = idfunction;
                model.cameraId = idcamera;
                model.urlImage = "http://nsc3.negatech.vn/" + getImgCamera_Function(idcamera, idfunction);
                //if (model.polygon != null)
                //{
                //    return PartialView(model);
                //}
                //else
                //{
                return View("_ptvShowImgFunction", model);
                //}
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera - Update Camera page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process Camera - Update Camera page");
            }
        }
        #endregion

        [HttpPost]
        //[ActionName("GetPolygon")]
        //[ValidateAntiForgeryToken]
        public ActionResult SaveCoor(List<Coor> Coordinates, short idfunc, int idcam, string url)
        {
            string data = "";
            foreach (var i in Coordinates)
            {
                data = data + Convert.ToString(i.x) + "|" + Convert.ToString(i.y) + "|";

            }
            var s = GetContext().TBL_CAMERA_FUNCTION.FirstOrDefault(x => x.FUNCTION_ID == idfunc && x.CAMERA_ID == idcam);
            if (s == null)
            {
                var item = new TBL_CAMERA_FUNCTION();

                item.POLYGON = "polygon|" + data;
                item.USER_ID = GetUserLogin.CurentCustomerId;
                item.CAMERA_ID = idcam;
                item.FUNCTION_ID = idfunc;
                item.URL_IMAGE = url.Substring(24);
                item.IMAGE_STATUS = 2;
                var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                if (update.STATUS_UPDATE == 0)
                {
                    update.STATUS_UPDATE = 1;
                }
                //item.UPDATE_STATUS = 0;
                GetContext().TBL_CAMERA_FUNCTION.Add(item);
                GetContext().SaveChanges();
            }
            else
            {
                s.POLYGON = "polygon|" + data;
                s.URL_IMAGE = url.Substring(24);
                s.IMAGE_STATUS = 2;
                var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                if (update.STATUS_UPDATE == 0)
                {
                    update.STATUS_UPDATE = 1;
                }
                //s.UPDATE_STATUS = 1;
                GetContext().SaveChanges();
            }
            return Json(Coordinates);
        }

        #endregion
        #region show image
        public ActionResult ShowImage(int idcam, int idfunc)
        {
            var data = GetContext().TBL_CAMERA_FUNCTION.FirstOrDefault(x => x.FUNCTION_ID == idfunc && x.CAMERA_ID == idcam);
            if (data == null)
            {
                var item = new TBL_CAMERA_FUNCTION();

                //item.POLYGON = "polygon|" + data;
                item.USER_ID = GetUserLogin.CurentCustomerId;
                item.CAMERA_ID = idcam;
                item.FUNCTION_ID = (short)idfunc;
                item.IMAGE_STATUS = 1;
                var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                if (update.STATUS_UPDATE == 0)
                {
                    update.STATUS_UPDATE = 1;
                }
                //item.UPDATE_STATUS = 0;
                GetContext().TBL_CAMERA_FUNCTION.Add(item);
                GetContext().SaveChanges();
            }
            else
            {
                data.IMAGE_STATUS = 1;
                var update = GetContext().TBL_UPDATE_STATUS.FirstOrDefault(x => x.UPDATE_STATUS_ID == 1);
                if (update.STATUS_UPDATE == 0)
                {
                    update.STATUS_UPDATE = 1;
                }
                GetContext().SaveChanges();
            }
            Thread.SpinWait(5000);
            var cam_func = GetContext().TBL_CAMERA_FUNCTION.FirstOrDefault(x => x.FUNCTION_ID == idfunc && x.CAMERA_ID == idcam);
            var model = new Camera_Function();
            model.cameraId = Convert.ToInt32(cam_func.CAMERA_ID);
            model.functionId = (short)cam_func.FUNCTION_ID;
            model.userId = GetUserLogin.CurentCustomerId;
            model.polygon = cam_func.POLYGON;
            model.urlImage = "http://nsc3.negatech.vn/" + cam_func.URL_IMAGE;
            return View("_ptvShowImgFunction", model);
        }
        #endregion
    }

}
