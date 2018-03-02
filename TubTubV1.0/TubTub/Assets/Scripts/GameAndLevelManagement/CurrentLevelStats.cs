//This script manages all the current level stats, such as enemies remaining to kill. This is also responsible for displaying general guis such as the 
//Round number and player winning. 

using UnityEngine;
using System.Collections;

public class CurrentLevelStats : MonoBehaviour {

    public int n_TotalEnemies = 0;
    public int n_EnemiesKilled = 0;
    public bool b_BeginRound = false;
    float f_Timer = 0.0f;
    float f_FinishTime = 0.0f;
    public int n_CurrentRound = 1;

    private GameManager ref_GameManager;
    public string s_StartMessage;

    public GUIStyle gs_TitleGUIStyle;
    public GUIStyle gs_TimerStyle;
    public GUIStyle gs_DetailsStyle;
    public GUIStyle gs_RoundGUIStyle;
    public bool b_ShowTimer = false;
    public GameObject go_InitialGateOpener;

    private bool b_AlreadyClosedGate = false;
    public bool b_RoundWon = false;
    private bool b_AlreadyAssignedStats = false;
    private bool b_LevelTransition = false;
    private float f_CameraBorderSize = 0.0f;
    private float f_CustomDeltaTime = 0.0f;
    private float f_CustomTimePrevious = 0.0f;
    public bool b_isTransitioningIn = true;
    private float f_CoopCameraPosition = 0.0f;
    public bool b_isOnMainMenu = false;

    private float f_TimerForEnemyArray = 0.0f;
    GameObject[] go_EnemiesKilled;

    //Set variables
    void Start()
    {
        ref_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        n_CurrentRound = ref_GameManager.n_CurrentRound;
        s_StartMessage = "Round " + n_CurrentRound;
    }

