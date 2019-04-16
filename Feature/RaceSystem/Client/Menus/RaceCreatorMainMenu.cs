using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuAPI;

namespace Client.Menus
{
    class RaceCreatorMainMenu
    {
        private Menu menu;
        public string typeString;

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
            menu = new Menu("Race Creator");
        }
    }
}
