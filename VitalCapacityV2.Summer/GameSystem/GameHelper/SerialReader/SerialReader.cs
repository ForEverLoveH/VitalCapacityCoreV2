using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Timers;
using Newtonsoft.Json;

namespace VitalCapacityV2.Summer.GameSystem.GameHelper 
{
    public class SerialReader
    {
        public SerialPort _serialPort;
        private int m_type = -1;

        public delegate void RecieveDataCallBack(byte[] data);

        public RecieveDataCallBack _RecieveDataCallBack;

        public delegate void SendDataCallBack(MachineMsgCode code);

        public SendDataCallBack _SendDataCallBack;

        public delegate void AnalyDataCallBack(SerialMsg msg);

        public AnalyDataCallBack _AnalyDataCallBack;
        private System.Timers.Timer waitTimer;
        public string qType = "  ";
        public string mac = " ";
        public string itemtype = "    ";
        public List<byte> _buffer = new List<byte>();
        public int number;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="number"></param>
        public SerialReader(int number)
        {
            _serialPort = new SerialPort();
            _serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceivedComData);
            this.number = number;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ports"></param>
        /// <param name="nBaudrate"></param>
        /// <param name="exError"></param>
        /// <returns></returns>
        public int OpenConnectionSerial(string ports, int nBaudrate, out string exError)
        {
            exError = "   ";
            if(_serialPort.IsOpen)_serialPort.Close();
            try
            {
                _serialPort.PortName = ports;
                _serialPort.BaudRate = nBaudrate;
                _serialPort.StopBits = StopBits.One;
                _serialPort.Parity = Parity.None;
                _serialPort.ReadTimeout = 30;
                _serialPort.WriteTimeout=1000;
                _serialPort.ReadBufferSize = 4069 * 10;
                _serialPort.Open();
                if (waitTimer != null)
                {
                    if(waitTimer.Enabled)waitTimer.Stop();
                    waitTimer.Dispose();
                    waitTimer = null;
                }

                waitTimer = new Timer(30);
                waitTimer.Elapsed += new ElapsedEventHandler(AnalyReceivedData);
                waitTimer.AutoReset = true;
                waitTimer.Enabled = true;
                waitTimer.Start();
                m_type = 0;
                return m_type;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                exError = e.Message;
                return -1;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void CloseConnectionSerial()
        {
            if (_serialPort.IsOpen) _serialPort.Close();
            if (waitTimer != null)
            {
                if (waitTimer.Enabled) waitTimer.Stop();
                waitTimer.Dispose();
                waitTimer = null;
            }
            m_type = -1;
        }
        /// <summary>
         /// 
         /// </summary>
         /// <returns></returns>
        public bool IsSerialOpen()
        {
            return _serialPort.IsOpen;
        }
        
        //缓存
        byte[] s232Buffer = new byte[2048];
        int s232Buffersp = 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReceivedComData(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int nCount = _serialPort.BytesToRead;
                if (nCount == 0)
                    return;
                byte[] btAryBuffer = new byte[nCount];
                _serialPort.Read(btAryBuffer, 0, nCount);
                //RunReceiveDataCallback(btAryBuffer);
                for (int i = 0; i < nCount; i++)
                {
                    s232Buffer[s232Buffersp] = btAryBuffer[i];
                    if (s232Buffersp < (s232Buffer.Length - 2))
                        s232Buffersp++;
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        /// <summary>
        /// 定时器处理数据
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void AnalyReceivedData(object source, System.Timers.ElapsedEventArgs e)
        {
            if (waitTimer != null)
                waitTimer.Stop();
            try
            {
                if (s232Buffersp != 0)
                {
                    byte[] btAryBuffer = new byte[s232Buffersp];
                    Array.Copy(s232Buffer, 0, btAryBuffer, 0, s232Buffersp);
                    Array.Clear(s232Buffer, 0, s232Buffersp);
                    s232Buffersp = 0;
                    _buffer.AddRange(btAryBuffer);
                    int step = 1;
                    while (_buffer.Count > 36 && step < _buffer.Count)
                    {
                        if (_buffer[0] == 0x7B)
                        {
                            if (_buffer[step] != 0x7D)
                            {
                                step++;
                                if (step > _buffer.Count)
                                    break;
                            }
                            else
                            {
                                byte[] receiveBytes = new byte[step + 1];
                                _buffer.CopyTo(0, receiveBytes, 0, step + 1);
                                RunReceiveDataCallback(receiveBytes);
                                _buffer.RemoveRange(0, step);
                            }
                        }
                        else
                        {
                            _buffer.RemoveAt(0);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
              return;
            }
            finally
            {
                if (waitTimer != null)
                    waitTimer.Start();
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="btAryBuffer"></param>
        private void RunReceiveDataCallback(byte[] btAryBuffer)
        {
            try
            {
                //{ 0x7B
                //} 0x7D

                // ReceiveCallback?.Invoke(btAryBuffer);
                int nCount = btAryBuffer.Length;
                string temp = ByteHelper.ParseToString(btAryBuffer);
                MachineMsgCode rValue = null;
                try
                {
                    //{ 0x7B
                    //} 0x7D
                    rValue = JsonConvert.DeserializeObject<MachineMsgCode>(temp);
                    Console.WriteLine($"---------------------------------------------Receive: ({temp})");
                }
                catch (Exception ex)
                {
                    rValue = null;
                    //LoggerHelper.Debug(ex);
                }

                if (rValue == null) return;

                switch (rValue.type)
                {
                    case 1:
                        //设备登录
                        mac = rValue.mac;
                        itemtype = rValue.itemtype;
                        rValue.code = 1;
                        SendMessage(rValue);
                        break;
                    case 2:
                        //设备开始测试返回命令
                        /* rValue.code = 1;
                         SendMessage(rValue);*/
                        break;
                    case 3:
                        //获取成绩
                        rValue.code = 1;
                        //  SendMessage(rValue);
                        break;
                    case 4:
                        //查询设备信息
                        break;
                }

                SerialMsg sm = new SerialMsg(rValue, number);
                _AnalyDataCallBack?.Invoke(sm);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mmc"></param>
        /// <returns></returns>
        public int SendMessage(MachineMsgCode mmc)
        {
            //串口连接方式
            if (m_type == 0)
            {
                if (!_serialPort.IsOpen)
                {
                    return -1;
                }

                mmc.mac = mac;
                mmc.itemtype = itemtype;
                string JsonStr = JsonConvert.SerializeObject(mmc);
                Console.WriteLine($"---------------------------------------------send: ({JsonStr})");
                byte[] btArySenderData = ByteHelper.GetBytes(JsonStr);
                _serialPort.Write(btArySenderData, 0, btArySenderData.Length);
                _SendDataCallBack?.Invoke(mmc);

                return 0;
            }

            return -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="btAryData"></param>
        /// <returns></returns>
        public byte CheckValue(byte[] btAryData)
        {
            return CheckSum(btAryData, 0, btAryData.Length);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="btAryBuffer"></param>
        /// <param name="nStartPos"></param>
        /// <param name="nLen"></param>
        /// <returns></returns>
        public static byte CheckSum(byte[] btAryBuffer, int nStartPos, int nLen)
        {
            byte btSum = 0x00;

            for (int nloop = nStartPos; nloop < nStartPos + nLen; nloop++)
            {
                btSum ^= btAryBuffer[nloop];
            }

            return btSum;
        }
    }
    public class ByteHelper
    {
        public static byte[] GetBytes(string parseString)
        {
            return Encoding.UTF8.GetBytes(parseString);
        }
        public static string ParseToString(byte[] utf8Bytes)
        {
            return Encoding.UTF8.GetString(utf8Bytes);
        }
    }
}