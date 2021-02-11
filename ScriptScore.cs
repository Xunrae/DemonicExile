using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script attaché au texte qui montre le score, pas grand chose à expliquer ici...
public class ScriptScore : MonoBehaviour
{

    public static int score;
    Text txtScore;

    private void Awake()
    {
        txtScore = GetComponent<Text>();
        score = 0;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        txtScore.text = "Pointage : " + score;
    }
}
