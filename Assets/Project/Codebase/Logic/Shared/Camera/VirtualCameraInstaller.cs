using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Shared.Cam
{
    public class VirtualCameraInstaller : MonoBehaviour
    {
        [SerializeField]
        private CinemachineCamera _virtualCamera;
        [SerializeField]
        private CinemachineOrbitalFollow _orbitalFollow;
        
        private void Awake()
        {
            DI.Register<VirtualCameraInstaller>(this, mode: RegisterMode.scene);
        }

        public CinemachineOrbitalFollow OrbitalFollow =>
            _orbitalFollow;

        public CinemachineCamera CinemachineCamera => 
            _virtualCamera;

    }
}
