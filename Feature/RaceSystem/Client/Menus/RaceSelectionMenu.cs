using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuAPI;

namespace Client.Menus
{
    public class RaceSelectionMenu : SubMenu
    {
        protected override void CreateMenu()
        {
            menu = new Menu("Race Selector");

            // TODO: Read races dynamically from database
        }
        
        public RaceSelectionMenu(MainMenu rootMenu) : base(rootMenu) { }
    }
}
