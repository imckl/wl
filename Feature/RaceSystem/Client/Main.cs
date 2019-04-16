using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CitizenFX.Core;
using static CitizenFX.Core.Native.API;
using Client.Menus;

namespace Client
{
    public class Main : BaseScript
    {
        private static MainMenu mainMenu;

        public Main()
        {
            mainMenu = new MainMenu();
            RegisterCommand("race", new Action<int, List<object>, string>((source, args, raw) => RaceCommand(source, args, raw)), false);
        }

        private void RaceCommand(int source, List<object> args, string raw)
        {
            mainMenu.GetMenu().OpenMenu();
        }
    }
}
