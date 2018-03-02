//Moves the player camera along a parabola according to user input

using UnityEngine;
using System.Collections;

public class CameraPosition : MonoBehaviour
{
    //Camera Position
    private float f_xOffset;
    private float f_yOffset;
    private float f_zOffset;
    public float f_SensitivityY = 2.0f;
    private float f_RotationY = 0.0f;
    private float f_LimitYRot = 0.0f;
    public float f_MinYRot = -40.0f;
    public float f_MaxYRot = 40.0f;
    public float f_SizeOfParabola = 5f;
    float f_ControllerInput = 0.0f;

    private string s_InputMethod;
    public GameObject go_InputReference;

    // Use this for initialization
    void Start()
    {
        f_xOffset = transform.localPosition.x;
        f_yOffset = transform.localPosition.y;
        f_zOffset = transform.localPosition.z;
        s_InputMethod = go_InputReference.name;
    }

    public void resetRotation()
    {
        f_RotationY = 0;
        f_LimitYRot = 0;
        transform.localPosition = new Vector3(f_xOffset, f_yOffset, f_zOffset);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0f)
        {
            //Keyboard input
            f_RotationY = 0;
            if (s_InputMethod == "Keyboard")
            {
                float mouseInput = Input.GetAxis("Mouse Y");
                if (mouseInput != 0)
                {
                    f_RotationY += mouseInput * f_SensitivityY;
                }
            }
            else
            {
                //controller input
                f_ControllerInput = Input.GetAxis(s_InputMethod + "RotateY");
                if (f_ControllerInput != 0)
                {
                    f_RotationY += -f_ControllerInput * f_SensitivityY;
                }
            }

            f_LimitYRot += (f_RotationY);
            if (f_LimitYRot <= f_MinYRot)
            {
                f_RotationY -= (f_LimitYRot - f_MinYRot);
                f_LimitYRot = f_MinYRot;
            }
            if (f_LimitYRot >= f_MaxYRot)
            {
                f_RotationY -= (f_LimitYRot - f_MaxYRot);
                f_LimitYRot = f_MaxYRot;
            }
            float f_CamY = transform.localPosition.y;

            //moves the camera along the parabola: z(y - f_yOffset) = ( f_SizeOfParabola * ((y - f_yOffset)^2) + f_zOffset)
            transform.localPosition = new Vector3(transform.localPosition.x,
                                                  (f_RotationY / -100) + f_CamY,
                                                  (f_SizeOfParabola * (Mathf.Pow((f_CamY + (f_RotationY / -100) - f_yOffset), 2)) + f_zOffset));
        }
    }
}
