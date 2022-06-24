using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMenuEnable : MonoBehaviour
{
    [SerializeField] GameObject menu;
    private bool menuEnabled = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            if (menuEnabled == true)
            {
                menu.SetActive(false);
                menuEnabled = false;
            }
            else
            {
                menu.SetActive(true);
                menuEnabled = true;
            }
        }
    }
}
