using System;
using System.Linq;
using System.Reflection;
using System.Data.Linq;
using System.Data.SqlClient;

namespace myFirstFocasProgram
{
    class Program
    {

        private static FOCAS64.ODBOMHIS2 _data = new FOCAS64.ODBOMHIS2();

        private static ushort _handle;
        private static short _ret;

        private static ushort _NumRecs;

        public static void Connect_CNC(string ipaddr)
        {

            _ret = FOCAS64.cnc_allclibhndl3(ipaddr, 8193, 6, out _handle);
            

            // Write the result to the console
            if (_ret == FOCAS64.EW_OK)
            {
                Console.WriteLine("\nConnect_CNC function executed successfully...");
            }
            else
            {
                Console.WriteLine("\nThere was an error connecting. Return value: " + _ret);
            }


            
        }
        public static string GetMode()
        {
                short _ret = 0;

            // Check we have a valid handle
            if (_handle == 0)
                return "UNAVAILABLE";

            // Create an instance of our structure
            FOCAS64.ODBST mode = new FOCAS64.ODBST();

            // Ask Fanuc for the status information
            _ret = FOCAS64.cnc_statinfo(_handle, mode);

            // Check to make sure the call was successfull
            // and convert the mode to a string and return it.
            if (_ret == FOCAS64.EW_OK)
                return GetModeString(mode.aut);
            return "UNAVAILABLE";
        }
        public static string GetStatus()
        {
            if (_handle == 0)
                return "UNAVAILABLE";

            FOCAS64.ODBST status = new FOCAS64.ODBST();

            _ret = FOCAS64.cnc_statinfo(_handle, status);

            if (_ret == FOCAS64.EW_OK)
                return GetStatusString(status.run);
            return "UNAVAILABLE";
        }

        private static string GetModeString(short mode)
        {
            switch (mode)
            {
                case 0:
                    {
                        return "MDI";
                    }
                case 1:
                    {
                        return "MEM";
                    }
                case 2:
                    {
                        return "****";
                    }
                case 3:
                    {
                        return "EDIT";
                    }
                case 4:
                    {
                        return "HND";
                    }
                case 5:
                    {
                        return "JOG";
                    }
                case 6:
                    {
                        return "T-JOG";
                    }
                case 7:
                    {
                        return "T-HND";
                    }
                case 8:
                    {
                        return "INC";
                    }
                case 9:
                    {
                        return "REF";
                    }
                case 10:
                    {
                        return "RMT";
                    }
                default:
                    {
                        return "UNAVAILABLE";
                    }
            }
        }
        private static string GetStatusString(short status)
        {
            switch (status)
            {
                case 0:
                    {
                        return "****";
                    }
                case 1:
                    {
                        return "STOP";
                    }
                case 2:
                    {
                        return "HOLD";
                    }
                case 3:
                    {
                        return "START";
                    }
                case 4:
                    {
                        return "MSTR";
                    }

                default:
                    {
                        return "UNAVAILABLE";
                    }
            }
        }
        public static void StopOpHis()
        {
            // Free the Focas handle
            FOCAS64.cnc_stopophis(_handle);
            Console.WriteLine("OpHist Stopped Successfully...");
        }

        public static void StartOpHis()
        {
            // Free the Focas handle
            FOCAS64.cnc_startophis(_handle);
            Console.WriteLine("OpHist Started Successfully...");

       
        }






        
        public static string GetOperatorMessageHistoryCOUNT()
        {
            if (_handle == 0)
                return "UNAVAILABLE";


            _ret = FOCAS64.cnc_rdomhisno(_handle, out _NumRecs);

            
            if (_ret == FOCAS64.EW_OK)
                return "Func --> GetOperatorMessageHistoryCOUNT has executed successfully...";
            
            return "Func --> GetOperatorMessageHistoryCOUNT has FAILED with exit code --> " + _ret;



        }
        

        public static string GetOperatorMessageHistory()
        {
            if (_handle == 0)
                return "UNAVAILABLE";


            ushort s_no = 1;

            ushort length = (ushort)(4 + 272 * _NumRecs);
            
            
            //FOCAS64.ODBOMHIS2 data = new FOCAS64.ODBOMHIS2(_NumRecs);
            FOCAS64.ODBOMHIS2 data = new FOCAS64.ODBOMHIS2();

            
            Console.WriteLine("Handle    = {0}", _handle );
            Console.WriteLine("Start num = {0}", s_no );
            Console.WriteLine("End Num   = {0}", _NumRecs );
            Console.WriteLine("Data Block Length = {0}", length );
            
            _ret = FOCAS64.cnc_rdomhistry2(_handle, s_no, _NumRecs, length, _data);

            if (_ret == FOCAS64.EW_OK)
                return "Func --> GetOperatorMessageHistory has executed successfully...exit code --> " + _ret;
            return "Func --> GetOperatorMessageHistory has FAILED with exit code --> " + _ret;
        }
        
        
        
