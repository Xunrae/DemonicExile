using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Script collé au DemonVide
public class Demon_Velocity : MonoBehaviour {
    public GameObject Firebrand;
    Animator anim;
    Rigidbody2D rbFirebrand;

    //forces
    float vx;
    float vy;
    float vxMax = 10;
    float vyMax = 30;
    
    //life and flight counters
    int cptFly = 0;
    int FirebrandLife = 5;
    
    //booleans
    bool isOnGround = false;
    bool isOnWall = false;
    bool isAttacking = false;
    bool isAttackingOnWall = false;
    bool isJumping = false;
    bool isWalking = false;
    bool isFlying = false;
    bool isIdle = false;
    bool isDead = false;
    bool isFalling = false;
    bool isHurt = false;
    
    //pour les objets
    public GameObject Sword;
    public GameObject LifePot;
    public GameObject Crane;
    public GameObject FireMissile;

    //sources audio
    public AudioClip SonChampi;
    public AudioClip SonPack;
    public AudioClip SonMort;

    // Use this for initialization
    void Start () {
        anim = Firebrand.GetComponentInChildren<Animator>();
        rbFirebrand = GetComponent<Rigidbody2D>();
	}

    // Update is called once per frame
    void Update() {

        //deplacement horizontal si on touche le sol
        if (Input.GetKey(KeyCode.LeftArrow)&& isOnGround == true)
        {
            vx = -vxMax;//vit. imposée à Firebrand
            GetComponentInChildren<SpriteRenderer>().flipX = true;
            
            isIdle = false;
            isJumping = false;
            isWalking = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow) && isOnGround == true)
        {
            vx = vxMax;//imposée à Firebrand
            GetComponentInChildren<SpriteRenderer>().flipX = false;
            
            isIdle = false;
            isJumping = false;
            isWalking = true;
        }


        //si on n'appuie sur aucune des touches ci-dessus
        else if (isOnGround == true)
        {
            vx = GetComponent<Rigidbody2D>().velocity.x;
            isWalking = false;
            isIdle = true;
        }


        //à n'importe quel moment, si on appuie sur les touches pour bouger horizontalement
        if (Input.GetKey(KeyCode.LeftArrow) && isDead == false)
        {
            vx = -vxMax;//vit. imposée à Firebrand

            //si il ne touche plus au mur, tourne le sprite
            if (isOnWall == false)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }

            //ces 2 if détectent si le personnage tombe ou saute, pour faire jouer la bonne animation
            if(isOnWall == true && (rbFirebrand.velocity.y < 0) && (rbFirebrand.velocity.x < -1) && GetComponentInChildren<SpriteRenderer>().flipX == true)
            {
                isOnWall = false;
                isFalling = true;
            }

