//Simple script to make a spinner spin at a set rate.

using UnityEngine;
using System.Collections;

public class SpinnerControl : MonoBehaviour {

    public float f_Speed = 5.0f;

	void Update () 
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * f_Speed);
	}
}
