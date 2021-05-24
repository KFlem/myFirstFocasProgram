using System;

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





        static void Main(string[] args)
        {
            Console.WriteLine("-------------------------------------------------------");
            Console.WriteLine("_data is a class definition of type " + _data.GetType());
               
            string ipaddr = "127.0.0.1";
            //string ipaddr = "10.0.2.99";
            //string ipaddr = "10.0.3.9";
            Connect(ipaddr);

            Console.WriteLine("MACHINE   MODE  = {0}", GetMode());
            Console.WriteLine("MACHINE  STATUS = {0}", GetStatus());
            StopOpHis();
            Console.WriteLine("NUM OP MESSAGES = {0}", GetOperatorMessageHistoryCOUNT());
            
            Console.WriteLine(GetOperatorMessageHistory());
            
            StartOpHis();

            Console.WriteLine(_data.s_no);
            Console.WriteLine(_data.e_no);
            Console.WriteLine(_data.opm_his.data1.dsp_flg);
            Console.WriteLine(_data.opm_his.data1.om_no);
            Console.WriteLine(_data.opm_his.data1.year);
            Console.WriteLine(_data.opm_his.data1.month);
            Console.WriteLine(_data.opm_his.data1.day);
            Console.WriteLine(_data.opm_his.data1.hour);
            Console.WriteLine(_data.opm_his.data1.minute);
            Console.WriteLine(_data.opm_his.data1.second);
            Console.WriteLine(_data.opm_his.data1.ope_msg);

            

            Disconnect();
                 
            Console.WriteLine("End Program");
            Console.WriteLine("-------------------------------------------------------");

        }
 
    }
}
