using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Server : MonoBehaviour
{
    [SerializeField]
    private InputField messageText;

    [SerializeField]
    private Text chat;

    private float lastTime;

    private Socket sender;


    private void Awake()
    {
        StartClient();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessage();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShutdownClient();
        }
    }

    private void StartClient()
    {
        // Connect to a remote device.  
        try
        {
            // Establish the remote endpoint for the socket.  
            // This example uses port 11000 on the local computer.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry("ec2-18-228-39-32.sa-east-1.compute.amazonaws.com");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 5050);

            // Create a TCP/IP  socket.  
            sender = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Connect the socket to the remote endpoint. Catch any errors.  
            try
            {


                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                StartReceiveMessage(sender);
            }
            catch (ArgumentNullException ane)
            {
                Debug.LogFormat("ArgumentNullException : {0}", ane.ToString());
            }
            catch (SocketException se)
            {
                Debug.LogFormat("SocketException : {0}", se.ToString());
            }
            catch (Exception e)
            {
                Debug.LogFormat("Unexpected exception : {0}", e.ToString());
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    private void StartReceiveMessage(Socket handler)
    {
        // An incoming connection needs to be processed. 
        AsyncCallback messageReceivedCallback = new AsyncCallback(EndReceiveMessage);
        SocketError socketError;

        // Data buffer for incoming data.  
        byte[] buffer = new Byte[1024];
        DataState dataState = new DataState(handler, buffer);

        handler.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, out socketError, messageReceivedCallback, dataState);
    }

    private void EndReceiveMessage(IAsyncResult asyncResult)
    {
        DataState dataState = (DataState)asyncResult.AsyncState;
        Socket handler = dataState.handler;
        byte[] buffer = dataState.buffer;
        int messageSize = handler.EndReceive(asyncResult);

        if (messageSize > 0)
        {
            string message = null;
            message = Encoding.ASCII.GetString(buffer, 0, messageSize);

            StartReceiveMessage(handler);
            OnMessageReceived(message);

        }
        else
        {
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }


    private void ShutdownClient()
    {
        // Release the socket.  
        sender.Shutdown(SocketShutdown.Both);
        sender.Close();
    }

    private void OnMessageReceived(string message)
    {
        chat.text += "\n" + "[" + DateTime.Now + "]" + message;
    }


    private void SendMessage()
    {
        if (sender.Connected)
        {
            // Data buffer for incoming data.  
            byte[] bytes = new byte[1024];

            // Encode the data string into a byte array.  
            byte[] msg = Encoding.ASCII.GetBytes(messageText.text);

            // Send the data through the socket.  
            int bytesSent = sender.Send(msg);

            messageText.text = "";
        }

    }


}