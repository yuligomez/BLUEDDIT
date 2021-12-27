using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Domain;
using ProtocolComunication;
using ProtocolComunication.Interface;
using ServerLogic;
using ServerLogicInterface;

namespace Server_GPRC_MQ
{
    public class ServerExecutionsHandler
    {
        private static IThemeLogic themeLogic;

        private static IPostLogic postLogic;

        private static IFileLogic fileLogic;

        private INetworkLogic networkLogic;

        private IFileExcutionHandler fileExcutionHandler;

        private CommonLog commonLog;

        public static List<TcpClientUsername> connectedClients;


        public ServerExecutionsHandler()
        {
            themeLogic = new ThemeLogic();
            postLogic = new PostLogic();
            fileLogic = new FileLogic();
            networkLogic = new NetworkLogic();
            fileExcutionHandler = new FileExecutionHandler();
            connectedClients = new List<TcpClientUsername>();
            commonLog = new CommonLog();
        }

        public List<File> GetFiles(FileFilter filter)
        {
            if (filter.DateFilter != "1" && filter.NameFilter != "1" && filter.SizeFilter != "1")
            {
                var files = fileLogic.GetFiles();
                return files;
            }
            else
            {
                var files = fileLogic.GetFilteredFiles(filter);
                return files;
            }
        }

        public List<Domain.Theme> GetThemes()
        {
            var themes = themeLogic.GetThemes();
            return themes;
        }

        public Domain.Post GetPostByName(string name)
        {
            Domain.Post post = postLogic.GetPostByName(name);
            return post;
        }

        private async Task<string> PrepareDataAync(Tuple<short, int> header, TcpClient client)
        {
            var dataLength = header.Item2;
            var data = new byte[dataLength];
            Array.Copy(await networkLogic.ReceiveDataAsync(dataLength, client), 0, data, 0, dataLength);
            var dataString = System.Text.Encoding.UTF8.GetString(data);
            return dataString;
        }

        public async Task AddThemeAsync(Tuple<short, int> header, TcpClient client)
        {
            string dataString = await PrepareDataAync(header, client);
            var dataStringArray = dataString.Split("/");
            var nameTheme = dataStringArray[0];
            var descriptionTheme = dataStringArray[1];
            var userName = dataStringArray[2];
            Domain.Theme theme = new Domain.Theme();
            theme.Name = nameTheme;
            theme.Description = descriptionTheme;
            var response = themeLogic.AddTheme(theme);
            await networkLogic.CompleteSendAsync(response.Message, client, CommandConstants.AddTheme);
            commonLog.AddLog(userName, response);
        }

        public async Task DeleteThemeAsync(Tuple<short, int> header, TcpClient client)
        {
            string dataString = await PrepareDataAync(header, client);
            var dataStringArray = dataString.Split("/");
            var themeName = dataStringArray[0];
            var userName = dataStringArray[1];
            var response = themeLogic.DeleteTheme(themeName);
            await networkLogic.CompleteSendAsync(response.Message, client, CommandConstants.DeleteTheme);
            commonLog.AddLog(userName, response);
        }

        public async Task PutThemeAsync(Tuple<short, int> header, TcpClient client)
        {
            string dataString = await PrepareDataAync(header, client);
            var dataStringArray = dataString.Split("/");
            var oldNameTheme = dataStringArray[0];
            var newNameTheme = dataStringArray[1];
            var newDescriptionTheme = dataStringArray[2];
            var userName = dataStringArray[3];
            Domain.Theme theme = new Domain.Theme();
            theme.Name = newNameTheme;
            theme.Description = newDescriptionTheme;
            var response = themeLogic.ModifyTheme(oldNameTheme, theme);
            await networkLogic.CompleteSendAsync(response.Message, client, CommandConstants.UpdateTheme);
            commonLog.AddLog(userName, response);
        }


        public async Task PostPostAsync(Tuple<short, int> header, TcpClient client)
        {
            string dataString = await PrepareDataAync(header, client);
            var dataStringArray = dataString.Split("/");
            var postName = dataStringArray[0];
            var themeName = dataStringArray[1];
            var userName = dataStringArray[2];
            Domain.Post post = new Domain.Post();
            post.Name = postName;
            post.CreationDate = DateTime.Now;
            var response = postLogic.PostPost(post, themeName);
            await networkLogic.CompleteSendAsync(response.Message, client, CommandConstants.AddPost);
            commonLog.AddLog(userName, response);
        }

        public async Task DeletePostAsync(Tuple<short, int> header, TcpClient client)
        {
            string dataString = await PrepareDataAync(header, client);
            var dataStringArray = dataString.Split("/");
            var postName = dataStringArray[0];
            var userName = dataStringArray[1];
            var response = postLogic.DeletePost(postName);
            await networkLogic.CompleteSendAsync(response.Message, client, CommandConstants.DeletePost);
            commonLog.AddLog(userName, response);
        }

