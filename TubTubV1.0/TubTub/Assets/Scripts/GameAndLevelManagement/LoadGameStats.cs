//Script used to load game stats from PlayerPrefs.

using UnityEngine;
using System.Collections;

public class LoadGameStats : MonoBehaviour {

    GameManager ref_GameManager;
	// Use this for initialization
	void Start () 
    {
        ref_GameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
	    if (PlayerPrefs.GetFloat("ReloadTime") != 0f)
        {
            ref_GameManager.f_ReloadTime = PlayerPrefs.GetFloat("ReloadTime");
            ref_GameManager.f_PlayerDamage = PlayerPrefs.GetFloat("Damage");
            ref_GameManager.f_PlayerHealth = PlayerPrefs.GetFloat("Health");
            ref_GameManager.n_CurrentRound = PlayerPrefs.GetInt("CurrentRound");
            ref_GameManager.n_PlayerCoins = PlayerPrefs.GetInt("Coins");
        }
	}
}