            if(isOnWall == true && (rbFirebrand.velocity.y >= 0.3f) && (rbFirebrand.velocity.x < -1))
            {
                isOnWall = false;
                isJumping = true;
            }
        }
        else if (Input.GetKey(KeyCode.RightArrow) && isDead == false)
        {
            vx = vxMax;//imposée à Firebrand

            //si il ne touche plus au mur, tourne le sprite
            if (isOnWall == false)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }

            //ces 2 if détectent si le personnage tombe ou saute, pour faire jouer la bonne animation
            if (isOnWall == true && rbFirebrand.velocity.y < 0 && rbFirebrand.velocity.x > 1 && GetComponentInChildren<SpriteRenderer>().flipX == false)
            {
                isOnWall = false;
                isFalling = true;
            }

            if (isOnWall == true && rbFirebrand.velocity.y >= 0.3f && rbFirebrand.velocity.x > 1)
            {
                isOnWall = false;
                isJumping = true;
            }

        }


        //Saut avec fleche Haut ou touche W
        if (Input.GetKeyDown(KeyCode.UpArrow) && (isOnGround == true || isOnWall == true))
        {
            vy = vyMax;//appliquee pendant 1 frame seulement
            
            isIdle = false;
            isOnGround = false;
            isWalking = false;
            isJumping = true;
        }

        //fait voler le personnage s'il est deja dans les airs.
        else if (Input.GetKeyDown(KeyCode.UpArrow) && (isOnGround == false && cptFly < 6))
        {
            vy = 20;
            
            isFalling = false;
            isJumping = false;
            isIdle = false;
            isFlying = true;

            cptFly++;

        }

        else
        {
            vy = (GetComponent<Rigidbody2D>().velocity.y)-0.6f;
            //lecture velocity, on laisse la gravité agir avec un petit supplément
        }


        //sous le seuil de velocité y, l'animation Fall commence
        if (GetComponent<Rigidbody2D>().velocity.y < -8)
        {
            
            isIdle = false;
            isJumping = false;
            isFlying = false;
            isWalking = false;
            isFalling = true;
        }


        //si le compteur de vol atteint 6, le personnage tombe
        if (cptFly == 6 && isHurt == false)
        {
            
            isFlying = false;
            isIdle = false;
            isFalling = true;
        }

        //une grosse condition, en gros si la personne appuie sur la touche opposée du regard du personnage quand il est sur un mur, il ne peut pas sauter
        //cette condition empêche le joueur de bugger mes animations, nottament celles de "saut"/"vol" lorsqu'il est accroché au mur :)
        if(isOnWall == true && (Input.GetKey(KeyCode.UpArrow) && ((Input.GetKey(KeyCode.LeftArrow)&& GetComponentInChildren<SpriteRenderer>().flipX ==false) || (Input.GetKey(KeyCode.RightArrow) && GetComponentInChildren<SpriteRenderer>().flipX == true))))
        {
            vy = 0;
        }


        //AJOUTER POUR ATTAQUE
        if (Input.GetKeyDown(KeyCode.Space) && isOnWall == false)
        {
            
            isIdle = false;
            isWalking = false;
            isJumping = false;
            isFalling = false;
            isFlying = false;
            isFalling = false;
            isAttacking = true;

            //le perso n'attaque plus
            Invoke("stopAttack", 0.1f);

            //crée un missile
            GameObject FireClone = Instantiate(FireMissile);
            FireClone.SetActive(true);

            //if / else pour mettre le missile du bon bord du personnage
            if (GetComponentInChildren<SpriteRenderer>().flipX == false)
            {
                FireClone.transform.position = transform.position + new Vector3(1, 1, 0);

                FireClone.GetComponent<Rigidbody2D>().velocity = new Vector2(15, 0);
            }
            else
            {
                FireClone.transform.position = transform.position + new Vector3(-1, 1, 0);

                FireClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-15, 0);
            }
        }

        //Si le personnage attaque sur un mur
        if (Input.GetKeyDown(KeyCode.Space) && isOnWall == true)
        {
            
            isIdle = false;
            isWalking = false;
            isJumping = false;
            isFalling = false;
            isFlying = false;
            isFalling = false;
            isAttackingOnWall = true;

            //arrete d'attaquer
            Invoke("stopAttack", 0.1f);

            //crée un missile
            GameObject FireClone = Instantiate(FireMissile);
            FireClone.SetActive(true);

            //if / else pour changer la direction du missile par rapport au personnage
            if (GetComponentInChildren<SpriteRenderer>().flipX == false)
            {
                FireClone.transform.position = transform.position + new Vector3(1, 1, 0);

                FireClone.GetComponent<Rigidbody2D>().velocity = new Vector2(15, 0);
            }
            else
            {
             

                FireClone.transform.position = transform.position + new Vector3(-1, 1, 0);

                FireClone.GetComponent<Rigidbody2D>().velocity = new Vector2(-15, 0);

                FireClone.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        

        //velocité du personnage devient celle définie par les touches (ou par la gravité)
        rbFirebrand.velocity = new Vector2(vx, vy);



        //Si la vie du personnage est à 0, il meurt
        if (FirebrandLife == 0)
        {
            
            isIdle = false;
            isWalking = false;
            isJumping = false;
            isFalling = false;
            isFlying = false;
            isFalling = false;
            isAttacking = false;
            isDead = true;

            //il arrête de bouger et est constraint partout
            rbFirebrand.velocity = new Vector2(0, 0);
            rbFirebrand.constraints = RigidbodyConstraints2D.FreezeAll;

            //et il crie
            GetComponent<AudioSource>().PlayOneShot(SonMort, .5f);

            //et l'écran de mort s'affiche
            Invoke("Restart", 3f);

        }

        //calibrage des booléens
        anim.SetBool("isIdle", isIdle);
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isJumping", isJumping);
        anim.SetBool("isFlying", isFlying);
        anim.SetBool("isDead", isDead);
        anim.SetBool("isFalling", isFalling);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetBool("isHurt", isHurt);
        anim.SetBool("isOnWall", isOnWall);
        anim.SetBool("isAttackingOnWall", isAttackingOnWall);
    }    
    
    //tests de collision
    //les SetBool sont inclus ici aussi pour que la réaction soit instantanée (pas d'attente après update)
    private void OnCollisionEnter2D(Collision2D autreObjet)
    {
        //si le personnage touche le sol, il est Idle et son compteur de vol est redémarré
        if ((autreObjet.gameObject.tag == "Floor"||autreObjet.gameObject.tag == "PlateformeBouge") && isDead == false)
        {
            
            isJumping = false;
            isFalling = false;
            isFlying = false;
            isOnWall = false;
            isOnGround = true;
            isIdle = true;
            cptFly = 0;
            
        }

        //si il touche à un mur, son animation d'accrocher au mur embarque, le compteur de vol est réinitialisé et on flip le sprite
        if (autreObjet.gameObject.tag == "Wall" && isDead == false)
        {
            isOnGround = false;
            isJumping = false;
            isIdle = false;
            isWalking = false;
            isFalling = false;
            isFlying = false;
            isOnWall = true;

            anim.SetBool("isOnWall", isOnWall);
            anim.SetBool("isWalking", isWalking);
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isIdle", isIdle);
            anim.SetBool("isFalling", isFalling);
            anim.SetBool("isFlying", isFlying);
            
            cptFly = 0;
            
            if(vx >= 0)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = true;
            }

            if(vx <= 0)
            {
                GetComponentInChildren<SpriteRenderer>().flipX = false;
            }
        }
        
        //Si le personnage touche la plateforme truquée, il doit attendre sa mort :)
        if(autreObjet.gameObject.tag == "PlateformPiege")
        {
            //compteur de battements d'ailes au maximum, il ne peut plus voler
            cptFly = 6;
        }
            
        //Si le personnage touche les piques, il est très mort
        if (autreObjet.gameObject.tag == "Spikes")
        {
                ScriptVie.FirebrandLife = 0;
                FirebrandLife = 0;
                cptFly = 0;
                
                isOnGround = false;
                isOnWall = false;
                isIdle = false;
                isJumping = false;
                isFalling = false;
                isFlying = false;
                isWalking = false;
                isAttacking = false;
                isAttackingOnWall = false;
                isDead = true;
            
            //je change les booléens de l'animator ici pour que ce soit plus "responsive"
                //sinon la transition se fait mal entre les animations
            anim.SetBool("isOnWall", isOnWall);
            anim.SetBool("isIdle", isIdle);
            anim.SetBool("isJumping", isJumping);
            anim.SetBool("isFalling", isFalling);
            anim.SetBool("isFlying", isFlying);
            anim.SetBool("isWalking", isWalking);
            anim.SetBool("isAttacking", isAttacking);
            anim.SetBool("isAttackingOnWall", isAttackingOnWall);
            anim.SetBool("isDead", isDead);

            //on gèle tout
            rbFirebrand.velocity = new Vector2(0,0);
            rbFirebrand.constraints = RigidbodyConstraints2D.FreezeAll;

            //et on repart à la scene Intro
            Invoke("Restart", 4f);
            
        }

        //Si on touche la potion
        if (autreObjet.collider.name == "LifePot")
        {
            //Guérit Firebrand
            if (FirebrandLife < 5)
            {
                FirebrandLife = 5;
                ScriptVie.FirebrandLife = 5;
            }

            //Enlève la potion
            autreObjet.gameObject.SetActive(false);

            //réactive la potion apres 7 secondes
            Invoke("ActiveLifePot", 7f);

            //jou le son de la potion (oui elle est pas mal sèche)
            GetComponent<AudioSource>().PlayOneShot(SonPack, .5f);
        }

        //Si on touche l'épée, on a mal, on perd des points
        if (autreObjet.collider.name == "Sword")
        {
            

                FirebrandLife--;
                ScriptVie.FirebrandLife--;
                

                if (ScriptScore.score <= 10)
                {
                    ScriptScore.score = 0;
                }
                else
                {
                    ScriptScore.score -= 10;
                }


                
                isIdle = false;
                isOnGround = false;
                isJumping = false;
                isFalling = false;
                isFlying = false;
                isWalking = false;
                isAttacking = false;
                isAttackingOnWall = false;
                isHurt = true;

                anim.SetBool("isIdle", isIdle);
                anim.SetBool("isJumping", isJumping);
                anim.SetBool("isFalling", isFalling);
                anim.SetBool("isFlying", isFlying);
                anim.SetBool("isWalking", isWalking);
                anim.SetBool("isAttacking", isAttacking);
                anim.SetBool("isAttackingOnWall", isAttackingOnWall);
                anim.SetBool("isHurt", isHurt);

                //fait bondir le personnage pour reinitialliser IsOnGround (debug)
                rbFirebrand.velocity = new Vector2(0, 10);
                
                //n'a plus mal
                Invoke("unHurt", 0.3f);
                
                //jou un son
                GetComponent<AudioSource>().PlayOneShot(SonChampi, .5f);
            
        }

        //si on touche un ennemi, on a mal, on perd des points et l'ennemi disparaît
        if (autreObjet.collider.tag == "Ennemi")
        {

                Destroy(autreObjet.gameObject);

                FirebrandLife--;
                ScriptVie.FirebrandLife--;
                

                if (ScriptScore.score <= 10)
                {
                    ScriptScore.score = 0;
                }
                else
                {
                    ScriptScore.score -= 10;
                }


                
                isIdle = false;
                isOnGround = false;
                isJumping = false;
                isFalling = false;
                isFlying = false;
                isWalking = false;
                isAttacking = false;
                isAttackingOnWall = false;
                isHurt = true;

                anim.SetBool("isIdle", isIdle);
                anim.SetBool("isJumping", isJumping);
                anim.SetBool("isFalling", isFalling);
                anim.SetBool("isFlying", isFlying);
                anim.SetBool("isWalking", isWalking);
                anim.SetBool("isAttacking", isAttacking);
                anim.SetBool("isAttackingOnWall", isAttackingOnWall);
                anim.SetBool("isHurt", isHurt);

                //fait bondir le personnage pour reinitialliser IsOnGround (debug)
                rbFirebrand.velocity = new Vector2(0, 10);

                //on arrête d'avoir mal
                Invoke("unHurt", 0.3f);

                //jou un son
                GetComponent<AudioSource>().PlayOneShot(SonChampi, .5f);
            
        }

        //Si on prend un crane, on gagne des points
        if (autreObjet.collider.tag == "Crane")
        {
            ScriptScore.score += 100;
            autreObjet.gameObject.SetActive(false);
        }

        //Si on va sur une plateforme, le démon devient enfant de la plateforme
        if(autreObjet.collider.tag == "PlateformeBouge")
        {
            transform.parent = autreObjet.gameObject.transform;
        }
        }



    //quand on quitte la-dite plateforme, le démon devient orphelin :'(    (pas de parent)
    void OnCollisionExit2D(Collision2D autreObjet)
    {
        if (autreObjet.collider.tag == "PlateformeBouge")
        {
            transform.parent = null;
        }
    }

    //fonction Réactive l'épée
    void ActiveSword()
    {
        Sword.SetActive(true);
            }

    //fonction Réactive la potion
    void ActiveLifePot()
    {
        LifePot.SetActive(true);
    }

    //fonction repars le jeu
    void Restart()
    {
        ScriptVie.FirebrandLife = 5;
       
    SceneManager.LoadScene("Mort");
    }

    //fonction t'as pas mal
    void unHurt()
    {
        isHurt = false;
        isIdle = true;
    }

    //fonction attaque pas
    void stopAttack()
    {
        isAttacking = false;
        isAttackingOnWall = false;
    }


}
