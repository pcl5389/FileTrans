﻿using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace FileTrans
{
    public static class FileSender
    {
        private const int BufferSize = 1024;

        public static void Send(IPEndPoint ip, string path)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(ip);
            using (FileStream reader = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                long send = 0L, length = reader.Length;
                sock.Send(BitConverter.GetBytes(length));
                string fileName = Path.GetFileName(path);
                sock.Send(Encoding.Default.GetBytes(fileName));
                Console.WriteLine("Sending file:" + fileName + ".Plz wait...");
                byte[] buffer = new byte[BufferSize];
                int read, sent;
                //断点发送 在这里判断设置reader.Position即可
                while ((read = reader.Read(buffer, 0, BufferSize)) != 0)
                {
                    sent = 0;
                    while ((sent += sock.Send(buffer, sent, read, SocketFlags.None)) < read)
                    {
                        send += (long)sent;
                        //Console.WriteLine("Sent " + send + "/" + length + ".");//进度
                    }
                }
                Console.WriteLine("Send finish.");
            }
        }

        public static void Receive(IPEndPoint ip, string path)
        {
            Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sock.Bind(ip);
            sock.Listen(8025);
            Socket client = sock.Accept();
            byte[] buffer = new byte[BufferSize];
            client.Receive(buffer);
            long receive = 0L, length = BitConverter.ToInt64(buffer, 0);
            string fileName = Encoding.Default.GetString(buffer, 0, client.Receive(buffer));
            Console.WriteLine("Receiveing file:" + fileName + ".Plz wait...");
            using (FileStream writer = new FileStream(Path.Combine(path, fileName), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                int received;
                //断点接受 在这里判断设置writer.Position即可
                while (receive < length)
                {
                    received = client.Receive(buffer);
                    writer.Write(buffer, 0, received);
                    writer.Flush();
                    receive += (long)received;
                    //Console.WriteLine("Received " + receive + "/" + length + ".");//进度
                }
            }
            Console.WriteLine("Receive finish.");
        }
    }
}