        public async Task ModifyPostAsync(Tuple<short, int> header, TcpClient client)
        {
            string dataString = await PrepareDataAync(header, client);
            var dataStringArray = dataString.Split("/");
            var oldNamePost = dataStringArray[0];
            var newNamePost = dataStringArray[1];
            var userName = dataStringArray[2];
            Domain.Post post = new Domain.Post();
            post.Name = newNamePost;
            var response = postLogic.ModifyPost(oldNamePost, post);
            await networkLogic.CompleteSendAsync(response.Message, client, CommandConstants.UpdatePost);
            commonLog.AddLog(userName, response);
        }
        public async Task AssociatePostToThemeAsync(Tuple<short, int> header, TcpClient client)
        {
            string dataString = await PrepareDataAync(header, client);
            var dataStringArray = dataString.Split("/");
            var namePost = dataStringArray[0];
            var nameTheme = dataStringArray[1];
            var userName = dataStringArray[2];
            var response = postLogic.AssociatePostToTheme(namePost, nameTheme);
            await networkLogic.CompleteSendAsync(response.Message, client, CommandConstants.AsociatePostToTheme);
            commonLog.AddLog(userName, response);
        }
        public async Task DissassociatePostToThemeAsync(Tuple<short, int> header, TcpClient client)
        {
            string dataString = await PrepareDataAync(header, client);
            var dataStringArray = dataString.Split("/");
            var namePost = dataStringArray[0];
            var nameTheme = dataStringArray[1];
            var userName = dataStringArray[2];
            var response = postLogic.DissassosiatePostToTheme(namePost, nameTheme);
            await networkLogic.CompleteSendAsync(response.Message, client, CommandConstants.DesasociatePostToTheme);
            commonLog.AddLog(userName, response);
        }

        public async Task RecivedFileForPostAsync(Tuple<short, int> header, TcpClient client)
        {
            var response = await fileExcutionHandler.RecivedFileForPostAsync(header, client);
            commonLog.AddLog(response.Client, response);
        }

        public void ShowPostByThemeAndOrder()
        {
            var themes = GetThemes();
            if (themes.Count == 0)
            {
                Console.WriteLine("No hay post agregados.");
            }
            else
            {
                foreach (Domain.Theme theme in themes)
                {
                    Console.WriteLine("Tema: " + theme.Name);
                    var posts = themeLogic.GetOrderPostFromTheme(theme);
                    foreach (Domain.Post post in posts)
                    {
                        Console.WriteLine("Post: " + post.Name);
                    }
                    Console.WriteLine();
                }
            }
        }

        public void ShowPostByOrder()
        {
            var posts = postLogic.GetPostsByOrder();
            if (posts.Count == 0)
            {
                Console.WriteLine("No hay post agregados.");
            }
            else
            {
                foreach (Domain.Post post in posts)
                {
                    Console.WriteLine("Nombre del post: " + post.Name);
                    Console.WriteLine("Fecha de creación: " + post.CreationDate);
                    string themes = "";
                    foreach (Domain.Theme theme in post.Themes)
                    {
                        themes += theme.Name + ", ";
                    }
                    themes = themes.Substring(0, themes.Length - 2);
                    Console.WriteLine("Temas asociados: " + themes);
                    Console.WriteLine();
                }
            }


        }

        public void ShowFiles()
        {
            var themeName = GetThemeName();
            var filter = DefineFilters();
            if (themeName == "")
            {
                ShowFiles(filter);
            }
            else
            {
                ShowFilesByTheme(themeName, filter);
            }
        }

        private void ShowFilesByTheme(string themeName, FileFilter filter)
        {
            var theme = themeLogic.GetThemeByName(themeName);
            if (theme == null)
            {
                Console.WriteLine("No existe un tema con ese nombre.");
            }
            else
            {
                foreach (Domain.Post post in theme.Posts)
                {
                    var files = postLogic.GetFilteredFiles(post, filter);
                    foreach (File file in files)
                    {
                        Console.WriteLine("Nombre: " + file.Name);
                        Console.WriteLine("Tamaño: " + file.Size);
                        Console.WriteLine("Fecha de creación: " + file.DateUploaded);
                        Console.WriteLine("Tema: " + theme.Name);
                        Console.WriteLine("Nombre del post: " + file.PostName);
                        Console.WriteLine();
                    }
                }
            }
        }

        private void ShowFiles(FileFilter filter)
        {
            var files = GetFiles(filter);
            foreach (File file in files)
            {
                var post = GetPostByName(file.PostName);
                string themes = "";
                foreach (Domain.Theme theme in post.Themes)
                {
                    themes += theme.Name + ", ";
                }
                themes = themes.Substring(0, themes.Length - 2);

                Console.WriteLine("Nombre: " + file.Name);
                Console.WriteLine("Tamaño: " + file.Size);
                Console.WriteLine("Fecha de creación: " + file.DateUploaded);
                Console.WriteLine("Nombre del post: " + file.PostName);
                Console.WriteLine("Temas asociados: " + themes);
                Console.WriteLine();
            }
        }

