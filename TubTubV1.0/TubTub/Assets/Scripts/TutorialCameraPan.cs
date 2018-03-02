//Pans the camera when user transitions into the level.

using UnityEngine;
using System.Collections;

public class TutorialCameraPan : MonoBehaviour {

    private float f_CamPos = 1.0f;
    public GameObject TutorialMenu;

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (f_CamPos > 0.0f)
        {
            f_CamPos -= 1.5f * Time.deltaTime;
        }
        else
        {
            f_CamPos = 0.0f;
            TutorialMenu.GetComponent<TutorialUI>().enabled = true;
        }
        gameObject.GetComponent<Camera>().rect = new Rect(f_CamPos, 0, 1, 1);
    }
}
