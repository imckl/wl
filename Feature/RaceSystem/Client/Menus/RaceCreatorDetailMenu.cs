using System.Threading.Tasks;
using CitizenFX.Core;
using MenuAPI;
using CitizenFX.Core.Native;

namespace Client.Menus
{
    public class RaceCreatorDetailMenu : SubMenu
    {
        protected override void CreateMenu()
        {
            // Add buttons again and again...
            menu = new Menu("Race Creator", "Detail");
            MenuItem raceName = new MenuItem("Name");
            menu.AddMenuItem(raceName);
            
            MenuItem backBtn = new MenuItem("Finish");
            menu.AddMenuItem(backBtn);
            
            menu.OnItemSelect += async (_menu, _item, _index) =>
            {
                if (_item.Text == "Name")
                {
                    API.AddTextEntry("FMMC_KEY_TIP1", "Input race name:");
                    API.DisplayOnscreenKeyboard(1, "FMMC_KEY_TIP1", "", "", "", "", "", 32);
                    while (API.UpdateOnscreenKeyboard() != 1 && API.UpdateOnscreenKeyboard() != 2)
                    {
                        await BaseScript.Delay(0);
                    }

                    if (API.UpdateOnscreenKeyboard() != 2)
                    {
                        string result = API.GetOnscreenKeyboardResult();
                        await BaseScript.Delay(500);
                        _item.Label = result;
                    }
                }
            };

            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item.Text == "Finish")
                {
                    rootMenu.raceName = raceName.Label;
                    menu.GoBack();
                }
            };
        }
        
        public RaceCreatorDetailMenu(MainMenu rootMenu) : base(rootMenu) { }
    }
}