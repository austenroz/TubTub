//This script determines where the enemy will move. Movement can be random or determined before-hand.

using UnityEngine;
using System.Collections;

public class EnemyBoatMovement : MonoBehaviour {


    //VARIABLES
    public float f_BoatSpeed = 200.0f;
    public float f_RotationSpeed = 60.0f;

    public enum EnemyMovementType { None, Circles, FollowPlayer, RandomMov };
    public bool b_AssignRandomMovementType = true;
    public EnemyMovementType enm_EnemyMovementType;

    private bool b_begin = false;

	// Use this for initialization
	void Start () {
        if (b_AssignRandomMovementType)
        {
            int x = Random.Range(0, 4);
            if (x == 0)
                enm_EnemyMovementType = EnemyMovementType.None;
            if (x == 1)
                enm_EnemyMovementType = EnemyMovementType.Circles;
            if (x == 2)
                enm_EnemyMovementType = EnemyMovementType.FollowPlayer;
            if (x == 3)
                enm_EnemyMovementType = EnemyMovementType.RandomMov;
        }
	}
	
	// Update is called once per frame
	void Update () {
        //If round has begun, call the appropriate method for moving the enemy boat
        if (b_begin)
        {
            if (enm_EnemyMovementType == EnemyMovementType.None)
                NoMovement();
            if (enm_EnemyMovementType == EnemyMovementType.Circles)
                CircleMovement();
            if (enm_EnemyMovementType == EnemyMovementType.FollowPlayer)
                FollowPlayer();
            if (enm_EnemyMovementType == EnemyMovementType.RandomMov)
                RandomMovement();
        }
        else if (GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>().b_BeginRound)
            b_begin = true;
	}

    //Keep the boat from moving
    void NoMovement()
    {
        gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
    }

    //Boat moves in circles
    void CircleMovement()
    {
        gameObject.GetComponent<Rigidbody>().velocity =  gameObject.transform.TransformDirection(0, 0, f_BoatSpeed * Time.deltaTime);
        gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, f_RotationSpeed * Time.deltaTime, 0);
    }

    //Boat follows closest player
    void FollowPlayer()
    {
        gameObject.GetComponent<Rigidbody>().velocity = gameObject.transform.TransformDirection(0, 0, f_BoatSpeed * Time.deltaTime);
        //Rotate enemy to face the player
        Quaternion rotate;
        if (GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().b_IsSinglePlayer)
        {
            if (GameObject.FindGameObjectWithTag("PlayerGraphic").GetComponent<MeshRenderer>().enabled == true)
            {
                rotate = Quaternion.LookRotation(new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, GameObject.FindGameObjectWithTag("Player").transform.position.z) -
                                                 new Vector3(transform.position.x, 0, transform.position.z));
            }
            else
            {
                int x = 0;
                GameObject[] boats = GameObject.FindGameObjectsWithTag("Boat");
                boats = GameObject.FindGameObjectsWithTag("Boat");
                for (int i = 0; i < boats.Length; i++)
                {
                    if (boats[i].GetComponent<BoatHealth>().b_IsActive == true)
                    {
                        x = i;
                    }
                }
                rotate = Quaternion.LookRotation(new Vector3(boats[x].transform.position.x, 0, boats[x].transform.position.z) -
                                                 new Vector3(transform.position.x, 0, transform.position.z));
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, f_RotationSpeed * Time.smoothDeltaTime);
        }
            //if two players
        else
        {
            GameObject[] boats = GameObject.FindGameObjectsWithTag("Boat");
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPostion = transform.position;
            foreach (GameObject potentialTarget in boats)
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
                    if (potentialTarget.transform.Find("Graphics") != null)
                    {
                        if (potentialTarget.transform.Find("Graphics").GetComponent<MeshRenderer>().enabled)//FIX THIS LINE
                        {
                            closestDistanceSqr = dSqrToTarget;
                            bestTarget = potentialTarget;
                        }
                    }
                }
            }
            if (bestTarget != null)
            {
                rotate = Quaternion.LookRotation(new Vector3(bestTarget.transform.position.x, 0, bestTarget.transform.position.z) -
                                                 new Vector3(transform.position.x, 0, transform.position.z));
                transform.rotation = Quaternion.RotateTowards(transform.rotation, rotate, f_RotationSpeed * Time.smoothDeltaTime);
            }
        }
    }

    //Boat will switch between the three different move states randomely
    float timer = 0.0f;
    int currentRandomMov;
    void RandomMovement()
    {
        if (timer > 2.0f)
        {
            currentRandomMov = Random.Range(0, 3);
            timer = 0.0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
        if (currentRandomMov == 0)
            NoMovement();
        if (currentRandomMov == 1)
            CircleMovement();
        if (currentRandomMov == 2)
            FollowPlayer();
    }
}
