using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Field
{
    public interface IField
    {
        void Spawn(GameObject prefab);
        GameObject FieldObject { get; }
    }
}
