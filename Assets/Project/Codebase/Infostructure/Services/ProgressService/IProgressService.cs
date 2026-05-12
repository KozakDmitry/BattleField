using Assets.Project.CodeBase.Data;
using Assets.Project.CodeBase.Data.Progress;

namespace Assets.Project.CodeBase.Infostructure.Services.ProgressService
{
    public interface IProgressService
    {
        PlayerProgress Progress { get; set; }
    }
}