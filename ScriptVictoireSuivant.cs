using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script attaché à l'image de la Scene Victoire
public class ScriptVictoireSuivant : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //appuie sur space, retourne à l'intro
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Intro");
        }
	}
}
