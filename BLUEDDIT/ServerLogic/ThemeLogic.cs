using Domain;
using ServerRepository;
using ServerRepositoryInterface;
using ServerLogicInterface;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ServerLogic
{
    public class ThemeLogic : IThemeLogic
    {
        private IThemeRepository themeRepository;
        private static readonly object locker = new object();
        private CommonLogic commonLogic;

        public ThemeLogic()
        {
            this.themeRepository = ThemeRepository.GetInstance();
            commonLogic = new CommonLogic();
        }

        public Response AddTheme(Theme theme)
        {
            Monitor.Enter(locker);
            try
            {
                var themeDB = themeRepository.GetThemeByName(theme.Name);
                var response = new Response();
                if (themeDB == null)
                { 
                    themeRepository.AddTheme(theme);
                    string message = $"Se agregó tema con nombre {theme.Name} y descripción {theme.Description}, correctamente!";
                    return commonLogic.GenerateInfoResponse(message, "Theme");
                }
                else {
                    string message = $"Lo sentimos, ya existe un tema con el nombre {theme.Name}.";
                    return commonLogic.GenerateWarningResponse(message, "Theme");
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }
        }

        public Response DeleteTheme(string themeName)
        {
            Monitor.Enter(locker);
            try
            {
                var themeDB = themeRepository.GetThemeByName(themeName);
                var response = new Response();
                if (themeDB != null)
                {
                    if (themeDB.Posts.Count == 0)
                    {
                        DeletePostFromThemes(themeDB);
                        themeRepository.DeleteTheme(themeDB);
                        string message = $"El tema con nombre {themeName} se ha borrado correctamente correctamente!";
                        return commonLogic.GenerateInfoResponse(message, "Theme");
                    }
                    else
                    {
                        string message = $"Lo sentimos, el tema con nombre {themeName} está asociado a un post.";
                        return commonLogic.GenerateWarningResponse(message, "Theme");
                    }
                }
                else 
                {
                    string message = $"Lo sentimos, el tema con nombre {themeName} no existe.";
                    return commonLogic.GenerateWarningResponse(message, "Theme");
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }               
        }

        public Response ModifyTheme(string themeName, Theme newTheme)
        {
            Monitor.Enter(locker);
            try
            {
                var response = new Response();
                var oldTheme = themeRepository.GetThemeByName(themeName);
                if (oldTheme != null)
                {
                    var themeDb = themeRepository.GetThemeByName(newTheme.Name);
                    if(themeDb == null)
                    {
                        oldTheme.Name = newTheme.Name;
                        string message = $"El tema con nombre {themeName} ha sido actualizado con el nombre {newTheme.Name}!";
                        return commonLogic.GenerateInfoResponse(message, "Theme");
                    }
                    else
                    {
                        string message = $"Lo sentimos, ya existe un tema con el nombre {newTheme.Name}.";
                        return commonLogic.GenerateWarningResponse(message, "Theme");
                    }
                }
                else
                {
                    string message = $"Lo sentimos, no existe un tema con el nombre {themeName}.";
                    return commonLogic.GenerateWarningResponse(message, "Theme");
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }

        }

        public List<Theme> GetThemes()
        {
            return themeRepository.GetThemes();
        }

        public void DeletePostFromThemes(Theme theme)
        {
            var posts = theme.Posts;
            foreach (Post post in posts)
            {
                post.Themes.Remove(theme);
            }
        }

        public List<Post> GetOrderPostFromTheme(Theme theme)
        {
            return theme.Posts.OrderBy(post => post.CreationDate).ToList();
        }

        public Theme GetThemeByName(string themeName)
        {
            return themeRepository.GetThemeByName(themeName);
        }
    }
}
