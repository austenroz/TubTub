using UnityEngine;
using System.Collections;

public class BoatCollision : MonoBehaviour {

    public GameObject go_CurrentPlayer;

	void OnCollisionEnter(Collision col)
	{
        if (gameObject.GetComponent<BoatHealth>().b_IsActive)
        {
            if (col.gameObject.tag == "Wall" || col.gameObject.tag == "Enemy")
		    {
                //Call the WallCollision method within boat movement script to bounce player back when they collide with a wall
			    GameObject.FindGameObjectWithTag("Player").GetComponent<BoatMovement>().WallCollision();
		    }
        }
	}
	
}