    //Used for displaying GUIs
    void OnGUI()
    {
        if (!b_BeginRound && !b_isTransitioningIn)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 30, 300, 30), s_StartMessage, gs_RoundGUIStyle);

            /*
            //Background Box (Position x, Position y, Size x, Size y)
            GUI.Box(new Rect(Screen.width - 210, 10, 200, 270), "Upgrade Ship");
            GUI.Label(new Rect(Screen.width - 140, 35, 200, 30), "Coins:  " + ref_GameManager.n_PlayerCoins);
            GUI.Box(new Rect(Screen.width - 210, 280, 200, 150), "Current Boat Stats");
            GUI.Label(new Rect(Screen.width - 200, 310, 200, 30), "Boat Health:  " + ref_GameManager.f_PlayerHealth);
            GUI.Label(new Rect(Screen.width - 200, 330, 200, 30), "Boat Damage:  " + ref_GameManager.f_PlayerDamage);
            if (ref_GameManager.f_ReloadTime < .04f)
                GUI.Label(new Rect(Screen.width - 200, 350, 200, 45), "Boat Fire Rate:  As fast as you can pull trigger");
            else if (ref_GameManager.f_ReloadTime < .06f)
                GUI.Label(new Rect(Screen.width - 200, 350, 200, 45), "Boat Fire Rate:  20 shots per second");
            else
                GUI.Label(new Rect(Screen.width - 200, 350, 200, 45), "Boat Fire Rate:  " + (1.0f / ref_GameManager.f_ReloadTime) + " shots per second");
            GUI.Box(new Rect(Screen.width - 210, 60, 200, 70), "Increase Health (+250)");
            GUI.Box(new Rect(Screen.width - 210, 130, 200, 70), "Increase Damage (+25)");
            GUI.Box(new Rect(Screen.width - 210, 200, 200, 70), "Increase Fire Rate");

            // Buttons
            if (GUI.Button(new Rect(Screen.width - 190, 85, 160, 30), "Purchase (5 Coins)"))
            {
                if (ref_GameManager.n_PlayerCoins >= 5)
                {
                    ref_GameManager.f_PlayerHealth += 250.0f;
                    ref_GameManager.n_PlayerCoins -= 5;
                }
            }

            if (GUI.Button(new Rect(Screen.width - 190, 155, 160, 30), "Purchase (5 Coins)"))
            {
                if (ref_GameManager.n_PlayerCoins >= 5)
                {
                    ref_GameManager.f_PlayerDamage += 25.0f;
                    ref_GameManager.n_PlayerCoins -= 5;
                }
            }
            if (ref_GameManager.f_ReloadTime > .05f)
            {
                if (GUI.Button(new Rect(Screen.width - 190, 225, 160, 30), "Purchase (5 Coins)"))
                {
                    if (ref_GameManager.n_PlayerCoins >= 5)
                    {
                        ref_GameManager.f_ReloadTime -= 0.05f;
                        ref_GameManager.n_PlayerCoins -= 5;
                    }
                }
            }*/
        }
        
        else if (f_FinishTime == 0.0f && b_ShowTimer && !b_isTransitioningIn)
        {
            GUI.Label(new Rect(Screen.width - 250, Screen.height - 30, 300, 30), "Time: " + f_Timer, gs_TimerStyle);
            //GUI.Label(new Rect(0, 0, 300, 30), "Time: " + f_Timer);
        }

        if (b_RoundWon)
        {
            //GUI FOR ROUND WON
            GUI.Box(new Rect(0, 20, Screen.width, 220), "");
            GUI.Box(new Rect(0, 20, Screen.width, 220), "");
            GUI.Box(new Rect(0, 20, Screen.width, 220), "");
            GUI.Box(new Rect(0, 190, Screen.width, 50), "");
            GUI.Label(new Rect(Screen.width / 2 - 150, 30, 300, 30), "Round Wonned!", gs_TitleGUIStyle);
            GUI.Label(new Rect(Screen.width / 2 - 150, 80, 300, 30), "Round Completed in " + f_FinishTime + " seconds.", gs_DetailsStyle);
            GUI.Label(new Rect(Screen.width / 2 - 150, 130, 300, 30), "You have earned " + (1 + (n_CurrentRound / 2)) + " coins.", gs_DetailsStyle);
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 300, 30), "Press 'Spacebar' (Keyboard) or A (Controller) to continue to next round.", gs_DetailsStyle);
        }
    }

	// Update is called once per frame
	void Update () {
        f_CustomDeltaTime = Time.realtimeSinceStartup - f_CustomTimePrevious;
        f_CustomTimePrevious = Time.realtimeSinceStartup;

        f_TimerForEnemyArray += Time.deltaTime;

        if (f_TimerForEnemyArray > 1f)
        {
            go_EnemiesKilled = new GameObject[n_TotalEnemies];
        }
        //Transition into level
        if (b_isTransitioningIn)
        {
            if (f_CameraBorderSize < 1.0f)
            {
                f_CameraBorderSize += 1f * Time.deltaTime;
                GameObject[] go_AllPlayers = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < go_AllPlayers.Length; i++)
                {
                    Camera cam_MainCamera = go_AllPlayers[i].transform.Find("Main Camera").GetComponent<Camera>();
                    Camera cam_BoatCamera = go_AllPlayers[i].transform.Find("Boat Camera").GetComponent<Camera>();
                    cam_MainCamera.rect = new Rect((1.0f - f_CameraBorderSize) / 2.0f, (1.0f - f_CameraBorderSize) / 2.0f, f_CameraBorderSize, f_CameraBorderSize);
                    cam_BoatCamera.rect = new Rect((1.0f - f_CameraBorderSize) / 2.0f, (1.0f - f_CameraBorderSize) / 2.0f, f_CameraBorderSize, f_CameraBorderSize);
                }
            }
            else
            {
                f_CameraBorderSize = 1.0f;
                GameObject[] go_AllPlayers = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < go_AllPlayers.Length; i++)
                {
                    Camera cam_MainCamera = go_AllPlayers[i].transform.Find("Main Camera").GetComponent<Camera>();
                    Camera cam_BoatCamera = go_AllPlayers[i].transform.Find("Boat Camera").GetComponent<Camera>();
                    cam_MainCamera.rect = new Rect(0f, 0f, 1f, 1f);
                    cam_BoatCamera.rect = new Rect(0f, 0f, 1f, 1f);
                }
                //Change positions of camera to screen space for when game is coop
                if (!ref_GameManager.b_IsSinglePlayer)
                {
                    if (f_CoopCameraPosition < .5)
                    {
                        f_CoopCameraPosition += 1f * Time.deltaTime;
                        for (int i = 0; i < go_AllPlayers.Length; i++)
                        {
                            Camera cam_MainCamera = go_AllPlayers[i].transform.Find("Main Camera").GetComponent<Camera>();
                            Camera cam_BoatCamera = go_AllPlayers[i].transform.Find("Boat Camera").GetComponent<Camera>();
                            if (go_AllPlayers[i].GetComponent<PauseMenu>() != null)
                            {
                                //Player 1
                                cam_MainCamera.rect = new Rect(0f, f_CoopCameraPosition, 1f, (1f - f_CoopCameraPosition)); //0, .5, 1, .5
                                cam_BoatCamera.rect = new Rect(0f, f_CoopCameraPosition, 1f, (1f - f_CoopCameraPosition));
                            }
                            else
                            {
                                //Player 2
                                cam_MainCamera.rect = new Rect(0f, 0f, 1f, (1f - f_CoopCameraPosition)); //0, 0, 1, .5
                                cam_BoatCamera.rect = new Rect(0f, 0f, 1f, (1f - f_CoopCameraPosition));
                            }
                        }
                    }
                    else
                    {
                        f_CoopCameraPosition = .5f;
                        for (int i = 0; i < go_AllPlayers.Length; i++)
                        {
                            Camera cam_MainCamera = go_AllPlayers[i].transform.Find("Main Camera").GetComponent<Camera>();
                            Camera cam_BoatCamera = go_AllPlayers[i].transform.Find("Boat Camera").GetComponent<Camera>();
                            if (go_AllPlayers[i].GetComponent<PauseMenu>() != null)
                            {
                                //Player 1
                                cam_MainCamera.rect = new Rect(0f, f_CoopCameraPosition, 1f, (1f - f_CoopCameraPosition)); //0, .5, 1, .5
                                cam_BoatCamera.rect = new Rect(0f, f_CoopCameraPosition, 1f, (1f - f_CoopCameraPosition));
                            }
                            else
                            {
                                //Player 2
                                cam_MainCamera.rect = new Rect(0f, 0f, 1f, (1f - f_CoopCameraPosition)); //0, 0, 1, .5
                                cam_BoatCamera.rect = new Rect(0f, 0f, 1f, (1f - f_CoopCameraPosition));
                            }
                        }
                        b_isTransitioningIn = false;
                    }
                }
                else
                {
                    b_isTransitioningIn = false;
                }
            }
        }
        //Call necessary methods and set variables correctly for when the round begins.
        if (b_BeginRound)
        {
            if (!go_InitialGateOpener.GetComponent<GateOpener>().b_GateOpen && !b_AlreadyClosedGate)
            {
                go_InitialGateOpener.GetComponent<GateOpener>().OpenGate();
                b_AlreadyClosedGate = true;
            }
            f_Timer += Time.deltaTime;
            if (!b_AlreadyAssignedStats)
            {
                b_AlreadyAssignedStats = true;
                assignPlayerStats();
            }
        }

        //Call necessary methods and set variables correctly for when the player wins the round
        if (b_RoundWon)
        {
            if (Input.GetButtonDown("Jump") 
                || Input.GetButtonDown("ControllerJump"))
            {
                b_RoundWon = false;
                b_LevelTransition = true;
                GameObject[] go_AllPlayers = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < go_AllPlayers.Length; i++)
                {
                    go_AllPlayers[i].GetComponent<BoatMovement>().setHealthGUIVisiblility(false);
                }
                //Player Continues to next round
                //ref_GameManager.n_CurrentRound = n_CurrentRound + 1;
                //ref_GameManager.LoadNextRound();
            }
        }

        //set camera rect to look like camera is zooming in
        if (b_LevelTransition)
        {
            if (f_CameraBorderSize > 0.0f)
            {
                f_CameraBorderSize -= 5f * f_CustomDeltaTime;
                GameObject[] go_AllPlayers = GameObject.FindGameObjectsWithTag("Player");
                for (int i = 0; i < go_AllPlayers.Length; i++)
                {
                    Camera cam_MainCamera = go_AllPlayers[i].transform.Find("Main Camera").GetComponent<Camera>();
                    Camera cam_BoatCamera = go_AllPlayers[i].transform.Find("Boat Camera").GetComponent<Camera>();
                    cam_MainCamera.rect = new Rect((1.0f - f_CameraBorderSize) / 2.0f, (1.0f - f_CameraBorderSize) / 2.0f, f_CameraBorderSize, f_CameraBorderSize);
                    cam_BoatCamera.rect = new Rect((1.0f - f_CameraBorderSize) / 2.0f, (1.0f - f_CameraBorderSize) / 2.0f, f_CameraBorderSize, f_CameraBorderSize);
                }
            }
            else
            {
                //Player Continues to next round
                ref_GameManager.n_CurrentRound = n_CurrentRound + 1;
                ref_GameManager.LoadNextRound();
            }
        }
	}

    //method to assign the correct stats to the boats currently in the level
    public void assignPlayerStats()
    {
        if (!b_isOnMainMenu)
        {
            GameObject[] go_AllPlayers = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < go_AllPlayers.Length; i++)
            {
                go_AllPlayers[i].GetComponent<BoatMovement>().setDamage(ref_GameManager.f_PlayerDamage);
                go_AllPlayers[i].GetComponent<BoatMovement>().setReloadTime(ref_GameManager.f_ReloadTime);
            }
            GameObject[] go_PlayerBoats = GameObject.FindGameObjectsWithTag("Boat");
            for (int i = 0; i < go_PlayerBoats.Length; i++)
            {
                go_PlayerBoats[i].GetComponent<BoatHealth>().SetBoatHealth(ref_GameManager.f_PlayerHealth);
            }
        }
    }

    //Method called when an enemy is killed in the scene
    public void KilledEnemy(GameObject go_KilledEnemy)
    {
        bool b_AddEnemy = true;
        for (int i = 0; i < go_EnemiesKilled.Length; i++ )
        {
            if (go_KilledEnemy == go_EnemiesKilled[i])
                b_AddEnemy = false;
            if (b_AddEnemy == true && go_EnemiesKilled[i] == null)
                go_EnemiesKilled[i] = go_KilledEnemy;
        }
        if (b_AddEnemy)
        {
            n_EnemiesKilled++;
            if ((n_EnemiesKilled >= n_TotalEnemies))
            {
                //Player Wins Round
                Debug.Log("PLAYER WINS ROUND " + n_CurrentRound);
                f_FinishTime = f_Timer;
                if ((1 + (n_CurrentRound / 2)) < 8)
                    ref_GameManager.n_PlayerCoins += (1 + (n_CurrentRound / 2));
                else
                    ref_GameManager.n_PlayerCoins += 8;
                Time.timeScale = 0.0f;
                b_RoundWon = true;
            }
        }
    }

    //method used to calculate the total number of enemies in the scene
    public void AddOneTotalEnemy()
    {
        n_TotalEnemies++;
    }
}
