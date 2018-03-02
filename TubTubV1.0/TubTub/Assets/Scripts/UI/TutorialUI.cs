//Display the Tutorial UI

using UnityEngine;
using System.Collections;

public class TutorialUI : MonoBehaviour {

    public GUIStyle gs_TitleText;
    public GUIStyle gs_ContentText;
    private int n_TutorialPage = 2;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        //Use input to change page number
        if (Input.GetButtonUp("ControllerJump") || Input.GetButtonUp("Jump"))
        {
            if (n_TutorialPage < 5)
            {
                n_TutorialPage++;
            }
            else
            {
                n_TutorialPage = 1;
            }
        }
        if (Input.GetKeyUp(KeyCode.Return) || Input.GetButtonDown("ControllerPauseGame"))
        {
            Application.LoadLevel("Bathtub");
        }
	}

    //Used to display tutorial ui based on page number
    void OnGUI()
    {
        GUI.Box(new Rect(20f, 20f, Screen.width - 40f, Screen.height - 40f), "");
        GUI.Box(new Rect(20f, 20f, Screen.width - 40f, Screen.height - 40f), "");
        GUI.Box(new Rect(20f, Screen.height - 95f, Screen.width - 40f, 75f), "");
        GUI.Label(new Rect(0, Screen.height - 55f, Screen.width, Screen.height), 
                  "Press Enter (Keyboard) or Start (Controller) to continue to the game", gs_ContentText);
        if (n_TutorialPage == 1)
        {
            GUI.Label(new Rect(0, 20f, Screen.width, Screen.height),
                      "Objective", gs_TitleText);
            GUI.Label(new Rect(0, 100f, Screen.width, Screen.height),
                      "The objective of Tub Tub is to destroy the enemy boats before they destroy you.\n\nEvery time you beat a round you will advance to the next round.\n\nRounds will get harder and harder so it is important to continually upgrade your\n ship with the coins awarded for beating each round.", gs_ContentText);
            GUI.Label(new Rect(0, Screen.height - 95f, Screen.width, Screen.height),
                      "Press Space (Keyboard) or A (Controller) to continue to next tutorial slide", gs_ContentText);
        }
        if (n_TutorialPage == 2)
        {
            GUI.Label(new Rect(0, 20f, Screen.width, Screen.height),
                      "Controls (Keyboard)", gs_TitleText);
            GUI.Label(new Rect(0, 100f, Screen.width, Screen.height),
                      "WASD - Move Player/Boat\nMouse - Move Camera\nSpacebar - Jump\nRight Mouse Button - Aim Cannon\nLeft Mouse Button - Fire Cannon\nF - Action Button\nP - Open Shop\nEsc - Pause Game", gs_ContentText);
            GUI.Label(new Rect(0, Screen.height - 95f, Screen.width, Screen.height),
                      "Press Space (Keyboard) or A (Controller) to continue to next tutorial slide", gs_ContentText);
        }
        if (n_TutorialPage == 3)
        {
            GUI.Label(new Rect(0, 20f, Screen.width, Screen.height),
                      "Controls (Controller)", gs_TitleText);
            GUI.Label(new Rect(0, 100f, Screen.width, Screen.height),
                      "Left Joystick - Move Player/Boat\nRight Joystick - Move Camera\nA - Jump\nLeft Trigger - Aim Cannon\nRight Trigger - Fire Cannon\nX - Action Button\nY - Open Shop\nStart - Pause Game", gs_ContentText);
            GUI.Label(new Rect(0, Screen.height - 95f, Screen.width, Screen.height),
                      "Press Space (Keyboard) or A (Controller) to continue to next tutorial slide", gs_ContentText);
        }
        if (n_TutorialPage == 4)
        {
            GUI.Label(new Rect(0, 20f, Screen.width, Screen.height),
                      "Saving Progress", gs_TitleText);
            GUI.Label(new Rect(0, 100f, Screen.width, Screen.height),
                      "Your progress will automatically save if you decide to leave the game through the pause menu.\nThis progress will be saved even after you close out of the game application.\n\nIf you want to start the game over, go to options on the main menu\nand click Reset Scores.", gs_ContentText);
            GUI.Label(new Rect(0, Screen.height - 95f, Screen.width, Screen.height),
                      "Press Space (Keyboard) or A (Controller) to continue to next tutorial slide", gs_ContentText);
        }
        if (n_TutorialPage == 5)
        {
            GUI.Label(new Rect(0, 20f, Screen.width, Screen.height),
                      "Good Luck", gs_TitleText);
            GUI.Label(new Rect(0, 100f, Screen.width, Screen.height),
                      "I hope you will enjoy your playthough of Tub Tub.\nGood luck advancing your way through the rounds.\n\n\nPress Enter (Keyboard) or Start (Controller) to continue to the game!", gs_ContentText);
            GUI.Label(new Rect(0, Screen.height - 95f, Screen.width, Screen.height),
                      "Press Space (Keyboard) or A (Controller) to go back to first tutorial slide", gs_ContentText);
        }
    }
}
