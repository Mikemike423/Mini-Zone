using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManeger : MonoBehaviour
{
    //Game objects
        //Veriable for controls
        private InputManeger controls;
        //Player spawn point
        public GameObject playerSpawn;
        //Player prefab
        public GameObject playerPrefab;
        //Player object
        private GameObject playerObj;
        //Spawner object
        public GameObject spawner;
        //Stores sand object
        public GameObject sand;
        //Array of posible spawner spawn locations
        public GameObject[] spawnerSpawns = new GameObject[6];
    //Game Logic Veriables
        //Counts amount of spawners active
        int spawnerCounter = 2;
        //True if game has started
        bool gameStarted = false;
        //True if game has ended
        bool gameFinished = false;
    //Timer Veriables
        //Time limit for game
        float timeLimit = 10;
        //Actual timer
        float curTime = 0;
    //Sound
        //Audio sources for music and sfx
        public AudioSource musicPlayer;
        //Audio clips
        public AudioClip startMusic;
        public AudioClip gameMusic;
        public AudioClip loseMusic;
        public AudioClip winMuisc;
    //Ui elements
        //Timer text elemant
        public TMP_Text timerText;
        //Spawners remaning text element
        public TMP_Text spawnerCounterText;
        //Start panel element
        public GameObject startPanel;
        //Game panel element
        public GameObject gamePanel;
        //Win panel element
        public GameObject winPanel;
        //Lose panel element
        public GameObject losePanel;

    void Awake()
    {
        //Gets the controls from the manager and sets them as controls
        controls = new InputManeger();
    }

    //Sets up game
    void StartGame(){
        //Set game started bool
        gameStarted = true;
        //Enable game panel
        gamePanel.SetActive(true);
        //Disable start panel
        startPanel.SetActive(false);
        //Spawn player
        playerObj = Instantiate(playerPrefab, playerSpawn.transform.position, this.transform.rotation);
        //Span Spawners
        SpawnSpawners();
        //Set curent time to time limit
        curTime = timeLimit;
        //Set timer text to time limit
        timerText.text = timeLimit.ToString();
        //Start game music
        musicPlayer.clip = gameMusic;
        musicPlayer.Play();
        //Start hourglass
        StartCoroutine(RunHourglass());
    }

    //Ends the game; true is won, false is lost
    public void EndGame(bool winstate){
        //destroy player if still alive
        if(playerObj!=null) Destroy(playerObj);
        //Disable game panel
        gamePanel.SetActive(false);
        //Set finshed state true
        gameFinished = true;
        //Set audio player to no longer loop and stop playing
        musicPlayer.loop = false;
        musicPlayer.Stop();
        //If game ends in win state
        if(winstate){
            winPanel.SetActive(true);
            musicPlayer.PlayOneShot(winMuisc);
        }
        //If game ends in loose state
        else if(!winstate){
            losePanel.SetActive(true);
            musicPlayer.PlayOneShot(loseMusic);
        }
    }

    //Spawns Spawners at two disctintct points from the list of posible points
    void SpawnSpawners(){
        //Set up veriables
        int spawn1;
        int spawn2;
        //Generate 2 distinct spawn numbers
        do {
            spawn1 = Random.Range(0,6);
            spawn2 = Random.Range(0,6);
        } while(spawn1==spawn2);
        //Instatiate spawners
        Instantiate(spawner, spawnerSpawns[spawn1].transform.position, this.transform.rotation);
        Instantiate(spawner, spawnerSpawns[spawn2].transform.position, this.transform.rotation);
    }

    //Corutine to move sand one unit at each second
    IEnumerator RunHourglass(){
        //Waits for seccond to pass
        yield return new WaitForSeconds(1.0f);
        //Update timer number
        curTime--;
        //Update timer text
        timerText.text = curTime.ToString();
        //Update sand every second
        sand.transform.Translate(0, -.5f, 0);
        if(curTime > 0) yield return StartCoroutine(RunHourglass());
        //Destroy player then end game
        else if (!gameFinished){
            Destroy(playerObj);
            EndGame(false);
        }
    }

    //Called by other classes to decrament the spawner counter and update text element as well as end game when all spawners are gone
    public void decramentSpawnerCounter(){
        spawnerCounter--;
        spawnerCounterText.text = spawnerCounter + "  X" ; 
        if(spawnerCounter <= 0) EndGame(true);
    }

    //Called when confirm button is pressed
    void OnConfirm(){
        //Starts the game if game hasnt started
        if(!gameStarted) StartGame();
        //Restarts the game if game has finished
        if(gameFinished) {
            SceneManager.LoadScene("Main Scene");
        }
    }

    //Called when escape key is presed
    void OnQuit(){
        Application.Quit();
    }

    private void OnEnable(){
        controls.Enable();
    }

    private void OnDisable(){
        controls.Disable();
    }

}
