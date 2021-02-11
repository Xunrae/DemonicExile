using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script collé aux plateformes qui peuvent tomber et toucher aux piques
public class PlatformDestroyOnSpikes : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spikes") { Destroy(gameObject); }
    }
}
