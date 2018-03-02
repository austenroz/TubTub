//Script used to set objects to the current water height at its position.

using UnityEngine;
using System.Collections;

public class SetToWaterHeight : MonoBehaviour {

    WaveGenerator waves;
    float f_ThisTimer;
    float f_ThisWaveSpeed;
    float f_ThisWaveSmoothness;
    float f_ThisWaveHeight;
    float f_HalfHeight;
    public float f_Offset = 2.0f;
    float f_CurrentAngle = 0.0f;
    public float f_Weight = 2f;
    float f_WaterHeight;
    float f_RotationWaterHeight; //Used for setting f_Weight of object so it will not be affected dynamically and break

	// Use this for initialization
	void Start () {
        waves = GameObject.FindGameObjectWithTag("Water").GetComponent<WaveGenerator>();
        f_ThisTimer = waves.f_Timer;
        f_ThisWaveSpeed = waves.f_WaveSpeed;
        f_ThisWaveSmoothness = waves.f_WaveSmoothness;
        f_ThisWaveHeight = waves.f_WaveHeight;
        f_HalfHeight = renderer.bounds.extents.y;
        if (gameObject.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb_Rig = gameObject.GetComponent<Rigidbody>();
            rb_Rig.useGravity = false;
        }
        f_WaterHeight = GameObject.FindGameObjectWithTag("Water").transform.position.y;
        f_RotationWaterHeight = waves.f_WaveHeight;
	}
	
	// Update is called once per frame
	void Update () {
        f_ThisTimer = waves.f_Timer;
        f_ThisWaveSpeed = waves.f_WaveSpeed;
        f_ThisWaveSmoothness = waves.f_WaveSmoothness;
        f_ThisWaveHeight = waves.f_WaveHeight;

        //Set height to position on sin curve based on objects x position
        gameObject.transform.position = new Vector3 (gameObject.transform.position.x,
                                                    ((Mathf.Sin((gameObject.transform.position.x + (f_ThisTimer * f_ThisWaveSpeed)) / f_ThisWaveSmoothness)) * f_ThisWaveHeight) - f_Offset + f_WaterHeight + f_HalfHeight, 
                                                    gameObject.transform.position.z);
        if (f_Weight != 0)
        {
            //Set the objects z rotation to the tangent line of the curve on the point x
            float f_TangentLine = (Mathf.Cos((gameObject.transform.position.x + (f_ThisTimer * f_ThisWaveSpeed)) / f_ThisWaveSmoothness)) / 2.0f;
            float f_Diff = (f_CurrentAngle - f_TangentLine) * -57.2957795f; //convert to degrees
            f_CurrentAngle = f_TangentLine;

            gameObject.transform.RotateAround(gameObject.transform.position, Vector3.forward, f_Diff / (f_Weight / f_RotationWaterHeight));
            //gameObject.transform.RotateAround(gameObject.transform.position, GameObject.FindGameObjectWithTag("Global").transform.forward, f_Diff / (f_Weight / f_RotationWaterHeight));
        }
	}
}