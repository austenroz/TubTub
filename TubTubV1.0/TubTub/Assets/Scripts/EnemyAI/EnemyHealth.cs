//This script controls the health of enemies, destruction of enemies, and blinking of enemies

using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    //VARIABLES
	public float f_Health = 100;
    public GameObject[] go_blinkGameObjects;
    public GameObject go_EnemyToDelete;
	private float f_Timer = 0.0f;
	Color[] col_Default;
	private int n_BlinkMaterial = 0;
	private int n_NumOfBlinks = 2;
	private int n_BlinkCounter = 0;
    private bool b_Dead = false;
	
    //Method to toggle the color from default color to red and back
	void toggleColor()
	{
        for (int i = 0; i < go_blinkGameObjects.Length; i++)
        {
            if (go_blinkGameObjects[i].renderer.material.color == col_Default[i])
            {
                go_blinkGameObjects[i].renderer.material.color = Color.red;
            }
            else
            {
                go_blinkGameObjects[i].renderer.material.color = col_Default[i];
            }
        }
	}

    
	void SetHealth(float x)
	{
		f_Health = x;
	}
	
    //Subtract the enemy health and call appropriate methods.
	public void SubtractHealth(float x)
	{
		f_Health -= x;
		if (f_Health <= 0)
		{
            GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>().KilledEnemy(go_EnemyToDelete);
            Destroy(go_EnemyToDelete);
			//Increase player points
		}
		f_Timer = 0.0f;
		n_BlinkMaterial ++;
	}

	float GetHealth()
	{
		return f_Health;
	}

    //When projectile collides with enemy
	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Projectile")
		{
			//SubtractHealth(boatmove.damage);
			Debug.Log ("Boat Health: " + f_Health);
		}
	}

    //Set variables to correct values
	void Start()
	{
        col_Default = new Color[go_blinkGameObjects.Length];
        for (int i = 0; i < go_blinkGameObjects.Length; i++ )
        {
            col_Default[i] = go_blinkGameObjects[i].renderer.material.color;
        }
        GameObject.FindGameObjectWithTag("LevelManager").GetComponent<CurrentLevelStats>().AddOneTotalEnemy();
            //col_Default = renderer.material.color;
	}

    //Timer for blinking the material
	void Update()
	{
		f_Timer += Time.deltaTime;
		if (n_BlinkMaterial > 0)
		{
			if (f_Timer >= .1f)
			{
				f_Timer = 0.0f;
				toggleColor();
				n_BlinkCounter++;
				if (n_BlinkCounter >= n_NumOfBlinks)
				{
                    n_BlinkMaterial--;
					n_BlinkCounter = 0;
				}
			}
		}
	}
}
