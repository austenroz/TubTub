//Very simple script to keep an object after a different level loads.

using UnityEngine;
using System.Collections;

public class KeepOnLoad : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
