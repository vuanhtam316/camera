using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.ModelChart
{
    public class DataMonth
    {
        public static IEnumerable<month> LoadDataMon()
        {
            List<month> lstData = new List<month>
            {
                new month(){ ID= 1,DataImg= 0},
                new month(){ ID= 2,DataImg= 0},
                new month(){ ID= 3,DataImg= 0},
                new month(){ ID= 4,DataImg= 0},
                new month(){ ID= 5,DataImg= 0},
                new month(){ ID= 6,DataImg= 0},
                new month(){ ID= 7,DataImg= 0},
                new month(){ ID= 8,DataImg= 0},
                new month(){ ID= 9,DataImg= 0},
                new month(){ ID= 10,DataImg= 0},
                new month(){ ID= 11,DataImg= 0},
                new month(){ ID= 12,DataImg= 0},
            };
            return lstData;
        }
    }
}