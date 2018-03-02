//The main menu UI. Deals with everything on the main menu.

using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    //VARIABLES
    public Texture txt_TitlePic;
    public GUIStyle gs_TitleText;
    public GUIStyle gs_ButtonText;
    public GUIStyle gs_TitleStyle;
    private GameManager ref_GameManager;

    private int n_NavigationNumber = 1;
    private float f_MenuDelay = 0.0f;

    private float f_TitleBounce = 1.0f;
    private bool b_InvertTitleBounce = false;
    private bool b_IsPlayingMusic = true;
    private string s_OptionPlayingMusicOutput = "Mute Music";

    public GameObject go_MenuSelect;
    public GameObject go_MenuMove;

    public enum CurrentMenu
    {
        MainMenu, Options, Coop, ControlsP1, ControlsP2, ConfigureControllerP1, ConfigureControllerP2, LevelSelect
    }
    public CurrentMenu enm_CurrentMenu = CurrentMenu.MainMenu;

    //Set Variables
    void Start()
    {
        ref_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        //Bounce title gui
        if (f_TitleBounce > 0.8f)
        {
            b_InvertTitleBounce = true;
            f_TitleBounce = 0.8f;
        }
        else if (f_TitleBounce < 0.6f)
        {
            b_InvertTitleBounce = false;
            f_TitleBounce = 0.6f;
        }

        if (b_InvertTitleBounce)
            f_TitleBounce -= .5f * Time.deltaTime;
        else
            f_TitleBounce += .5f * Time.deltaTime;

        //Menu delay timer
        f_MenuDelay += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            f_MenuDelay += .21f;
        }
        //See when player uses start button on controller and continue to next menu afterwards
        if (Input.GetButtonDown("ControllerPauseGame") && enm_CurrentMenu == CurrentMenu.ConfigureControllerP1)
        {
            if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().b_IsSinglePlayer)
                enm_CurrentMenu = CurrentMenu.LevelSelect;
            else
                enm_CurrentMenu = CurrentMenu.ControlsP2;
        }
        if (Input.GetButtonDown("ControllerPauseGame") && enm_CurrentMenu == CurrentMenu.ConfigureControllerP2)
        {
            enm_CurrentMenu = CurrentMenu.LevelSelect;
        }

        //Switches for Menu Navigation
        //Each of these includes an input for moving selection up, down, and selecting. Switch is used to call the correct instructions when
        //a selection is made by user.
        //MAIN MENU
        if (enm_CurrentMenu == CurrentMenu.MainMenu)
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
                        //Start Game Button
                        n_NavigationNumber = 1;
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        enm_CurrentMenu = CurrentMenu.Coop;
                        break;
                    case 2:
                        //Options Button
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        enm_CurrentMenu = CurrentMenu.Options;
                        break;
                    case 3:
                        //Exit Game Button
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        Application.Quit();
                        break;
                }
            }
        }

        //COOP MENU
        else if (enm_CurrentMenu == CurrentMenu.Coop)
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
                        //Single Player
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        ref_GameManager.b_IsSinglePlayer = true;
                        enm_CurrentMenu = CurrentMenu.ControlsP1;
                        break;
                    case 2:
                        //Coop
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        ref_GameManager.b_IsSinglePlayer = false;
                        enm_CurrentMenu = CurrentMenu.ControlsP1;
                        break;
                    case 3:
                        //Return to main menu
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        enm_CurrentMenu = CurrentMenu.MainMenu;
                        break;
                }
            }
        }

        //Player 1 Controls MENU
        else if (enm_CurrentMenu == CurrentMenu.ControlsP1)
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
                        //Keyboard
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        ref_GameManager.s_Player1Controller = "Keyboard";
                        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().b_IsSinglePlayer)
                            enm_CurrentMenu = CurrentMenu.LevelSelect;
                        else
                            enm_CurrentMenu = CurrentMenu.ControlsP2;
                        break;
                    case 2:
                        //Controller
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        ref_GameManager.n_PlayerNumber = 1;
                        ref_GameManager.b_ConfigureController = true;
                        enm_CurrentMenu = CurrentMenu.ConfigureControllerP1;
                        break;
                    case 3:
                        //Return to main menu
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        enm_CurrentMenu = CurrentMenu.MainMenu;
                        break;
                }
            }
        }

        //Player 2 Controls MENU
        else if (enm_CurrentMenu == CurrentMenu.ControlsP2)
        {
            if (ref_GameManager.s_Player1Controller == "Keyboard" && n_NavigationNumber == 1)
            {
                n_NavigationNumber = 2;
            }
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetAxis("ControllerVertical") > .2f)
            {
                if (n_NavigationNumber > 1 && f_MenuDelay > 0.2f)
                {
                    if (ref_GameManager.s_Player1Controller != "Keyboard" || n_NavigationNumber > 2)
                    {
                        Instantiate(go_MenuMove, transform.position, transform.rotation);
                        f_MenuDelay = 0.0f;
                        n_NavigationNumber -= 1;
                    }
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
                        //Keyboard
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        ref_GameManager.s_Player2Controller = "Keyboard";
                        enm_CurrentMenu = CurrentMenu.LevelSelect;
                        break;
                    case 2:
                        //Controller
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        ref_GameManager.n_PlayerNumber = 2;
                        ref_GameManager.b_ConfigureController = true;
                        enm_CurrentMenu = CurrentMenu.ConfigureControllerP2;
                        break;
                    case 3:
                        //Return to main menu
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        enm_CurrentMenu = CurrentMenu.MainMenu;
                        break;
                }
            }
        }

        //Options
        else if (enm_CurrentMenu == CurrentMenu.Options)
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
                        //Pause or play music
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        if (b_IsPlayingMusic)
                        {
                            b_IsPlayingMusic = false;
                            s_OptionPlayingMusicOutput = "Unmute Music";
                            ref_GameManager.GetComponent<AudioSource>().Stop();
                        }
                        else
                        {
                            b_IsPlayingMusic = true;
                            s_OptionPlayingMusicOutput = "Mute Music";
                            ref_GameManager.GetComponent<AudioSource>().Play();
                        }
                        n_NavigationNumber = 1;

                        break;
                    case 2:
                        //Reset Scores
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        ref_GameManager.n_CurrentRound = 1;
                        ref_GameManager.n_PlayerCoins = 0;
                        ref_GameManager.f_PlayerDamage = 50f;
                        ref_GameManager.f_ReloadTime = 0.5f;
                        ref_GameManager.f_PlayerHealth = 500.0f;

                        PlayerPrefs.SetFloat("ReloadTime", ref_GameManager.f_ReloadTime);
                        PlayerPrefs.SetFloat("Damage", ref_GameManager.f_PlayerDamage);
                        PlayerPrefs.SetFloat("Health", ref_GameManager.f_PlayerHealth);
                        PlayerPrefs.SetInt("CurrentRound", ref_GameManager.n_CurrentRound);
                        PlayerPrefs.SetInt("Coins", ref_GameManager.n_PlayerCoins);
                        PlayerPrefs.Save();
                        break;
                    case 3:
                        //Return to main menu
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        enm_CurrentMenu = CurrentMenu.MainMenu;
                        break;
                }
            }
        }

        //Level Select menu
        else if (enm_CurrentMenu == CurrentMenu.LevelSelect)
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
                        //Tutorial
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        Application.LoadLevel("Tutorial");
                        break;
                    case 2:
                        //Play Game/Resume Game
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        Application.LoadLevel("Bathtub");
                        break;
                    case 3:
                        //Return to main menu
                        Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        enm_CurrentMenu = CurrentMenu.MainMenu;
                        break;
                }
            }
        }

        //Configure Joystick number menu for Player 1 and 2
        else if (enm_CurrentMenu == CurrentMenu.ConfigureControllerP1 || enm_CurrentMenu == CurrentMenu.ConfigureControllerP2)
        {
            if (Input.GetButtonUp("ControllerJump") || Input.GetButtonUp("Jump") || Input.GetKeyUp(KeyCode.Return))
            {
                                        //Return to main menu
                Instantiate(go_MenuSelect, transform.position, transform.rotation);
                        n_NavigationNumber = 1;
                        enm_CurrentMenu = CurrentMenu.MainMenu;
            }
        }


    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.yellow;
        //Version Number
        GUI.Box(new Rect(Screen.width - 210, Screen.height - 50, 200, 50), "Made by Austen Rozanski \nLast Updated: 4/29/2015 \nVersion 1.00");
        GUI.Box(new Rect(Screen.width - 210, Screen.height - 50, 200, 50), "Made by Austen Rozanski \nLast Updated: 4/29/2015 \nVersion 1.00");
        GUI.Box(new Rect(Screen.width - 210, Screen.height - 50, 200, 50), "Made by Austen Rozanski \nLast Updated: 4/29/2015 \nVersion 1.00");

        //Title
        GUI.Box(new Rect((Screen.width / 2f) + (250.0f * -f_TitleBounce), 10, Screen.width / 1.5f * (f_TitleBounce), Screen.height / 5.5f * (f_TitleBounce)), txt_TitlePic, gs_TitleStyle);

        //Load Main Menu
        if (enm_CurrentMenu == CurrentMenu.MainMenu)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "Main Menu", gs_TitleText);
            GUI.Label(new Rect(10, 95, Screen.width / 4, Screen.height - 20), "Start Game", gs_ButtonText);
            GUI.Label(new Rect(10, 140, Screen.width / 4, Screen.height - 20), "Options", gs_ButtonText);
            GUI.Label(new Rect(10, 185, Screen.width / 4, Screen.height - 20), "Exit Game", gs_ButtonText);
        }

        //Load Single or Co-op Menu
        if (enm_CurrentMenu == CurrentMenu.Coop)
        {

            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "1 or 2 Players?", gs_TitleText);
            GUI.Label(new Rect(10, 95, Screen.width / 4, Screen.height - 20), "One Player", gs_ButtonText);
            GUI.Label(new Rect(10, 140, Screen.width / 4, Screen.height - 20), "Two Players", gs_ButtonText);
            GUI.Label(new Rect(10, 185, Screen.width / 4, Screen.height - 20), "Return to Main Menu", gs_ButtonText);
        }

        //Load Controls Menu for player 1
        if (enm_CurrentMenu == CurrentMenu.ControlsP1)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "Player 1 Controls", gs_TitleText);
            GUI.Label(new Rect(10, 95, Screen.width / 4, Screen.height - 20), "Keyboard & Mouse", gs_ButtonText);
            GUI.Label(new Rect(10, 140, Screen.width / 4, Screen.height - 20), "Controller", gs_ButtonText);
            GUI.Label(new Rect(10, 185, Screen.width / 4, Screen.height - 20), "Return to Main Menu", gs_ButtonText);
        }

        //Load configure joystick menu for player 1
        if (enm_CurrentMenu == CurrentMenu.ConfigureControllerP1)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 2) + 40, Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 2) + 40, Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "Player 1:", gs_TitleText);
            GUI.Label(new Rect(10, 80, Screen.width / 4, Screen.height - 20), "Push Start Button", gs_TitleText);
            GUI.Label(new Rect(10, 180, Screen.width / 4, Screen.height - 20), "Return to Main Menu", gs_ButtonText);
        }

        //Load Controls Menu for player 2
        if (enm_CurrentMenu == CurrentMenu.ControlsP2)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "Player 2 Controls", gs_TitleText);
            if (ref_GameManager.s_Player1Controller != "Keyboard")
            {
                GUI.Label(new Rect(10, 95, Screen.width / 4, Screen.height - 20), "Keyboard & Mouse", gs_ButtonText);
            }
            GUI.Label(new Rect(10, 140, Screen.width / 4, Screen.height - 20), "Controller", gs_ButtonText);
            GUI.Label(new Rect(10, 185, Screen.width / 4, Screen.height - 20), "Return to Main Menu", gs_ButtonText);
        }

        //Configure joystick for player 2
        if (enm_CurrentMenu == CurrentMenu.ConfigureControllerP2)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 2) + 40, Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 2) + 40, Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "Player 2:", gs_TitleText);
            GUI.Label(new Rect(10, 80, Screen.width / 4, Screen.height - 20), "Push Start Button", gs_TitleText);
            GUI.Label(new Rect(10, 180, Screen.width / 4, Screen.height - 20), "Return to Main Menu", gs_ButtonText);
        }

        //Load Options Menu
        if (enm_CurrentMenu == CurrentMenu.Options)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "Options", gs_TitleText);
            GUI.Label(new Rect(10, 95, Screen.width / 4, Screen.height - 20), s_OptionPlayingMusicOutput, gs_ButtonText);
            GUI.Label(new Rect(10, 140, Screen.width / 4, Screen.height - 20), "Reset Scores", gs_ButtonText);
            GUI.Label(new Rect(10, 185, Screen.width / 4, Screen.height - 20), "Return to Main Menu", gs_ButtonText);
        }

        //Level Select Menu
        if (enm_CurrentMenu == CurrentMenu.LevelSelect)
        {
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "");

            //Selector
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");
            GUI.Box(new Rect(10, 45 * (n_NavigationNumber + 1), Screen.width / 4, 45), "");

            GUI.Label(new Rect(10, 10, Screen.width / 4, Screen.height - 20), "Level Select", gs_TitleText);
            GUI.Label(new Rect(10, 95, Screen.width / 4, Screen.height - 20), "Tutorial", gs_ButtonText);
            if (ref_GameManager.n_CurrentRound > 1)
            {
                GUI.Label(new Rect(10, 140, Screen.width / 4, Screen.height - 20), "Resume Game", gs_ButtonText);
            }
            else
            {
                GUI.Label(new Rect(10, 140, Screen.width / 4, Screen.height - 20), "New Game", gs_ButtonText);
            }
            GUI.Label(new Rect(10, 185, Screen.width / 4, Screen.height - 20), "Return to Main Menu", gs_ButtonText);
        }
    }
}