namespace RepositoryInterface
{
    public interface IThemeRepository
    {
        string AddTheme(Theme theme);
        string ModifyTheme(string themeName);
        string DeleteTheme(string themeName);
    }
}