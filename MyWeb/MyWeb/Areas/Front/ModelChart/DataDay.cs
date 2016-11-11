using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.ModelChart
{
    public class DataDay
    {
        public static IEnumerable<Day> LoadDataDay()
        {
            List<Day> lstDay = new List<Day>(){
                new Day(){ID=1,DataDay=0},
                new Day(){ID=2,DataDay=0},
                new Day(){ID=3,DataDay=0},
                new Day(){ID=4,DataDay=0},
                new Day(){ID=5,DataDay=0},
                new Day(){ID=6,DataDay=0},
                new Day(){ID=7,DataDay=0},
                new Day(){ID=8,DataDay=0},
                new Day(){ID=9,DataDay=0},
                new Day(){ID=10,DataDay=0},
                new Day(){ID=11,DataDay=0},
                new Day(){ID=12,DataDay=0},
                new Day(){ID=13,DataDay=0},
                new Day(){ID=14,DataDay=0},
                new Day(){ID=15,DataDay=0},
                new Day(){ID=16,DataDay=0},
                new Day(){ID=17,DataDay=0},
                new Day(){ID=18,DataDay=0},
                new Day(){ID=19,DataDay=0},
                new Day(){ID=20,DataDay=0},
                new Day(){ID=21,DataDay=0},
                new Day(){ID=22,DataDay=0},
                new Day(){ID=23,DataDay=0},
                new Day(){ID=24,DataDay=0},
                new Day(){ID=25,DataDay=0},
                new Day(){ID=26,DataDay=0},
                new Day(){ID=27,DataDay=0},
                new Day(){ID=28,DataDay=0},
                new Day(){ID=29,DataDay=0},
                new Day(){ID=30,DataDay=0},
                new Day(){ID=31,DataDay=0},
                };

            return lstDay;
        }
    }
}