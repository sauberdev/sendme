using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace sendme
{
    class Program
    {
        static void Main(string[] args)
        {
            // Parse arguments
            Arguments parsedArgs = null;
            try
            {
                parsedArgs = ParseArguments(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Terminate();
            }

            byte canChannel = (byte)parsedArgs.GetChannel();
            // Init chai device
            try
            {
        
                // CiInit
                var chaiRes = chai.CANDevice.CiInit();
                if (chaiRes != 0)
                    throw new InvalidOperationException("Failed init CAN Device. ");

                // CiOpen
                chaiRes = chai.CANDevice.CiOpen((byte) parsedArgs.GetChannel(), 0x4);
                if (chaiRes != 0)
                    throw new InvalidOperationException("Failed open CAN Device. ");

                // CiSetBaud
                chaiRes =  chai.CANDevice.CiSetBaud(canChannel, 0x01, 0x1C);
                if (chaiRes != 0)
                    throw new InvalidOperationException("Failed set baudrate. ");
                // CiWriteTout
                // TODO: Add missing implementation

                // CiStart
                chaiRes = chai.CANDevice.CiStart(canChannel);
                if (chaiRes != 0)
                    throw new InvalidOperationException("Failed start CAN channel. ");
            }
            catch (Exception e)
            {
                // CiStop
                chai.CANDevice.CiStop((byte) parsedArgs.GetChannel());
                // CiClose
                chai.CANDevice.CiClose(canChannel);

                Console.WriteLine(e.ToString());
                Terminate();
            }

            // Parse Excel sheet
            Parser parser = new Parser(parsedArgs.GetPath());
            var parcels = parser.Parse();

            TransmitParcels(parcels, canChannel);                

            // Clean up
            // CiStop
            chai.CANDevice.CiStop(canChannel);
            // CiClose
            chai.CANDevice.CiClose(canChannel);

            Terminate();
        }

        unsafe static void TransmitParcels(List<CANParcel> parcels, byte chanNum)
        {
            if (parcels.Count == 0)
            {
                Console.WriteLine("Nothng to transmit. Log file is empty.");
                return;
            }

            DateTime curTime = parcels[0].GetTimestamp();
            Int16 res = 0;
            for (int i = 0; i < parcels.Count; i++)
            {
                Console.WriteLine("Sending " + parcels[i].ToString());
                chai.CANMsg msg = parcels[i].GetCANMsg();

                res = chai.CANDevice.CiWrite(chanNum, &msg, 1);

                if (res != 1)
                    Console.WriteLine("Failed to send.");

                try
                {
                    DateTime nextFrameTime = parcels[i + 1].GetTimestamp();
                    TimeSpan ts = nextFrameTime - curTime;
                    int sleepTime = (int)ts.TotalMilliseconds;

                    // Update current time
                    curTime = nextFrameTime;

                    Console.WriteLine("Sleep for " + sleepTime.ToString() + "ms");
                    Thread.Sleep(sleepTime);
                }
                catch
                {
                    return;
                }
            }
        }
        static Arguments ParseArguments(string[] args)
        {
            Arguments parsedArgs = new Arguments();
            
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Contains('-'))
                {
                    string optionName = args[i].Substring(1);

                    switch (optionName)
                    {
                        case ("ch"):
                            String channelNum = "";
                            try
                            {
                                channelNum = args[i + 1];
                                parsedArgs.SetChannel(int.Parse(channelNum));
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Console.WriteLine("Missing value for argument option " + optionName);
                                Terminate();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                                Terminate();
                            }
                            break;
                        case ("p"):
                            String path = "";                          
                            try
                            {
                                path = args[i + 1];
                                
                                if (System.IO.File.Exists(path) == false)
                                {
                                    Console.WriteLine("Log file doesn't exist!");
                                    Terminate();
                                }
                                parsedArgs.SetPath(path);                                
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                Console.WriteLine("Missing value for argument option " + optionName);
                                Terminate();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.ToString());
                                Terminate();
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("Unrecognized option " + optionName);
                    }
                }
            }
            return parsedArgs;
        }

        static void Terminate()
        {
            Console.WriteLine("Terminate. Press any key to close terminal.");
            Console.ReadKey();

            Environment.Exit(0);
        }
    }
}
