using sendme.chai;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sendme
{

    public class CANParcel
    {
        

        #region Fields
        private byte[] mData = null;
        private UInt32 mID = 0x00;
        private DateTime mTimeStamp;
        #endregion
        #region Setters
        public void SetData(byte[] data)
        {
            mData = data;
        }

        public void SetID(UInt32 id)
        {
            mID = id;
        }

        public void SetTimeStamp(DateTime time)
        {
            mTimeStamp = time;
        }
        #endregion
        #region Getters
        public DateTime GetTimestamp()
        {
            return mTimeStamp;
        }
        #endregion

        public override String ToString()
        {
            String output = mID.ToString("X8");

            for (int i = 0; i < mData.Length; i++)
                output += " " + mData[i].ToString("X2");

            output += " " + mTimeStamp.ToString("HH:mm:ss.fff");

            return output;
        }

        unsafe public CANMsg GetCANMsg()
        {
            CANMsg msg = new CANMsg();

            msg.id = mID;
            
            for (int i = 0; i < mData.Length; i++)
                msg.data[i] = mData[i];

            msg.length = (byte) mData.Length;
            msg.flags = 0x4;

            return msg;
        }
    }
}
