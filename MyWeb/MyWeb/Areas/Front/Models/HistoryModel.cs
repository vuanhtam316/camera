using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyWeb.Models;

namespace MyWeb.Areas.Front.Models
{
    public class HistoryModel
    {
        //public List<ImagesHistory> ImagesHistory { get; set; }
        //public List<VideoHistory> VideoHistory { get; set; }
        //public Function GetFunction { get; set; } 
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public List<CameraModel> ListCameras { get; set; }
    }

    public class ImagesHistory
    {
        public int CameraId { get; set; }
        public int UserId { get; set; }
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
        public DateTime? TimeStart { get; set; }
        public DateTime? TimeEnd { get; set; }
        public int? DayTimeStart { get; set; }
        public int? MonthTimeStart { get; set; }
        public int? YearTimeStart { get; set; }
        public int? DayTimeEnd { get; set; }
        public int? MonthTimeEnd { get; set; }
        public int? YearTimeEnd { get; set; }
        public int? FuntionId { get; set; }
        public string Info { get; set; }

    }

    public class HistoryImageModel
    {
        public int CAMERA_ID { get; set; }
        public int? FUNCTION_ID { get; set; }
        public int HISTORYIMAGES_ID { get; set; }
        public string HISTORYIMAGES_INFO { get; set; }
        public string HISTORYIMAGES_URL { get; set; }
        public int USER_ID { get; set; }
        public DateTime? HISTORYIMAGES_TIMEEND { get; set; }
        public DateTime? HISTORYIMAGES_TIMESTART { get; set; }
        public int? DAYTIMESTART { get; set; }
        public int? MONTHTIMESTART { get; set; }
        public int? YEARTIMESTART { get; set; }

    }

    public class VideoHistory
    {
        public int CameraId { get; set; }
        public int UserId { get; set; }
        public int VideoId { get; set; }
        public string VideoUrl { get; set; }
    }

    public class ListCamera
    {
        public int CameraId { get; set; }
        public string CameraName { get; set; }
        public string CameraUrl { get; set; }
        public string CameraCode { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
    }
    public class Function
    {
        public int FunctionId { get; set; }
        public List<SelectListItem> GetListItems { get; set; }
    }
}