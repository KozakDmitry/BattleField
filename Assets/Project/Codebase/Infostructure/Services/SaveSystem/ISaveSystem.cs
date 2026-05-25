namespace Assets.Project.CodeBase.Infostructure.Services.SaveSystem
{
    public interface ISaveSystem
    {
        void Save<T>(string key, T data);
        T Load<T>(string key) where T : class;
    }
}
