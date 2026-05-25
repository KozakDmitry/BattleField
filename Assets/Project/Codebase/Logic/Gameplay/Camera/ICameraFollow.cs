using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Cam
{
    internal interface ICameraFollow
    {
        void FollowTarget(Transform target);
    }
}