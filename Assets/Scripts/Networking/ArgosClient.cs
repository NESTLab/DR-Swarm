using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FullSerializer;

public class ArgosClient : MonoBehaviour
{
    private static readonly fsSerializer _serializer = new fsSerializer();

    Thread recvThread;
    TcpClient client;
    bool collectData = true;

    private static int port = 8051;

    // Start is called before the first frame update
    void Start()
    {
        recvThread = new Thread(new ThreadStart(RecieveData));
        recvThread.IsBackground = true;
        recvThread.Start();
    }

    private void RecieveData()
    {
        client = new TcpClient();

        NetworkStream stream = null;
        Byte[] data = new Byte[256];
        while (collectData)
        {
            try
            {
                if (!client.Connected)
                {
                    client.Connect("127.0.0.1", port);
                    stream = client.GetStream();
                } else {
                    String responseData = String.Empty;
                    Int32 bytes = stream.Read(data, 0, data.Length);

                    if (bytes > 0)
                    {
                        responseData = Encoding.ASCII.GetString(data, 0, bytes);
                        Debug.Log(responseData);

                        fsData jsonData = fsJsonParser.Parse(responseData);

                        object dataPack = null;
                        _serializer.TryDeserialize(jsonData, typeof(SerializedDataPack), ref dataPack).AssertSuccessWithoutWarnings();
                        ((SerializedDataPack)dataPack).AssignVariables();
                    }
                    else
                    {
                        client.Close();
                        client = new TcpClient();
                    }
                }
            }
            catch (Exception e)
            {
                Thread.Sleep(1000);
                Debug.Log(String.Format("Failed to connect to ARGoS server ({0}), re-attempting connection...", e.ToString()));
            }
        }

        if (client != null)
            client.Close();

        recvThread.Abort();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        collectData = false;
    }
}
