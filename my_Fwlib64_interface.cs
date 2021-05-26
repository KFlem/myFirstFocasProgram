/*-------------------------------------------------------------------*/
/* FWLIB64.cs                                                        */
/*                                                                   */
/* CNC/PMC Data Window Library for FOCAS1/Ethernet                   */
/*                                                                   */
/* Copyright (C) 2002-2014 by FANUC CORPORATION All rights reserved. */
/*                                                                   */
/* kfleming@watsonmills.com mod start 5.21.2021  --- end ##.##.####  */
/*                                                                   */
/*-------------------------------------------------------------------*/

#define LEAVE_OLD_STYLE

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Collections;  

public class FOCAS64
{
    /* Axis define */
    public const short MAX_AXIS = 8;
    public const short ALL_AXES     = (-1);
    public const short ALL_SPINDLES = (-1);
    public const short EW_OK        = (short)focas_ret.EW_OK;

    /* Error Codes */
    public enum focas_ret {
        EW_PROTOCOL =     (-17),           /* protocol error */
        EW_SOCKET   =     (-16),           /* Windows socket error */
        EW_NODLL    =     (-15),           /* DLL not exist error */
        EW_BUS      =     (-11),           /* bus error */
        EW_SYSTEM2  =     (-10),           /* system error */
        EW_HSSB     =     (-9) ,           /* hssb communication error */
        EW_HANDLE   =     (-8) ,           /* Windows library handle error */
        EW_VERSION  =     (-7) ,           /* CNC/PMC version missmatch */
        EW_UNEXP    =     (-6) ,           /* abnormal error */
        EW_SYSTEM   =     (-5) ,           /* system error */
        EW_PARITY   =     (-4) ,           /* shared RAM parity error */
        EW_MMCSYS   =     (-3) ,           /* emm386 or mmcsys install error */
        EW_RESET    =     (-2) ,           /* reset or stop occured error */
        EW_BUSY     =     (-1) ,           /* busy error */
        EW_OK       =     0    ,           /* no problem */
        EW_FUNC     =     1    ,           /* command prepare error */
        EW_NOPMC    =     1    ,           /* pmc not exist */
        EW_LENGTH   =     2    ,           /* data block length error */
        EW_NUMBER   =     3    ,           /* data number error */
        EW_RANGE    =     3    ,           /* address range error */
        EW_ATTRIB   =     4    ,           /* data attribute error */
        EW_TYPE     =     4    ,           /* data type error */
        EW_DATA     =     5    ,           /* data error */
        EW_NOOPT    =     6    ,           /* no option error */
        EW_PROT     =     7    ,           /* write protect error */
        EW_OVRFLOW  =     8    ,           /* memory overflow error */
        EW_PARAM    =     9    ,           /* cnc parameter not correct error */
        EW_BUFFER   =     10   ,           /* buffer error */
        EW_PATH     =     11   ,           /* path error */
        EW_MODE     =     12   ,           /* cnc mode error */
        EW_REJECT   =     13   ,           /* execution rejected error */
        EW_DTSRVR   =     14   ,           /* data server error */
        EW_ALARM    =     15   ,           /* alarm has been occurred */
        EW_STOP     =     16   ,           /* CNC is not running */
        EW_PASSWD   =     17   ,           /* protection data error */
    /*
        Result codes of DNC operation
    */
        DNC_NORMAL  =  (-1)    ,           /* normal completed */
        DNC_CANCEL  =  (-32768),           /* DNC operation was canceled by CNC */
        DNC_OPENERR =  (-514)  ,           /* file open error */
        DNC_NOFILE  =  (-516)  ,           /* file not found */
        DNC_READERR =  (-517)              /* read error */
    };
    
/*-------------*/
/* CNC: Others */
/*-------------*/
 
    /* free library handle */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_freelibhndl")]
    public static extern short cnc_freelibhndl( ushort FlibHndl );

        /* read CNC status information */
    [DllImport("FWLIB64.dll", EntryPoint="cnc_statinfo")]
    public static extern short cnc_statinfo( ushort FlibHndl, [Out,MarshalAs(UnmanagedType.LPStruct)] ODBST a );



/*-------------------------------------*/
/* CNC: Operation history data related */
/*-------------------------------------*/

    /* stop logging operation history data */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_stopophis")]
    public static extern short cnc_stopophis( ushort FlibHndl );

