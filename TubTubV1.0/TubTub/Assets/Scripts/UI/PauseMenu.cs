//Controls everything dealing with the pause menu and pausing the game

using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

    private ShopMenu ref_ShopMenu;

    public bool b_IsShowingPauseMenu = false;
    private int n_NavigationNumber = 1;
    private float f_MenuDelay = 0.0f;
    public GUIStyle gs_TitleText;
    public GUIStyle gs_ButtonText;
    private float f_CustomDeltaTime = 0.0f;
    GameManager ref_GameManager;
    private CurrentLevelStats ref_LevelStats;

    public GameObject go_MenuSelect;
    public GameObject go_MenuMove;

	// Use this for initialization
	void Start () {
        ref_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Screen.showCursor = false;
        Screen.lockCursor = true;
        Time.timeScale = 1.0f;
        ref_ShopMenu = gameObject.GetComponent<ShopMenu>();
        ref_LevelStats = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>();
	}
	
	// Update is called once per frame
	void Update () {
        //Menu delay timer
        f_MenuDelay += Time.realtimeSinceStartup - f_CustomDeltaTime;
        f_CustomDeltaTime = Time.realtimeSinceStartup;
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            f_MenuDelay += .21f;
        }

        //Toggle pause menu visibility
        if (!ref_ShopMenu.b_IsShowingShopMenu && !GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>().b_RoundWon)
        {
            if (!ref_LevelStats.b_isTransitioningIn)
            {
                if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("ControllerPauseGame"))
                {
                    Debug.Log("PAUSE MENU");
                    if (!b_IsShowingPauseMenu)
                    {
                        n_NavigationNumber = 1;
                        b_IsShowingPauseMenu = true;
                        Screen.showCursor = true;
                        Screen.lockCursor = false;
                        Time.timeScale = 0.0f;
                    }
                    else
                    {
                        n_NavigationNumber = 1;
                        b_IsShowingPauseMenu = false;
                        Screen.showCursor = false;
                        Screen.lockCursor = true;
                        Time.timeScale = 1.0f;
                    }
                }
            }
        }

        //Allow for input if menu is currently showing
        if (b_IsShowingPauseMenu)
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
                if (n_NavigationNumber < 3 && f_MenuDelay > 0.2f)
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
                        //Resume Game Button
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        Screen.showCursor = false;
                        Screen.lockCursor = true;
                        Time.timeScale = 1.0f;
                        b_IsShowingPauseMenu = false;
                        break;
                    case 2:
                        //Restart Round
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        Screen.showCursor = false;
                        Screen.lockCursor = true;
                        Time.timeScale = 1.0f;
                        Application.LoadLevel(Application.loadedLevel);
                        break;
                    case 3:
                        //Exit To Main Menu
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        Screen.showCursor = false;
                        Screen.lockCursor = true;
                        n_NavigationNumber = 1;
                        Time.timeScale = 1.0f;

                        PlayerPrefs.SetFloat("ReloadTime", ref_GameManager.f_ReloadTime);
                        PlayerPrefs.SetFloat("Damage", ref_GameManager.f_PlayerDamage);
                        PlayerPrefs.SetFloat("Health", ref_GameManager.f_PlayerHealth);
                        PlayerPrefs.SetInt("CurrentRound", ref_GameManager.n_CurrentRound);
                        PlayerPrefs.SetInt("Coins", ref_GameManager.n_PlayerCoins);
                        PlayerPrefs.Save();
                        Application.LoadLevel("MainMenu");
                        break;
                }
            }
        }
	}

    //Used to actually display the GUI
    void OnGUI()
    {
        if (b_IsShowingPauseMenu)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "Pause Menu", gs_TitleText);
            GUI.Label(new Rect(10, 95, Screen.width / 4, Screen.height - 20), "Resume Game", gs_ButtonText);
            GUI.Label(new Rect(10, 140, Screen.width / 4, Screen.height - 20), "Restart Round", gs_ButtonText);
            GUI.Label(new Rect(10, 185, Screen.width / 4, Screen.height - 20), "Exit to Main Menu", gs_ButtonText);
        }
    }
}
