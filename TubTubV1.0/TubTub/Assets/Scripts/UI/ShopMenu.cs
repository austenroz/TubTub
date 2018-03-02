//Deals with everything regarding the shop gui and upgrading stats.

using UnityEngine;
using System.Collections;

public class ShopMenu : MonoBehaviour
{
    private GameManager ref_GameManager;
    private PauseMenu ref_PauseMenu;
    private CurrentLevelStats ref_CurrentLevelStats;

    public bool b_IsShowingShopMenu = false;
    private int n_NavigationNumber = 1;
    private float f_MenuDelay = 0.0f;
    public GUIStyle gs_TitleText;
    public GUIStyle gs_ButtonText;
    public GUIStyle gs_AvailableText;
    public GUIStyle gs_NotAvailableText;
    public GUIStyle gs_DescriptionText;
    public GUIStyle gs_DescriptionText2;
    private float f_CustomDeltaTime = 0.0f;
    private CurrentLevelStats ref_LevelStats;

    public GameObject go_MenuSelect;
    public GameObject go_MenuMove;

    void Start()
    {
        ref_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        ref_PauseMenu = gameObject.GetComponent<PauseMenu>();
        ref_CurrentLevelStats = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>();
        ref_LevelStats = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>();
    }

    // Update is called once per frame
    void Update()
    {
        //Menu delay timer
        f_MenuDelay += Time.realtimeSinceStartup - f_CustomDeltaTime;
        f_CustomDeltaTime = Time.realtimeSinceStartup;
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            f_MenuDelay += .21f;
        }

        //Toggle shop menu visibility
        if (!ref_PauseMenu.b_IsShowingPauseMenu && !ref_CurrentLevelStats.b_BeginRound)
        {
            if (!ref_LevelStats.b_isTransitioningIn)
            {
                if (Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("ControllerShop"))
                {
                    if (!b_IsShowingShopMenu)
                    {
                        b_IsShowingShopMenu = true;
                        n_NavigationNumber = 1;
                        Time.timeScale = 0.0f;
                    }
                    else
                    {
                        b_IsShowingShopMenu = false;
                        n_NavigationNumber = 1;
                        Time.timeScale = 1.0f;
                    }
                }
            }
        }

