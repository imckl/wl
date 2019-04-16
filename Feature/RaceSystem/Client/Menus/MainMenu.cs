﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MenuAPI;

namespace Client.Menus
{
    class MainMenu
    {
        private Menu menu;
        private static RaceSelectionMenu raceSelectionMenu;
        private static RaceCreatorTypeMenu raceCreatorMenu;

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
            // Menu align with left side
            MenuController.MenuAlignment = MenuController.MenuAlignmentOption.Left;

            // Create the main menu
            menu = new Menu("Race System") { Visible = false };
            raceCreatorMenu = new RaceCreatorTypeMenu();
            raceSelectionMenu = new RaceSelectionMenu();

            MenuController.AddMenu(menu);
            MenuController.MainMenu = menu;

            // Menu buttons
            MenuItem raceStartMenuBtn = new MenuItem("Start Race", "Start a race you want!");
            MenuItem createRaceMenuBtn = new MenuItem("Create Race", "Try to create your own race!");
            MenuItem exitMenuBtn = new MenuItem("Exit", "Enjoy the race next time!");

            menu.AddMenuItem(raceStartMenuBtn);
            menu.AddMenuItem(createRaceMenuBtn);
            menu.AddMenuItem(exitMenuBtn);

            // Add and bind buttons to submenus
            MenuController.AddSubmenu(menu, raceCreatorMenu.GetMenu());
            MenuController.AddSubmenu(menu, raceSelectionMenu.GetMenu());
            MenuController.BindMenuItem(menu, raceCreatorMenu.GetMenu(), createRaceMenuBtn);
            MenuController.BindMenuItem(menu, raceSelectionMenu.GetMenu(), raceStartMenuBtn);

            // Button handler for exit button
            menu.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_item.Text == "Exit")
                    menu.CloseMenu();
            };
        }
    }
}