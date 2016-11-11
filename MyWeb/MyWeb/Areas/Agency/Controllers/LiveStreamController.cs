using MyWeb.Areas.Agency.Models;
using MyWeb.Business;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Agency.Controllers
{
    public class LiveStreamController : ProviderControllerBase
    {
        //
        // GET: /Agency/LiveStream/
        [Authorize(Roles = "Agency")]
        public ActionResult Index()
        {
            return View();
        }
        //#region
        [HttpGet]
        public ActionResult LiveStreamOfFourCamera(int userid)
        {
            try
            {
                var model = new CameraOfUserModel { CameraInUser = GetFourCamera(userid) };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }
        //#endregion
        public ActionResult LiveStreamOfNineCamera(int userid)
        {
            try
            {
                var model = new CameraOfUserModel { CameraInUser = GetFourCamera(userid) };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }
        public ActionResult LiveStreamOfLimitCamera(int userid)
        {
            try
            {
                var model = new CameraOfUserModel { CameraInUser = GetFourCamera(userid) };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }
        //get user total camera<4
        public ActionResult GetFour()
        {
            var list = FourCamera();
            var listUser = new List<object>();

            foreach (var item in list)
            {
                listUser.Add(new
                {
                    FullName = item.Text,
                    UserID = item.Value
                });
            }
            return Json(listUser, JsonRequestBehavior.AllowGet);
        }
        //get user total 4<camera<9
        public ActionResult GetNine()
        {
            var list = NineCamera();
            var listUser = new List<object>();

            foreach (var item in list)
            {
                listUser.Add(new
                {
                    FullName = item.Text,
                    UserID = item.Value
                });
            }
            return Json(listUser, JsonRequestBehavior.AllowGet);
        }
        //get user limit camera
        public ActionResult GetLimit()
        {
            var list = LimitCamera();
            var listUser = new List<object>();

            foreach (var item in list)
            {
                listUser.Add(new
                {
                    FullName = item.Text,
                    UserID = item.Value
                });
            }
            return Json(listUser, JsonRequestBehavior.AllowGet);
        }
        #region four camera
        //public ActionResult GetListUserOfAgencyFourCamera()
        //{
        //    ViewBag.Action = "ListFourCamera";
        //    var model = new SelectListUserInAgency { GetListUser_Agency = FourCamera() };
        //    var listUser = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/LiveStream/_ptvSelectUser.cshtml", model);
        //    var json = Json(new { listUser });
        //    return json;
        //}
        private List<SelectListItem> FourCamera()
        {
            var list = new List<UserOfAgency>();
            var data = GetUserOfAgency();
            foreach (var item in data)
            {
                short v = Convert.ToInt16(item.Value);
                var cam = GetContext().TBL_CAMERA.Where(x => x.USER_ID == v).ToList();
                int count = cam.Count();
                if (cam.Count < 4)
                {
                    var user = new UserOfAgency();
                    user.FullName = item.Text;
                    user.UserId = v;
                    list.Add(user);
                }
            }
            return list.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.UserId.ToString()
            }).ToList();
        }
        #endregion
        #region nine camera
        //public ActionResult GetListUserOfAgencyNineCamera()
        //{
        //    ViewBag.Action = "ListNineCamera";
        //    var model = new SelectListUserInAgency { GetListUser_Agency = NineCamera() };
        //    var listUser = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/LiveStream/_ptvSelectUser.cshtml", model);
        //    var json = Json(new { listUser });
        //    return json;
        //}
        private List<SelectListItem> NineCamera()
        {
            var list = new List<UserOfAgency>();
            var data = GetUserOfAgency();
            foreach (var item in data)
            {
                short v = Convert.ToInt16(item.Value);
                var cam = GetContext().TBL_CAMERA.Where(x => x.USER_ID == v).ToList();
                int count = cam.Count();
                if (cam.Count <= 9 && cam.Count > 4)
                {
                    var user = new UserOfAgency();
                    user.FullName = item.Text;
                    user.UserId = v;
                    list.Add(user);
                }
            }
            return list.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.UserId.ToString()
            }).ToList();
        }
        #endregion
        #region limit camera
        //public ActionResult GetListUserOfAgencyLimitCamera()
        //{
        //    ViewBag.Action = "ListLimitCamera";
        //    var model = new SelectListUserInAgency { GetListUser_Agency = LimitCamera() };
        //    var listUser = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/LiveStream/_ptvSelectUser.cshtml", model);
        //    var json = Json(new { listUser });
        //    return json;
        //}
        private List<SelectListItem> LimitCamera()
        {
            var list = new List<UserOfAgency>();
            var data = GetUserOfAgency();
            foreach (var item in data)
            {
                short v = Convert.ToInt16(item.Value);
                var cam = GetContext().TBL_CAMERA.Where(x => x.USER_ID == v).ToList();
                int count = cam.Count();
                if (cam.Count > 9)
                {
                    var user = new UserOfAgency();
                    user.FullName = item.Text;
                    user.UserId = v;
                    list.Add(user);
                }
            }
            return list.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.UserId.ToString()
            }).ToList();
        }
        private List<SelectListItem> GetUserOfAgency()
        {
            try
            {
                var data = GetContext().TBL_USER.Where(x => x.AGENCY_ID == GetUserLogin.CurentCustomerId && x.REVIEW == 1).ToList();
                return data.Select(item => new SelectListItem
                {
                    Text = item.NAME,
                    Value = item.USER_ID.ToString(CultureInfo.InvariantCulture)
                }).ToList();
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Function by cameraId with erro: {0}", ex));
                return null;
            }
        }
        #endregion

        //#region show four camera
        //[HttpPost]
        //public ActionResult ListFourCamera(int userid)
        //{
        //    var model = GetFourCamera(userid);
        //    var listFourCamera = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/LiveStream/_ptvFourCamera.cshtml", model);
        //    var json = Json(new { listFourCamera });
        //    return json;
        //}
        //#endregion
        //#region show nine camera
        //[HttpPost]
        //public ActionResult ListNineCamera(int userid)
        //{
        //    var model = GetFourCamera(userid);
        //    var listNineCamera = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/LiveStream/_ptvNineCamera.cshtml", model);
        //    var json = Json(new { listNineCamera });
        //    return json;
        //}
        //#endregion
        //#region show limit camera
        //[HttpPost]
        //public ActionResult ListLimitCamera(int userid)
        //{
        //    var model = GetFourCamera(userid);
        //    var listLimitCamera = RenderRazorViewToString(this.ControllerContext, "~/Areas/Agency/Views/LiveStream/_ptvLimitCamera.cshtml", model);
        //    var json = Json(new { listLimitCamera });
        //    return json;
        //}
        //#endregion
        private List<CameraOfUser> GetFourCamera(int userid)
        {
            List<CameraOfUser> list = new List<CameraOfUser>();
            var data = GetContext().TBL_CAMERA.Where(x => x.USER_ID == userid).ToList();
            foreach (var item in data)
            {
                var cam = new CameraOfUser();
                cam.CameraId = Convert.ToInt32(item.CAMERA_ID);
                cam.CameraName = item.CAMERA_NAME;
                cam.Camera_Url_Stream = item.CAMERA_URL_STREAM;
                list.Add(cam);
            }
            return list;
        }

    }
}
