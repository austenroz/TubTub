using UnityEngine;
using System.Collections;

public class BoatHealth : MonoBehaviour {

    //VARIABLES
	private float f_boatHealth = 500.0f;
	public bool b_IsActive = false;
    public GameObject go_CurrentPlayer;


	public void SetBoatHealth(float x)
	{
		f_boatHealth = x;
	}

    //Call this method to subtract health from player's boat
	public void SubtractBoatHealth(float amount)
	{
        if (b_IsActive)
        {
            f_boatHealth -= amount;
            go_CurrentPlayer.GetComponent<BoatMovement>().f_BoatHealth = f_boatHealth;
            if (f_boatHealth <= 0.0f)
            {
                Debug.Log("Boat is dead");
                if (b_IsActive)
                {
                    go_CurrentPlayer.GetComponent<BoatMovement>().b_BoatDead = true;
                    gameObject.tag = "DeadBoat";
                }
            }
        }
	}

	public float getHealth()
	{
		return f_boatHealth;
	}
}
