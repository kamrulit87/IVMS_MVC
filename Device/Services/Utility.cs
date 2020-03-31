using BLL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;


namespace Device.Services
{
    public class Utility
    {
        CZKEM objCZKEM;
        string message = string.Empty;
        //bool isConnected = false;      
        private Result result = new Result();
      
       
        public Result Connect( string ip, int machineNo, int portNo)
        {
            objCZKEM = new CZKEM();
            try
            {
                result.isSucess = objCZKEM.Connect_Net(ip, 4370);         
            }
            catch (Exception ex)
            {
               result.message = ex.Message;
            }
            return result;
        }

        public Result AssignToDevice(int deviceNO, string cardID, string cardNumber)
        {
            string sName = "Visitor";
            string sPassword = "0";
            int iPrivilege = 0;
            bool bEnabled = true;

            result.isSucess = objCZKEM.EnableDevice(deviceNO, false);

            objCZKEM.set_CardNumber(0, Convert.ToInt32(cardNumber));

            result.isSucess = objCZKEM.SSR_SetUserInfo(deviceNO, cardID, sName, sPassword, iPrivilege, bEnabled);

            result.isSucess = objCZKEM.EnableDevice(deviceNO, true);
            result.isSucess = objCZKEM.RefreshData(deviceNO);
            result.message = "Successful";

            return result;
        }

        public Result UnAssignFromDevice(int deviceNO, int cardID)
        {
            string sName = "Visitor";
            string sPassword = "0";
            int iPrivilege = 0;
            bool bEnabled = true;
            int cardNumber = 0;

            result.isSucess = objCZKEM.EnableDevice(deviceNO, false);
            objCZKEM.set_CardNumber(0, cardNumber);
            result.isSucess = objCZKEM.SSR_SetUserInfo(deviceNO, cardID.ToString(), sName, sPassword, iPrivilege, bEnabled);

            result.isSucess = objCZKEM.EnableDevice(deviceNO, true);
            result.isSucess = objCZKEM.RefreshData(deviceNO);
            result.message = "Successful";

            return result;            
        }
    }
}
