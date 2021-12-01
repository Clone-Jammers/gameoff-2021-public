namespace Components.Theming
{
    public interface IThemeable
    {
        public ITheme Theme { get; }
        
        public void SetTheme(ITheme theme);
    }
}