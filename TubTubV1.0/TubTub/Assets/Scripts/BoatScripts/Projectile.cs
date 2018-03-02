//This script is used for the player's projectiles

using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    //VARIABLES
	public float f_ProjectileVelocity = 500f;
	float f_Timer = 0.0f;
	float f_DeleteTime = 6.0f;
	private BoatMovement ref_BoatMovement;
    public GameObject go_PopSound;


	void Start () 
	{
		Vector3 f = transform.TransformDirection (0, 0, f_ProjectileVelocity);
		rigidbody.AddForce(f);
		ref_BoatMovement = GameObject.FindGameObjectWithTag ("Player").GetComponent<BoatMovement> ();
	}

    //When the projecile collides with an enemy, call SubtractHealth method from enemy, otherwise remove the projectile.
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Enemy")
		{
            Instantiate(go_PopSound, transform.position, transform.rotation);
			col.gameObject.GetComponent<EnemyHealth>().SubtractHealth(ref_BoatMovement.getDamage());
		}
        if (col.gameObject.tag != "EnemyProjectile1")
		    Destroy (gameObject);
	}

    //Self destruct timer
	void Update()
	{
		f_Timer += Time.deltaTime;
		if (f_Timer >= f_DeleteTime)
			Destroy (gameObject);
	}
}
