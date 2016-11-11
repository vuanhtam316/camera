using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyWeb.Areas.Agency.Models
{
    public class GetAgencyModel
    {
        public IEnumerable<AgencyModel> getAgency { get; set; }
    }
}