//Rotate the camera 

using UnityEngine;
using System.Collections;

public class CameraRotate : MonoBehaviour
{

    public float f_SensitivityY = 5.0f;
    public float f_MinimumY = -60F;
    public float f_MaximumY = 60F;
    float f_RotationY = 0F;
    float f_ControllerInput = 0.0f;

    private string s_InputMethod;
    public GameObject go_InputReference;

    void Start()
    {
        s_InputMethod = go_InputReference.name;
    }

    public void resetRotation()
    {
        f_RotationY = 0;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0f)
        {
            //If input is for keyboard
            if (s_InputMethod == "Keyboard")
            {
                float f_MouseInput = Input.GetAxis("Mouse Y");
                if (f_MouseInput != 0)
                {
                    f_RotationY += f_MouseInput * f_SensitivityY;
                    f_RotationY = Mathf.Clamp(f_RotationY, f_MinimumY, f_MaximumY);
                    transform.localEulerAngles = new Vector3(-f_RotationY, transform.localEulerAngles.y, 0);
                }
            }
            else
            {
                //Input for controller
                f_ControllerInput = Input.GetAxis(s_InputMethod + "RotateY");
                if (f_ControllerInput != 0)
                {
                    f_RotationY += -f_ControllerInput * f_SensitivityY;
                    f_RotationY = Mathf.Clamp(f_RotationY, f_MinimumY, f_MaximumY);
                    transform.localEulerAngles = new Vector3(-f_RotationY, transform.localEulerAngles.y, 0);
                }
            }
        }
    }
}
