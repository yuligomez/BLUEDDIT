using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Client
{
    class ProgramClient
    {
        private static ClientHandler clientHandler;
        private static ClientExecutionsHandler clientExecutions;
        private static TcpClient tcpClient;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Bienvenido a BLUEDDIT");
            clientHandler = new ClientHandler();
            clientExecutions = new ClientExecutionsHandler();
            await clientHandler.ConnectAsync();
            tcpClient = clientHandler.TcpClient;
            await clientExecutions.LoadUsernameAsync(tcpClient);
            await ShowMenuAsync();
        }

        public static async Task ShowMenuAsync()
        {
            Console.WriteLine("1 - Alta Tema");
            Console.WriteLine("2 - Baja Tema");
            Console.WriteLine("3 - Modificar Tema");
            Console.WriteLine("4 - Alta Post");
            Console.WriteLine("5 - Baja Post");
            Console.WriteLine("6 - Modificacion Post");
            Console.WriteLine("7- Asociar Post a tema");
            Console.WriteLine("8 - Desasociar Post a tema");
            Console.WriteLine("9 - Subir archivo a post ");
            Console.WriteLine("10 - Salir \n");

            var exit = false;
            try
            {
                while (!exit)
                {
                    var option = Console.ReadLine();
                    switch (option)
                    {
                        case "1":
                            await clientExecutions.PostThemeAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "2":
                            await clientExecutions.DeleteThemeAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "3":
                            await clientExecutions.PutThemeAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "4":
                            await clientExecutions.AddPostAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "5":
                            await clientExecutions.DeletePostAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "6":
                            await clientExecutions.PutPostAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "7":
                            await clientExecutions.AssociatePostToThemeAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "8":
                            await clientExecutions.DisassociatePostToThemeAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "9":
                            await clientExecutions.SendFileAsync(tcpClient);
                            await ShowMenuAsync();
                            break;
                        case "10":
                            exit = true;
                            clientHandler.Desconnect();
                            break;
                        default:
                            Console.WriteLine("Opcion invalida...");
                            break;
                    }
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Se perdió la conexión con el servidor: " + e.Message);
            }
        }
       
    }
}