        public static void Disconnect_CNC()
        {
            // Free the Focas handle
            _ret = FOCAS64.cnc_freelibhndl(_handle);
            
            if(_ret == FOCAS64.EW_OK)
                Console.WriteLine("Disconnect_CNC Function Executed Successfully...");

            else
                Console.WriteLine("Disconnect_CNC Function FAILED with exit code " + _ret);

        }

        public static string GetMachineString(string ip)
        {
            if (ip == "127.0.0.1")
                return "ncGuide on Localhost";
            else if (ip == "10.0.2.99")
                return "EXXACT-OLD";
            else if (ip == "10.0.3.9")
                return "STRATOS-NEW";
            else
                return "No Ip Address Provided... ABORTING ...";
        }
            



        static void Main(string[] args)
        {

            Console.WriteLine("\n-------------------------------------------------------");

            string server = @"Server=10.0.3.3;";
            string uid = @"uid=application;";
            string password = @"password=kjhfaglk4831j*984e7kuhgfkasd98e-0p75gfdo;";
            string database = @"Initial Catalog=CNC_DATA;";

            string connString = server + database + uid + password;

            string cmd1 = @"SELECT Ope_msg FROM OperatorMessages;";
            
            /*
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(cmd1, connection);

                connection.Open();
                Console.WriteLine("***************************************************************");
                Console.WriteLine("Inside the Block that is using the connection");
                Console.WriteLine("***************************************************************");

                Console.WriteLine(connString);
                using(SqlDataReader reader = command.ExecuteReader())
                {
                    Console.WriteLine("***************************************************************");
                    while (reader.Read())
                    {
                     
                        Console.WriteLine(String.Format("{0}", reader[0]));
                    }
                }

            }
            */
            
            //DataContext db = new DataContext(connString);
   
            //Console.WriteLine("db is a class definition of type " + db.GetType());


            //Console.WriteLine("-------------------------------------------------------");
            //Console.WriteLine("_data is a class definition of type " + _data.opm_his.GetType());

            /*
            foreach (var rec in _data.opm_his)
            {
                Console.WriteLine(rec.GetType());
            }
            */


            string ipaddr = "127.0.0.1";
            //string ipaddr = "10.0.2.99";
            //string ipaddr = "10.0.3.9";
            
            
            
            
            Connect_CNC(ipaddr);
            GetOperatorMessageHistoryCOUNT();
            Console.WriteLine("\n===============");
            Console.WriteLine("MACHINE = {0}", GetMachineString(ipaddr));
            Console.WriteLine("MACHINE   MODE  = {0}", GetMode());
            Console.WriteLine("MACHINE  STATUS = {0}", GetStatus());
            StopOpHis();
            GetOperatorMessageHistoryCOUNT();
            Console.WriteLine("NUM OP MESSAGES = {0}", _NumRecs );
            Console.WriteLine("===============\n");

            Console.WriteLine(GetOperatorMessageHistory());




            StartOpHis();
            Disconnect_CNC();

            Type t1 = _data.GetType();
            //int l1 = _data

            Type t2 = _data.opm_his.GetType();
            Type t3 = _data.opm_his.data1.GetType();
            Type t4 = _data.opm_his.data1.dsp_flg.GetType();
            var t4Val = _data.opm_his.data1.dsp_flg;
            Type t5 = _data.opm_his.data1.om_no.GetType();
            var t5Val = _data.opm_his.data1.om_no;

        


            Console.WriteLine("=======================================================\n");
            Console.WriteLine("t1 type = " + t1);
            Console.WriteLine("t2 type = " + t2);
            Console.WriteLine("t3 type = " + t3);
            Console.WriteLine("t4 type = " + t4);
            Console.WriteLine("t4 value = " + t4Val);
            Console.WriteLine("t5 type = " + t5);
            Console.WriteLine("t5 value = " + t5Val);
            _data.opm_his.data1.om_no = 9;
            Console.WriteLine("t5 value = " + _data.opm_his.data1.om_no);

            foreach (PropertyInfo pi in t1.GetProperties())
            {
                Console.WriteLine("Write line --> " + pi);
            }

            PropertyInfo[] properties = _data.opm_his.data1.GetType().GetProperties();
            int LENGTH = properties.Length;
            
            foreach (var i in _data.opm_his.data1)
            {
                Console.WriteLine(i);
            }

            
            foreach (var i in _data.opm_his.data2)
            {
                Console.WriteLine(i);
            }


            
            Console.WriteLine("Length of class properties is = " + LENGTH);
            foreach (PropertyInfo pi in properties)
            {
                Console.WriteLine(pi);
            }
            
            Console.WriteLine("\n----------START OUT-----------");
            /*
            foreach (int value in Enumerable.Range(1, _NumRecs))
            {
                Console.Write("\nLoop -- " + value + "  ");
            }

            // for item in array loop through the structure
            Console.WriteLine("\n\n---Record 1        -----------");

            Console.WriteLine("FLAG NO  = " + _data.opm_his.data1.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data1.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data1.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data1.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data1.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data1.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data1.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data1.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data1.ope_msg);
            */
            Console.WriteLine("End Program");
            Console.WriteLine("-------------------------------------------------------\n");
            Console.WriteLine("-------------------------------------------------------\n");
            
        }
 
    }
}
