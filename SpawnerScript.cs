using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script attaché au générateur de fantomes
public class SpawnerScript : MonoBehaviour {
    public GameObject Monster;
	// Use this for initialization
	void Start () {
        InvokeRepeating("CloneMonster",0 , 6f);
	}
	
	void CloneMonster()
    {
        //crée un clone du monstre choisi
        GameObject monsterClone = Instantiate(Monster);

        //il devient actif parce que je n'ai pas utilisé de prefab
        monsterClone.SetActive(true);

        //il est généré exactement à ces positions (je vais utiliser plusieurs méthodes cet été pour générer les ennemis, je sais que celle-ci n'est pas optimale)
        monsterClone.transform.position = new Vector2 (Random.Range(12,28),Random.Range (9.5f,14.5f));

        //après 7 secondes le clone n'a plus raison d'être
        Destroy(monsterClone, 7f);
    }
}
