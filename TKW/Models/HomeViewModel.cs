using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TKW.Models
{
    public class HomeViewModel
    {
        public List<Xe> XeChinh { get; set; }    // 12 xe ở phần chính
        public List<Xe> XeMoi { get; set; }      // xe mới
        public List<Xe> GiaTot { get; set; }     // xe giá tốt
        public List<Xe> Sedan { get; set; }      // xe sedan
    }

}