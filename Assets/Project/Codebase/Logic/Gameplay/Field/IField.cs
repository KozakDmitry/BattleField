using Assets.Project.Codebase.Logic.Gameplay.Player;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Field
{
    public interface IField
    {
        void Spawn(GameObject prefab);
        Vector3 GetSpawnPosition();
    }
}
