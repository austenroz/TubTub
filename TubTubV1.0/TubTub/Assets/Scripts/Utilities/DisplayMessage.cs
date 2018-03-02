//Display a message when play gets close enough to object this script is attached to.

using UnityEngine;
using System.Collections;

public class DisplayMessage : MonoBehaviour {

    public string s_GuiMessage = "Hit \"F\" to open gate!";
    bool b_DisplayGUI = false;
    public GUIStyle gs_MyGUIStyle;
    GameObject go_Player;
    GameObject go_Player2;

    GameObject[] go_Players;
    ShopMenu ref_ShopMenu;
    PauseMenu ref_PauseMenu;

    void Start()
    {
        go_Players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < go_Players.Length; i++)
        {
            Debug.Log(go_Players[i].GetComponent<ShopMenu>());
            if (go_Players[i].GetComponent<ShopMenu>() != null)
            {
                ref_ShopMenu = go_Players[i].GetComponent<ShopMenu>();
                ref_PauseMenu = go_Players[i].GetComponent<PauseMenu>();
            }
        }
    }

    void OnTriggerEnter(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            if (go_Player == null && hit.gameObject != go_Player2)
            {
                b_DisplayGUI = true;
                go_Player = hit.gameObject;
            }
            else if (go_Player2 == null && hit.gameObject != go_Player)
            {
                b_DisplayGUI = true;
                go_Player2 = hit.gameObject;
            }
        }
    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.gameObject.tag == "Player")
        {
            if (hit.gameObject == go_Player)
            {
                go_Player = null;
            }
            else
            {
                go_Player2 = null;
            }
            if (go_Player == null && go_Player2 == null)
            {
                b_DisplayGUI = false;
            }
        }
    }

    void OnGUI()
    {
        if (b_DisplayGUI)
        {
            if (!ref_PauseMenu.b_IsShowingPauseMenu && !ref_ShopMenu.b_IsShowingShopMenu)
            {
                if (go_Player != null)
                {
                    float offsetHeight = go_Player.transform.Find("Main Camera").GetComponent<Camera>().rect.y;
                    GUI.Box(new Rect(0, (Screen.height - (Screen.height * offsetHeight)) - 60, Screen.width, 45), "");
                    GUI.Box(new Rect(0, (Screen.height - (Screen.height * offsetHeight)) - 60, Screen.width, 45), "");
                    GUI.Label(new Rect(0, (Screen.height - (Screen.height * offsetHeight)) - 55, Screen.width, 20), s_GuiMessage, gs_MyGUIStyle);
                }
                if (go_Player2 != null)
                {
                    float offsetHeight = go_Player2.transform.Find("Main Camera").GetComponent<Camera>().rect.y;
                    GUI.Box(new Rect(0, (Screen.height - (Screen.height * offsetHeight)) - 60, Screen.width, 45), "");
                    GUI.Box(new Rect(0, (Screen.height - (Screen.height * offsetHeight)) - 60, Screen.width, 45), "");
                    GUI.Label(new Rect(0, (Screen.height - (Screen.height * offsetHeight)) - 55, Screen.width, 20), s_GuiMessage, gs_MyGUIStyle);
                }
            }
        }
        //GUI.Label(new Rect(10, (Screen.height - (Screen.height * offsetHeight)) - 25, 200, 20), "Boat Health: " + boatHealth, healthDisplay);
    }
}
