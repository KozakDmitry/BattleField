using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Project.Scripts.Data.Progress
{
    [Serializable]
    public class LevelData
    {
        public LevelData()
        {
            currentLevel = 0;
        }

        public int currentLevel;
    }
}
