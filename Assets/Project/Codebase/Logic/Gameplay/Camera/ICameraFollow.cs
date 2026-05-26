using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Cam
{
    internal interface ICameraFollow
    {
        Transform CameraTransform { get; }

        void FollowTarget(Transform target);
    }
}