using Microsoft.AspNet.SignalR;
using MyWeb.Models;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MyWeb.Business
{
    public class NotificationComponent
    {
        public void RegisterNotification(DateTime currentTime)
        {
            string constr = @"User Id=REMOTECAMERA;Password=11012253;Data Source=10.62.1.150:1521/camera";
            //string constr = ConfigurationManager.ConnectionStrings["Entities1"].ConnectionString;
            var userid = GetUserLogin.CurentCustomerId;
            string sqlCommand = @"select img.HISTORYIMAGES_TIMESTART, img.CAMERA_ID from TBL_HISTORY_IMAGES img where img.USER_ID= " + userid + " and img.HISTORYIMAGES_TIMESTART > '@HISTORYIMAGES_TIMESTART'";
            using (OracleConnection con = new OracleConnection(constr))
            {

                OracleCommand cmd = new OracleCommand(sqlCommand, con);
                cmd.Parameters.Add("@HISTORYIMAGES_TIMESTART", OracleDbType.TimeStamp, 6).Value = currentTime;
                //cmd.AddRowid = true;
                if (con.State != System.Data.ConnectionState.Open)
                {
                    con.Open();
                }
                cmd.Notification = null;
                OracleDependency sqlDep = new OracleDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;
                cmd.ExecuteNonQuery();


            }
        }

        void sqlDep_OnChange(object sender, OracleNotificationEventArgs eventArgs)
        {
            //var userid = GetUserLogin.CurentCustomerId;
            if (eventArgs.Type == OracleNotificationType.Change)
            {
                OracleDependency sqlDep = sender as OracleDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<notificationHub>();
                notificationHub.Clients.All.notify("added");
                RegisterNotification(DateTime.Now);
            }
        }

        public List<TBL_HISTORY_IMAGES> GetInfoImages(DateTime afterDate)
        {
            using (Entities1 en = new Entities1())
            {
                return en.TBL_HISTORY_IMAGES.Where(x => x.HISTORYIMAGES_TIMESTART > afterDate && x.USER_ID == GetUserLogin.CurentCustomerId).OrderByDescending(x => x.HISTORYIMAGES_TIMESTART).ToList();
            }
        }
    }
}