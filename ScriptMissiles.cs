using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script collé aux missiles projetés par le démon
public class ScriptMissiles : MonoBehaviour {
    Animator anim;
    
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}

    // Update is called once per frame
    void Update() {
        //si le missile va vers la gauche, son sprite est flippé
        if (GetComponent<Rigidbody2D>().velocity.x < 0) { GetComponent<SpriteRenderer>().flipX = true; }
    }

    private void OnCollisionEnter2D(Collision2D autreObjet)
    {
        anim.SetBool("isMoving", false);
        anim.SetBool("isExploding", true);

        //détruit le missile après un petit délai pour qu'on puisse voir son animation de frappe
        Destroy(gameObject, 0.3f);

        //Si on touche un ennemi
        //éventuellement les ennemis auront des points de vie, ce script va faire descendre leurs points de vie donc.
        if (autreObjet.gameObject.tag == "Ennemi")
        {
            //détruit l'ennemi, augmente le score
            Destroy(autreObjet.gameObject);
            ScriptScore.score += 10;
        }
    }
}
