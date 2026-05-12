using Assets.Project.CodeBase.Data;
using Assets.Project.CodeBase.Data.Progress;
using Cysharp.Threading.Tasks;

namespace Assets.Project.CodeBase.Infostructure.Services.SaveService
{

    public interface ISavedProgress
    {
        void UpdateProgress(PlayerProgress progress);
    }
}
