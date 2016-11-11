using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWeb.Areas.Admin.Models;
using MyWeb.Business;

namespace MyWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProcessController : ProviderControllerBase
    {
        //
        // GET: /Admin/Process/

        public ActionResult Index()
        {
            var model = GetListProcess();
            return View(model);
        }

        private List<ProcessModel> GetListProcess()
        {
            var cameraStaut = GetContext().TBL_CAMERA_STATUS.ToList();
            return cameraStaut.Select(item => new ProcessModel
            {
                CameraId = (int)item.CAMERA_ID,
                CameraName = item.TBL_CAMERA.CAMERA_NAME,
                FunctionId = item.FUNCTION_ID,
                FunctionName = item.TBL_FUNCTION.FUNCTION_NAME,
                CustomerId = (int)item.TBL_CAMERA.TBL_USER.USER_ID,
                CustomerName = item.TBL_CAMERA.TBL_USER.DISPLAY_NAME,
                Status = item.CAMERA_STATUS
            }).ToList();
        }
    }
}
