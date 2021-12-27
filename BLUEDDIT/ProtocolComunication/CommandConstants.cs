using System;
namespace ProtocolComunication
{
    public static class CommandConstants
    {


        public const short AddTheme = 1;

        public const short DeleteTheme = 2;

        public const short UpdateTheme = 3;

        public const short AddPost = 4;

        public const short DeletePost = 5;

        public const short UpdatePost = 6;

        public const short AsociatePostToTheme = 7;

        public const short DesasociatePostToTheme = 8;

        public const short AddFileToPost = 9;

        public const short SendUsername = 10;

        public const short LoadUsername = 11; 

        public const short DesconectClient = 12;
        
    }
}
