using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using CKFinder.Settings;
using log4net;
using MyWeb.Areas.Front.Models;
using MyWeb.Models;
using Oracle.ManagedDataAccess.Client;
using System.Net.Http;
using System.Threading.Tasks;


namespace MyWeb.Business
{
    public abstract class ProviderControllerBase : Controller
    {
        #region Variables
        protected Dictionary<string, object> ListBusiness = new Dictionary<string, object>();
        protected Entities1 Context;
        protected readonly ILog Log = LogManager.GetLogger("ProductController");
        protected const string MessagePartialView = "~/Views/Shared/_Message.cshtml";
        protected const string LinkNewModuble = "/tin-tuc/";
        protected const string LinkProductModuble = "/san-pham/";
        protected const string LinkPageContent = "/page-new/";

        //Message Erro
        protected const string ErrorControllerErrorTitle = "Page Not Found!";
        protected const string ErrorControllerErrorText = "Error!";
        protected const string ErrorControllerExpFomat = "An error occurred while processing in ErrorController!";
        protected const string ErrorControllerGetLockedAccountMessage = "Tài khoản của bạn đã bị khóa. Hãy liên hệ với chăm sóc khách hàng để được xử lý!";
        protected const string ErrorControllerErrorMsg = "We're sorry, the page you requested cannot be found.";
        protected const string ErrorControllerDefaultMsg = "Sorry, our site is temporarily down for maintenance";
        protected const string ErrorControllerAccessDenied = "Access Denied!";
        protected const string SerrverErro = "An error occurred while processing your request.";
        protected const string IsAdmin = "Administrator";
        public const string IsCustomer = "Custommer";
        public const string IsAgency = "Agency";
        protected const int MonthConst = 12;
        protected const int DayConst = 31;
        protected const string HostData = "http://10.62.1.150/";
        public const int RowPerPage = 6;
        #endregion
        public Entities1 GetContext()
        {
            return Context ?? (Context = new Entities1());
        }
        public void CheckValidate()
        {
            var errors1 = ModelState.Where(x => x.Value.Errors.Count > 0).Select(x => new { x.Key, x.Value.Errors }).ToArray();
        }

