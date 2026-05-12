
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public abstract class SceneInitializing : MonoBehaviour
{
    public void Awake()
    {
        //if (!SceneTransition.IsLoading) Initialize();
    }

    public abstract UniTask Init();
}
