using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Sprites;

public class Bullets : MonoBehaviour
{
    //Stores speed of bullet
    public int speed;
    //Stores direction of bullet
    private float dir;
    //Stores sprites
    public Sprite leftBullets;
    public Sprite rightBullets;

    void FixedUpdate()
    {
        //move object forward in correct direction
        this.transform.Translate(dir * speed * Time.deltaTime, 0 ,0);
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.tag.Equals("Enemy") || col.tag.Equals("Wall")) Destroy(this.gameObject);
        else if(col.tag.Equals("Spawner")) {
            //Take health from the spawner
            col.gameObject.GetComponent<Spawner>().takeHealth(); 
            //Destory the bullets
            Destroy(this.gameObject);
            }
    }

    //Function acsessed by player to set direction and sets sprite acordingly
    public void SetDirection(float direction){
        //set sprite based on direction
        if(direction == -1) { 
            dir = -1;
            GetComponent<SpriteRenderer>().sprite = leftBullets; 
        }
        if(direction == 1) {
            dir = 1;
            GetComponent<SpriteRenderer>().sprite = rightBullets;
        }
    }
}
