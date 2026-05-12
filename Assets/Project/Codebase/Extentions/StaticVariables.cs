using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Project.CodeBase.Extentions
{

    public static class SceneNames
    {
        public static readonly Dictionary<Name, string> Names = new()
        {
            { Name.Start, "Splash" },
            { Name.Menu, "Menu" },
            { Name.GamePlay, "Game" },
            { Name.Loading, "Loading" }
        };
        public static string GetSceneName(Name scene) => Names[scene];
        public enum Name
        {
            Start,
            Menu,
            GamePlay,
            Loading
        }

    }
}
