using UnityEngine;
using System.Collections;

public class EnemyFire : MonoBehaviour {

    //VARIABLES
    private bool b_Begin = false;

	public bool b_FireEnabled = true;
	private float f_CannonTimer = 0.0f;
	public float f_ReloadTime = 1.0f;
	public GameObject go_Projectile;
	

	// Update is called once per frame
    //Fire Cannon everytime timer goes above the reloadtime.
	void Update () {
        if (b_Begin)
        {
            if (f_CannonTimer <= f_ReloadTime)
            {
                f_CannonTimer += Time.deltaTime;
            }
            else
            {
                f_CannonTimer = 0.0f;
                Instantiate(go_Projectile, transform.position + transform.TransformDirection(0f, 0f, 3f), transform.rotation);
            }
        }
        else if (GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>().b_BeginRound)
            b_Begin = true;
	}
}
