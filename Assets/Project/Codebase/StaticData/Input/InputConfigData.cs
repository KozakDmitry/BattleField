using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.CodeBase.StaticData.Input
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "StaticData/Input/InputConfig")]
    public class InputConfigData : ScriptableObject
    {
        public float MinimumSwipeRange;
        public float MaximumSwipeTime;
    }
}
