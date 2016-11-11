using MyWeb.Areas.Agency.Models;
using MyWeb.Business;
using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Agency.Controllers
{
    public class HomeAgencyController : ProviderControllerBase
    {
        //
        // GET: /Agency/HomeAgency/
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
                var list = new List<UserOfAgency>();
                var data = GetContext().TBL_USER.Where(x => x.AGENCY_ID == GetUserLogin.CurentCustomerId).ToList();
                var agency = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);

                var ag = new UserOfAgency();
                ag.UserId = Convert.ToInt32(agency.USER_ID);
                ag.Total = data.Count.ToString() + " chi nhanh";
                ag.FullName = agency.USER_NAME;
                list.Add(ag);


                foreach (var item in data)
                {
                    var role = GetContext().TBL_USER_ROLE.FirstOrDefault(x => x.USER_ID == item.USER_ID);
                    var user = new UserOfAgency();
                    user.UserId = Convert.ToInt32(item.USER_ID);
                    user.FullName = item.USER_NAME;
                    if (role.ROLE_NAME == "Custommer")
                    {
                        user.Total = item.TBL_CAMERA.Count.ToString();
                    }
                    else
                    {
                        var data2 = GetContext().TBL_USER.Where(x => x.AGENCY_ID == item.USER_ID).ToList();
                        user.Total = data2.Count.ToString() + " khach hang";
                    }
                    user.Agency_Id = Convert.ToInt32(item.AGENCY_ID);
                    list.Add(user);
                }
                //var list = (from item in data
                //            //let active = item.AGENCY_ID == GetUserLogin.CurentCustomerId
                //            select new UserOfAgency
                //            {
                //                UserId = Convert.ToInt32(item.USER_ID),
                //                FullName = item.NAME,
                //                TotalCamera = item.TBL_CAMERA.Count,
                //                Agency_Id = Convert.ToInt32(item.AGENCY_ID)
                //            }).ToList();

                foreach (var item1 in data)
                {
                    var us = GetContext().TBL_USER.Where(x => x.AGENCY_ID == item1.USER_ID).ToList();
                    foreach (var item2 in us)
                    {
                        var user1 = new UserOfAgency();
                        user1.UserId = Convert.ToInt32(item2.USER_ID);
                        user1.FullName = item2.USER_NAME;
                        user1.Total = item2.TBL_CAMERA.Count.ToString();
                        user1.Agency_Id = Convert.ToInt32(item2.AGENCY_ID);
                        list.Add(user1);
                    }
                }

                //list = (from item2 in us
                //        select new UserOfAgency
                //        {
                //            UserId = Convert.ToInt32(item2.USER_ID),
                //            FullName = item2.NAME,
                //            TotalCamera = item2.TBL_CAMERA.Count,
                //            Agency_Id = Convert.ToInt32(item.AGENCY_ID)
                //        }).ToList();




                return list;

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Customer list with erro: {0}", ex));
                return null;
            }
        }
        #endregion

        public ActionResult _Avatar()
        {
            try
            {
                //var model = new GetAgencyModel { getAgency = GetAgency() };
                var model = new AgencyModel();
                var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);
                if (data.IMAGE_URL != null)
                {
                    model.avatar = "http://nsc3.negatech.vn/" + data.IMAGE_URL;
                }
                else
                {
                    model.avatar = "http://nsc3.negatech.vn/Avatar/logo.png";
                }
                model.Agency_Name = data.NAME;
                return PartialView("~/Areas/Agency/Views/Shared/PartialAgencyShared/_Avatar.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }

        public ActionResult _ptvShowAvatar()
        {
            try
            {
                //var model = new GetAgencyModel { getAgency = GetAgency() };
                var model = new AgencyModel();
                var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId);
                if (data.IMAGE_URL != null)
                {
                    model.avatar = "http://nsc3.negatech.vn/" + data.IMAGE_URL;
                }
                else
                {
                    model.avatar = "http://nsc3.negatech.vn/Avatar/logo.png";
                }
                model.Agency_Name = data.NAME;
                return PartialView("~/Areas/Agency/Views/Shared/PartialAgencyShared/_ptvShowAvatar.cshtml", model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }

        #region Get Agency
        private IEnumerable<AgencyModel> GetAgency()
        {
            try
            {
                var data = GetContext().TBL_USER.Where(x => x.USER_ID == GetUserLogin.CurentCustomerId).ToList();
                var list = (from item in data
                            //let active = item.AGENCY_ID == GetUserLogin.CurentCustomerId
                            select new AgencyModel
                            {
                                avatar = "http://nsc3.negatech.vn/" + item.IMAGE_URL
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
        public ActionResult AgencyLi()
        {
            var model = new GetUser { Getuser = GetUser() };
            return PartialView("~/Areas/Agency/Views/User/ptvCreateAgency.cshtml", model);
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
                    user.AGENCY_ID = Convert.ToInt32(item.AGENCY_ID);
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
        #region
        public async Task<ActionResult> InfoOfUser(int userId)
        {
            var role = GetContext().TBL_USER_ROLE.FirstOrDefault(x => x.USER_ID == userId);
            var user = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == userId);
            ViewBag.Review = user.REVIEW;
            ViewBag.Agency = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId).AGENCY_ID;
            if (role.ROLE_NAME == "Custommer")
            {
                var model = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(userId), Role = "Customer", AgencyReview = (short)user.REVIEW };
                return PartialView("_ptvInfoOfUser", model);
            }
            else
            {
                var model = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(userId), Role = "Agency" };
                return PartialView("_ptvInfoOfUser", model);
            }

        }
        #endregion
        #region GetList User In Agency
        private async Task<List<CameraOfUser>> GetListCameraAllUser(int userid)
        {
            try
            {
                ViewBag.UserId = userid;
                var list = new List<CameraOfUser>();
                var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == userid);
                ViewBag.NameUser = data.NAME;
                ViewBag.Agen = data.REVIEW;
                var role = GetContext().TBL_USER_ROLE.FirstOrDefault(x => x.USER_ID == userid);
                if (role.ROLE_NAME == "Custommer")
                {
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
                }
                else
                {
                    var user = GetContext().TBL_USER.Where(c => c.AGENCY_ID == userid).ToList();
                    foreach (var i in user)
                    {
                        //var s = GetContext().TBL_CAMERA_STATUS.FirstOrDefault(x => x.CAMERA_ID == i.CAMERA_ID);
                        var model = new CameraOfUser();
                        model.User_name = i.NAME;
                        model.Total_Camera = i.TBL_CAMERA.Count;
                        //model.CameraId = Convert.ToInt32(i.CAMERA_ID);
                        //model.CameraName = i.CAMERA_NAME;
                        //model.FullName = data.NAME;
                        //model.Status = await GetStatusCamera(i.CAMERA_URL_STREAM);
                        //if (s != null)
                        //{
                        //    model.Status_Function = 1;
                        //}
                        //else
                        //{
                        //    model.Status_Function = 0;
                        //}
                        list.Add(model);
                    }
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
        #region
        public ActionResult ShowLiveStream(int userid)
        {
            try
            {
                var model = new CameraOfUserModel { CameraInUser = GetFourCamera(userid) };
                return View("_ptvShowLiveCam", model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }
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
        #endregion
        public async Task<ActionResult> OnSetUp(int userId)
        {
            var role = GetContext().TBL_USER_ROLE.FirstOrDefault(x => x.USER_ID == userId);
            var user = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == userId);
            ViewBag.Review = user.REVIEW;
            ViewBag.Agency = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId).AGENCY_ID;
            var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == userId);
            if (data != null)
            {
                data.REVIEW = 4;
                GetContext().SaveChanges();
            }
            var model = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(userId), Role = "Customer", AgencyReview = (short)user.REVIEW };
            //var model = new GetUser { Getuser = GetUser() };
            return PartialView("_ptvInfoOfUser", model);
        }
        public async Task<ActionResult> OffSetUp(int userId)
        {
            var role = GetContext().TBL_USER_ROLE.FirstOrDefault(x => x.USER_ID == userId);
            var user = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == userId);
            ViewBag.Review = user.REVIEW;
            ViewBag.Agency = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == GetUserLogin.CurentCustomerId).AGENCY_ID;
            var data = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == userId);
            if (data != null)
            {
                data.REVIEW = 3;
                GetContext().SaveChanges();
            }
            var model = new CameraOfUserModel { CameraInUser = await GetListCameraAllUser(userId), Role = "Customer", AgencyReview = (short)user.REVIEW };
            //var model = new GetUser { Getuser = GetUser() };
            return PartialView("_ptvInfoOfUser", model);
        }
    }
}
