using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script collé à la plateforme finale, qui amène le joueur à la victoire
public class PlatformSliderUp : MonoBehaviour {


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //si le démon touche la plateforme, le moteur s'active
        if (collision.gameObject.name == "DemonVide")
        {
            GetComponent<SliderJoint2D>().useMotor = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //si le démon quitte la plateforme, après 6 secondes le moteur s'arrête et la plateforme retombe
        if (collision.gameObject.name == "DemonVide")
        {
            Invoke("StopMotor", 6f);
        }
    }

    //le moteur s'arrête
    void StopMotor()
    {
        GetComponent<SliderJoint2D>().useMotor = false;
    }
}
