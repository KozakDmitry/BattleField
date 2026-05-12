using Assets.Project.CodeBase.Data.Progress;

namespace Assets.Project.CodeBase.Infostructure.Services.ProgressService
{
    /// <summary>
    /// Класс для сохранений, есть только у сервисов, а они - передают доступ остальным
    /// </summary>
    public class ProgressService : IProgressService
    {
        public PlayerProgress Progress
        {
            get;
            set;
        }


        public ProgressService()
        {
            Progress = new PlayerProgress();
        }
    }
}
