using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using FullSerializer;

/// <summary>
/// Connects to a Variable Publisher server and feeds the data into the data model
/// </summary>
public class ArgosClient : MonoBehaviour
{
    /// <summary>
    /// The ip address or hostname of the server
    /// </summary>
    [SerializeField]
    private string _serverAddr = "192.168.1.108";
    public string serverAddr
    {
        get { return _serverAddr; }
        set
        {
            // Stop collecting data and wait for the thread to end.
            collectData = false;
            if (recvThread.Join(1000)) // Wait 1 second for it to die
            {
                recvThread.Abort(); // Thread didn't stop, kill it
            }

            // Change the server address and re-start the thread
            _serverAddr = value;
            collectData = true;
            this.CreateRecvThread();
        }
    }

    private static readonly fsSerializer _serializer = new fsSerializer();

    Thread recvThread;
    TcpClient client;
    bool collectData = true;

    private static int port = 8052;

    // Start is called before the first frame update
    void Start() { }

    /// <summary>
    /// Create a new thread to recieve data
    /// </summary>
    private void CreateRecvThread()
    {
        recvThread = new Thread(new ThreadStart(RecieveData));
        recvThread.IsBackground = true;
        recvThread.Start();
    }

    /// <summary>
    ///     Connects to the server at <see cref="serverAddr"/> and parse data from
    ///     the sever into our data model.
    /// </summary>
    /// <remarks>
    ///     Designed to run in a separate thread, since it has a while loop that
    ///     runs for a long time.
    /// </remarks>
    private void RecieveData()
    {
        client = new TcpClient();

        StringBuilder builder = new StringBuilder();
        NetworkStream stream = null;
        Byte[] data = new Byte[256];

        // Loop until we're supposed to stop collecitng data
        while (collectData)
        {
            try
            {
                // If we aren't connected try to connect
                // (or re-connect if connection was lost at some point)
                if (!client.Connected)
                {
                    client.Connect(serverAddr, port);
                    stream = client.GetStream();
                } else {
                    String responseData = String.Empty;
                    Int32 bytes = stream.Read(data, 0, data.Length);

                    if (bytes > 0)
                    {
                        // Decode the data from the server with UTF8 formatting
                        responseData = Encoding.UTF8.GetString(data, 0, bytes);

                        // Add the parsed data to the string builder and check if \n was recieved
                        builder.Append(responseData);
                        if (responseData.Contains("\n"))
                        {
                            // Parse the data recieved as JSON
                            String jsonString = builder.ToString();
                            jsonString = jsonString.Substring(4);
                            Debug.Log(jsonString);
                            builder.Clear();

                            fsData jsonData = fsJsonParser.Parse(jsonString);

                            object dataPack = null;
                            _serializer.TryDeserialize(jsonData, typeof(SerializedDataPack), ref dataPack).AssertSuccessWithoutWarnings();

                            // Assign the JSON data to the data model
                            ((SerializedDataPack)dataPack).AssignVariables();
                        }
                    }
                    else
                    {
                        client.Close();
                        client = new TcpClient();
                    }
                }
            }
            catch (SocketException e)
            {
                // If we disconnected, wait a second and loop again
                Thread.Sleep(1000);
                Debug.LogWarning(String.Format("Failed to connect to ARGoS server ({0}), re-attempting connection...", e.ToString()));
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }

        // If client was created, try and close it
        if (client != null)
            client.Close();

        // Kill thread
        recvThread.Abort();
    }

    // Update is called once per frame
    void Update() { }

    private void OnDestroy()
    {
        collectData = false;
    }
}
