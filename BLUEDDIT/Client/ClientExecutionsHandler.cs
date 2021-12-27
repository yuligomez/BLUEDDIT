using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ProtocolComunication;
using ProtocolComunication.Interface;

namespace Client
{
    public class ClientExecutionsHandler
    {
        private static IHeaderHandler header;
        private static IFileExcutionHandler fileExcutionHandler;
        private readonly INetworkLogic networkLogic;
        private string userName { get; set; }

        public ClientExecutionsHandler()
        {
            header = new HeaderHandler();
            networkLogic = new NetworkLogic();
            fileExcutionHandler = new FileExecutionHandler();
        }

        public async Task PostThemeAsync( TcpClient client)
        {
            Console.Write("Ingrese el nombre del tema: ");
            var nameTheme = Console.ReadLine();
            Console.Write("Ingrese la descripcion del tema: ");
            var descriptionTheme = Console.ReadLine();
            var specialCaracters = networkLogic.CountSpecialCharacter(nameTheme + descriptionTheme + userName);
            var headerBytes = header.EncodeHeader(CommandConstants.AddTheme,
                nameTheme.Length + 1 + descriptionTheme.Length + specialCaracters + 1 + userName.Length);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(nameTheme + "/" + descriptionTheme + "/" + userName);
            await networkLogic.SendAsync(dataByte, client);
            Console.WriteLine();
            string response = await networkLogic.CompleteRecivedAsync(client);
            Console.WriteLine(response);
            Console.WriteLine();
        }

        public async Task DeleteThemeAsync(TcpClient client)
        {
            Console.Write("Ingrese el nombre del tema a eliminar: ");
            var nameTheme = Console.ReadLine();
            var specialCaracters = networkLogic.CountSpecialCharacter(nameTheme + userName);
            var headerBytes = header.EncodeHeader(CommandConstants.DeleteTheme, nameTheme.Length + specialCaracters + userName.Length + 1);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(nameTheme + "/" + userName);
            await networkLogic.SendAsync(dataByte, client);
            Console.WriteLine();
            string response = await  networkLogic.CompleteRecivedAsync(client);
            Console.WriteLine(response);
            Console.WriteLine();
        }

        public async Task PutThemeAsync( TcpClient client)
        {
            Console.Write("Ingrese el nombre del tema a modificar: ");
            var nameTheme = Console.ReadLine();
            Console.Write("Ingrese el nuevo nombre del tema: ");
            var newNameTheme = Console.ReadLine();
            Console.Write("Ingrese la nueva descripción del tema: ");
            var newDescriptionTheme = Console.ReadLine();
            var specialCaracters = networkLogic.CountSpecialCharacter(newNameTheme + newDescriptionTheme + nameTheme + userName);
            var headerBytes = header.EncodeHeader(CommandConstants.UpdateTheme, nameTheme.Length + newNameTheme.Length + 
                newDescriptionTheme.Length + 3 + specialCaracters + userName.Length);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(nameTheme + "/" + newNameTheme + "/" + newDescriptionTheme + "/" + userName);
            await networkLogic.SendAsync(dataByte, client);
            Console.WriteLine();
            string response = await networkLogic.CompleteRecivedAsync(client);
            Console.WriteLine(response);
            Console.WriteLine();
        }

        public async Task AddPostAsync(TcpClient client)
        {
            Console.Write("Ingrese el nombre del post: ");
            var namePost = Console.ReadLine();
            Console.Write("Ingrese el nombre del tema asociado al post: ");
            var nameTheme = Console.ReadLine();
            var specialCaracters = networkLogic.CountSpecialCharacter(namePost + nameTheme + userName);
            var headerBytes = header.EncodeHeader(CommandConstants.AddPost, namePost.Length + nameTheme.Length + 2 + specialCaracters + userName.Length);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(namePost + "/" + nameTheme + "/" + userName);
            await networkLogic.SendAsync(dataByte, client);
            Console.WriteLine();
            string response = await networkLogic.CompleteRecivedAsync(client);
            Console.WriteLine(response);
            Console.WriteLine();
        }


