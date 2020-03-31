using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IVMS.Models
{
    public class Logs
    {
        public DateTime ADate { get; set; }
        public int MachineNO { get; set; }
        public int EmpNumber { get; set; }
        public int VerifyMethod { get; set; }
        public DateTime PunchTime { get; set; }

    }
    public class LogCollection : List<Logs>, IEnumerable<SqlDataRecord>
    {
        IEnumerator<SqlDataRecord> IEnumerable<SqlDataRecord>.GetEnumerator()
        {
            var sqlRow = new SqlDataRecord(
                  new SqlMetaData("ADate", SqlDbType.DateTime),
                  new SqlMetaData("MachineNO", SqlDbType.Int),
                  new SqlMetaData("EmpNumber", SqlDbType.Int),
                  new SqlMetaData("VerifyMethod", SqlDbType.Int),
                  new SqlMetaData("PunchTime", SqlDbType.DateTime)
                  );

            foreach (Logs log in this)
            {
                sqlRow.SetDateTime(0, log.ADate);
                sqlRow.SetInt32(1, log.MachineNO);
                sqlRow.SetInt32(2, log.EmpNumber);
                sqlRow.SetInt32(3, log.VerifyMethod);
                sqlRow.SetDateTime(4, log.PunchTime);
                yield return sqlRow;
            }
        }
    } 
}