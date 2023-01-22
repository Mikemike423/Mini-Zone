using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;

public class Enemy : MonoBehaviour
{
    //Stores speed of Enemy
    public int speed;
    //Stores which direction enemy is facing
    private float horizontalDir;
    //Stores which direction enemy heads during movement
    private float verticalDir;
    //Stores length of time enemey moves in generated direction
    private float moveTime;
    //Bool for if the enemy is exploding
    bool enemyExploding = false;
    //Stores auido source
    public AudioSource enemyAudio;
    //Stores explosion audio clip
    public AudioClip explosionAudio;

    void Awake(){
        //Generate starting movement pattern
        GenMovePattern();
    }

    void FixedUpdate()
    {
        //Move enemy once per interval if not exploding
        if(!enemyExploding) MoveEnemy();
    }

    //Function to handle enemy movement
    void MoveEnemy(){
        //move object in generated direction for generated amount of time
        this.transform.Translate(horizontalDir * speed * Time.deltaTime, verticalDir * speed * Time.deltaTime, 0);
        //Count down timer
        moveTime -= Time.deltaTime;
        //If countdown complete Generate new movement pattern
        if(moveTime <= 0) GenMovePattern();
    }

    //Function Called to generate movement pattern 
    void GenMovePattern(){
        //Generate time to move in direction
        moveTime = Random.Range(0.1f,1f);
        //Generate vertical movement direction
        verticalDir = Random.Range(-3,3);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag.Equals("Wall")) Destroy(this.gameObject);
        if(col.tag.Equals("Bullet")) {
            //Set exploding booleen
            enemyExploding = true;
            //play sfx
            enemyAudio.PlayOneShot(explosionAudio);
            //Turn off collider
            this.GetComponent<BoxCollider2D>().enabled = false;
            //Remove sprite
            this.GetComponent<SpriteRenderer>().enabled = false;
            //start particle system
            this.GetComponent<ParticleSystem>().Play();
        }
    }

    //Called when paritcle sytem ends
    void OnParticleSystemStopped() {
        Destroy(this.gameObject);
    }

    //Method called by spawner to determine which direction the enemy heads in
    public void SetHorizontalDir(float dir){
        horizontalDir = dir;
    }
}