        private string GetThemeName()
        {
            Console.Write("Si desea mostrar los archivos por nombre de tema, ingrese el nombre del tema, de lo contrario presione enter: ");
            var themeName = Console.ReadLine();
            return themeName;
        }

        private FileFilter DefineFilters()
        {
            FileFilter filter;
            string sizeFilter = "";
            string nameFilter = "";
            string dateFilter = "";
            Console.Write("Si desea ordenar por tamaño de archivo ingrese 1, de lo contrario presione enter: ");
            sizeFilter = Console.ReadLine();
            if (sizeFilter != "1")
            {
                Console.Write("Si desea ordenar por nombre de archivo ingrese 1, de lo contrario presione enter: ");
                nameFilter = Console.ReadLine();
                if (nameFilter != "1")
                {
                    Console.Write("Si desea ordenar por fecha de subida de archivo ingrese 1, de lo contrario presione enter: ");
                    dateFilter = Console.ReadLine();
                }
            }
            filter = new FileFilter { DateFilter = dateFilter, NameFilter = nameFilter, SizeFilter = sizeFilter };
            return filter;
        }

        public void ShowThemes()
        {
            var themes = GetThemes();
            if (themes.Count == 0)
            {
                Console.WriteLine("No hay temas agregados.");
            }
            else
            {
                foreach (Domain.Theme theme in themes)
                {
                    Console.WriteLine("Nombre del tema: " + theme.Name + " - Descripción: " + theme.Description);
                    ShowPostFromATheme(theme);
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }


        }

        public void ShowPostFromATheme(Domain.Theme theme)
        {
            Console.Write("Posts: ");
            string posts = "";
            foreach (Domain.Post post in theme.Posts)
            {
                posts += post.Name + ", ";
            }
            if (posts != "")
            {
                posts = posts.Substring(0, posts.Length - 2);
            }
            Console.Write(posts);
        }

        public void ShowPostByTheme()
        {

            var themes = GetThemes();
            if (themes.Count == 0)
            {
                Console.WriteLine("No hay post agregados");
            }
            else
            {
                foreach (Domain.Theme theme in themes)
                {
                    Console.WriteLine("Tema: " + theme.Name);
                    foreach (Domain.Post post in theme.Posts)
                    {
                        Console.WriteLine("Post: " + post.Name);
                    }
                    Console.WriteLine();
                }
            }

        }

        public void ShowPost()
        {
            Console.Write("Ingrese el nombre del post que desea ver: ");
            string postName = Console.ReadLine();
            Domain.Post post = GetPostByName(postName);
            if (post != null)
            {
                Console.WriteLine("Nombre del post: " + post.Name);
                Console.WriteLine("Fecha de creación: " + post.CreationDate);
                string themes = "";
                foreach (Domain.Theme theme in post.Themes)
                {
                    themes += theme.Name + ", ";
                }
                themes = themes.Substring(0, themes.Length - 2);
                Console.WriteLine("Temas asociados: " + themes);
            }
            else
            {
                Console.WriteLine("Lo sentimos, no existe ningún post asociado a ese nombre");
            }
        }

        public void ShowFilesFromAPost()
        {
            Console.Write("Ingrese el nombre del post del cual desea ver los archivos: ");
            string postName = Console.ReadLine();
            Domain.Post post = GetPostByName(postName);
            if (post != null)
            {
                if (post.Files.Count > 0)
                {
                    foreach (File file in post.Files)
                    {
                        Console.WriteLine("Nombre del archivo: " + file.Name);
                        Console.WriteLine("Fecha de alta: " + file.DateUploaded);
                        Console.WriteLine("Tamaño: " + file.Size);
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine("El post no tiene archivos asociados.");
                }
            }
            else
            {
                Console.WriteLine("Lo sentimos, no existe un post asociado a ese nombre");
            }
        }

        public void ShowThemeWhithMorePosts()
        {
            var max = 0;
            var themes = GetThemes();
            var themesArray = themes.ToArray();
            for (int i = 0; i < themes.Count; i++)
            {
                if (themesArray[i].Posts.Count > max)
                {
                    max = themesArray[i].Posts.Count;
                }
            }
            foreach (Domain.Theme theme in themes)
            {
                if (theme.Posts.Count == max)
                {
                    Console.WriteLine("Nombre: " + theme.Name);
                    Console.WriteLine("Descripción: " + theme.Description);
                    foreach (Domain.Post post in theme.Posts)
                    {
                        Console.WriteLine("Post: " + post.Name);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
