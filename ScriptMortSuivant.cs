﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script collé à l'image de la page Mort
public class ScriptMortSuivant : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        //appuie sur space, passe au suivant
        if (Input.GetKey(KeyCode.Space))
        {
            SceneManager.LoadScene("Intro");
        }
	}
}
