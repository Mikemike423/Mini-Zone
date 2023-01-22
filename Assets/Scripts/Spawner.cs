using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;
using UnityEngine.Animations;

public class Spawner : MonoBehaviour
{
    //Stores enemy prefab
    public GameObject enemyPrefab;
    //Stores cooldown timer between spawns
    private float coolDownTimer;
    //Stores direction to spawn enemeies in
    private float spawnerDir;
    //Bool for if the spawner is exploding
    bool spawnerExploding = false;
    //Stores animator
    Animator spawnerAnim;
    //Stores hits taken 
    int hitsTaken;
    //Stores game maneger script
    GameManeger manegerScript;
    //Stores auido source
    public AudioSource spawnerAudio;
    //Stores explosion audio clip
    public AudioClip explosionAudio;

    void Awake(){
        //Set up animator and hits taken veriable
        spawnerAnim = this.GetComponent<Animator>();
        hitsTaken = spawnerAnim.GetInteger("Hit Counter");
        //Set game maneger script
        manegerScript = GameObject.FindGameObjectWithTag("Game Maneger").GetComponent<GameManeger>();
    }

    void FixedUpdate()
    {
        if(!spawnerExploding) {
            coolDownTimer -= Time.deltaTime;
            if(coolDownTimer <= 0){
                SpawnEnimies();
            }
        }
    }

    //Function to spawn enimies and generate a random cool down timer
    void SpawnEnimies() {
        //set timer
        coolDownTimer = Random.Range(0,3);
        //Spawn enemy and get script
        Enemy enemy = Instantiate(enemyPrefab, this.transform.position, this.transform.rotation).GetComponent<Enemy>();
        //Set enemy direction based on location of spawner
        if(this.transform.position.x < 0) enemy.SetHorizontalDir(1);
        if(this.transform.position.x > 0) enemy.SetHorizontalDir(-1);
    }

    //Function called by bullets to take health or destroy the spawner
    public void takeHealth() {
        //incrament amount of hits taken
        hitsTaken++;
        //if the hits taken start explosion proces
        if(hitsTaken >= 3) {
            //play explosion
            this.GetComponent<ParticleSystem>().Play();
            //disable spawning
            spawnerExploding = true;
            //Play sxf
            spawnerAudio.PlayOneShot(explosionAudio);
            //disable sprite
            this.GetComponent<SpriteRenderer>().enabled = false;
            //disable colider
            this.GetComponent<BoxCollider2D>().enabled = false;
            //update counter
            manegerScript.decramentSpawnerCounter();
        }
        //set the hit counter on the animator to be equal to the amount of hits taken
        spawnerAnim.SetInteger("Hit Counter", hitsTaken);
    }

    //Call back function from particle sytem when sytem ends
    void OnParticleSystemStopped() {
        Destroy(this.gameObject);
    }

}
