//Destroy game object after timer

using UnityEngine;
using System.Collections;

public class DestroyAfterTimer : MonoBehaviour {

    public float f_TimeToDestroy = 2.0f;
    private float f_Timer = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        f_Timer += Time.deltaTime;
        if (f_Timer >= f_TimeToDestroy)
        {
            Destroy(gameObject);
        }
	}
}