    /* restart logging operation history data */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_startophis")]
    public static extern short cnc_startophis( ushort FlibHndl );

    /* stop logging operation history data */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_stopomhis")]
    public static extern short cnc_stopomhis( ushort FlibHndl );

    /* restart logging operation history data */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_startomhis")]
    public static extern short cnc_startomhis( ushort FlibHndl );

    /* read number of operater message history data */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_rdomhisno")]
    public static extern short cnc_rdomhisno( ushort FlibHndl, out ushort a );


    /* read operater message history data F30i */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_rdomhistry2")]
    public static extern short cnc_rdomhistry2( ushort FlibHndl,
        ushort a, ushort b, ushort c, [Out,MarshalAs(UnmanagedType.LPStruct)] ODBOMHIS2 d );


/*---------------------*/
/* Ethernet connection */
/*---------------------*/

    /* allocate library handle 3 */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_allclibhndl3")]
    public static extern short cnc_allclibhndl3( [In,MarshalAs(UnmanagedType.AsAny)] Object ip,
        ushort port,int timeout, out ushort FlibHndl);

    /* allocate library handle 4 */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_allclibhndl4")]
    public static extern short cnc_allclibhndl4( [In,MarshalAs(UnmanagedType.AsAny)] Object ip,
        ushort port,int timeout, uint id, out ushort FlibHndl);

    /* set timeout for socket */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_settimeout")]
    public static extern short cnc_settimeout( ushort FlibHndl, int a );

    /* reset all socket connection */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_resetconnect")]
    public static extern short cnc_resetconnect( ushort FlibHndl );

    /* get option state for FOCAS1/Ethernet */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_getfocas1opt")]
    public static extern short cnc_getfocas1opt( ushort FlibHndl, short a, out int b );

    /* read Ethernet board information */
    [DllImport("Fwlib64.dll", EntryPoint="cnc_rdetherinfo")]
    public static extern short cnc_rdetherinfo( ushort FlibHndl, out short a, out short b );

    /* ---------------------------------------- */
    /* ---------------------------------------- */
    /* cnc_rdomhistry2:read operater message history data */
    [StructLayout( LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=4)]
    public class ODBOMHIS2_data : IEnumerable
    {
        public short   dsp_flg;     /* Dysplay flag(ON/OFF) */
        public short   om_no;       /* operater message number */
        public short   year;        /* year */
        public short   month;       /* month */
        public short   day;         /* day */
        public short   hour;        /* Hour */
        public short   minute;      /* Minute */
        public short   second;      /* Second */
        [MarshalAs(UnmanagedType.ByValTStr,SizeConst=256)]
        public string  ope_msg = new string(' ',256) ;  /* operator message message */

        public IEnumerator GetEnumerator()
        {
            yield return dsp_flg;
            yield return om_no;
            yield return year;
            yield return month;
            yield return day;
            yield return hour;
            yield return minute;
            yield return second;
            yield return ope_msg;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }



    }   




    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class OPM_HIS
    {
        public ODBOMHIS2_data   data1 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data2 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data3 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data4 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data5 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data6 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data7 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data8 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data9 = new ODBOMHIS2_data();
        public ODBOMHIS2_data   data10 = new ODBOMHIS2_data();
        
    }



    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBOMHIS2
    {   

        public ushort  s_no;   /* start number */
        
        public ushort  e_no;   /* end number */
        
        public OPM_HIS opm_his = new OPM_HIS();

    }

    /* ---------------------------------------- */






    /* ---------------------------------------- */
    /* cnc_statinfo:read CNC status information */
    [StructLayout(LayoutKind.Sequential,Pack=4)]
    public class ODBST
    {
    public short  hdck ;        /* Status of manual handle re-trace */
    public short  tmmode ;      /* T/M mode selection              */
    public short  aut ;         /* AUTOMATIC/MANUAL mode selection */
    public short  run ;         /* Status of automatic operation   */
    public short  motion ;      /* Status of axis movement,dwell   */
    public short  mstb ;        /* Status of M,S,T,B function      */
    public short  emergency ;   /* Status of emergency             */
    public short  alarm ;       /* Status of alarm                 */
    public short  edit ;        /* Status of program editing       */

    }
    /* ---------------------------------------- */

}
