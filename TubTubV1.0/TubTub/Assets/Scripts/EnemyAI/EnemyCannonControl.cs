using UnityEngine;
using System.Collections;

public class EnemyCannonControl : MonoBehaviour {

    private bool b_Begin = false;

    //Cannon rotation variables
	//private GameObject go_Player;
	//private GameObject go_Graphic;
	public float f_RotationSpeed = 40.0f;
	//private GameObject[] boats;

    public float f_FireHeightPos = 1.0f;
    public float f_ProjectileSpeed = 2000f;

    //Cannon Fire variables
    private bool b_FireEnabled = true;
    private float f_CannonTimer = 0.0f;
    public float f_ReloadTime = 1.0f;
    public GameObject go_Projectile;
    public bool b_FireWhenFacingPlayer = true;

    //Fire Types
    public enum EnemyFireType { None, SingleFire, BurstFire, Cone };
    public EnemyFireType enm_enemyFireType;
    public float f_ConeWidth = 45.0f;
    public int n_InitialBurstFireCount = 3;

    int n_BurstFireCount = 0;
    float f_BurstFireTimer = 0.0f;

	// Use this for initialization
	void Start () {
		//go_Player = GameObject.FindGameObjectWithTag ("Player");
		//go_Graphic = GameObject.FindGameObjectWithTag ("PlayerGraphic");
	}

    void FireCannon()
    {
        //If Fire type is set to a single projectile at a time
        if (enm_enemyFireType == EnemyFireType.SingleFire)
        {
            GameObject go_ProjectileInstance = Instantiate(go_Projectile, transform.position + transform.TransformDirection(0f, f_FireHeightPos, 3f),
                                                           transform.rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;
        }

        //If Fire type is set to fire a burst of shots at a time
        else if (enm_enemyFireType == EnemyFireType.BurstFire)
        {
            n_BurstFireCount = n_InitialBurstFireCount;
            //Instantiate(go_Projectile, transform.position + transform.TransformDirection(0f, 1f, 3f), transform.rotation);
        }

        //If Fire type is set to fire three projectiles in a cone
        else if (enm_enemyFireType == EnemyFireType.Cone)
        {
            Quaternion qt_Rotation = transform.rotation;
            GameObject go_ProjectileInstance = Instantiate(go_Projectile, transform.position + 
                                                           transform.TransformDirection(0f, f_FireHeightPos, 3f), qt_Rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;

            qt_Rotation = Quaternion.Euler(qt_Rotation.eulerAngles.x, qt_Rotation.eulerAngles.y + f_ConeWidth, qt_Rotation.eulerAngles.z);
            go_ProjectileInstance = Instantiate(go_Projectile, transform.position + 
                                                           transform.TransformDirection(.5f, f_FireHeightPos, 3f), qt_Rotation) as GameObject;
            go_ProjectileInstance.GetComponent<EnemyProjectile>().f_ProjectileVelocity = f_ProjectileSpeed;

            qt_Rotation = Quaternion.Euler(qt_Rotation.eulerAngles.x, qt_Rotation.eulerAngles.y - f_ConeWidth*2, qt_Rotation.eulerAngles.z);
            go_ProjectileInstance = Instantiate(go_Projectile, transform.position + 
                                                           transform.TransformDirection(-.5f, f_FireHeightPos, 3f), qt_Rotation) as GameObject;
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
            }
            else
            {
                f_BurstFireTimer += Time.deltaTime;
            }


            //Rotate cannon to face the go_Player
            Quaternion rotate = transform.rotation;
            //Single player condition
            if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().b_IsSinglePlayer)
            {
                if (GameObject.FindGameObjectWithTag("PlayerGraphic").GetComponent<MeshRenderer>().enabled == true)
                {
                    rotate = Quaternion.LookRotation(GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
                }
                else
                {
                    int x = 0;
                    GameObject[] boats = GameObject.FindGameObjectsWithTag("Boat");
                    //boats = GameObject.FindGameObjectsWithTag("Boat");
                    for (int i = 0; i < boats.Length; i++)
                    {
                        if (boats[i].GetComponent<BoatHealth>().b_IsActive == true)
                        {
                            x = i;
                        }
                    }
                    rotate = Quaternion.LookRotation(boats[x].transform.position - transform.position);
                }
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, f_RotationSpeed * Time.smoothDeltaTime);
            }
            //Two player condition
            else
            {
                GameObject[] boats = GameObject.FindGameObjectsWithTag("Boat");
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		        GameObject bestTarget = null;
		        float closestDistanceSqr = Mathf.Infinity;
		        Vector3 currentPostion = transform.position;
		        foreach(GameObject potentialTarget in boats)
		        {
			        Vector3 directionToTarget = potentialTarget.transform.position - currentPostion;
			        float dSqrToTarget = directionToTarget.sqrMagnitude;
			        if (dSqrToTarget < closestDistanceSqr)
			        {
                        if (potentialTarget.GetComponent<BoatHealth>().b_IsActive)
                        {
                            closestDistanceSqr = dSqrToTarget;
                            bestTarget = potentialTarget;
                        }
			        }
		        }
                foreach (GameObject potentialTarget in players)
                {
                    Vector3 directionToTarget = potentialTarget.transform.position - currentPostion;
                    float dSqrToTarget = directionToTarget.sqrMagnitude;
                    if (dSqrToTarget < closestDistanceSqr)
                    {
                        if (potentialTarget.transform.Find("Graphics").GetComponent<MeshRenderer>().enabled)
                        {
                            closestDistanceSqr = dSqrToTarget;
                            bestTarget = potentialTarget;
                        }
                    }
                }
                rotate = Quaternion.LookRotation(bestTarget.transform.position - transform.position);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, f_RotationSpeed * Time.smoothDeltaTime);
	        }

            //Fire when cannon rotation is close to player position
            if (b_FireEnabled)
            {
                if (b_FireWhenFacingPlayer)
                {
                    if (Mathf.Abs(transform.rotation.eulerAngles.y - rotate.eulerAngles.y) < 40.0f)
                    {
                        b_FireEnabled = false;
                        f_CannonTimer = 0.0f;
                        FireCannon();
                    }
                }
                else
                {
                    b_FireEnabled = false;
                    f_CannonTimer = 0.0f;
                    FireCannon();
                }
            }
        }
        else if (GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>().b_BeginRound)
            b_Begin = true;
	}
}
