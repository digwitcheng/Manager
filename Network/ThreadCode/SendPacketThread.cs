﻿using AGV_V1._0.Network.ThreadCode;
using AGV_V1._0.Network;
using AGV_V1._0.NLog;
using AGV_V1._0.Queue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGVSocket.Network.Packet;
using AGVSocket.Network;

namespace AGV_V1._0.ThreadCode
{
    class SendPacketThread:BaseThread
    {
        private volatile bool isCanSendNext = true;
        public bool IsCanSendNext
        {
            set
            {
                isCanSendNext = value;
            }
        }
         private static SendPacketThread instance;
        public static SendPacketThread Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SendPacketThread();
                }
                return instance;
            }
        }
        private SendPacketThread() { }

        protected override string ThreadName()
        {
            return "SendPacketThread";
        }
        protected override void Run()
        {
            try
            {
                if (isCanSendNext)
                {
                    if (SendPacketQueue.Instance.IsHasData())
                    {
                        SendBasePacket sp = SendPacketQueue.Instance.Peek();// SendPacketQueue.Instance.Dequeue();//
                        AgvServerManager.Instance.Send(sp);
                        isCanSendNext = false;
                    }
                }
            }
            catch (Exception e)
            {
                Logs.Error("sendpacketThread" + e);
            }
        }
    }
}
