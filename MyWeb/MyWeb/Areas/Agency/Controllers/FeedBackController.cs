using MyWeb.Areas.Agency.Models;
using MyWeb.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyWeb.Areas.Agency.Controllers
{
    public class FeedBackController : ProviderControllerBase
    {
        //
        // GET: /Agency/FeedBack/
        [Authorize(Roles = "Agency")]
        public ActionResult Index()
        {
            try
            {
                var model = new FeedBackModel { ListFeedBack = GetListFeedBackOfUser() };
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get categoty in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get categoty in master admin");
            }
        }
        #region GetList feedback of user
        private List<FeedBack> GetListFeedBackOfUser()
        {
            try
            {
                var list = new List<FeedBack>();
                var data = GetContext().TBL_USER.Where(x => x.AGENCY_ID == GetUserLogin.CurentCustomerId).ToList();
                foreach (var item in data)
                {
                    var fb = GetContext().TBL_FEEDBACK.Where(c => c.USER_ID == item.USER_ID).ToList();
                    foreach (var i in fb)
                    {
                        var feedback = new FeedBack();
                        feedback.FeedbackID = Convert.ToInt32(i.FEEDBACK_ID);
                        feedback.FullName = item.NAME;
                        feedback.Title = i.TITLE;
                        feedback.Content = i.FEEDBACK_CONTENT;
                        feedback.Summary = "Xem chi tiết";
                        feedback.Time = i.FEEDBACK_TIME;
                        list.Add(feedback);
                    }
                }
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
