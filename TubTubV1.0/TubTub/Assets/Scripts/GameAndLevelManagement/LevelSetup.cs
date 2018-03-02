//Instantiates all objects necessary into the scene and sets their values correctly according to stats found in GameManager.cs.
//Also sets values correctly according to round number.

using UnityEngine;
using System.Collections;

public class LevelSetup : MonoBehaviour {

    public GameObject go_PlayerBoat;
    public GameObject go_Boat1Spawn;
    public GameObject go_Boat2Spawn;
    public GameObject go_EnemyBoatSpawn;

    public GameObject go_Player1Spawn;
    public GameObject go_Player2Spawn;

    public GameObject go_Player1Full;
    public GameObject go_Player1Half;
    public GameObject go_Player2;

    public GameObject go_BasicEnemyShip;
    public GameObject go_EliteEnemyShip;
    public GameObject go_LightningEnemyShip;
    public GameObject go_BasicEnemySpinner;
    public GameObject go_EliteEnemySpinner;
    public GameObject go_LightningEnemySpinner;
    public GameObject go_BasicLighthouse;
    public GameObject go_EliteLighthouse;
    public GameObject go_LightningLighthouse;

    private int n_CurrentRound = 1;
    private GameManager ref_GameManager;

	// Use this for initialization
	void Start () 
    {
        ref_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        n_CurrentRound = ref_GameManager.n_CurrentRound;
        //Spawn boats
        Instantiate(go_PlayerBoat, go_Boat1Spawn.transform.position, go_Boat1Spawn.transform.rotation);
        Instantiate(go_PlayerBoat, go_Boat2Spawn.transform.position, go_Boat1Spawn.transform.rotation);

        //Spawn players
        if (ref_GameManager.b_IsSinglePlayer)
        {
            GameObject go_P1Ref = Instantiate(go_Player1Full, go_Player1Spawn.transform.position, go_Player1Spawn.transform.rotation) as GameObject;
            go_P1Ref.transform.Find("ControllerInput").name = ref_GameManager.s_Player1Controller;
        }
        else
        {
            GameObject go_P1Ref = Instantiate(go_Player1Half, go_Player1Spawn.transform.position, go_Player1Spawn.transform.rotation) as GameObject;
            go_P1Ref.transform.Find("ControllerInput").name = ref_GameManager.s_Player1Controller;
            GameObject go_P2Ref = Instantiate(go_Player2, go_Player2Spawn.transform.position, go_Player2Spawn.transform.rotation) as GameObject;
            go_P2Ref.transform.Find("ControllerInput").name = ref_GameManager.s_Player2Controller;
        }

        //Set Random Wave variables depending on Round Level
        WaveGenerator ref_Waves = GameObject.FindGameObjectWithTag("Water").GetComponent<WaveGenerator>();
        float f_WaveHeight = Random.Range(0.1f, Mathf.Clamp(((float)n_CurrentRound / 20.0f), 0.15f, 1.0f));
        float f_WaveSpeed = Random.Range(1.0f, Mathf.Clamp((((float)n_CurrentRound * 5.0f) / 30.0f), 1.1f, 5.0f));
        ref_Waves.f_WaveHeight = f_WaveHeight;
        ref_Waves.f_WaveSpeed = f_WaveSpeed;


        //Spawn Enemies
        /*Basic ship counts as 1
         * Elite Ship counts as 2
         * Lightning Ship counts as 3
         * Basic Spinner counts as 2
         * Elite Spinner counts as 4
         * Lightning Spinner counts as 6
         * Basic Lighthouse counts as 3
         * Elite Lighthouse counts as 5
         * Lightning Lighthouse counts as 7
         * 
         * Round number is the number of points allotted when spawning enemies randomely
         * Health of ships scales up with round number (Ratio: EnemyHealth = EnemyHealth * (1 + (RoundNumber/5.0f))
         * Speed of Projectile scales with round number (Ratio: Velocity = Velocity * (1 + (RoundNumber/10.0f))
         * 
         * Special Fire Types:
         * 1/2 chance of enemy spawning with special fire type
         * special fire type adds 2 to counter above
         * 
         * Set position to random value within a radius (Part of Random class)
        */
        int n_SpawningPoints = n_CurrentRound;
        while (n_SpawningPoints > 0)
        {
            int n_RandomNum = 1;
            if (n_SpawningPoints < 7)
            {
                n_RandomNum = Random.Range(1, n_SpawningPoints + 1);
            }
            else
            {
                n_RandomNum = Random.Range(1, 8);
            }
            n_SpawningPoints -= n_RandomNum;
            GameObject go_SpawnedEnemy; //Set this value equal to whatever gets spawned below
            Vector2 vec2_SpawnPosition = Random.insideUnitCircle;
            int n_RandEnemy = Random.Range(0, 2);
            EnemyCannonControl ref_CannonControl;
            SpinnerFireControl ref_SpinnerFireControl;
            const float f_MaxProVelocity = 10000f;
            switch (n_RandomNum)
            {
                case 1:
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Spawn basic ship
                    //For position, get random position in a radius and then multiply x by 80 and z by 120
                    //Use Enemy boat spawn position and rotation
                    go_SpawnedEnemy = Instantiate(go_BasicEnemyShip, 
                                                  new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                              0.0f,  go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                  go_EnemyBoatSpawn.transform.rotation) as GameObject;

                    ref_CannonControl = go_SpawnedEnemy.transform.Find("Cannon")
                                                           .Find("CannonTopBase").Find("Rotator").GetComponent<EnemyCannonControl>();

                    ref_CannonControl.f_RotationSpeed += (ref_CannonControl.f_RotationSpeed) * ((float)n_CurrentRound / 5f);
                    ref_CannonControl.f_ProjectileSpeed += (ref_CannonControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                    ref_CannonControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                    if (ref_CannonControl.f_ProjectileSpeed > f_MaxProVelocity)
                    {
                        ref_CannonControl.f_ProjectileSpeed = f_MaxProVelocity;
                    }
                    go_SpawnedEnemy.GetComponent<EnemyHealth>().f_Health += (go_SpawnedEnemy.GetComponent<EnemyHealth>().f_Health) * ((float)n_CurrentRound / 10f);

                    //Special Fire Type Potential
                    if (n_SpawningPoints > 1)
                    {
                        int n_RandomFireType = Random.Range(1, 5);
                        {
                            switch (n_RandomFireType)
                            {
                                case 1:
                                case 2:
                                    break;
                                case 3:
                                    ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.BurstFire;
                                    n_SpawningPoints -= 2;
                                    break;
                                case 4:
                                    ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.Cone;

                                    n_SpawningPoints -= 2;
                                    break;
                            }
                        }
                    }
                    break;
                case 2:
                    //Spawn elite ship or basic spinner
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Spawn Elite Ship
                    if (n_RandEnemy == 1)
                    {
                        go_SpawnedEnemy = Instantiate(go_EliteEnemyShip,
                                                       new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                                   0.0f, go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                       go_EnemyBoatSpawn.transform.rotation) as GameObject;

                        ref_CannonControl = go_SpawnedEnemy.transform.Find("Cannon")
                                                               .Find("CannonTopBase").Find("Rotator").GetComponent<EnemyCannonControl>();
                        //Set variables to scale up with round level
                        ref_CannonControl.f_RotationSpeed += (ref_CannonControl.f_RotationSpeed) * ((float)n_CurrentRound / 5f);
                        ref_CannonControl.f_ProjectileSpeed += (ref_CannonControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                        ref_CannonControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                        if (ref_CannonControl.f_ProjectileSpeed > f_MaxProVelocity)
                        {
                            ref_CannonControl.f_ProjectileSpeed = f_MaxProVelocity;
                        }
                        go_SpawnedEnemy.GetComponent<EnemyHealth>().f_Health += (go_SpawnedEnemy.GetComponent<EnemyHealth>().f_Health) * ((float)n_CurrentRound / 10f);

                        //Special Fire Type Potential
                        if (n_SpawningPoints > 1)
                        {
                            int n_RandomFireType = Random.Range(1, 5);
                            {
                                switch (n_RandomFireType)
                                {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.BurstFire;
                                        n_SpawningPoints -= 2;
                                        break;
                                    case 4:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.Cone;
                                        n_SpawningPoints -= 2;
                                        break;
                                }
                            }
                        }
                    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Spawn Basic Spinner
                    else
                    {
                        go_SpawnedEnemy = Instantiate(go_BasicEnemySpinner,
                                                      new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                                  0.0f, go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                      go_EnemyBoatSpawn.transform.rotation) as GameObject;

                        ref_SpinnerFireControl = go_SpawnedEnemy.transform.Find("Spinner").Find("Cannon")
                                       .Find("CannonTopBase").Find("Rotator").GetComponent<SpinnerFireControl>();
                        //Set variables to scale with round level
                        ref_SpinnerFireControl.f_ProjectileSpeed += (ref_SpinnerFireControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                        ref_SpinnerFireControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                        if (ref_SpinnerFireControl.f_ProjectileSpeed > f_MaxProVelocity)
                        {
                            ref_SpinnerFireControl.f_ProjectileSpeed = f_MaxProVelocity;
                        }
                        go_SpawnedEnemy.transform.Find("Spinner").Find("TopCollider").GetComponent<EnemyHealth>().f_Health +=
                            (go_SpawnedEnemy.transform.Find("Spinner").Find("TopCollider").GetComponent<EnemyHealth>().f_Health) * ((float)n_CurrentRound / 10f);

                        //Special Fire Type Potential
                        if (n_SpawningPoints > 1)
                        {
                            int n_RandomFireType = Random.Range(1, 5);
                            {
                                switch (n_RandomFireType)
                                {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        ref_SpinnerFireControl.enm_EnemyFireType = SpinnerFireControl.EnemyFireType.BurstFire;
                                        n_SpawningPoints -= 2;
                                        break;
                                    case 4:
                                        ref_SpinnerFireControl.enm_EnemyFireType = SpinnerFireControl.EnemyFireType.Cone;
                                        n_SpawningPoints -= 2;
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case 3: 
                    //Spawn lightning ship or basic lighthouse
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Lightning Ship
                    if (n_RandEnemy == 1)
                    {
                        go_SpawnedEnemy = Instantiate(go_EliteEnemyShip,
                                                       new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                                   0.0f, go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                       go_EnemyBoatSpawn.transform.rotation) as GameObject;

                        ref_CannonControl = go_SpawnedEnemy.transform.Find("Cannon")
                                                               .Find("CannonTopBase").Find("Rotator").GetComponent<EnemyCannonControl>();
                        //Set variables to scale up with round level
                        ref_CannonControl.f_RotationSpeed += (ref_CannonControl.f_RotationSpeed) * ((float)n_CurrentRound / 5f);
                        ref_CannonControl.f_ProjectileSpeed += (ref_CannonControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                        ref_CannonControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                        if (ref_CannonControl.f_ProjectileSpeed > f_MaxProVelocity)
                        {
                            ref_CannonControl.f_ProjectileSpeed = f_MaxProVelocity;
                        }
                        go_SpawnedEnemy.GetComponent<EnemyHealth>().f_Health += (go_SpawnedEnemy.GetComponent<EnemyHealth>().f_Health) * ((float)n_CurrentRound / 10f);

                        //Special Fire Type Potential
                        if (n_SpawningPoints > 1)
                        {
                            int n_RandomFireType = Random.Range(1, 5);
                            {
                                switch (n_RandomFireType)
                                {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.BurstFire;
                                        n_SpawningPoints -= 2;
                                        break;
                                    case 4:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.Cone;
                                        n_SpawningPoints -= 2;
                                        break;
                                }
                            }
                        }
                    }
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Basic Lighthouse
                    else
                    {
                        go_SpawnedEnemy = Instantiate(go_BasicLighthouse,
                                                       new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                                   0.0f, go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                       go_EnemyBoatSpawn.transform.rotation) as GameObject;

                        ref_CannonControl = go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("Cannon")
                                                               .Find("CannonTopBase").Find("Rotator").GetComponent<EnemyCannonControl>();
                        //Set variables to scale up with round level
                        ref_CannonControl.f_RotationSpeed += (ref_CannonControl.f_RotationSpeed) * ((float)n_CurrentRound / 5f);
                        ref_CannonControl.f_ProjectileSpeed += (ref_CannonControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                        ref_CannonControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                        if (ref_CannonControl.f_ProjectileSpeed > f_MaxProVelocity)
                        {
                            ref_CannonControl.f_ProjectileSpeed = f_MaxProVelocity;
                        }

                        go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("TopCollider").GetComponent<EnemyHealth>().f_Health +=
                            go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("TopCollider").GetComponent<EnemyHealth>().f_Health *
                            ((float)n_CurrentRound / 10f);

                        //Special Fire Type Potential
                        if (n_SpawningPoints > 1)
                        {
                            int n_RandomFireType = Random.Range(1, 5);
                            {
                                switch (n_RandomFireType)
                                {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.BurstFire;
                                        n_SpawningPoints -= 2;
                                        break;
                                    case 4:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.Cone;
                                        n_SpawningPoints -= 2;
                                        break;
                                }
                            }
                        }
                    }

                    break;
                case 4:
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Spawn Elite Spinner
                        go_SpawnedEnemy = Instantiate(go_EliteEnemySpinner,
                                                      new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                                  0.0f, go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                      go_EnemyBoatSpawn.transform.rotation) as GameObject;

                        ref_SpinnerFireControl = go_SpawnedEnemy.transform.Find("Spinner").Find("Cannon")
                                       .Find("CannonTopBase").Find("Rotator").GetComponent<SpinnerFireControl>();
                        //Set variables to scale with round level
                        ref_SpinnerFireControl.f_ProjectileSpeed += (ref_SpinnerFireControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                        ref_SpinnerFireControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                        if (ref_SpinnerFireControl.f_ProjectileSpeed > f_MaxProVelocity)
                        {
                            ref_SpinnerFireControl.f_ProjectileSpeed = f_MaxProVelocity;
                        }
                        go_SpawnedEnemy.transform.Find("Spinner").Find("TopCollider").GetComponent<EnemyHealth>().f_Health +=
                            (go_SpawnedEnemy.transform.Find("Spinner").Find("TopCollider").GetComponent<EnemyHealth>().f_Health) * ((float)n_CurrentRound / 10f);

                        //Special Fire Type Potential
                        if (n_SpawningPoints > 1)
                        {
                            int n_RandomFireType = Random.Range(1, 5);
                            {
                                switch (n_RandomFireType)
                                {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        ref_SpinnerFireControl.enm_EnemyFireType = SpinnerFireControl.EnemyFireType.BurstFire;
                                        n_SpawningPoints -= 2;
                                        break;
                                    case 4:
                                        ref_SpinnerFireControl.enm_EnemyFireType = SpinnerFireControl.EnemyFireType.Cone;
                                        n_SpawningPoints -= 2;
                                        break;
                                }
                            }
                        }
                    break;
                case 5:
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Spawn Elite Lighthouse
                        go_SpawnedEnemy = Instantiate(go_EliteLighthouse,
                                                       new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                                   0.0f, go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                       go_EnemyBoatSpawn.transform.rotation) as GameObject;

                        ref_CannonControl = go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("Cannon")
                                                               .Find("CannonTopBase").Find("Rotator").GetComponent<EnemyCannonControl>();
                        //Set variables to scale up with round level
                        ref_CannonControl.f_RotationSpeed += (ref_CannonControl.f_RotationSpeed) * ((float)n_CurrentRound / 5f);
                        ref_CannonControl.f_ProjectileSpeed += (ref_CannonControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                        ref_CannonControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                        if (ref_CannonControl.f_ProjectileSpeed > f_MaxProVelocity)
                        {
                            ref_CannonControl.f_ProjectileSpeed = f_MaxProVelocity;
                        }
                        go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("TopCollider").GetComponent<EnemyHealth>().f_Health +=
                            go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("TopCollider").GetComponent<EnemyHealth>().f_Health *
                            ((float)n_CurrentRound / 10f);

                        //Special Fire Type Potential
                        if (n_SpawningPoints > 1)
                        {
                            int n_RandomFireType = Random.Range(1, 5);
                            {
                                switch (n_RandomFireType)
                                {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.BurstFire;
                                        n_SpawningPoints -= 2;
                                        break;
                                    case 4:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.Cone;
                                        n_SpawningPoints -= 2;
                                        break;
                                }
                            }
                        }
                    break;
                case 6:
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Spawn Lightning Spinner
                        go_SpawnedEnemy = Instantiate(go_LightningEnemySpinner,
                                                      new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                                  0.0f, go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                      go_EnemyBoatSpawn.transform.rotation) as GameObject;

                        ref_SpinnerFireControl = go_SpawnedEnemy.transform.Find("Spinner").Find("Cannon")
                                       .Find("CannonTopBase").Find("Rotator").GetComponent<SpinnerFireControl>();
                        //Set variables to scale with round level
                        ref_SpinnerFireControl.f_ProjectileSpeed += (ref_SpinnerFireControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                        ref_SpinnerFireControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                        if (ref_SpinnerFireControl.f_ProjectileSpeed > f_MaxProVelocity)
                        {
                            ref_SpinnerFireControl.f_ProjectileSpeed = f_MaxProVelocity;
                        }
                        go_SpawnedEnemy.transform.Find("Spinner").Find("TopCollider").GetComponent<EnemyHealth>().f_Health +=
                            (go_SpawnedEnemy.transform.Find("Spinner").Find("TopCollider").GetComponent<EnemyHealth>().f_Health) * ((float)n_CurrentRound / 10f);

                        //Special Fire Type Potential
                        if (n_SpawningPoints > 1)
                        {
                            int n_RandomFireType = Random.Range(1, 5);
                            {
                                switch (n_RandomFireType)
                                {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        ref_SpinnerFireControl.enm_EnemyFireType = SpinnerFireControl.EnemyFireType.BurstFire;
                                        n_SpawningPoints -= 2;
                                        break;
                                    case 4:
                                        ref_SpinnerFireControl.enm_EnemyFireType = SpinnerFireControl.EnemyFireType.Cone;
                                        n_SpawningPoints -= 2;
                                        break;
                                }
                            }
                        }
                    break;
                case 7:
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    //Spawn Lightning Lighthouse
                        go_SpawnedEnemy = Instantiate(go_LightningLighthouse,
                                                       new Vector3(go_EnemyBoatSpawn.transform.position.x + (vec2_SpawnPosition.x * 80.0f),
                                                                   0.0f, go_EnemyBoatSpawn.transform.position.z + (vec2_SpawnPosition.y * 100.0f)),
                                                       go_EnemyBoatSpawn.transform.rotation) as GameObject;

                        ref_CannonControl = go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("Cannon")
                                                               .Find("CannonTopBase").Find("Rotator").GetComponent<EnemyCannonControl>();
                        //Set variables to scale up with round level
                        ref_CannonControl.f_RotationSpeed += (ref_CannonControl.f_RotationSpeed) * ((float)n_CurrentRound / 5f);
                        ref_CannonControl.f_ProjectileSpeed += (ref_CannonControl.f_ProjectileSpeed) * ((float)n_CurrentRound / 10f);
                        ref_CannonControl.f_ConeWidth *= (1 / ((float)n_CurrentRound * .08f));

                        if (ref_CannonControl.f_ProjectileSpeed > f_MaxProVelocity)
                        {
                            ref_CannonControl.f_ProjectileSpeed = f_MaxProVelocity;
                        }
                        go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("TopCollider").GetComponent<EnemyHealth>().f_Health +=
                            go_SpawnedEnemy.transform.Find("LightHouseBase").Find("Top").Find("TopCollider").GetComponent<EnemyHealth>().f_Health *
                            ((float)n_CurrentRound / 10f);

                        //Special Fire Type Potential
                        if (n_SpawningPoints > 1)
                        {
                            int n_RandomFireType = Random.Range(1, 5);
                            {
                                switch (n_RandomFireType)
                                {
                                    case 1:
                                    case 2:
                                        break;
                                    case 3:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.BurstFire;
                                        n_SpawningPoints -= 2;
                                        break;
                                    case 4:
                                        ref_CannonControl.enm_enemyFireType = EnemyCannonControl.EnemyFireType.Cone;
                                        n_SpawningPoints -= 2;
                                        break;
                                }
                            }
                        }
                    break;
                default:
                    break;
            }
        }
    }
}
