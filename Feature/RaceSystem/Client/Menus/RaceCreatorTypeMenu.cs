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
    class RaceCreatorTypeMenu
    {
        private Menu menu;
        private Vector3 origCamCoord;
        private static RaceCreatorMainMenu raceCreatorMainMenu;

        public bool isPlacingCP;

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
            raceCreatorMainMenu = new RaceCreatorMainMenu();
            MenuController.AddSubmenu(menu, raceCreatorMainMenu.GetMenu());

            // Initialize race type items
            List<string> items = new List<string>() { "Land Race", "Air Race", "Sea Race" };
            items.ForEach(item => AddButton(raceCreatorMainMenu.GetMenu(), new MenuItem(item)));
            
            // Button handler
            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                string selectedTypeString = _item.Text;

                // Pass the type string to new creator submenu
                raceCreatorMainMenu.typeString = selectedTypeString;

                // Setup the environment for player
                PlayerSetup();
            };
        }

        private void AddButton(Menu subMenu, MenuItem menuItem)
        {
            menu.AddMenuItem(menuItem);
            MenuController.BindMenuItem(menu, subMenu, menuItem);
        }

        private void PlayerSetup()
        {
            int playerCamHandle = GetRenderingCam();
            origCamCoord = GetCamCoord(playerCamHandle);

            // Raise the camera to the sky
            // TODO: Change Z axis
            SetCamCoord(playerCamHandle, origCamCoord.X, origCamCoord.Y, origCamCoord.Z);
        }
    }
}