        public async Task DeletePostAsync( TcpClient client)
        {
            Console.Write("Ingrese el nombre del post a eliminar: ");
            var namePost = Console.ReadLine();
            var specialCaracters = networkLogic.CountSpecialCharacter(namePost + userName);
            var headerBytes = header.EncodeHeader(CommandConstants.DeletePost, namePost.Length + specialCaracters + 1 + userName.Length);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(namePost + "/" + userName);
            await networkLogic.SendAsync(dataByte, client);
            Console.WriteLine();
            string response = await networkLogic.CompleteRecivedAsync(client);
            Console.WriteLine(response);
            Console.WriteLine();
        }

        public async Task PutPostAsync(TcpClient client)
        {
            Console.Write("Ingrese el nombre del post a modificar: ");
            var namePost = Console.ReadLine();
            Console.Write("Ingrese el nuevo nombre del post: ");
            var newNamePost = Console.ReadLine();
            var specialCaracters = networkLogic.CountSpecialCharacter(namePost + newNamePost + userName);
            var headerBytes = header.EncodeHeader(CommandConstants.UpdatePost, namePost.Length + newNamePost.Length + 2 + specialCaracters + userName.Length);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(namePost + "/" + newNamePost + "/" + userName);
            await networkLogic.SendAsync(dataByte, client);
            Console.WriteLine();
            string response = await networkLogic.CompleteRecivedAsync(client);
            Console.WriteLine(response);
            Console.WriteLine();
        }

        public async Task AssociatePostToThemeAsync(TcpClient client)
        {
            Console.Write("Ingrese el nombre del post a asociar: ");
            var namePost = Console.ReadLine();
            Console.Write("Ingrese el nombre del tema a asociar: ");
            var nameTheme = Console.ReadLine();
            var specialCaracters = networkLogic.CountSpecialCharacter(namePost + nameTheme + userName);
            var headerBytes = header.EncodeHeader(CommandConstants.AsociatePostToTheme, namePost.Length + nameTheme.Length + 2 + specialCaracters + userName.Length);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(namePost + "/" + nameTheme + "/" + userName);
            await networkLogic.SendAsync(dataByte, client);
            Console.WriteLine();
            string response = await networkLogic.CompleteRecivedAsync(client);
            Console.WriteLine(response);
            Console.WriteLine();
        }

        public async Task DisassociatePostToThemeAsync(TcpClient client)
        {
            Console.Write("Ingrese el nombre del post a desasociar: ");
            var namePost = Console.ReadLine();
            Console.Write("Ingrese el nombre del tema a desasociar: ");
            var nameTheme = Console.ReadLine();
            var specialCaracters = networkLogic.CountSpecialCharacter(namePost + nameTheme + userName);
            var headerBytes = header.EncodeHeader(CommandConstants.DesasociatePostToTheme, namePost.Length + nameTheme.Length + 2 + specialCaracters + userName.Length);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(namePost + "/" + nameTheme + "/" + userName);
            await networkLogic.SendAsync(dataByte, client);
            Console.WriteLine();
            string response = await networkLogic.CompleteRecivedAsync(client);
            Console.WriteLine(response);
            Console.WriteLine();
        }

        public async Task LoadUsernameAsync( TcpClient client)
        {
            Console.Write("Ingrese su nombre: ");
            var name = Console.ReadLine();
            userName = name;
            var specialCaracters = networkLogic.CountSpecialCharacter(name);
            var headerBytes = header.EncodeHeader(CommandConstants.LoadUsername, name.Length + specialCaracters);
            await networkLogic.SendAsync(headerBytes, client);
            var dataByte = Encoding.UTF8.GetBytes(name);
            await networkLogic.SendAsync(dataByte, client);

            Console.WriteLine();
        }

        public async Task SendFileAsync(TcpClient tcpClient)
        {
            Console.Write("Ingrese el nombre del post al cual desea asociar el archivo: ");
            var postName = Console.ReadLine();
            Console.Write("Ingrese el path del archivo a asociar al post: ");
            var path = Console.ReadLine();
            await fileExcutionHandler.SendFileAsync(postName, userName, path, tcpClient);
            Console.WriteLine();
        }

    }
}
