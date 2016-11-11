using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MyWeb.Areas.Admin.Models;
using MyWeb.Business;
using MyWeb.Models;

namespace MyWeb.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RoleManagerController : ProviderControllerBase
    {
        //
        // GET: /Admin/RoleManager/
        #region MyRegion
        public ActionResult Index()
        {
            try
            {
                var model = GetUserForRole();
                return View(model);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera in master admin with erro: {0}", ex));
                return null;
            }
        }
        #endregion
        #region RoleAddToUser
        [HttpGet]
        public PartialViewResult RoleAddToUser()
        {
            var dataUser = GetContext().TBL_USER.ToList();
            var selectListUser = dataUser.Select(item => new SelectListItem
            {
                Value = item.USER_ID.ToString(CultureInfo.InvariantCulture),
                Text = item.USER_NAME
            }).ToList();
            var roleData = GetContext().TBL_ROLE.ToList();
            var selectlistRole = roleData.Select(item => new SelectListItem
            {
                Value = item.ROLE_ID.ToString(CultureInfo.InvariantCulture),
                Text = item.ROLE_NAME
            }).ToList();
            var model = new RoleAddToUserModel { ListRoles = selectlistRole, ListUsers = selectListUser };
            return PartialView("_ptvRoleAddToUser", model);
        }
        [HttpPost]
        public PartialViewResult RoleAddToUser(RoleAddToUserModel model)
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
                    var iexit = GetContext().TBL_USER_ROLE.FirstOrDefault(x => x.ROLE_ID == model.RoleId && x.USER_ID == model.UserId);
                    if (iexit != null)
                    {
                        messageModel.MessageContent = "User đã được gán vào quyền này";
                        messageModel.MessageType = MesageModel.MessageType.Warning;
                    }
                    else
                    {
                        var userName = GetContext().TBL_USER.FirstOrDefault(x => x.USER_ID == model.UserId);
                        var roleName = GetContext().TBL_ROLE.FirstOrDefault(x => x.ROLE_ID == model.RoleId);
                        if (userName != null && roleName != null)
                        {
                            var userRole = new TBL_USER_ROLE
                            {
                                ROLE_ID = (short)model.RoleId,
                                USER_ID = model.UserId,
                                USER_NAME = userName.USER_NAME,
                                ROLE_NAME = roleName.ROLE_NAME,
                                USER_ROLE_DESCRIPTION = null
                            };
                            GetContext().TBL_USER_ROLE.Add(userRole);
                            GetContext().SaveChanges();
                            messageModel.MessageContent = "Gán quyền cho user thành công";
                            messageModel.MessageType = MesageModel.MessageType.Success;
                            Log.Info(String.Format("Model RoleAddToUserModel is not valid"));
                        }
                    }

                }
                else
                {
                    Log.Info(String.Format("Model RoleAddToUserModel is not valid"));
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process RoleAddToUserModel item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region RemoveUsersFromRoles

        public ActionResult RemoveUsersFromRoles(string id)
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
                if (id != null)
                {
                    var roleId = int.Parse(id.Split('_')[0]);
                    var userId = int.Parse(id.Split('_')[1]);
                    var userAndRole = GetContext().TBL_USER_ROLE.FirstOrDefault(x => x.ROLE_ID == roleId && x.USER_ID == userId);
                    if (userAndRole != null)
                    {
                        GetContext().TBL_USER_ROLE.Remove(userAndRole);
                        GetContext().SaveChanges();
                        messageModel.MessageContent = "Xóa quyền cho user thành công";
                        messageModel.MessageType = MesageModel.MessageType.Success;
                    }
                }
                else
                {
                    Log.Info(String.Format("Model RemoveUsersFromRoles is not valid"));
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process RemoveUsersFromRoles item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #region Danh sách role
        public ActionResult ListAllRole()
        {
            var model = GetListRole();
            return View(model);
        }

        #region RoleAddToUser
        [HttpGet]
        public PartialViewResult CreateRole()
        {
            var model = new RolesModel();
            return PartialView("_ptvAddRole", model);
        }
        [HttpPost]
        public ActionResult CreateRole(RolesModel model)
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
                    var role = new TBL_ROLE { ROLE_NAME = model.RoleName, ROLE_DESCRIPTION = model.Description };
                    GetContext().TBL_ROLE.Add(role);
                    GetContext().SaveChanges();
                    messageModel.MessageContent = "Tạo quyền thành công";
                    messageModel.MessageType = MesageModel.MessageType.Success;

                }
                else
                {
                    Log.Info(String.Format("Model RolesModel is not valid"));
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process CreateRole item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }

        [HttpPost]
        public ActionResult DeleteRole(string roleName)
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
                if (!string.IsNullOrEmpty(roleName))
                {
                    var roleProvider = new CustomRoleProvider();
                    var exit = roleProvider.GetUsersInRole(roleName);
                    if (exit.Any())
                    {
                        messageModel.MessageContent = "Quyền này đã được gán cho user. Không thể xóa";
                        messageModel.MessageType = MesageModel.MessageType.Warning;
                        return PartialView("_Message", ViewBag.MessageCommand);
                    }
                    if (roleProvider.DeleteRole(roleName, false))
                    {
                        messageModel.MessageContent = "Xóa quyền thành công";
                        messageModel.MessageType = MesageModel.MessageType.Success;
                    }
                }
                else
                {
                    Log.Info(String.Format("Model DeleteRole is not valid"));
                }
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process DeleteRole item in master admin with erro: {0}", ex));
            }
            return PartialView("_Message", ViewBag.MessageCommand);
        }
        #endregion
        #endregion
        #region Search
        [HttpGet]
        public PartialViewResult SearchResult()
        {
            IEnumerable<UserForRolesModel> cam = GetUserForRole();
            return PartialView("_ptvShow", cam);
        }
        #endregion
        #region Research Role
        [HttpGet]
        public PartialViewResult SearchResultRole()
        {
            IEnumerable<RolesModel> rolse = GetListRole();
            return PartialView("_ptvShowAllRole", rolse);
        }
        #endregion
        #region Hepper

        private IEnumerable<UserForRolesModel> GetUserForRole()
        {
            var data = GetContext().TBL_USER_ROLE.ToList();
            return data.Select(item => new UserForRolesModel
            {
                RoleName = item.TBL_ROLE.ROLE_NAME,
                UserName = item.TBL_USER.USER_NAME,
                RoleId = item.ROLE_ID,
                UserId = (int)item.USER_ID
            }).ToList();
        }

        private IEnumerable<RolesModel> GetListRole()
        {
            var data = GetContext().TBL_ROLE.ToList();
            var model = data.Select(item => new RolesModel
            {
                RoleId = item.ROLE_ID,
                RoleName = item.ROLE_NAME,
                Description = item.ROLE_DESCRIPTION
            }).ToList();
            return model;
        }
        #endregion

    }
}
