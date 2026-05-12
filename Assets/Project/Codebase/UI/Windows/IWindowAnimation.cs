using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IWindowAnimation
{
    public UniTask OpenAnimation();
    public UniTask CloseAnimation();
}
