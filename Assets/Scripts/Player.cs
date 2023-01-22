using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;
using UnityEngine.Audio;
public class Player : MonoBehaviour
{
    //Private veriable for controls
    private InputManeger controls;
    //Private veriable for animator
    private Animator animator;
    //Public veriable for speed mutiplyer
    public float speed;
    //Public veriable for bullets prefab
    public GameObject bullets;
    //Veriable to store direction
    private float dir = -1;
    //Bool set if player is exploding
    bool playerExploding = false;
    //Stores game maneger script
    GameManeger manegerScript;
    //Stores auido source
    public AudioSource playerAudio;
    //Stores shooting audio clip
    public AudioClip shootAudio;
    //Stores explosion audio clip
    public AudioClip explosionAudio;

    void Awake(){
        //Gets the controls from the manager and sets them as controls
        controls = new InputManeger();
        //Gets animator
        animator = this.GetComponent<Animator>();
        //Set game maneger script
        manegerScript = GameObject.FindGameObjectWithTag("Game Maneger").GetComponent<GameManeger>();
        //Set auido manager
        AudioSource playerAudio = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        //if not exploding allow player to move
        if(!playerExploding) move();
    }

    //Function that handles all player movement
    void move(){
        //reads movement vector value
        Vector2 movement = controls.Player.Movement.ReadValue<Vector2>();
        //set dir acording to horizontal direction and change animator state
        if(movement.x > 0) {
            dir = 1;
            animator.SetBool("FacingLeft", false);
        }
        if(movement.x < 0) {
            dir = -1;
            animator.SetBool("FacingLeft", true);
        }
        //asigns vector value to seprate veriables after smoothing it by time and multipling it by speed
        float horizontalMove = + movement.x * Time.deltaTime * speed ;
        float vertiacalMove = movement.y * Time.deltaTime * speed;
        //moves player by translating its position and multiply horizontal
        this.transform.Translate(horizontalMove, vertiacalMove, 0);
        //move object forward in correct direction
        this.transform.Translate(dir * Time.deltaTime, 0 ,0);       
    }

    //Fuction that is called when shoot binding is presed
    void OnShoot(){
        //Play sound
        playerAudio.PlayOneShot(shootAudio);
        //Instantiate correct bullets based on player direction and set direction
        if (dir == -1) {
            //Set spawn for bullets
            Vector3 tempVector = new Vector3(this.transform.position.x - .75f, this.transform.position.y - 0.05f, 0);
            //Create bullets
            Bullets bulletObj = Instantiate(bullets.gameObject, tempVector, this.transform.rotation).GetComponent<Bullets>();
            //Set bullets direction
            bulletObj.SetDirection(dir);
        }
        if (dir == 1) {
            //Set spawn for bullets
            Vector3 tempVector = new Vector3(this.transform.position.x + .75f, this.transform.position.y - 0.05f, 0);
            //Create bullets
            Bullets bulletObj = Instantiate(bullets.gameObject, tempVector, this.transform.rotation).GetComponent<Bullets>();
            //Set bullets direction
            bulletObj.SetDirection(dir);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        //play explosion and disable sprite
        if(col.tag.Equals("Enemy") || col.tag.Equals("Spawner")) {
            //Set explosion bool
            playerExploding = true;
            //start particle system
            this.GetComponent<ParticleSystem>().Play();
            //Disable sprite
            this.GetComponent<SpriteRenderer>().enabled = false;
            //Play explosion sfx
            playerAudio.PlayOneShot(explosionAudio);
            //End the game
            manegerScript.EndGame(false);
        }
    }

    //Call back function from particle sytem when sytem ends
    void OnParticleSystemStopped() {
        Destroy(this.gameObject);
    }

    //enables and dissable conrols with this script
    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }
}
