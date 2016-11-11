using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyWeb.Models
{
    public class UserInfoModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        
        public string FullName { get; set; }

        
        public string DisplayName { get; set; }

        
        public string Email { get; set; }

        
        public string UserName { get; set; }

        
        
        public string Password { get; set; }

        //[DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        //[Required(ErrorMessage = "Vui lòng nhập Password")]
        //[Display(Name = "ConfigPassword")]
        public string ConfigPassword { get; set; }
    }
}