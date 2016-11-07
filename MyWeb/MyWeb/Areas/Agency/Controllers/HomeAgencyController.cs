using MyWeb.Areas.Agency.Models;
using MyWeb.Business;
using MyWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                ag.Total = data.Count.ToString() + "chi nhanh";
                ag.FullName = agency.NAME;
                list.Add(ag);


                foreach (var item in data)
                {
                    var user = new UserOfAgency();
                    user.UserId = Convert.ToInt32(item.USER_ID);
                    user.FullName = item.NAME;
                    if (item.TBL_CAMERA.Count != 0)
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
                        user1.FullName = item2.NAME;
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
        public ActionResult InfoOfUser()
        {
            return PartialView("_ptvInfoOfUser");
        }
        #endregion
    }
}
