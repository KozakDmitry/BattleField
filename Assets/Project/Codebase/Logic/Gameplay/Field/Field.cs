using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Field
{
    public class Field : MonoBehaviour, IField
    {
        public GameObject FieldObject { get; private set; }

        public void Spawn(GameObject prefab)
        {
            FieldObject = Object.Instantiate(prefab);
        }
    }
}
