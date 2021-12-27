using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServerRepositoryInterface
{
    public interface IThemeRepository
    {
        void AddTheme(Theme theme);
        void DeleteTheme(Theme theme);
        Theme GetThemeByName(string name);
        List<Theme> GetThemes();
    }
}
