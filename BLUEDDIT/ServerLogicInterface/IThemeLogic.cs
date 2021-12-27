using Domain;
using System.Collections.Generic;


namespace ServerLogicInterface
{
    public interface IThemeLogic
    {
        Response AddTheme(Theme theme);
        Response DeleteTheme(string themeName);
        Response ModifyTheme(string themeName, Theme newTheme);
        List<Theme> GetThemes();
        List<Post> GetOrderPostFromTheme(Theme theme);
        Theme GetThemeByName(string themeName);
    }
}
