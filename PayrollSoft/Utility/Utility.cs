using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using PayrollSoft.Models;

namespace PayrollSoft.Utility
{
    public class UtilityBase
    {
        public void WriteToLog(string textcontent)
        {

            string path = AppDomain.CurrentDomain.BaseDirectory + "ErrorLog.txt";

            using (var tw = new StreamWriter(path, true))
            {
                tw.WriteLine("{0}", DateTime.Now.ToLocalTime());
                tw.WriteLine("    :{0}", textcontent);
                tw.WriteLine("-----------------------------------------------------------");
                tw.Close();
            }

        }

        public int GetRecordStatusId(string status)
        {
            int id = 0;
            using (var context = new DataContext())
            {

                try
                {
                    RecordStatus recordstat = context.RecordStatuses.FirstOrDefault(x => x.Description == status);
                    id = recordstat.RecordStatusId;
                }
                catch (Exception e)
                {
                    WriteToLog(e.Message.ToString() + "" + e.Source.ToString());
                }
            }


            return id;
        }
    }
}