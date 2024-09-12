using System.Collections;
using System.Collections.Generic;
using NetMQ.Sockets;
using NetMQ;
using UnityEngine;
using Meocap;
using System.Text;
using System.Runtime.InteropServices;
using System;

namespace Meocap.DataSource
{
    
  
    public class MeoSubscriber : MonoBehaviour
    {
        // Start is called before the first frame update
        [Header("���ø�����Դ�󶨵�Actorʵ��")]
        public Perform.MeoActor actor;
        [Header("������IP��ַ")]
        public string address = "127.0.0.1";
        [Header("�����˶˿ں�")]
        public short port = 14999;
        [Header("��ǰ֡ID")]
        public int frameId = 0;
        [Header("��Actor�Ǽ�ͬ�����ͻ���")]
        public bool syncBonePos = false;
        [Header("Command Server IP��ַ")]
        public string commandAddress = "127.0.0.1";
        [Header("Command Servr �˿ں�")]
        public short commandPort = 15999;
        Meocap.MeoFrame frame;
        private ulong sock_ptr;
        private ulong sock_command_ptr;
        

        void Start()
        {
            AsyncIO.ForceDotNet.Force();
            string[] ip_addr = address.Split('.');
            if(ip_addr.Length != 4 ) {
                Debug.LogError("MeoSubscriber: IPAddress Format Error");
                return;
            }
            this.sock_ptr = MeocapSDK.meocap_connect_server_char(byte.Parse(ip_addr[0]), byte.Parse(ip_addr[1]), byte.Parse(ip_addr[2]), byte.Parse(ip_addr[3]), (ushort)port);
            if(this.syncBonePos)
            {
                string[] ip_command_addr = commandAddress.Split(".");
                this.sock_command_ptr = MeocapSDK.meocap_connect_command_server_char(byte.Parse(ip_command_addr[0]), byte.Parse(ip_command_addr[1]), byte.Parse(ip_command_addr[2]), byte.Parse(ip_command_addr[3]), (ushort)commandPort);
                Debug.Log(this.sock_command_ptr);
                if (this.actor && this.sock_command_ptr != 0)
                {
                    var frame = this.actor.SyncBonePosToClient();
                    var ret = MeocapSDK.meocap_command_set_skel(this.sock_command_ptr, ref frame);
                    Debug.Log(ret);

                }
            }
        }

        private void Update()
        {
            int ret = MeocapSDK.meocap_recv_frame(this.sock_ptr, out this.frame);
            if (ret == 0)
            {
                if (actor != null)
                {
                    actor.Perform(this.frame);
                }
                this.frameId = this.frame.frame_id;
            }
        }

        private void OnDestroy()
        {
            if(this.sock_ptr != 0)
            {
                MeocapSDK.meocap_clean_up(this.sock_ptr);
            }
        }
    }

}
