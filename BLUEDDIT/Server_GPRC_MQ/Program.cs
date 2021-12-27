using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;
using ProtocolComunication;
using ProtocolComunication.Interface;

namespace Server_GPRC_MQ
{
    public class Program
    {
        private static bool exit = false;

        private static List<TcpClientUsername> connectedClients;

        private static ServerHandler serverHandler;

        private static ServerExecutionsHandler serverExecutionsHandler;

        private static INetworkLogic networkLogic;

        private static CommonLog commonLog;

        public static void Main(string[] args)
        {

            commonLog = new CommonLog();
            connectedClients = new List<TcpClientUsername>();
            serverHandler = new ServerHandler();
            serverExecutionsHandler = new ServerExecutionsHandler();
            networkLogic = new NetworkLogic();
            var task = Task.Run(async () => await ListenForConnectionsAsync(serverHandler.TcpListener));
            Console.WriteLine("Bienvenido al servidor BLUEDDIT");
            var taskShowMenu = Task.Run(() => ShowMenu());
            //taskShowMenu.Wait();

            CreateHostBuilder(args).Build().Run();


        }

        public static void CloseServer(TcpListener tcpListenner)
        {
            exit = true;
            tcpListenner.Stop();
            foreach (var tcpClient in connectedClients)
            {
                try
                {
                    tcpClient.TcpClient.Close();
                    connectedClients.Remove(tcpClient);
                }
                catch (Exception)
                {
                    Console.WriteLine("El cliente ya no está conectado");
                }

                Console.WriteLine("Cerrando clientes..");
            }
        }

