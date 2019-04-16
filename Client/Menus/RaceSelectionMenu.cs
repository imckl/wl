using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuAPI;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace Client.Menus
{
    class RaceSelectionMenu
    {
        private Menu menu;

        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }

        private void CreateMenu()
        {
            menu = new Menu("Race Selector");

            //TODO: Read races dynamically from databse
            
        }
    }
}
