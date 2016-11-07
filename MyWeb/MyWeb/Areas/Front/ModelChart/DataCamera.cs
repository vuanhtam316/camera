using MyWeb.Areas.Front.Models;
using MyWeb.Business;
using MyWeb.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace MyWeb.Areas.Front.ModelChart
{
    public class DataCamera : ProviderControllerBase
    {
        //public  List<CameraModel> GetCamera()
        //{
        //    var list = GetContext().
        //}
        #region
        //public static List<ViewModel> GetTotalFileOFCamera()
        //{
        //    List<ViewModel> listTotalFile = new List<ViewModel>();
        //    var lstPara = new List<OracleParameter>();

        //    var list = GetContext().TBL_CAMERA.Where(z => z.USER_ID == GetUserLogin.CurentCustomerId).GroupJoin(GetContext().TBL_CAMERA_STATUS, n => n.CAMERA_ID, x => x.CAMERA_ID,
        //        (n, x) => new
        //        {
        //            n,
        //            x
        //        }).ToList();

        //    string sql = "select count(*) from TBL_HISTORY_IMAGES  img ";

        //    foreach (var item in list)
        //    {
        //        var viee = new ViewModel();
        //        viee.CAMERA_ID = (int)item.n.CAMERA_ID;
        //        viee.CAMERA_NAME = item.n.CAMERA_NAME;
        //        foreach (var i in item.x)
        //        {
        //            lstPara.Add(new OracleParameter("cameraId", item.n.CAMERA_ID));
        //            lstPara.Add(new OracleParameter("userId", GetUserLogin.CurentCustomerId));
        //            sql += " where img.USER_ID=: cameraId and img.CAMERA_ID=: userId";
        //            viee.totalFIle = GetContext().Database.SqlQuery<int>(sql, lstPara.ToArray()).FirstOrDefault();
        //        }
        //        listTotalFile.Add(viee);
        //    }
        //    return listTotalFile;
        //}


        #endregion
    }
}