        private static async Task ListenForConnectionsAsync(TcpListener tcpListener)
        {
            while (!exit)
            {
                try
                {
                    var acceptedClient = await serverHandler.TcpListener.AcceptTcpClientAsync();
                    var tcpClientUsername = new TcpClientUsername();
                    tcpClientUsername.TcpClient = acceptedClient;
                    connectedClients.Add(tcpClientUsername);
                    var task = Task.Run(async () => await HandleClientAsync(acceptedClient));
                }
                catch (SocketException)
                {
                    Console.WriteLine("El servidor está cerrándose...");
                    exit = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
        public static void ShowMenu()
        {
      
            while (true)
            {
                Console.WriteLine("Ingrese una opción para continuar...");
                Console.WriteLine();
                Console.WriteLine("1 - Mostrar clientes conectados");
                Console.WriteLine("2 - Ver listado de temas");
                Console.WriteLine("3 - Listar posts por órden de creado");
                Console.WriteLine("4 - Ver listado de post por tema");
                Console.WriteLine("5 - Listar post por tema y por órden de creado");
                Console.WriteLine("6 - Ver post");
                Console.WriteLine("7 - Ver archivos de post");
                Console.WriteLine("8 - Ver tema con más posts");
                Console.WriteLine("9 - Ver listado de archivos");
                Console.WriteLine("10 - Help");
                Console.WriteLine("11 - Salir \n");
                var selectedOption = Console.ReadLine();
                switch (selectedOption)
                {
                    case "1":
                        ShowConnectedUsers();
                        Console.WriteLine();
                        break;
                    case "2":
                        serverExecutionsHandler.ShowThemes();
                        Console.WriteLine();
                        break;
                    case "3":
                        serverExecutionsHandler.ShowPostByOrder();
                        Console.WriteLine();
                        break;
                    case "4":
                        serverExecutionsHandler.ShowPostByTheme();
                        Console.WriteLine();
                        break;
                    case "5":
                        serverExecutionsHandler.ShowPostByThemeAndOrder();
                        Console.WriteLine();
                        break;
                    case "6":
                        serverExecutionsHandler.ShowPost();
                        Console.WriteLine();
                        break;
                    case "7":
                        serverExecutionsHandler.ShowFilesFromAPost();
                        Console.WriteLine();
                        break;
                    case "8":
                        serverExecutionsHandler.ShowThemeWhithMorePosts();
                        Console.WriteLine();
                        break;
                    case "9":
                        serverExecutionsHandler.ShowFiles();
                        Console.WriteLine();
                        break;
                    case "10":
                        ShowMenu();
                        break;
                    case "11":
                        CloseServer(serverHandler.TcpListener);
                        break;
                    default:
                        Console.WriteLine("Opcion invalida...");
                        break;
                }
            }
        }

        private static void SelectOption()
        {
            try
            {
                while (!exit)
                {
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            ShowConnectedUsers();
                            Console.WriteLine();
                            break;
                        case "2":
                            serverExecutionsHandler.ShowThemes();
                            Console.WriteLine();
                            break;
                        case "3":
                            serverExecutionsHandler.ShowPostByOrder();
                            Console.WriteLine();
                            break;
                        case "4":
                            serverExecutionsHandler.ShowPostByTheme();
                            Console.WriteLine();
                            break;
                        case "5":
                            serverExecutionsHandler.ShowPostByThemeAndOrder();
                            Console.WriteLine();
                            break;
                        case "6":
                            serverExecutionsHandler.ShowPost();
                            Console.WriteLine();
                            break;
                        case "7":
                            serverExecutionsHandler.ShowFilesFromAPost();
                            Console.WriteLine();
                            break;
                        case "8":
                            serverExecutionsHandler.ShowThemeWhithMorePosts();
                            Console.WriteLine();
                            break;
                        case "9":
                            serverExecutionsHandler.ShowFiles();
                            Console.WriteLine();
                            break;
                        case "10":
                            ShowMenu();
                            break;
                        case "11":
                            CloseServer(serverHandler.TcpListener);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Se perdió la conexión con el servidor: " + e.Message);
            }
        }

        public static void ShowConnectedUsers()
        {
            if (connectedClients.Count == 0)
            {
                Console.WriteLine("No hay usuarios conectados.");
            }
            else
            {
                foreach (var connectedUsers in connectedClients)
                {
                    Console.WriteLine("cliente: " + connectedUsers.Username + "  hora de conexión: " + connectedUsers.Date.ToString("hh:mm:ss tt"));
                }
            }
        }

        private static async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                while (!exit)
                {
                    var headerHandler = new HeaderHandler();
                    var buffer = new byte[HeaderConstants.CommandLength + HeaderConstants.DataLength];
                    var data = await networkLogic.ReceiveDataAsync(buffer.Length, client);
                    Tuple<short, int> header = headerHandler.DecodeHeader(data);
                    switch (header.Item1)
                    {
                        case CommandConstants.AddTheme:
                            await serverExecutionsHandler.AddThemeAsync(header, client);
                            break;
                        case CommandConstants.DeleteTheme:
                            await serverExecutionsHandler.DeleteThemeAsync(header, client);
                            break;
                        case CommandConstants.UpdateTheme:
                            await serverExecutionsHandler.PutThemeAsync(header, client);
                            break;
                        case CommandConstants.AddPost:
                            await serverExecutionsHandler.PostPostAsync(header, client);
                            break;
                        case CommandConstants.DeletePost:
                            await serverExecutionsHandler.DeletePostAsync(header, client);
                            break;
                        case CommandConstants.UpdatePost:
                            await serverExecutionsHandler.ModifyPostAsync(header, client);
                            break;
                        case CommandConstants.AsociatePostToTheme:
                            await serverExecutionsHandler.AssociatePostToThemeAsync(header, client);
                            break;
                        case CommandConstants.DesasociatePostToTheme:
                            await serverExecutionsHandler.DissassociatePostToThemeAsync(header, client);
                            break;
                        case CommandConstants.AddFileToPost:
                            await serverExecutionsHandler.RecivedFileForPostAsync(header, client);
                            break;
                        case CommandConstants.LoadUsername:
                            await LoadUsernameAsync(header, client);
                            break;
                        default:
                            Console.WriteLine("Comando recibido no valido ");
                            break;
                    }
                }
            }
            catch (SocketException)
            {
                RemoveClient(client);
            }
        }

        public static void RemoveClient(TcpClient client)
        {
            if (connectedClients.Count > 0)
            {
                foreach (var connectedClient in connectedClients.ToList())
                {
                    if (connectedClient.TcpClient == client)
                    {
                        connectedClients.Remove(connectedClient);

                    }
                }
            }
        }

        public static async Task LoadUsernameAsync(Tuple<short, int> header, TcpClient client)
        {
            var dataLength = header.Item2;
            var data = new byte[dataLength];
            Array.Copy(await networkLogic.ReceiveDataAsync(dataLength, client), 0, data, 0, dataLength);
            var name = System.Text.Encoding.UTF8.GetString(data);
            var response = new Response { Level = "[info]", Message = "Inicio de conexión", ObjectType = "Logueo" };
            commonLog.AddLog(name, response);
            foreach (var connectedClient in connectedClients)
            {
                if (connectedClient.TcpClient == client)
                {
                    connectedClient.Username = name;
                    connectedClient.Date = DateTime.Now;
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(options =>
                {
                    // Setup a HTTP/2 endpoint without TLS.
                    options.ListenLocalhost(5007, o => o.Protocols =
                        HttpProtocols.Http2);
                });
                webBuilder.UseStartup<Startup>();
            });

        /*public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });*/
    }
}
