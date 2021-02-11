using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script collé à l'objet qui fait gagner la partie
public class ScriptVictoire : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //si le démon touche l'objet, le joueur gagne
        if(collision.collider.name == "DemonVide") {

            SceneManager.LoadScene("Victoire");
        }
    }
}
