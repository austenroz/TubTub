using UnityEngine;
using System.Collections;

public class EnemyProjectile : MonoBehaviour {
	
	public float f_ProjectileVelocity = 3500f;
	float f_Timer = 0.0f;
	float f_DeleteTime = 6.0f;
	float f_Damage = 100.0f;
    public GameObject go_PopSound;
	
	void Start () 
	{
		Vector3 f = transform.TransformDirection (0, 0, f_ProjectileVelocity);
		rigidbody.AddForce(f);
	}

	void setDamage(float x)
	{
		f_Damage = x;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Player")
		{
			col.gameObject.GetComponent<PlayerCollisionDetection> ().KilledPlayer ();
		}
		if (col.gameObject.tag == "Boat")
		{
            Instantiate(go_PopSound, transform.position, transform.rotation);
			col.gameObject.GetComponent<BoatHealth> ().SubtractBoatHealth (f_Damage);
		}
		//if (col.gameObject.tag != "Enemy")
		//{
			Destroy (gameObject);
		//}
	}
	void Update()
	{
		f_Timer += Time.deltaTime;
		if (f_Timer >= f_DeleteTime)
			Destroy (gameObject);
	}
}
