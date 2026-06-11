using UnityEngine;

namespace Assets.Project.Codebase.StaticData
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private MovementConfig _movementConfig;

        public MovementConfig MovementConfig => _movementConfig;
    }
}
