//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_HISTORY_VIDEO
    {
        public TBL_HISTORY_VIDEO()
        {
            this.TBL_HISTORY_IMAGES = new HashSet<TBL_HISTORY_IMAGES>();
        }
    
        public long HISTORYVIDEO_ID { get; set; }
        public Nullable<System.DateTime> HISTORYVIDEO_TIMESTART { get; set; }
        public Nullable<System.DateTime> HISTORYVIDEO_TIMEEND { get; set; }
        public long USER_ID { get; set; }
        public long CAMERA_ID { get; set; }
        public string HISTORYVIDEO_URL { get; set; }
    
        public virtual TBL_CAMERA TBL_CAMERA { get; set; }
        public virtual ICollection<TBL_HISTORY_IMAGES> TBL_HISTORY_IMAGES { get; set; }
        public virtual TBL_USER TBL_USER { get; set; }
    }
}
