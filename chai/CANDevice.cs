using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace sendme.chai
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe public struct CANMsg
    {
        public UInt32 id;
        public fixed byte data[8];
        public byte length;
        public UInt16 flags;
        public UInt32 ts;
    }

    public class CANDevice
    {
        [System.Runtime.InteropServices.DllImport("chai.dll", EntryPoint = "CiInit")]
        public static extern Int16 CiInit();

        [System.Runtime.InteropServices.DllImport("chai.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int16 CiOpen(byte chan, byte flags);

        [System.Runtime.InteropServices.DllImport("chai.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int16 CiClose(byte chan);

        [System.Runtime.InteropServices.DllImport("chai.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int16 CiStart(byte chan);
        [System.Runtime.InteropServices.DllImport("chai.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int16 CiStop(byte chan);

        [System.Runtime.InteropServices.DllImport("chai.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern Int16 CiSetBaud(byte chan, byte bt0, byte bt1);

        [System.Runtime.InteropServices.DllImport("chai.dll", CallingConvention = CallingConvention.Cdecl)]

        unsafe public static extern Int16 CiWrite(byte chan, CANMsg* buffer, UInt16 cnt);

    }
}
