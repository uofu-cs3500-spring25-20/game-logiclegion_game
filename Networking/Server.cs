﻿// <copyright file="Server.cs" company="UofU-CS3500">
// Copyright (c) 2025 UofU-CS3500. All rights reserved.
// </copyright>

using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CS3500.Networking;

/// <summary>
///     Represents a server task that waits for connections on a given port and calls the provided delegate when a connection is made.
/// </summary>
public static class Server
{

    /// <summary>
    ///     Wait on a TcpListener for new connections. Alert the main program via a callback (delegate) mechanism.
    /// </summary>
    /// <param name="handleConnect">
    ///     Handler for what the user wants to do when a connection is made.
    ///     This should be run asynchronously via a new thread.
    /// </param>
    /// <param name="port"> The port (e.g., 11000) to listen on. </param>
    public static void StartServer(Action<NetworkConnection> handleConnect, int port)
    {
        TcpListener listener = new(IPAddress.Any, 10000);
        listener.Start();

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            NetworkConnection nc = new(client);
            Console.WriteLine("accepted a connection");
            new Thread(() => HandleClient(nc)).Start();
        }
    }

    /// <summary>
    ///     Helper method for handling a client.
    /// </summary>
    /// <param name="client"> A client. </param>
    public static void HandleClient(NetworkConnection client)
    {
        while (true)
        {
            try
            {
                string message = client.ReadLine();
                Console.WriteLine("received message: " + message);
            }
            catch (Exception e)
            {
                Console.WriteLine("closed");
                Console.WriteLine(e.ToString());
                return;
            }
        }
    }
}