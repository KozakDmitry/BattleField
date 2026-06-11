using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Player
{
    public interface IPlayerSpawner
    {
        MovementController PlayerInstance { get; }
        UniTask Spawn(Vector3 position);
        void CleanUp();
    }
}
