using MenuAPI;

namespace Client.Menus
{
    public class RaceCreatorPlacementMenu : SubMenu
    {
        protected override void CreateMenu()
        {
            menu = new Menu(null, "Starting Point");
            
            MenuItem backBtn = new MenuItem("Finish");
            menu.AddMenuItem(backBtn);

            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item.Text == "Finish")
                {
                    rootMenu.isPlacingCP = false;
                    menu.GoBack();
                    MenuController.DisableBackButton = false;
                }
            };
        }
        
        public RaceCreatorPlacementMenu(MainMenu rootMenu) : base(rootMenu) { }
    }
}