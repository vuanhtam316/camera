using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Front.Models
{
    public class DateTimeModel
    {
        public List<DataDate> DataDates { get; set; }
    }
    public class DataDate : IEnumerable
    {
        public List<SelectDay> Day { get; set; }
        public bool ExitData { get; set; }
        public int Month { get; set; }
        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    public class SelectDay
    {
        public string TextDay { get; set; }
        public int Days { get; set; }
    }
}