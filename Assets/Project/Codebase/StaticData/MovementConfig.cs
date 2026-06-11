using UnityEngine;

namespace Assets.Project.Codebase.StaticData
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Configs/Movement Config")]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField] private float _walkSpeed = 1f;
        [SerializeField] private float _runSpeed = 2.5f;
        [SerializeField] private float _maxVitality = 100f;
        [SerializeField] private float _vitalityDrainRate = 20f;
        [SerializeField] private float _vitalityRegenRate = 15f;

        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float MaxVitality => _maxVitality;
        public float VitalityDrainRate => _vitalityDrainRate;
        public float VitalityRegenRate => _vitalityRegenRate;
    }
}
