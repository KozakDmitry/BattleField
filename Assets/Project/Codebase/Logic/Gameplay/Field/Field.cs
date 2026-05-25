using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Field
{
    public class Field : MonoBehaviour, IField
    {
        public GameObject FieldObject { get; private set; }

        public Vector3 GetSpawnPosition()
        {
            if (FieldObject == null)
                return Vector3.zero;

            var fieldObj = FieldObject.GetComponent<FieldObject>();
            return fieldObj != null && fieldObj.spawnPoint != null
                ? fieldObj.spawnPoint.GetSpawnPosition()
                : FieldObject.transform.position;
        }

        public void Spawn(GameObject prefab)
        {
            FieldObject = Object.Instantiate(prefab);
        }
    }
}
