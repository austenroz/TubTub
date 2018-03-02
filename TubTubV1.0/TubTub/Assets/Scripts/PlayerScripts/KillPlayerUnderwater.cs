//Kills the player if the go underneath the water waves

using UnityEngine;
using System.Collections;

public class KillPlayerUnderwater : MonoBehaviour
{

    WaveGenerator ref_Waves;
    float f_ThisTimer;
    float f_ThisWaveSpeed;
    float f_ThisWaveSmoothness;
    float f_ThisWaveHeight;
    float f_HalfHeight;
    public float f_Offset = 2.0f;
    float f_WaterHeight;

    // Use this for initialization
    void Start()
    {
        ref_Waves = GameObject.FindGameObjectWithTag("Water").GetComponent<WaveGenerator>();
        f_ThisTimer = ref_Waves.f_Timer;
        f_ThisWaveSpeed = ref_Waves.f_WaveSpeed;
        f_ThisWaveSmoothness = ref_Waves.f_WaveSmoothness;
        f_ThisWaveHeight = ref_Waves.f_WaveHeight;
        f_HalfHeight = renderer.bounds.extents.y;
        f_WaterHeight = GameObject.FindGameObjectWithTag("Water").transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        f_ThisTimer = ref_Waves.f_Timer;
        f_ThisWaveSpeed = ref_Waves.f_WaveSpeed;
        f_ThisWaveSmoothness = ref_Waves.f_WaveSmoothness;
        f_ThisWaveHeight = ref_Waves.f_WaveHeight;
        float currentWaveHeight = ((Mathf.Sin((gameObject.transform.position.x + (f_ThisTimer * f_ThisWaveSpeed)) / f_ThisWaveSmoothness)) * f_ThisWaveHeight) - f_Offset + f_WaterHeight + f_HalfHeight;
        if (transform.position.y < currentWaveHeight)
        {
            //Kill Player
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCollisionDetection>().KilledPlayer();
        }
    }
}