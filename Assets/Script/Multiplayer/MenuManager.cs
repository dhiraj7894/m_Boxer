using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MenuManager : Singleton<MenuManager>
{
    public List<Menu> Menu = new List<Menu>();

    public void OpenMenu(string name)
    {
        for (int i = 0; i < Menu.Count; i++)
        {
            if(Menu[i].MenuName == name)
            {
                OpenMenu(Menu[i]);
            }
            else if(Menu[i].isOpen)
            {
                CloseMenu(Menu[i]);
            }
        }
    }
    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < Menu.Count; i++)
        {
            if (Menu[i].isOpen)
            {
                CloseMenu(Menu[i]);
            }            
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu)
    {
        menu.Close();
    }

}
