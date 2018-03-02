//Used to rotate the actual character model.

using UnityEngine;
using System.Collections;

public class PlayerRotate : MonoBehaviour
{

    float f_RotationSpeed = 200.0f;
    float f_ControllerInput = 0.0f;
	float f_Controller2Input = 0.0f;
    private string s_InputMethod;
    public GameObject go_InputReference;

    void Start()
    {
        s_InputMethod = go_InputReference.name;
    }

    // Update is called once per frame
    void Update()
    {
        //Keyboard input
        if (s_InputMethod == "Keyboard")
        {
            float f_MouseInput = Input.GetAxis("Mouse X");
            if (f_MouseInput != 0)
            {
                transform.localEulerAngles += new Vector3(0, f_MouseInput * Time.deltaTime * f_RotationSpeed, 0);
            }
        }
        else
        {
            //Controller input
            f_ControllerInput = Input.GetAxis(s_InputMethod + "RotateX");
            if (f_ControllerInput != 0)
            {
                transform.localEulerAngles += new Vector3(0, f_ControllerInput * Time.deltaTime * f_RotationSpeed, 0);
            }
        }
    }
}
