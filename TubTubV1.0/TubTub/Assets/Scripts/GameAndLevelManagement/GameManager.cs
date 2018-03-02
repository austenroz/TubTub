//This script will not be deleted when the level changes. All important stats that need to be kept between levels is stored in here.

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //Player Controls
    public bool b_IsSinglePlayer = true;

    public string s_Player1Controller = "Controller1";
    public string s_Player2Controller = "Controller2";

    public int n_PlayerNumber = 1;
    public bool b_ConfigureController = false;

    //Player Stats
    public float f_PlayerDamage = 50.0f;
    public float f_ReloadTime = 0.2f;
    public float f_PlayerHealth = 500.0f;
    public int n_PlayerCoins = 0;
    public int n_CurrentRound = 0;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadNextRound()
    {
        Time.timeScale = 1.0f;
        Screen.showCursor = true;
        Screen.lockCursor = false;
        Application.LoadLevel(Application.loadedLevel);
    }

    void Update()
    {
        ConfigureController();
    }

    void ConfigureController()
    {
        if (b_ConfigureController)
        {
            if (Input.GetButtonDown("Controller1Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller1";
                }
                else
                {
                    s_Player2Controller = "Controller1";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller2Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller2";
                }
                else
                {
                    s_Player2Controller = "Controller2";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller3Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller3";
                }
                else
                {
                    s_Player2Controller = "Controller3";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller4Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller4";
                }
                else
                {
                    s_Player2Controller = "Controller4";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller5Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller5";
                }
                else
                {
                    s_Player2Controller = "Controller5";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller6Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller6";
                }
                else
                {
                    s_Player2Controller = "Controller6";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller7Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller7";
                }
                else
                {
                    s_Player2Controller = "Controller7";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller8Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller8";
                }
                else
                {
                    s_Player2Controller = "Controller8";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller9Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller9";
                }
                else
                {
                    s_Player2Controller = "Controller9";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller10Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller10";
                }
                else
                {
                    s_Player2Controller = "Controller10";
                }
                b_ConfigureController = false;
            }
            if (Input.GetButtonDown("Controller11Start"))
            {
                if (n_PlayerNumber == 1)
                {
                    s_Player1Controller = "Controller11";
                }
                else
                {
                    s_Player2Controller = "Controller11";
                }
                b_ConfigureController = false;
            }
        }
    }
}
