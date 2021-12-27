using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using ServerLogic;
using ServerLogicInterface;

namespace Server_GPRC_MQ
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        private IThemeLogic themeLogic;
        private IPostLogic postLogic;
        private CommonLog commonLog;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
            themeLogic = new ThemeLogic();
            postLogic = new PostLogic();
            commonLog = new CommonLog();
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override Task<CommonReply> AddTheme (AddThemeRequest request, ServerCallContext context)
        {
            // se agrega el tema 
            var theme = new Domain.Theme { Name = request.Theme.Name, Description = request.Theme.Description };
            var response = themeLogic.AddTheme(theme);
            commonLog.AddLog(request.Username, response);
            return Task.FromResult(new CommonReply
            {
                Message = response.Message
            });
        }

        public override Task<CommonReply> ModifyTheme(ModifyThemeRequest request, ServerCallContext context)
        {
            var oldThemeName = request.OldName;
            var theme = new Domain.Theme { Name = request.NewTheme.Name, Description = request.NewTheme.Description };
            var response = themeLogic.ModifyTheme(oldThemeName, theme);
            commonLog.AddLog(request.Username, response);
            return Task.FromResult(new CommonReply
            {
                Message = response.Message
            });
        }

        public override Task<CommonReply> DeleteTheme(DeleteThemeRequest request, ServerCallContext context)
        {
            var themeName = request.Name;
            var response = themeLogic.DeleteTheme(themeName);
            commonLog.AddLog(request.Username, response);
            return Task.FromResult(new CommonReply
            {
                Message = response.Message
            });
        }

        public override Task<CommonReply> AddPost(AddPostRequest request, ServerCallContext context)
        {
            var post = new Domain.Post { Name = request.Post.Name };
            var themeName = request.ThemeName;
            var response = postLogic.PostPost(post, themeName);
            commonLog.AddLog(request.Username, response);
            return Task.FromResult(new CommonReply
            {
                Message = response.Message
            });
        }

        public override Task<CommonReply> ModifyPost(ModifyPostRequest request, ServerCallContext context)
        {
            var post = new Domain.Post { Name = request.NewPost.Name };
            var oldPostName = request.OldName;
            var response = postLogic.ModifyPost(oldPostName, post);
            commonLog.AddLog(request.Username, response);
            return Task.FromResult(new CommonReply
            {
                Message = response.Message
            });
        }

        public override Task<CommonReply> DeletePost(DeletePostRequest request, ServerCallContext context)
        {
            var postName = request.Name;
            var response = postLogic.DeletePost(postName);
            commonLog.AddLog(request.Username, response);
            return Task.FromResult(new CommonReply
            {
                Message = response.Message
            });
        }

        public override Task<CommonReply> AssociatePostToTheme(AssociatePostToThemeRequest request, ServerCallContext context)
        {
            var postName = request.PostName;
            var themeName = request.ThemeName;
            var response = postLogic.AssociatePostToTheme(postName, themeName);
            commonLog.AddLog(request.Username, response);
            return Task.FromResult(new CommonReply
            {
                Message = response.Message
            });
        }

        public override Task<CommonReply> DessassociatePostToTheme(DessassociatePostToThemeRequest request, ServerCallContext context)
        {
            var postName = request.PostName;
            var themeName = request.ThemeName;
            var response = postLogic.DissassosiatePostToTheme(postName, themeName);
            commonLog.AddLog(request.Username, response);
            return Task.FromResult(new CommonReply
            {
                Message = response.Message
            });
        }
    }
}
