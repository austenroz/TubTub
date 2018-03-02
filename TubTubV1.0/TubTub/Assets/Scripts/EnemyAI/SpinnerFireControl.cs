//Almost the exact same as EnemyCannonControl. The only difference is that the cannon will no longer turn to face toward the player.

using UnityEngine;
using System.Collections;

public class SpinnerFireControl : MonoBehaviour {

    private bool b_Begin = false;

    //Cannon rotation variables
	//private GameObject go_Player;
	//private GameObject go_Graphic;
	public float f_RotationSpeed = 40.0f;
    public float f_FireHeightPos = 1.0f;
    public float f_ProjectileSpeed = 2000f;

    //Cannon Fire variables
    private bool b_FireEnabled = true;
    private float f_CannonTimer = 0.0f;
    public float f_ReloadTime = 1.0f;
    public GameObject go_Projectile;
    public int n_InitialBurstFireCount = 3;

    //Fire Types
    public enum EnemyFireType { None, SingleFire, BurstFire, Cone };
    public EnemyFireType enm_EnemyFireType;
    public float f_ConeWidth = 45.0f;

    int n_BurstFireCount = 0;
    float f_BurstFireTimer = 0.0f;

	// Use this for initialization
	void Start () {
		//go_Player = GameObject.FindGameObjectWithTag ("Player");
		//go_Graphic = GameObject.FindGameObjectWithTag ("PlayerGraphic");
	}

    void FireCannon()
    {
        if (enm_EnemyFireType == EnemyFireType.SingleFire)
        {
            GameObject go_ProjectileInstance = Instantiate(go_Projectile, transform.position + 
                                                           transform.TransformDirection(0f, f_FireHeightPos, 3f), transform.rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;
            //
            go_ProjectileInstance = Instantiate(go_Projectile, transform.position +
                                               transform.TransformDirection(0f, f_FireHeightPos -1f, 3f), transform.rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;
        }
        else if (enm_EnemyFireType == EnemyFireType.BurstFire)
        {
            n_BurstFireCount = n_InitialBurstFireCount;
            //Instantiate(go_Projectile, transform.position + transform.TransformDirection(0f, 1f, 3f), transform.rotation);
        }
        else if (enm_EnemyFireType == EnemyFireType.Cone)
        {
            //
            Quaternion qt_Rotation = transform.rotation;
            GameObject go_ProjectileInstance = Instantiate(go_Projectile, transform.position + 
                                                           transform.TransformDirection(0f, f_FireHeightPos, 3f), qt_Rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;
            qt_Rotation = transform.rotation;
            go_ProjectileInstance = Instantiate(go_Projectile, transform.position +
                                                           transform.TransformDirection(0f, f_FireHeightPos-1f, 3f), qt_Rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;

            //
            qt_Rotation = Quaternion.Euler(qt_Rotation.eulerAngles.x, qt_Rotation.eulerAngles.y + f_ConeWidth, qt_Rotation.eulerAngles.z);
            go_ProjectileInstance = Instantiate(go_Projectile, transform.position + 
                                                           transform.TransformDirection(.5f, f_FireHeightPos, 3f), qt_Rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;
            qt_Rotation = Quaternion.Euler(qt_Rotation.eulerAngles.x, qt_Rotation.eulerAngles.y + f_ConeWidth, qt_Rotation.eulerAngles.z);
            go_ProjectileInstance = Instantiate(go_Projectile, transform.position +
                                                           transform.TransformDirection(.5f, f_FireHeightPos-1f, 3f), qt_Rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;

            //
            qt_Rotation = Quaternion.Euler(qt_Rotation.eulerAngles.x, qt_Rotation.eulerAngles.y - f_ConeWidth*2, qt_Rotation.eulerAngles.z);
            go_ProjectileInstance = Instantiate(go_Projectile, transform.position + 
                                                           transform.TransformDirection(-.5f, f_FireHeightPos, 3f), qt_Rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;
            qt_Rotation = Quaternion.Euler(qt_Rotation.eulerAngles.x, qt_Rotation.eulerAngles.y - f_ConeWidth * 2, qt_Rotation.eulerAngles.z);
            go_ProjectileInstance = Instantiate(go_Projectile, transform.position +
                                                           transform.TransformDirection(-.5f, f_FireHeightPos-1f, 3f), qt_Rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;
        }
    }


	// Update is called once per frame
	void Update () {
        if (b_Begin)
        {
            //Timer for cannon fire
            if (f_CannonTimer <= f_ReloadTime)
            {
                f_CannonTimer += Time.deltaTime;
            }
            else
            {
                b_FireEnabled = true;
            }

            //Timer for burst fire
            if (f_BurstFireTimer >= 0.05f && n_BurstFireCount > 0)
            {
                n_BurstFireCount--;
                f_BurstFireTimer = 0.0f;
                GameObject go_ProjectileInstance = Instantiate(go_Projectile, transform.position + 
                                                               transform.TransformDirection(0f, f_FireHeightPos, 3f), transform.rotation) as GameObject;
                go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;
                go_ProjectileInstance = Instantiate(go_Projectile, transform.position +
                                               transform.TransformDirection(0f, f_FireHeightPos-1f, 3f), transform.rotation) as GameObject;
                go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;

            }
            else
            {
                f_BurstFireTimer += Time.deltaTime;
            }

            //if cannon is reloaded
            if (b_FireEnabled)
            {
                b_FireEnabled = false;
                f_CannonTimer = 0.0f;
                FireCannon();
            }
        }
        else if (GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>().b_BeginRound)
            b_Begin = true;
	}
}
