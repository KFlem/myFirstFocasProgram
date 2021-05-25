using System;
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

        public static void Connect(string ipaddr)
        {

            _ret = FOCAS64.cnc_allclibhndl3(ipaddr, 8193, 6, out _handle);
            

            // Write the result to the console
            if (_ret == FOCAS64.EW_OK)
            {
                Console.WriteLine("We are connected!");
            }
            else
            {
                Console.WriteLine("There was an error connecting. Return value: " + _ret);
            }


            Console.WriteLine("Handle Successfully Allocated...");
            
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
            Console.WriteLine("Length    = {0}", length );
            
            _ret = FOCAS64.cnc_rdomhistry2(_handle, s_no, _NumRecs, length, _data);

            if (_ret == FOCAS64.EW_OK)
                return "Func --> GetOperatorMessageHistory has executed successfully...exit code --> " + _ret;
            return "Func --> GetOperatorMessageHistory has FAILED with exit code --> " + _ret;
        }
        
        
        
        public static void Disconnect()
        {
            // Free the Focas handle
            _ret = FOCAS64.cnc_freelibhndl(_handle);
            
            if(_ret == FOCAS64.EW_OK)
                Console.WriteLine("Disconnect Function Executed Successfully...");

            else
                Console.WriteLine("Disconnect Function FAILED with exit code " + _ret);

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
            


        private static int CreateCommand(string queryString, string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(
                    connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Connection.Open();
                return command.ExecuteNonQuery();
            }
        }


        static void Main(string[] args)
        {
            string server = @"Server=10.0.3.3;";
            string uid = @"uid=application;";
            string password = @"password=kjhfaglk4831j*984e7kuhgfkasd98e-0p75gfdo;";
            string database = @"Initial Catalog=CNC-DATA;";
            string connString = server + database + uid + password;
            string cmd = @"SELECT ope_msg FROM all_operator_messages;";
            
            
            using (SqlConnection connection = new SqlConnection(connString))
            {
                SqlCommand command = new SqlCommand(cmd, connection);

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



            Console.WriteLine(CreateCommand(cmd, connString));
            
            
            
            
            DataContext db = new DataContext(connString);
            
            Console.WriteLine("db is a class definition of type " + db.GetType());
            
            
            Console.WriteLine("Data Context is as follows...");
            Console.WriteLine(db.Mapping.GetTables());
            
            var datamodel = db.Mapping;

            Console.WriteLine("ContextType = " + datamodel.ContextType);
            Console.WriteLine("Name = " + datamodel.DatabaseName);
            Console.WriteLine("Mapping Source = " + datamodel.MappingSource);
            Console.WriteLine("Provider Type = " + datamodel.ProviderType);

            
            foreach (var r in datamodel.GetTables())
            {
                Console.WriteLine(r.TableName);
                Console.WriteLine();
            }
    


            /*
            var query = from message in db.all_operator_messages
                        where message.machineID == 1
                        select message;

            foreach (var message in query)
                Console.WriteLine("Machine 1 has executed part {0}", message.ope_msg);
            */


            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("_data is a class definition of type " + _data.GetType());
               
            string ipaddr = "127.0.0.1";
            //string ipaddr = "10.0.2.99";
            //string ipaddr = "10.0.3.9";
            
            
            
            Connect(ipaddr);
            Console.WriteLine("MACHINE = {0}", GetMachineString(ipaddr));
            Console.WriteLine("MACHINE   MODE  = {0}", GetMode());
            Console.WriteLine("MACHINE  STATUS = {0}", GetStatus());
            StopOpHis();
            Console.WriteLine("NUM OP MESSAGES = {0}", GetOperatorMessageHistoryCOUNT());
         
            Console.WriteLine(GetOperatorMessageHistory());
            
            StartOpHis();
            Disconnect();


            Console.WriteLine("----------START OUT-----------");
            
            
            
            
            Console.WriteLine("START NO = " + _data.s_no);
            Console.WriteLine("END  NO  = " + _data.e_no);
            Console.WriteLine("---Record 1        -----------");

            Console.WriteLine("FLAG NO  = " + _data.opm_his.data1.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data1.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data1.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data1.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data1.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data1.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data1.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data1.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data2.ope_msg);

            Console.WriteLine("---Record 2        -----------");
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data2.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data2.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data2.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data2.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data2.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data2.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data2.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data2.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data2.ope_msg); 
            /*
            Console.WriteLine("---Record 3        -----------");           
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data3.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data3.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data3.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data3.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data3.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data3.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data3.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data3.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data3.ope_msg); 
            Console.WriteLine("---Record 4        -----------");           
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data4.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data4.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data4.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data4.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data4.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data4.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data4.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data4.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data4.ope_msg); 
            
            Console.WriteLine("---Record 5        -----------");
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data5.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data5.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data5.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data5.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data5.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data5.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data5.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data5.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data5.ope_msg);            
            Console.WriteLine("---Record 6        -----------");
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data6.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data6.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data6.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data6.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data6.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data6.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data6.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data6.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data6.ope_msg);            
            Console.WriteLine("---Record 7        -----------");
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data7.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data7.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data7.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data7.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data7.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data7.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data7.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data7.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data7.ope_msg);            
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data8.dsp_flg);
            Console.WriteLine("---Record 8        -----------");
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data8.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data8.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data8.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data8.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data8.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data8.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data8.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data8.ope_msg); 
            Console.WriteLine("---Record 9        -----------");           
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data9.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data9.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data9.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data9.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data9.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data9.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data9.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data9.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data9.ope_msg);            
            Console.WriteLine("---Record 10        -----------");           
            Console.WriteLine("FLAG NO  = " + _data.opm_his.data10.dsp_flg);
            Console.WriteLine("MSG  NO  = " + _data.opm_his.data10.om_no);
            Console.WriteLine("YEAR     = " + _data.opm_his.data10.year);
            Console.WriteLine("MONTH    = " + _data.opm_his.data10.month);
            Console.WriteLine("DAY      = " + _data.opm_his.data10.day);
            Console.WriteLine("HOUR     = " + _data.opm_his.data10.hour);
            Console.WriteLine("MINUTE   = " + _data.opm_his.data10.minute);
            Console.WriteLine("SECOND   = " + _data.opm_his.data10.second);
            Console.WriteLine("MESSAGE  = " + _data.opm_his.data10.ope_msg);
            Console.WriteLine("-----------END  OUT-----------");
            */

            

                 
            Console.WriteLine("End Program");
            Console.WriteLine("-------------------------------------------------------");

        }
 
    }
}
