//When player uses action button next close enough to this button, the gate in the scene will be opened.
//This script is also called when player enters boat for the first time and gate is not open.

using UnityEngine;
using System.Collections;

public class GateOpener : MonoBehaviour {

    public GameObject go_Gate;
    public float f_GateSpeed = 20.0f;
    public float f_GateHeight = 30.0f;
    private float f_GateInitialHeight;
    private float f_LocalGateInitialHeight;
    bool b_OpeningGate = false;
    public bool b_GateOpen = false;

	// Use this for initialization
	void Start () {
        f_GateInitialHeight = go_Gate.transform.position.y;
        f_LocalGateInitialHeight = go_Gate.transform.localPosition.y;
	}
	
	// Update is called once per frame
	void Update () {
        //If gate is called to open and is not currently open, begin opening the gate
        if (b_OpeningGate && b_GateOpen == false)
        {
            if (go_Gate.transform.position.y < f_GateInitialHeight + f_GateHeight)
            {
                go_Gate.transform.position += new Vector3(0, f_GateSpeed * Time.deltaTime, 0);
            }
            else
            {
                go_Gate.transform.position = new Vector3(go_Gate.transform.position.x, f_GateInitialHeight + f_GateHeight, go_Gate.transform.position.z);
                b_OpeningGate = false;
                b_GateOpen = true;
                transform.FindChild("OpenGateMessage").GetComponent<DisplayMessage>().s_GuiMessage = "Press 'F' (Keyboard) or X button (Controller) to Close the Gate!";
            }
        }
        //If gate is called to open and it is already opened, begin closing the gate
        else if (b_OpeningGate)
        {
            if (go_Gate.transform.localPosition.y > f_LocalGateInitialHeight)
            {
                go_Gate.transform.position -= new Vector3(0, f_GateSpeed * Time.deltaTime, 0);
            }
            else
            {
                go_Gate.transform.localPosition = new Vector3(go_Gate.transform.localPosition.x, f_LocalGateInitialHeight, go_Gate.transform.localPosition.z);
                b_OpeningGate = false;
                b_GateOpen = false;
                transform.FindChild("OpenGateMessage").GetComponent<DisplayMessage>().s_GuiMessage = "Press 'F' (Keyboard) or X button (Controller) to Open the Gate!";
            }
        }
	}

    public void OpenGate()
    {
        if (go_Gate != null)
        {
            b_OpeningGate = true;
        }
    }
}