        public void SubmitDatabase()
        {
            try
            {
                GetContext().SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting
                        // the current instance as InnerException
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        #region Get Dashboard

        public void GetDashboard(string url, string nameParent, string nameLast)
        {
            string db = string.Empty;
            if (!string.IsNullOrEmpty(url))
            {
                var urlRw = "../.." + url;
                db = string.Format("<li><a href=\"{0}\">{1}</a>&nbsp;&nbsp;<i class=\"fa fa-angle-right\"></i>&nbsp;&nbsp;</li>", urlRw, nameParent);
                db += "<li class=\"active\">" + nameLast + "</li>";
            }
            else
            {
                db += "<li class=\"active\">" + nameLast + "</li>";
            }
            ViewBag.Dashboard = !string.IsNullOrEmpty(db) ? db : string.Empty;
        }
        #endregion

        public static String RenderRazorViewToString(ControllerContext controllerContext, String viewName, Object model)
        {
            controllerContext.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                var viewContext = new ViewContext(controllerContext, viewResult.View, controllerContext.Controller.ViewData, controllerContext.Controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        #region Get listfunction
        public List<FunctionsModel> GetListFunction()
        {
            try
            {
                //const string defaulCam = @"\Images\not-found-cam.jpg";
                var data = GetContext().TBL_FUNCTION.ToList();
                var list = new List<FunctionsModel>();
                foreach (var item in data)
                {
                    var func = new FunctionsModel { };
                    //if (!string.IsNullOrEmpty(item.CAMERA_URL))
                    //{
                    //    cam.CAMERA_URL = item.CAMERA_URL;
                    //}
                    func.FunctionId = item.FUNCTION_ID;
                    func.FunctionName = item.FUNCTION_NAME;
                    func.Description = item.FUNCTION_DESCRIPTION;
                    func.FunctionImage = "http://nsc3.negatech.vn/" + item.FUNCTION_IMAGE;
                    //cam.CAMERA_IP = item.CAMERA_IP;
                    //cam.CAMERA_NUMBER = item.CAMERA_NUMBER;
                    list.Add(func);
                }
                return list;
                //return data.Select(item => new CameraModel
                //{
                //    //CAMERA_NUMBER = item.CAMERA_NUMBER,
                //    CAMERA_ID = (int)item.CAMERA_ID,
                //    CAMERA_NAME = item.CAMERA_NAME,
                //    CAMERA_URL = item.CAMERA_URL,
                //    //CAMERA_IP = item.CAMERA_IP,
                //    USER_ID = (int)item.TBL_USER.USER_ID
                //}).ToList();

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera list with erro: {0}", ex));
                return null;
            }
        }
        #endregion

        #region Get liststatus
        public List<CameraStatus> GetListCameraStatus()
        {
            try
            {
                //const string defaulCam = @"\Images\not-found-cam.jpg";
                var data = GetContext().TBL_CAMERA_STATUS.ToList();
                var list = new List<CameraStatus>();
                foreach (var item in data)
                {
                    var cam = new CameraStatus();
                    cam.Camera_Id = (int)item.CAMERA_ID;
                    cam.CameraStatus_Id = (int)item.CAMERA_STATUS_ID;
                    cam.Function_Id = item.FUNCTION_ID;
                    cam.Function_Regis = item.FUNCTION_REGIGES;
                    cam.Camera_Status = item.CAMERA_STATUS;
                    //cam.CAMERA_IP = item.CAMERA_IP;
                    //cam.CAMERA_NUMBER = item.CAMERA_NUMBER;
                    list.Add(cam);
                }
                return list;
                //return data.Select(item => new CameraModel
                //{
                //    //CAMERA_NUMBER = item.CAMERA_NUMBER,
                //    CAMERA_ID = (int)item.CAMERA_ID,
                //    CAMERA_NAME = item.CAMERA_NAME,
                //    CAMERA_URL = item.CAMERA_URL,
                //    //CAMERA_IP = item.CAMERA_IP,
                //    USER_ID = (int)item.TBL_USER.USER_ID
                //}).ToList();

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera list with erro: {0}", ex));
                return null;
            }
        }
        #endregion

        #region get CameraStatus
        public List<ViewModel> GetListViewModel(int idfunction)
        {
            List<ViewModel> listModel = new List<ViewModel>();

            var list = GetContext().TBL_CAMERA.Where(z => z.USER_ID == GetUserLogin.CurentCustomerId).GroupJoin(GetContext().TBL_CAMERA_STATUS.Where(z => z.FUNCTION_ID == idfunction), n => n.CAMERA_ID, x => x.CAMERA_ID,
                (n, x) => new
            {
                n,
                x
            }).ToList();//&& z.x.FUNCTION_ID == function
            //var camera = GetContext().TBL_CAMERA;
            //var cameraStatus = GetContext().TBL_CAMERA_STATUS;

            foreach (var item in list)
            {
                var viee = new ViewModel();
                viee.CAMERA_ID = (int)item.n.CAMERA_ID;
                viee.CAMERA_NAME = item.n.CAMERA_NAME;
                foreach (var i in item.x)
                {
                    if (i.FUNCTION_ID == idfunction)
                    {
                        viee.Function_regis = i.FUNCTION_REGIGES;
                        viee.Function_Id = i.FUNCTION_ID;
                    }
                }
                listModel.Add(viee);
            }
            return listModel;
        }

        #endregion

        #region Get listcamera

        public List<CameraModel> GetListCamera()
        {
            try
            {
                const string defaulCam = @"\Images\not-found-cam.jpg";
                var data = GetContext().TBL_CAMERA.Where(x => x.USER_ID == GetUserLogin.CurentCustomerId).ToList();
                var list = new List<CameraModel>();
                foreach (var item in data)
                {
                    var cam = new CameraModel { CAMERA_URL = defaulCam };
                    if (!string.IsNullOrEmpty(item.CAMERA_URL))
                    {
                        cam.CAMERA_URL = item.CAMERA_URL;
                    }
                    cam.CAMERA_ID = (int)item.CAMERA_ID;
                    cam.CAMERA_NAME = item.CAMERA_NAME;
                    cam.USER_ID = (int)item.TBL_USER.USER_ID;
                    cam.CAMERA_URL_STREAM = item.CAMERA_URL_STREAM;
                    //cam.CAMERA_IP = item.CAMERA_IP;
                    //cam.CAMERA_NUMBER = item.CAMERA_NUMBER;
                    list.Add(cam);
                }
                return list;
                //return data.Select(item => new CameraModel
                //{
                //    //CAMERA_NUMBER = item.CAMERA_NUMBER,
                //    CAMERA_ID = (int)item.CAMERA_ID,
                //    CAMERA_NAME = item.CAMERA_NAME,
                //    CAMERA_URL = item.CAMERA_URL,
                //    //CAMERA_IP = item.CAMERA_IP,
                //    USER_ID = (int)item.TBL_USER.USER_ID
                //}).ToList();

            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get Camera list with erro: {0}", ex));
                return null;
            }
        }
        #endregion
        #region Get Model HistoryImage
        public IEnumerable<HistoryImageModel> GetImagesHistory(int cameraid, int? funtionId)
        {
            try
            {
                var lstPara = new List<OracleParameter>();
                string sql = @"select img.CAMERA_ID,img.FUNCTION_ID,img.HISTORY_VIDEO_ID,img.HISTORYIMAGES_ID,img.HISTORYIMAGES_INFO,
                CONCAT('../Uploads/',img.HISTORYIMAGES_URL) as HISTORYIMAGES_URL,img.USER_ID,img.HISTORYIMAGES_TIMEEND,img.HISTORYIMAGES_TIMESTART,
                EXTRACT(DAY FROM HISTORYIMAGES_TIMESTART ) AS DayTimeStart,
                EXTRACT(MONTH FROM HISTORYIMAGES_TIMESTART ) AS MonthTimeStart,
                EXTRACT(YEAR FROM HISTORYIMAGES_TIMESTART ) AS YearTimeStart
                from TBL_HISTORY_IMAGES img where img.CAMERA_ID=:cameraId and img.USER_ID=:userId ";
                lstPara.Add(new OracleParameter("cameraId", cameraid));
                lstPara.Add(new OracleParameter("userId", GetUserLogin.CurentCustomerId));
                if (funtionId != null)
                {
                    sql += " and img.FUNCTION_ID=:functionId";
                    lstPara.Add(new OracleParameter("functionId", funtionId));
                }
                var list = GetContext().Database.SqlQuery<HistoryImageModel>(sql, lstPara.ToArray()).ToList();
                return list;
            }
            catch (Exception ex)
            {
                Log.Error(String.Format("An error occurred in the process get HistoryImage list with erro: {0}", ex));
                return null;
            }
        }

        public List<HistoryImageModel> GetHistoyImageModels(int cameraId, int? day, int? month, int? functionId, out int total, int? page, int rowperpage)
        {
            var lstPara = new List<OracleParameter>();
            //            string sql = @"select img.CAMERA_ID,img.FUNCTION_ID,img.HISTORY_VIDEO_ID,img.HISTORYIMAGES_ID,img.HISTORYIMAGES_INFO,
            //            CONCAT('http://nsc3.negatech.vn/',img.HISTORYIMAGES_URL) as HISTORYIMAGES_URL,img.USER_ID,img.HISTORYIMAGES_TIMEEND,img.HISTORYIMAGES_TIMESTART,
            //            EXTRACT(DAY FROM HISTORYIMAGES_TIMESTART ) AS DayTimeStart,
            //            EXTRACT(MONTH FROM HISTORYIMAGES_TIMESTART ) AS MonthTimeStart,
            //            EXTRACT(YEAR FROM HISTORYIMAGES_TIMESTART ) AS YearTimeStart
            //            from TBL_HISTORY_IMAGES img where img.CAMERA_ID=:cameraId and img.USER_ID=:userId";

            string sql = @"SELECT CAMERA_ID,FUNCTION_ID,HISTORY_VIDEO_ID,HISTORYIMAGES_ID,HISTORYIMAGES_INFO,
                            CONCAT('http://nsc3.negatech.vn/',HISTORYIMAGES_URL) as HISTORYIMAGES_URL,USER_ID,HISTORYIMAGES_TIMEEND,HISTORYIMAGES_TIMESTART
                            FROM TBL_HISTORY_IMAGES  img where TRUNC(HISTORYIMAGES_TIMESTART)=
                            (SELECT Max(TRUNC(HISTORYIMAGES_TIMESTART)) DT FROM TBL_HISTORY_IMAGES 
                            where  CAMERA_ID=: cameraId and USER_ID=: userId) 
                            and CAMERA_ID=: cameraId and USER_ID=: userId order by HISTORYIMAGES_TIMESTART";

            string sqlCount = "select count(*) from TBL_HISTORY_IMAGES img where TRUNC(HISTORYIMAGES_TIMESTART)= (SELECT Max(TRUNC(HISTORYIMAGES_TIMESTART)) DT FROM TBL_HISTORY_IMAGES where  CAMERA_ID=: cameraId and USER_ID=: userId) and CAMERA_ID=: cameraId and USER_ID=: userId order by HISTORYIMAGES_TIMESTART";

            if (page == null)
            {
                page = 1;
            }
            var temp = (int)((page - 1) * rowperpage);
            lstPara.Add(new OracleParameter("cameraId", cameraId));
            lstPara.Add(new OracleParameter("userId", GetUserLogin.CurentCustomerId));
            if (day != null)
            {
                sql += " ORDER BY  (EXTRACT(DAY FROM HISTORYIMAGES_TIMESTART)) DESC";
                sqlCount += " ORDER BY  (EXTRACT(DAY FROM HISTORYIMAGES_TIMESTART)) DESC";
                lstPara.Add(new OracleParameter("dayStart", day));
            }
            if (month != null)
            {
                sql += "(EXTRACT(MONTH FROM HISTORYIMAGES_TIMESTART)) ";
                sqlCount += "(EXTRACT(MONTH FROM HISTORYIMAGES_TIMESTART)) ";
                lstPara.Add(new OracleParameter("monthStart", month));
            }
            if (functionId != null)
            {
                sql += " and img.FUNCTION_ID=:functionID";
                sqlCount += " and img.FUNCTION_ID=:functionID";
                lstPara.Add(new OracleParameter("functionID", functionId));
            }
            //var lst=new List<HistoryImageModel>();
            total = GetContext().Database.SqlQuery<int>(sqlCount, lstPara.ToArray()).FirstOrDefault();
            var lst = GetContext().Database.SqlQuery<HistoryImageModel>(sql, lstPara.ToArray()).Skip(temp).Take(rowperpage).ToList();
            return lst;
        }

        public List<HistoryImageModel> SearchImageModels(int cameraId, DateTime? fromTime, DateTime? toTime, int? functionId, out int total, int? page, int rowperpage)//, int? page, int rowperpage,
        {
            var lstPara = new List<OracleParameter>();
            string sql = @"select img.CAMERA_ID,img.FUNCTION_ID,img.HISTORY_VIDEO_ID,img.HISTORYIMAGES_ID,img.HISTORYIMAGES_INFO,
            CONCAT('http://nsc3.negatech.vn/',img.HISTORYIMAGES_URL) as HISTORYIMAGES_URL,img.USER_ID,img.HISTORYIMAGES_TIMESTART,
            EXTRACT(DAY FROM HISTORYIMAGES_TIMESTART ) AS DayTimeStart,
            EXTRACT(MONTH FROM HISTORYIMAGES_TIMESTART ) AS MonthTimeStart,
            EXTRACT(YEAR FROM HISTORYIMAGES_TIMESTART ) AS YearTimeStart
            
            from TBL_HISTORY_IMAGES img where img.CAMERA_ID=:cameraId and img.USER_ID=:userId";

            string sqlCount = "select count(*) from TBL_HISTORY_IMAGES img where img.CAMERA_ID=:cameraId and img.USER_ID=:userId";

            if (page == null)
            {
                page = 1;
            }
            var temp = (int)((page - 1) * rowperpage);
            lstPara.Add(new OracleParameter("cameraId", cameraId));
            lstPara.Add(new OracleParameter("userId", GetUserLogin.CurentCustomerId));
            if (fromTime != null)
            {
                sql += " and img.HISTORYIMAGES_TIMESTART>=:fromTime";
                sqlCount += " and img.HISTORYIMAGES_TIMESTART>=:fromTime";
                //DateTime reFromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day, fromDate.Value.Hour, fromDate.Value.Minute, fromDate.Value.Second);
                lstPara.Add(new OracleParameter("fromTime", fromTime));
            }
            //toDate = fromDate.Value.AddDays(1);
            if (toTime != null)
            {
                sql += " and img.HISTORYIMAGES_TIMESTART<=:toTime ";
                sqlCount += " and img.HISTORYIMAGES_TIMESTART<=:toTime ";
                //DateTime reToDate = new DateTime(toDate.Value.Year, toDate.Value.Month, toDate.Value.Day, toDate.Value.Hour, toDate.Value.Minute, toDate.Value.Second);
                lstPara.Add(new OracleParameter("toTime", toTime));
            }
            //if (fromTime != null)
            //{
            //    sql += " and img.HISTORYIMAGES_TIMESTART >=:fromTime";
            //    sqlCount += " and img.HISTORYIMAGES_TIMESTART >=:fromTime";
            //    //DateTime reFromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day, fromDate.Value.Hour, fromDate.Value.Minute, fromDate.Value.Second);
            //    lstPara.Add(new OracleParameter("fromTime", fromTime));
            //}

            //if (toTime != null)
            //{
            //    sql += " and img.HISTORYIMAGES_TIMESTART <=:toTime";
            //    sqlCount += " and img.HISTORYIMAGES_TIMESTART <=:toTime";
            //    //DateTime reFromDate = new DateTime(fromDate.Value.Year, fromDate.Value.Month, fromDate.Value.Day, fromDate.Value.Hour, fromDate.Value.Minute, fromDate.Value.Second);
            //    lstPara.Add(new OracleParameter("toTime", toTime));
            //}

            if (functionId != null)
            {
                sql += " and img.FUNCTION_ID=:functionID";
                sqlCount += " and img.FUNCTION_ID=:functionID";
                lstPara.Add(new OracleParameter("functionID", functionId));
            }
            sql += " Order By img.HISTORYIMAGES_TIMESTART ";
            total = GetContext().Database.SqlQuery<int>(sqlCount, lstPara.ToArray()).FirstOrDefault();

            var lst = GetContext().Database.SqlQuery<HistoryImageModel>(sql, lstPara.ToArray()).Skip(temp).Take(rowperpage).ToList();//, int? page, int rowperpage,
            return lst;
        }
        #endregion
        #region
        public List<ViewModel> GetTotalFileOfCamera()
        {
            List<ViewModel> listTotalFile = new List<ViewModel>();
            var lstPara = new List<OracleParameter>();
            var list = GetContext().TBL_CAMERA.Where(x => x.USER_ID == GetUserLogin.CurentCustomerId).ToList();
            foreach (var item in list)
            {
                var viee = new ViewModel();
                viee.CAMERA_ID = (int)item.CAMERA_ID;
                viee.CAMERA_NAME = item.CAMERA_NAME;
                string sql = "select count(*) from TBL_HISTORY_IMAGES  img where img.USER_ID=" + GetUserLogin.CurentCustomerId + "and img.CAMERA_ID=" + item.CAMERA_ID;
                viee.totaFile = GetContext().Database.SqlQuery<int>(sql).FirstOrDefault();
                listTotalFile.Add(viee);
            }
            return listTotalFile;
        }
        #endregion

        #region
        public List<ViewModel> GetTotalFileOfMonth(int cameraid)
        {
            List<ViewModel> listTotalFile = new List<ViewModel>();
            var lstPara = new List<OracleParameter>();
            for (int i = 1; i <= 12; i++)
            {
                var viee = new ViewModel();
                string sql = "select count(*) from TBL_HISTORY_IMAGES img where img.USER_ID=" + GetUserLogin.CurentCustomerId + "and img.CAMERA_ID= " + cameraid + " and EXTRACT(month FROM img.HISTORYIMAGES_TIMESTART)=" + i;
                viee.month = Convert.ToString(i);
                viee.totaFile = GetContext().Database.SqlQuery<int>(sql).FirstOrDefault();
                listTotalFile.Add(viee);
            }
            return listTotalFile;
        }
        #endregion
        #region
        public List<ViewModel> GetTotalFileOfDay(int cameraid, int month)
        {
            List<ViewModel> listTotalFile = new List<ViewModel>();
            var lstPara = new List<OracleParameter>();
            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    for (int i = 1; i <= 31; i++)
                    {
                        var viee = new ViewModel();
                        string sql = "select count(*) from TBL_HISTORY_IMAGES img where img.USER_ID=" + GetUserLogin.CurentCustomerId + "and img.CAMERA_ID= " + cameraid + " and EXTRACT(month FROM img.HISTORYIMAGES_TIMESTART)=" + month + "and EXTRACT(day FROM img.HISTORYIMAGES_TIMESTART)=" + i;
                        viee.day = Convert.ToString(i);
                        viee.totaFile = GetContext().Database.SqlQuery<int>(sql).FirstOrDefault();
                        listTotalFile.Add(viee);
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    for (int i = 1; i <= 30; i++)
                    {
                        var viee = new ViewModel();
                        string sql = "select count(*) from TBL_HISTORY_IMAGES img where img.USER_ID=" + GetUserLogin.CurentCustomerId + "and img.CAMERA_ID= " + cameraid + " and EXTRACT(month FROM img.HISTORYIMAGES_TIMESTART)=" + month + "and EXTRACT(day FROM img.HISTORYIMAGES_TIMESTART)=" + i;
                        viee.day = Convert.ToString(i);
                        viee.totaFile = GetContext().Database.SqlQuery<int>(sql).FirstOrDefault();
                        listTotalFile.Add(viee);
                    }
                    break;
                case 2:
                    for (int i = 1; i <= 29; i++)
                    {
                        var viee = new ViewModel();
                        string sql = "select count(*) from TBL_HISTORY_IMAGES img where img.USER_ID=" + GetUserLogin.CurentCustomerId + "and img.CAMERA_ID= " + cameraid + " and EXTRACT(month FROM img.HISTORYIMAGES_TIMESTART)=" + month + "and EXTRACT(day FROM img.HISTORYIMAGES_TIMESTART)=" + i;
                        viee.day = Convert.ToString(i);
                        viee.totaFile = GetContext().Database.SqlQuery<int>(sql).FirstOrDefault();
                        listTotalFile.Add(viee);
                    }
                    break;
                default:
                    break;
            }
            return listTotalFile;
        }
        #endregion
        #region
        public async Task<string> AddStream(string name, string url)
        {
            string s = "http://10.62.1.152:9999/addstream?name=" + name + "&link=" + url;
            string s1 = "http://10.62.1.152:9999/onstream?name=" + name;
            string s2 = "http://10.62.1.152:9999/addstartup?name=" + name;

            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(s);
            response.EnsureSuccessStatusCode();
            HttpResponseMessage response1 = await client.GetAsync(s1);
            response1.EnsureSuccessStatusCode();
            HttpResponseMessage response2 = await client.GetAsync(s2);
            response2.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var result1 = await response1.Content.ReadAsStringAsync();
            var result2 = await response2.Content.ReadAsStringAsync();
            return result;
        }
        #endregion

        #region
        public async Task<string> CheckBandwidth(string name)
        {
            string s = "http://10.62.1.152:9999/getbandwidth?name=" + name;
            var client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(s);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        #endregion
        #region
        public async Task<string> UpdateStream(string name, string url)
        {
            string s = "http://10.62.1.152:9999/editstream?name=" + name + "&link=" + url;

            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(s);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
        #endregion
        #region
        public async Task DeleteStream(string name)
        {
            string s = "http://10.62.1.152:9999/deletestartup?name=" + name;
            string s1 = "http://10.62.1.152:9999/offstream?name=" + name;
            string s2 = "http://10.62.1.152:9999/deletestream?name=" + name;
            var client = new HttpClient();

            HttpResponseMessage response = await client.GetAsync(s);
            response.EnsureSuccessStatusCode();
            HttpResponseMessage response1 = await client.GetAsync(s1);
            response.EnsureSuccessStatusCode();
            HttpResponseMessage response2 = await client.GetAsync(s2);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            var result1 = await response1.Content.ReadAsStringAsync();
            var result2 = await response2.Content.ReadAsStringAsync();
            //return result;
        }
        #endregion
        #region
        public static string RandomString()
        {
            Random ran = new Random();
            int i = ran.Next(0, 1000000);
            return "camera" + Convert.ToString(i);
        }
        #endregion

        #region
        public List<Coor> GetCoordinate2(int userid, int cameraid)
        {

            var list = new List<Coor>();
            string sql = "select cf.POLYGON from TBL_CAMERA_FUNCTION cf where cf.CAMERA_ID=" + cameraid + " and cf.USER_ID=" + userid;
            string data = GetContext().Database.SqlQuery<string>(sql).FirstOrDefault();
            string polygon = data.Substring(8);
            string polygon2 = polygon.Substring(0, polygon.Length - 1);
            //while(polygon.Length!=0)
            //{
            //    lis
            //}
            string[] arrListStr = polygon2.Split('|');//tách trong chuỗi str trên khi gặp ký tự ','
            //kết quả arrListStr[0]='hàm xử lý' và arrListStr[1]='xữ lý chuỗi c#'
            //string[] buf = polygon.ToCharArray();
            for (int i = 0; i < arrListStr.Length; i++)
            {
                var coor = new Coor();
                coor.x = Int32.Parse(arrListStr[i]);
                coor.y = Int32.Parse(arrListStr[i + 1]);
                list.Add(coor);
                i++;
            }

            return list;
        }
        #endregion
        #region
        public string GetCoordinate(int idfunction, int cameraid, int userid)
        {

            string sql = "select cf.POLYGON from TBL_CAMERA_FUNCTION cf where cf.CAMERA_ID=" + cameraid + " and cf.USER_ID=" + userid + "and cf.FUNCTION_ID=" + idfunction;
            string data = GetContext().Database.SqlQuery<string>(sql).FirstOrDefault();
            if (data != null)
            {
                string polygon = data.Substring(8);
                return polygon;
            }
            else
                return null;
        }
        #endregion


        public string getImgFunction(int cameraId)
        {
            string sql = "select HISTORYIMAGES_URL from TBL_HISTORY_IMAGES where HISTORYIMAGES_ID= (select max(img.HISTORYIMAGES_ID) from TBL_HISTORY_IMAGES img where img.USER_ID=" + GetUserLogin.CurentCustomerId + "and img.CAMERA_ID=" + cameraId + ")";
            string data = GetContext().Database.SqlQuery<string>(sql).FirstOrDefault();
            return data;
        }
        public string getImgCamera_Function(int cameraId, int functionId)
        {
            var data = GetContext().TBL_CAMERA_FUNCTION.FirstOrDefault(x => x.CAMERA_ID == cameraId && x.FUNCTION_ID == functionId);
            if (data != null)
            {
                return data.URL_IMAGE;
            }
            else
                return getImgFunction(cameraId);
        }
    }
}