using MyWeb.Areas.Agency.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using MyWeb.Business;
using MyWeb.Models;
using System.Net;
using System.Web.UI;
using System.Threading.Tasks;
using System.Net.Http;
namespace MyWeb.Areas.Agency.Controllers
{
    public class CameraOfUserController : ProviderControllerBase
    {
        //
        // GET: /Agency/CameraOfUser/
        [Authorize(Roles = "Agency")]
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

        [HttpPost]
        public async Task<ActionResult> GetLisCameraOfUser(int userid)
        {
            var model = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(userid) };
            var listCamera = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/CameraOfUser/_ptvListCamera.cshtml", model);
            var json = Json(new { listCamera });
            return json;
        }

        #region GetList User In Agency
        private async Task<List<CameraOfUser>> GetListCameraAllUser(int userid)
        {
            try
            {
                ViewBag.UserId = userid;
                var list = new List<CameraOfUser>();
                var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == userid);
                ViewBag.NameUser = data.NAME;
                //foreach (var item in data)
                //{
                var cam = GetContext().TBL_CAMERA.Where(c => c.USER_ID == userid).ToList();
                foreach (var i in cam)
                {
                    var s = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.CAMERA_ID == i.CAMERA_ID);
                    var model = new CameraOfUser();
                    model.CameraId = Convert.ToInt32(i.CAMERA_ID);
                    model.CameraName = i.CAMERA_NAME;
                    model.FullName = data.NAME;
                    model.Status = await GetStatusCamera(i.CAMERA_URL_STREAM);
                    if (s != null)
                    {
                        model.Status_Function = 1;
                    }
                    else
                    {
                        model.Status_Function = 0;
                    }
                    list.Add(model);
                }
                //}
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Customer list with erro: {0}", ex));
                return null;
            }
        }

        private async Task<string> GetStatusCamera(string name)
        {
            string s = "http://10.62.1.152:9999/viewstatus?name=" + name;
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(s);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        #endregion
        string name = RandomString();
        #region Create Camera
        public ActionResult CreateCamera(int userid)
        {
            try
            {
                ViewBag.userid = userid;
                var model = new CameraOfUser();
                return PartialView("_ptvCreateCamera", model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get CustomerModel - create category page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process CustomerModel - create category page");
            }
        }
        [HttpPost]
        [ActionName("CreateCamera")]
        //[ValidateAntiForgeryToken]
        public async Task<PartialViewResult> CreateCamera(CameraOfUser model)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel
            {
                MessageContent = "Xảy ra lỗi trong quá trình xử lý. Vui lòng thử lại",
                MessageType = MesageModel.MessageType.Error
            };
            ViewBag.MessageCommand = messageModel;
            string s1 = model.Model_camera;
            string url;
            switch (s1)
            {
                case "Foscam":
                    url = "rtsp://" + model.Username + ":" + model.Password + "@" + model.Domain + ":" + model.Port + "/videoMain";
                    await AddStream(name, url);
                    break;
                case "Tiandy":
                    url = "rtsp://" + model.Username + ":" + model.Password + "@" + model.Domain + ":" + model.Port + "/1/m.c.str.(normal)";
                    await AddStream(name, url);
                    break;
                case "Vantech":
                    url = "rtsp://" + model.Domain + ":" + model.Port + "/user=" + model.Username + "_password=" + model.Password + "_channel=1_stream=0.sdp";
                    await AddStream(name, url);
                    break;
                case "DaHua":
                    url = "rtsp://" + model.Username + ":" + model.Password + "@" + model.Domain + ":" + model.Port + "/live";
                    await AddStream(name, url);
                    break;
                case "Hikvision":
                    url = "rtsp://" + model.Username + ":" + model.Password + "@" + model.Domain + ":" + model.Port + "/Streaming/Channels/1";
                    await AddStream(name, url);
                    break;
                default:
                    break;
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var userid = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == model.UserId);
                    if (userid != null)
                    {
                        var camera = GetContext().TBL_CAMERA.FirstOrDefault(x => x.CAMERA_NAME == model.CameraName);
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
                                        cam.CAMERA_NAME = model.CameraName;

                                        cam.CAMERA_URL_STREAM = name;
                                        cam.USER_ID = model.UserId;
                                        GetContext().TBL_CAMERA.Add(cam);
                                        GetContext().SaveChanges();

                                        messageModel.MessageContent = "Thêm camera thành công";
                                        messageModel.MessageType = MesageModel.MessageType.Success;
                                        //ViewBag.MessageCommand = messageModel;
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
                                cam.CAMERA_NAME = model.CameraName;
                                cam.CAMERA_URL_STREAM = name;
                                cam.USER_ID = model.UserId;
                                GetContext().TBL_CAMERA.Add(cam);

                                GetContext().SaveChanges();
                                messageModel.MessageContent = "Thêm camera thành công";
                                messageModel.MessageType = MesageModel.MessageType.Success;
                                //ViewBag.MessageCommand = messageModel;
                            }
                        }
                        else
                        {
                            messageModel.MessageContent = "Camera đã tồn tại";
                            messageModel.MessageType = MesageModel.MessageType.Info;
                            //ViewBag.MessageCommand = messageModel;
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
                    //ViewBag.MessageCommand = messageModel;
                    await DeleteStream(name);
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process insert Camera item in master admin with erro: {0}", ex));
            }
            //var cammodel = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(model.UserId) };
            //return PartialView("_ptvShowCameraOfUser", cammodel);
            return PartialView("_Message", ViewBag.MessageCommand);
            //return PartialView("CreateCamera");
        }
        #endregion
        #region show list function
        public ActionResult ShowFunction(int cameraid)
        {
            ViewBag.cameraid = cameraid;
            var model = new ListFunctionModel { ListFunction = GetFunction(cameraid) };
            return PartialView("_ptvShowFunction", model);
        }
        [HttpGet]
        public PartialViewResult ActiveFunction(int FunctionID, int CameraID)
        {
            try
            {
                ViewBag.MessageCommand = "";
                var messageModel = new MesageModel.MessagesModel();
                ViewBag.MessageCommand = messageModel;
                var st = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.FUNCTION_ID == FunctionID && x.CAMERA_ID == CameraID);
                if (st == null)
                {
                    var sta = new TBL_CAMERA_STATUS();
                    sta.CAMERA_ID = CameraID;
                    sta.FUNCTION_ID = (short)FunctionID;
                    sta.FUNCTION_REGIGES = 1;
                    sta.CAMERA_STATUS = 1;
                    sta.CAMERA_STATUS_TIME_START = "00 : 00 : AM";
                    sta.CAMERA_STATUS_TIME_END = "11 : 59 : PM";
                    GetContext().TBL_CAMERA_STATUS.Add(sta);
                    GetContext().SaveChanges();
                    messageModel.MessageContent = "Kích hoạt thành công";
                    messageModel.MessageType = MesageModel.MessageType.Info;
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process update Camera item in master admin with erro: {0}", ex));
            }
            var model = new ListFunctionModel { ListFunction = GetFunction(CameraID) };
            //var listFunc = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/CameraOfUser/_ptvShowFunction.cshtml", model);
            //var json = Json(new { listFunc });
            //return json;
            return PartialView("_ptvShowFunction", model);
        }
        private List<FunctionOfCamera> GetFunction(int cameraid)
        {
            List<FunctionOfCamera> listModel = new List<FunctionOfCamera>();

            var list = GetContext().TBL_FUNCTION.GroupJoin(GetContext().TBL_CAMERA_STATUS.Where(z => z.CAMERA_ID == cameraid), n => n.FUNCTION_ID, x => x.FUNCTION_ID,
                (n, x) => new
                {
                    n,
                    x
                }).ToList();
            foreach (var item in list)
            {
                var fc = new FunctionOfCamera();
                fc.FunctionID = item.n.FUNCTION_ID;
                fc.FunctionName = item.n.FUNCTION_NAME;
                foreach (var i in item.x)
                {
                    if (i.CAMERA_ID == cameraid)
                    {
                        fc.Status = Convert.ToInt32(i.CAMERA_STATUS);
                        fc.Register = Convert.ToInt32(i.FUNCTION_REGIGES);
                    }
                }
                listModel.Add(fc);
            }
            return listModel;
        }
        #endregion
        #region
        [HttpGet]
        public async Task<PartialViewResult> OnloadCamera(int userid)
        {
            var model = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(userid) };
            //IEnumerable<CameraModel> cam = GetListCamera();
            return PartialView("_ptvShowCameraOfUser", model);
        }

        #endregion

        //#region active function of camera
        //public PartialViewResult ActiveFunction(int FunctionID, int CameraID)
        //{
        //    try
        //    {
        //        ViewBag.CameraID = CameraID;
        //        var model = new FunctionOfCamera();
        //        var user = GetFunctionCamera(FunctionID).FirstOrDefault(x => x.FunctionID == FunctionID);
        //        model = user;
        //        return PartialView("_ptvActiveFunction", model);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(String.Format("An error occurred in the process get Camera - Update Camera page in master admin with erro: {0}", ex));
        //        throw new BusinessException("An error occurred in the process Camera - Update Camera page");
        //    }
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public PartialViewResult ActiveFunction(FunctionOfCamera model)
        //{
        //    try
        //    {
        //        ViewBag.MessageCommand = "";
        //        var messageModel = new MesageModel.MessagesModel();
        //        ViewBag.MessageCommand = messageModel;
        //        var st = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.FUNCTION_ID == model.FunctionID && x.CAMERA_ID == model.CameraID);
        //        if (st == null)
        //        {
        //            var sta = new TBL_CAMERA_STATUS();
        //            sta.CAMERA_ID = model.CameraID;
        //            sta.FUNCTION_ID = (short)model.FunctionID;
        //            sta.FUNCTION_REGIGES = 1;
        //            sta.CAMERA_STATUS = 1;
        //            sta.CAMERA_STATUS_TIME_START = "00 : 00 : AM";
        //            sta.CAMERA_STATUS_TIME_END = "11 : 59 : PM";
        //            GetContext().TBL_CAMERA_STATUS.Add(sta);
        //            GetContext().SaveChanges();
        //            messageModel.MessageContent = "Kích hoạt thành công";
        //            messageModel.MessageType = MesageModel.MessageType.Info;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(String.Format("An error occurred in the process update Camera item in master admin with erro: {0}", ex));
        //    }
        //    var listFunction = new ListFunctionModel { ListFunction = GetFunction(model.CameraID) };
        //    //var userOfAgency = new UserAgencyModel { UserModel = GetListUserInAgency() };
        //    return PartialView("_ptvShowFunction", listFunction);
        //    //return PartialView("_Message", listFunction);
        //}
        //#endregion

        #region get user of agency
        public List<FunctionOfCamera> GetFunctionCamera(int functionid)
        {
            try
            {
                var data = GetContext().TBL_FUNCTION.Where(x => x.FUNCTION_ID == functionid).ToList();
                var list = new List<FunctionOfCamera>();
                foreach (var item in data)
                {
                    var func = new FunctionOfCamera();
                    func.FunctionID = item.FUNCTION_ID;
                    func.FunctionName = item.FUNCTION_NAME;
                    //func.Email = item.EMAIL;
                    //func.Username = item.USER_NAME;
                    list.Add(func);
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

        #region function manage
        public ActionResult FunctionManage()
        {
            try
            {
                var model = new ListFunctionModel { ListFunction = GetFunction() };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }

        }

        private IEnumerable<FunctionOfCamera> GetFunction()
        {
            try
            {
                var data = GetContext().TBL_FUNCTION.ToList();
                var list = (from item in data
                            //let active = item.AGENCY_ID == GetUserLogin.CurentCustomerId
                            select new FunctionOfCamera
                            {
                                FunctionName = item.FUNCTION_NAME,
                                FunctionID = item.FUNCTION_ID,
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
        #region Active All Function
        public ActionResult ActiveAllFunction(int cameraid)
        {
            ViewBag.Cam = cameraid;
            var model = new ListFunctionModel { ListFunction = GetFunction() };
            return PartialView("_ptvActiveAllFunction", model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActiveAllFunction(ListFunctionModel model)
        {
            ViewBag.MessageCommand = "";
            var messageModel = new MesageModel.MessagesModel();
            var list = new ListFunctionModel { ListFunction = GetFunction() };
            foreach (var item in list.ListFunction)
            {
                var sta = new TBL_CAMERA_STATUS();
                sta.CAMERA_ID = model.CameraId;
                sta.FUNCTION_ID = (short)item.FunctionID;
                sta.FUNCTION_REGIGES = 1;
                sta.CAMERA_STATUS = 1;
                sta.CAMERA_STATUS_TIME_START = "00 : 00 : AM";
                sta.CAMERA_STATUS_TIME_END = "11 : 59 : PM";
                GetContext().TBL_CAMERA_STATUS.Add(sta);
            }
            GetContext().SaveChanges();
            messageModel.MessageContent = "Kích hoạt thành công";
            messageModel.MessageType = MesageModel.MessageType.Info;
            ViewBag.MessageCommand = messageModel;
            var data = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(341) };
            var listCamera = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/CameraOfUser/_ptvListCamera.cshtml", data);
            var json = Json(new { listCamera });
            return json;
            //var cam = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(281) };
            //return PartialView("_ptvListCamera", cam);
            //return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion

        #region search name
        public ActionResult SearchName(string name)
        {
            try
            {
                var model = new UserAgencyModel { UserModel = GetListUserInAgency() };
                return PartialView("Index", model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }
        #endregion


        //#region Select Item User
        //public ActionResult GetListUserOfAgency()
        //{
        //    var model = new SelectListUserInAgency { GetListUser_Agency = GetUserOfAgency() };
        //    var listUser = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/CameraOfUser/_ptvSelectListUserOfAgency.cshtml", model);
        //    var json = Json(new { listUser });
        //    return json;
        //}
        //private List<SelectListItem> GetUserOfAgency()
        //{
        //    try
        //    {
        //        var data = GetContext().TBL_USER.Where(x => x.AGENCY_ID == GetUserLogin.CurentCustomerId).ToList();
        //        return data.Select(item => new SelectListItem
        //        {
        //            Text = item.NAME,
        //            Value = item.USER_ID.ToString(CultureInfo.InvariantCulture)
        //        }).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(String.Format("An error occurred in the process get Function by cameraId with erro: {0}", ex));
        //        return null;
        //    }
        //}
        //#endregion
    }
}
