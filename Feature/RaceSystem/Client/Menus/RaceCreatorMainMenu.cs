using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuAPI;

namespace Client.Menus
{
    public class RaceCreatorMainMenu : SubMenu
    {
        private static RaceCreatorDetailMenu raceCreatorDetailMenu;
        private static RaceCreatorPlacementMenu raceCreatorPlacementMenu;
        
        public string TypeString;

        protected override void CreateMenu()
        {
            // Initialize menu as usual
            menu = new Menu("Race Creator", "Editor");
            raceCreatorDetailMenu = new RaceCreatorDetailMenu(rootMenu);
            raceCreatorPlacementMenu = new RaceCreatorPlacementMenu(rootMenu);
            MenuController.AddSubmenu(menu, raceCreatorDetailMenu.GetMenu());
            MenuController.AddSubmenu(menu, raceCreatorPlacementMenu.GetMenu());
            
            // Detail Button
            MenuItem detailBtn = new MenuItem("Race Details", "Change the details of the race.");
            menu.AddMenuItem(detailBtn);
            MenuController.BindMenuItem(menu, raceCreatorDetailMenu.GetMenu(), detailBtn);
            
            // Placement Button
            MenuItem placementBtn = new MenuItem("Placement", "Place your checkpoints!");
            menu.AddMenuItem(placementBtn);
            MenuController.BindMenuItem(menu, raceCreatorPlacementMenu.GetMenu(), placementBtn);
            
            // Add other buttons
            List<string> btns = new List<string>() { "Test", "Save", "Publish", "Exit" };
            btns.ForEach(btn => menu.AddMenuItem(new MenuItem(btn)));

            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                string selected = _item.Text;
                if (selected == "Exit")
                {
                    MenuController.DisableBackButton = true;
                    MenuController.CloseAllMenus();
                    rootMenu.requestCleanUp = true;
                }
                else if (selected == "Test")
                {
                    
                }
                else if (selected == "Save")
                {
                    rootMenu.requestSave = true;
                }
                else if (selected == "Publish")
                {
                    
                }
                else if (selected == "Placement")
                {
                    rootMenu.isPlacingCP = true;
                }
            };
        }

        public RaceCreatorMainMenu(MainMenu rootMenu) : base(rootMenu) { }
    }
}
