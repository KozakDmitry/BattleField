using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Project.Codebase.Logic.Gameplay.Field.SpawnPoints
{
    public class SpawnPoint : MonoBehaviour
    {
        public Vector3 GetSpawnPosition() => 
            transform.position;
    }
}
