//Kill the player if they collide with a projectile

using UnityEngine;
using System.Collections;

public class PlayerCollisionDetection : MonoBehaviour {

	public void KilledPlayer()
	{
		Debug.Log ("Player has died");
		Application.LoadLevel (Application.loadedLevel);
	}

	void OnControllerColliderHit(ControllerColliderHit col)
	{
		if (col.gameObject.tag == "EnemyProjectile1" || 
		    col.gameObject.tag == "EnemyProjectile2" ||
		    col.gameObject.tag == "EnemyProjectile3" ||
		    col.gameObject.tag == "PlayerDeath" ||
            col.gameObject.tag == "Water" )
		{
			Debug.Log("Player has died");
            KilledPlayer();
		}
	}
}
