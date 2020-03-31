using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Models
{
    public class ParametersCollectionModel
    {
        public int? lastInsertedID { get; set; }
        public int? InstallmentID { get; set; }
        public string Discount { get; set; }
    }
}