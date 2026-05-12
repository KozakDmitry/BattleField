using Assets.Project.Scripts.Data.Progress;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Project.CodeBase.Data.Progress
{
    public class PlayerProgress
    {
        public LevelData _levelData;

        public PlayerProgress()
        {
            _levelData = new LevelData();
        }

        public void SetFromSave(PlayerProgress progress)
        {
            if (progress == null)
            {
                return;
            }
        }
    }
}
