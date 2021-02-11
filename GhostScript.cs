using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script collé aux fantômes générés par le "spawner"
public class GhostScript : MonoBehaviour {
    float vx;

    Rigidbody2D rbGhost;
    
	// Use this for initialization
	void Start () {
        rbGhost = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        rbGhost.velocity = new Vector2(-7,0);
	}
}
