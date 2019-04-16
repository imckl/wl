using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuAPI;

namespace Client.Menus
{
    class RaceCreatorMenu
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
            menu = new Menu("Race Creator", "Race Type");

            // Initialize race type items
            List<string> items = new List<string>() { "Land Race", "Air Race", "Sea Race" };
            items.ForEach(item => menu.AddMenuItem(new MenuItem(item)));

            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                string selectedTypeString = _item.Text;

                //TODO:Pass the type string to new creator submenu
            };
        }
    }
}
