using MenuAPI;

namespace Client.Menus
{
    public class SubMenu
    {
        protected MainMenu rootMenu;
        protected Menu menu;

        protected SubMenu(MainMenu rootMenu)
        {
            this.rootMenu = rootMenu;
        }
        
        public Menu GetMenu()
        {
            if (menu == null)
            {
                CreateMenu();
            }
            return menu;
        }

        protected virtual void CreateMenu()
        {
        }
    }
}