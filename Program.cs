using System;

namespace myFirstFocasProgram
{
    class Program
    {
        private static ushort _handle;
        private static short _ret;
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


            Console.WriteLine("Hello World-Allocate Handle Successful");
            
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
        public static void Disconnect()
        {
            // Free the Focas handle
            FOCAS64.cnc_freelibhndl(_handle);
       
        }
        static void Main(string[] args)
        {
            string ipaddr = "127.0.0.1";

            Console.WriteLine("Start Program");

            Connect(ipaddr);

            
            string mode = GetMode();
            Console.WriteLine("MODE = " + mode);

            string status = GetStatus();
            Console.WriteLine("STATUS = " + status);
            
            
            
            
            
            Console.WriteLine("Hello World-2222");

            Disconnect();

            
            Console.WriteLine("Handle Successfully Destroyed...");
            Console.WriteLine("End Program");
        }
    }
}
