using ServerRepositoryInterface;
using System.Collections.Generic;
using Domain;


namespace ServerRepository
{
    public class ThemeRepository : IThemeRepository
    {
        private static ThemeRepository Instance = null;
        private static readonly object ObjectLock = new object();
        public List<Theme> Themes { get; set; }

        private ThemeRepository() 
        {
            this.Themes = new List<Theme>();
        }

        public static ThemeRepository GetInstance()
        {
            if(ThemeRepository.Instance == null)
            {
                lock (ThemeRepository.ObjectLock)
                {
                    if (ThemeRepository.Instance == null)
                    {
                        ThemeRepository.Instance = new ThemeRepository();
                    }
                }
            }
            return ThemeRepository.Instance;
        }

        public void AddTheme(Theme theme)
        {
            Themes.Add(theme);
        }

        public void DeleteTheme(Theme theme)
        {
            Themes.Remove(theme);
        }

        public Theme GetThemeByName(string name)
        {
            Theme theme = null;
            foreach (Theme themeDB in Themes)
            {
                if (themeDB.Name == name)
                {
                    theme = themeDB;
                }
            }
            return theme;
        }

        public List<Theme> GetThemes()
        {
            return this.Themes;
        }
    }
}