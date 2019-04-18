using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuAPI;
using CitizenFX.Core;
using CitizenFX.Core.Native;

namespace Client.Menus
{
    public class RaceCreatorTypeMenu : SubMenu
    {
        private static RaceCreatorMainMenu raceCreatorMainMenu;

        protected override void CreateMenu()
        {
            menu = new Menu("Race Creator", "Race Type");
            raceCreatorMainMenu = new RaceCreatorMainMenu(rootMenu);
            MenuController.AddSubmenu(menu, raceCreatorMainMenu.GetMenu());

            // Initialize race type items
            List<string> items = new List<string>() { "Land Race", "Air Race", "Sea Race" };
            items.ForEach(item => AddButton(new MenuItem(item)));
            
            // Button handler
            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                string selectedTypeString = _item.Text;

                // Pass the type string to new creator submenu
                raceCreatorMainMenu.TypeString = selectedTypeString;

                MenuController.DisableBackButton = true;
            };
        }

        private void AddButton(MenuItem menuItem)
        {
            // Add button to menu and bind them to submenus
            menu.AddMenuItem(menuItem);
            MenuController.BindMenuItem(menu, raceCreatorMainMenu.GetMenu(), menuItem);
        }
        
        public RaceCreatorTypeMenu(MainMenu rootMenu) : base(rootMenu) { }
    }
}
