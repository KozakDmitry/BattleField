using Assets.Project.CodeBase.Infostructure.Services;
using System;
using UnityEngine;

namespace Assets.Project.CodeBase.Infostructure.Input
{
    public interface IInputService
    {
        event InputService.Move OnMoveEvent;
        event InputService.EndT OnEndEvent;

        void Disable();
        void Enable();
    }
}