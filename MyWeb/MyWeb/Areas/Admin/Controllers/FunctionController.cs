using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWeb.Areas.Admin.Models;
using MyWeb.Business;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class FunctionController : ProviderControllerBase
    {
        //
        // GET: /Admin/Function/

        public ActionResult Index()
        {
            var model = GetListFunction();
            return View(model);
        }
        #region Create
        public ActionResult Create()
        {
            try
            {
                var model = new CreateFunctionModel();
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get form - create Function page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the get form Create function");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateFunctionModel model)
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
                    var fc = new TBL_FUNCTION
                    {
                        FUNCTION_NAME = model.FunctionName,
                        //FUNCTION_DESCRIPTION = model.Description
                    };
                    GetContext().TBL_FUNCTION.Add(fc);
                    GetContext().SaveChanges();
                    messageModel.MessageContent = "Thêm chức năng thành công";
                    messageModel.MessageType = MesageModel.MessageType.Success;
                }
                else
                {
                    Log.Info(String.Format("Model FunctionsModel is not valid"));
                }

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process insert Functions item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Update
        [HttpGet]
        public ActionResult Edit(int idFunction)
        {
            try
            {
                var model = GetListFunction().FirstOrDefault(x => x.FunctionId == idFunction);
                return PartialView(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get form Update function page in master admin with erro: {0}", ex));
                throw new BusinessException("An error occurred in the process get form Update function");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CreateFunctionModel model)
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
                    var fc = GetContext().TBL_FUNCTION.FirstOrDefault(x => x.FUNCTION_ID == model.FunctionId);
                    if (fc != null)
                    {
                        fc.FUNCTION_NAME = model.FunctionName;
                        GetContext().SaveChanges();
                        messageModel.MessageContent = "Cập nhật chức năng thành công";
                        messageModel.MessageType = MesageModel.MessageType.Success;
                    }
                }
                else
                {
                    Log.Info(String.Format("Model FunctionsModel in update Function is not valid"));
                }

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process create Functions item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
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
                var fc = GetContext().TBL_FUNCTION.FirstOrDefault(x => x.FUNCTION_ID == id);
                if (fc != null)
                {
                    var check = GetContext().TBL_CAMERA_STATUS.Count(x => x.FUNCTION_ID == id);
                    if (check <= 0)
                    {
                        GetContext().TBL_FUNCTION.Remove(fc);
                        GetContext().SaveChanges();
                        messageModel.MessageContent = "Xóa chức năng thành công";
                        messageModel.MessageType = MesageModel.MessageType.Success;
                    }
                    else
                    {
                        messageModel.MessageContent = "Chức năng này đang xử dụng - Không thể xóa";
                        messageModel.MessageType = MesageModel.MessageType.Warning;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process Delete Functions item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region SearchResult

        public PartialViewResult SearchResult()
        {
            var model = GetListFunction();
            return PartialView("_ptvShow", model);
        }
        #endregion
        #region Hepper

        private List<CreateFunctionModel> GetListFunction()
        {
            var data = GetContext().TBL_FUNCTION.ToList();
            return data.Select(item => new CreateFunctionModel
            {
                FunctionId = item.FUNCTION_ID,
                FunctionName = item.FUNCTION_NAME
            }).ToList();
        }
        #endregion
    }
}
