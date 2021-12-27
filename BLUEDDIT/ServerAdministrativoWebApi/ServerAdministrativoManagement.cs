using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace ServerAdministrativoWebApi
{
    public class ServerAdministrativoManagement
    {
        private static Greeter.GreeterClient client;
        public ServerAdministrativoManagement()
        {

        }
        public void Connect()
        {
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            Console.WriteLine("Bienvenido al GRPC Client!");

            // creo un canal de comunicación
            var channel = GrpcChannel.ForAddress("http://localhost:5007");

            // creo un greeter y le paso el channel 
            client = new Greeter.GreeterClient(channel);
        }

        public async Task<string> CreateThemeAsync(Theme theme, string username)
        {
            var replayAddTheme = await client.AddThemeAsync(
                new AddThemeRequest { 
                    Theme = theme, 
                    Username = username 
                });
            return replayAddTheme.Message;
        }

        public async Task<string> UpdateThemeAsync(string oldName, Theme newTheme, string username)
        {
            var replayUpdateTheme = await client.ModifyThemeAsync(
                new ModifyThemeRequest { 
                    NewTheme = newTheme, 
                    OldName = oldName, 
                    Username = username 
                });
            return replayUpdateTheme.Message;
        }

        public async Task<string> DeleteThemeAsync(string themeName, string username)
        {
            var replayDeleteTheme = await client.DeleteThemeAsync(
                new DeleteThemeRequest {
                    Name = themeName, 
                    Username = username 
                });
            return replayDeleteTheme.Message;
        }

        public async Task<string> CreatePostAsync(Post post, string themeName, string username)
        {
            var replayCreatePost = await client.AddPostAsync(
                new AddPostRequest { 
                    Post = post, 
                    ThemeName = themeName, 
                    Username = username 
                });
            return replayCreatePost.Message;
        }

        public async Task<string> ModifyPostAsync(Post post, string oldName, string username)
        {
            var replayModifyPost = await client.ModifyPostAsync(new ModifyPostRequest
            {
                NewPost = post,
                OldName = oldName,
                Username = username
            });
            return replayModifyPost.Message;
        }

        public async Task<string> DeletePostAsync(string postName, string username)
        {
            var replayDeletePost = await client.DeletePostAsync(
                new DeletePostRequest
                {
                    Name = postName,
                    Username = username
                });
            return replayDeletePost.Message;
        }

        public async Task<string> AssociatePostToThemeAsync(string postName, string themeName, string username)
        {
            var replayAssociatePostToTheme = await client.AssociatePostToThemeAsync(
                new AssociatePostToThemeRequest
                {
                    PostName = postName,
                    ThemeName = themeName,
                    Username = username
                });
            return replayAssociatePostToTheme.Message;
        }

        public async Task<string> DissasociatePostToThemeAsync(string postName, string themeName, string username)
        {
            var replayDissasociatePostToTheme = await client.DessassociatePostToThemeAsync(
                new DessassociatePostToThemeRequest
                {
                    PostName = postName,
                    ThemeName = themeName,
                    Username = username
                });
            return replayDissasociatePostToTheme.Message;
        }
    }
}
