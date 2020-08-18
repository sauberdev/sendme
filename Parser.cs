using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sendme
{
    using System.Globalization;

    public class Parser
    {
        private static int ID_INDEX = 0;
        private static int DATA_0_INDEX = 1;
        private static int TIMESTAMP_INDEX = 9;
        public Parser(string path)
        {
            mPath = path;
        }

        private String mPath = "";

        public List<CANParcel> Parse()
        {
            List<CANParcel> parcels = new List<CANParcel>();

            if ((mPath.Length == 0) || (System.IO.File.Exists(mPath) == false))
                throw new ArgumentOutOfRangeException("File is not accessible.");

            var lines = System.IO.File.ReadAllLines(mPath);

            for (int i = 0; i < lines.Length; i++)
            {
                // Split line by tabs
                string[] strings = lines[i].Split('\t');

                try
                {
                    CANParcel parcel = new CANParcel();

                    //Parse ID
                    UInt32 id = Convert.ToUInt32(strings[ID_INDEX], 16);
                    parcel.SetID(id);

                    // Parse data
                    List<byte> data = new List<byte>();
                    for (int data_index = DATA_0_INDEX; data_index < DATA_0_INDEX + 8; data_index++)
                    {
                        if ((strings[data_index] != "") && (strings[data_index] != null))
                        {
                            byte value = Convert.ToByte(strings[data_index], 16);
                            data.Add(value);
                        }
                    }
                    parcel.SetData(data.ToArray());

                    // Parse timestamp
                    try
                    {
                        DateTime timestamp = DateTime.ParseExact(strings[TIMESTAMP_INDEX], "HH:mm:ss.fff", CultureInfo.InvariantCulture);
                        parcel.SetTimeStamp(timestamp);
                    }
                    catch (FormatException e)
                    {
                        Console.WriteLine("Failed to parse timestamp. The format of time shall be next: hh:mm:ss:ms.");
                        throw;
                    }

                    parcels.Add(parcel);

                }
                catch
                {
                    Console.WriteLine("Failed to parse CAN parcel at line " + i.ToString());                        
                }
            }

            return parcels;     
        }
    }
}
