using ServerRepositoryInterface;
using ServerLogicInterface;
using ServerRepository;
using System;
using System.Collections.Generic;
using System.Text;
using Domain;
using System.Threading;
using System.Linq;

namespace ServerLogic
{
    public class PostLogic : IPostLogic
    {
        private IPostRepository postRepository;
        private IThemeRepository themeRepository;
        private static readonly object locker = new object();
        private CommonLogic commonLogic;
        public PostLogic()
        {
            this.postRepository = PostRepository.GetInstance();
            this.themeRepository = ThemeRepository.GetInstance();
            commonLogic = new CommonLogic();
        }

        

        public Response DeletePost(string namePost)
        {
            Monitor.Enter(locker);
            try
            {
                Post postDb = this.postRepository.GetPostByName(namePost);
                var response = new Response();
                if(postDb != null)
                {
                    postRepository.DeletePost(postDb);
                    DeletePostFromThemes(postDb);
                    string message = $"Se ha eliminado el post con nombre {namePost} correctamente!";
                    return commonLogic.GenerateInfoResponse(message, "Post");
                }
                else
                {
                    string message = $"Lo sentimos, no existe un post con nombre {namePost}.";
                    return commonLogic.GenerateWarningResponse(message, "Post");
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }

        }

        public Response AssociatePostToTheme(string namePost, string nameTheme)
        {
            Monitor.Enter(locker);
            try
            {
                Post postDb = postRepository.GetPostByName(namePost);
                Theme themeDb = themeRepository.GetThemeByName(nameTheme);
                var response = new Response();
                if(postDb != null)
                {
                    if(themeDb != null)
                    {
                        if (postDb.Themes.Contains(themeDb))
                        {
                            string message = $"Lo sentimos, el post {namePost} ya está asociado al tema {nameTheme}.";
                            return commonLogic.GenerateWarningResponse(message, "Post");
                        }
                        else
                        {
                            postDb.Themes.Add(themeDb);
                            themeDb.Posts.Add(postDb);
                            string message = $"El post {namePost} fue asociado al tema {nameTheme} correctamente!";
                            return commonLogic.GenerateWarningResponse(message, "Post");
                        }
                    }
                    else
                    {
                        var message = $"Lo sentimos, no existe un tema con el nombre {nameTheme}.";
                        return commonLogic.GenerateWarningResponse(message, "Post");
                    }
                }
                else
                {
                    var message = $"Lo sentimos, no existe un post con el nombre {namePost}.";
                    return commonLogic.GenerateWarningResponse(message, "Post");
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }

        public Response PostPost(Post post, string themeName)
        {
            Monitor.Enter(locker);
            try
            {
                Post postDb = postRepository.GetPostByName(post.Name);
                if(postDb == null)
                {
                    Theme theme = themeRepository.GetThemeByName(themeName);
                    if (theme != null)
                    {
                        post.Themes.Add(theme);
                        theme.Posts.Add(post);
                        postRepository.AddPost(post);
                        var message = $"El post con nombre {post.Name} fue agregado correctamente!";
                        return commonLogic.GenerateInfoResponse(message, "Post");
                    }
                    else
                    {
                        var message = $"Lo sentimos, no existe un tema con ese nombre {themeName}.";
                        return commonLogic.GenerateWarningResponse(message, "Post");
                    }
                }
                else
                {
                    var message = $"Lo sentimos, ya existe un post con ese nombre {post.Name}.";
                    return commonLogic.GenerateWarningResponse(message, "Post");
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }

        public Response ModifyPost(string oldPostName, Post newPost)
        {
            Monitor.Enter(locker);
            try
            {
                Post oldPost = postRepository.GetPostByName(oldPostName);
                if (oldPost != null)
                {
                    Post postDb = postRepository.GetPostByName(newPost.Name);
                    if (postDb == null)
                    {
                        oldPost.Name = newPost.Name;
                        var message = $"El post {oldPostName} ha sido modificado correctamente con el nombre {newPost.Name}!";
                        return commonLogic.GenerateInfoResponse(message, "Post");
                    }
                    else
                    {
                        var message = $"Lo sentimos, ya existe un post con el nombre {newPost.Name}.";
                        return commonLogic.GenerateWarningResponse(message, "Post");
                    }
                }
                else
                {
                    var message = $"Lo sentimos, no existe un post con el nombre {oldPostName}.";
                    return commonLogic.GenerateWarningResponse(message, "Post");
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }

        public Response DissassosiatePostToTheme(string namePost, string nameTheme)
        {
            Monitor.Enter(locker);
            try
            {
                var postDb = postRepository.GetPostByName(namePost);
                var themeDb = themeRepository.GetThemeByName(nameTheme);
                if(postDb != null)
                {
                    if(themeDb != null)
                    {
                        if (postDb.Themes.Contains(themeDb))
                        {
                            if(postDb.Themes.Count > 1)
                            {
                                postDb.Themes.Remove(themeDb);
                                themeDb.Posts.Remove(postDb);
                                var message = $"El tema {nameTheme} fue desasociado al post {namePost} correctamente!";
                                return commonLogic.GenerateInfoResponse(message, "Post");
                            }
                            else
                            {
                                var message = $"Lo sentimos, el post {namePost} no puede quedar sin temas asociados.";
                                return commonLogic.GenerateWarningResponse(message, "Post");
                            }
                        }
                        else
                        {
                            var message = $"Lo sentimos, el tema {nameTheme} no está asociado al post {namePost}.";
                            return commonLogic.GenerateWarningResponse(message, "Post");
                        }
                    }
                    else
                    {
                        var message = $"Lo sentimos, no existe un tema con nombre {nameTheme} asociado al post {namePost}.";
                        return commonLogic.GenerateWarningResponse(message, "Post");
                    }
                }
                else
                {
                    var message = $"Lo sentimos, no existe un post con nombre {namePost}.";
                    return commonLogic.GenerateWarningResponse(message, "Post");
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }

        public Post GetPostByName(string name)
        {
            return postRepository.GetPostByName(name);
        }

        public void DeletePostFromThemes(Post post)
        {
            var themes = post.Themes;
            foreach(Theme theme in themes)
            {
                theme.Posts.Remove(post);
            }
        }

        public List<Post> GetPostsByOrder()
        {
            return postRepository.GetPostsByOrder();
        }

        public List<File> GetFilteredFiles(Post post, FileFilter filter)
        {
            var files = post.Files;
            if (filter.NameFilter == "1")
            {
                return files.OrderBy(file => file.Name).ToList();
            }
            else if (filter.SizeFilter == "1")
            {
                return files.OrderBy(file => file.Size).ToList();
            }
            else
            {
                return files.OrderBy(file => file.DateUploaded).ToList();
            }
        }
    }
}
