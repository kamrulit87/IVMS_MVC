using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public class Result
    {
        public string message { get; set; }
        public bool isSucess { get; set; }
        public Int64 lastInsertedID { get; set; }
        public bool sessionOut { get; set; }

        public string SaveSuccessfull(string tableName)
        {
            return tableName + " Save Successful !!!"; ;
        }
        public string UpdateSuccessfull(string tableName)
        {
            return tableName + " Update Successful !!!";
        }
        public string DeleteSuccessfull(string tableName)
        {
            return tableName + " Delete Successful !!!";
        }
        public string Exists(string tableName)
        {
            return tableName + "This " + tableName + " Already Exists! Try Another One !!!";
        }
    }
}
