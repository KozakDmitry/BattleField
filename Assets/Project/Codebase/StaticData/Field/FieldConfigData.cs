using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.CodeBase.StaticData.Field
{
    [CreateAssetMenu(fileName = "FieldConfig", menuName = "StaticData/Field/FieldConfig")]
    public class FieldConfigData : ScriptableObject
    {
        public Vector2 relativeFieldSize;
        public Vector2 relativePosition;
    }

}