        //If shop is showing, allow for user input
        if (b_IsShowingShopMenu)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetAxis("ControllerVertical") > .2f)
            {
                if (n_NavigationNumber > 1 && f_MenuDelay > 0.2f)
                {
                    Instantiate(go_MenuMove, transform.position, transform.rotation);
                    f_MenuDelay = 0.0f;
                    n_NavigationNumber -= 1;
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetAxis("ControllerVertical") < -.2f)
            {
                if (n_NavigationNumber < 4 && f_MenuDelay > 0.2f)
                {
                    Instantiate(go_MenuMove, transform.position, transform.rotation);
                    f_MenuDelay = 0.0f;
                    n_NavigationNumber += 1;
                }
            }
            if (Input.GetButtonUp("ControllerJump") || Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Return))
            {
                switch (n_NavigationNumber)
                {
                    case 1:
                        //Upgrade Health
                        if (ref_GameManager.n_PlayerCoins >= 5)
                        {
                            Instantiate(go_MenuSelect, transform.position, transform.rotation);
                            ref_GameManager.f_PlayerHealth += 150.0f;
                            ref_GameManager.n_PlayerCoins -= 5;
                        }
                        break;
                    case 2:
                        //Upgrade Damage
                        if (ref_GameManager.n_PlayerCoins >= 5)
                        {
                            Instantiate(go_MenuSelect, transform.position, transform.rotation);
                            ref_GameManager.f_PlayerDamage += 20.0f;
                            ref_GameManager.n_PlayerCoins -= 5;
                        }
                        break;
                    case 3:
                        //Upgrade Fire Rate
                        if (ref_GameManager.n_PlayerCoins >= 5 && (1f / ref_GameManager.f_ReloadTime) < 9.0f)
                        {
                            Instantiate(go_MenuSelect, transform.position, transform.rotation);
                            ref_GameManager.f_ReloadTime = (1f / ((1f / ref_GameManager.f_ReloadTime) + 1f));
                            ref_GameManager.n_PlayerCoins -= 5;
                        }
                        break;
                    case 4:
                        //Close Shop Menu
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        Time.timeScale = 1;
                        b_IsShowingShopMenu = false;
                        break;
                }
            }
        }
    }

    //Used to actually show the gui
    void OnGUI()
    {
        if (b_IsShowingShopMenu)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 3.5f, Screen.height - 20), "");
            GUI.Box(new Rect(10, 10, Screen.width / 3.5f, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 90 * (n_NavigationNumber + 1) + 5, Screen.width / 3.5f, 45), "");
            GUI.Box(new Rect(10, 90 * (n_NavigationNumber + 1) + 5, Screen.width / 3.5f, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 3.5f, Screen.height - 20), "Shop Menu", gs_TitleText);
            GUI.Label(new Rect(10, 95, Screen.width / 3.5f, Screen.height - 20), "Coins: " + ref_GameManager.n_PlayerCoins, gs_ButtonText);
            GUI.Label(new Rect(10, 160, Screen.width / 3.5f, Screen.height - 20), "Cost: 5 Coins", gs_DescriptionText);
            GUI.Label(new Rect(10, 250, Screen.width / 3.5f, Screen.height - 20), "Cost: 5 Coins", gs_DescriptionText);
            GUI.Label(new Rect(10, 340, Screen.width / 3.5f, Screen.height - 20), "Cost: 5 Coins", gs_DescriptionText);
            if (ref_GameManager.n_PlayerCoins >= 5)
            {
                GUI.Label(new Rect(10, 190, Screen.width / 3.5f, Screen.height - 20), "Upgrade Health (+150)", gs_AvailableText);
                GUI.Label(new Rect(10, 280, Screen.width / 3.5f, Screen.height - 20), "Upgrade Damage (+20)", gs_AvailableText);
                if ((1f / ref_GameManager.f_ReloadTime) < 9.0f)
                    GUI.Label(new Rect(10, 370, Screen.width / 3.5f, Screen.height - 20), "Upgrade Fire Rate (+1)", gs_AvailableText);
                else
                    GUI.Label(new Rect(10, 370, Screen.width / 3.5f, Screen.height - 20), "MAXED OUT", gs_NotAvailableText);
            }
            else
            {
                GUI.Label(new Rect(10, 190, Screen.width / 3.5f, Screen.height - 20), "Upgrade Health (+150)", gs_NotAvailableText);
                GUI.Label(new Rect(10, 280, Screen.width / 3.5f, Screen.height - 20), "Upgrade Damage (+20)", gs_NotAvailableText);
                if ((1f / ref_GameManager.f_ReloadTime) < 9.0f)
                    GUI.Label(new Rect(10, 370, Screen.width / 3.5f, Screen.height - 20), "Upgrade Fire Rate (+1)", gs_NotAvailableText);
                else
                    GUI.Label(new Rect(10, 370, Screen.width / 3.5f, Screen.height - 20), "MAXED OUT", gs_NotAvailableText);
            }

            GUI.Label(new Rect(10, 460, Screen.width / 3.5f, Screen.height - 20), "Close Shop Menu ", gs_ButtonText);

            //Boat stats
            GUI.Box(new Rect(Screen.width - ((Screen.width / 3.5f) + 10), 10, Screen.width / 3.5f, 250), "");
            GUI.Box(new Rect(Screen.width - ((Screen.width / 3.5f) + 10), 10, Screen.width / 3.5f, 250), "");
            GUI.Label(new Rect(Screen.width - ((Screen.width / 3.5f) + 10), 10, Screen.width / 3.5f, Screen.height - 20), "Current Boat Stats", gs_TitleText);

            GUI.Label(new Rect(Screen.width - ((Screen.width / 3.5f)), 100, Screen.width / 3.5f, Screen.height - 20), 
                "Boat Health: " + ref_GameManager.f_PlayerHealth, gs_DescriptionText2);
            GUI.Label(new Rect(Screen.width - ((Screen.width / 3.5f)), 140, Screen.width / 3.5f, Screen.height - 20),
                "Boat Damage: " + ref_GameManager.f_PlayerDamage, gs_DescriptionText2);
            GUI.Label(new Rect(Screen.width - ((Screen.width / 3.5f)), 180, Screen.width / 3.5f, Screen.height - 20),
                "Boat Fire Rate: " + ( 1 / ref_GameManager.f_ReloadTime) + " shots per sec.", gs_DescriptionText2);


        }
        else if (!ref_PauseMenu.b_IsShowingPauseMenu && !ref_CurrentLevelStats.b_isTransitioningIn && !ref_CurrentLevelStats.b_BeginRound
                 && ref_GameManager.n_PlayerCoins >= 5)
        {
            GUI.Box(new Rect(0, 85, Screen.width, 100), "");
            GUI.Label(new Rect(Screen.width / 4, 90, Screen.width / 2, 100), "Press 'P' (Keyboard) or Y button (Controller)", gs_TitleText);
            GUI.Label(new Rect(Screen.width / 4, 130, Screen.width / 2, 100), "to Upgrade Ship!", gs_TitleText);
        }
    }
}
