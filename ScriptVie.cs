using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script attaché au texte qui montre la vie du personnage
public class ScriptVie : MonoBehaviour
{

    public static int FirebrandLife = 5;
    Text txtVie;

    private void Awake()
    {
        txtVie = GetComponent<Text>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        txtVie.text = "Points de vie : " + FirebrandLife;
    }
}
