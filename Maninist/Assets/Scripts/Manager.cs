using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//This is the manager for the game
public class Manager : MonoBehaviour {
    public static Manager current;          //A public static reference to itself (make's it visible to other objects without a reference)
    public GameObject ball;                 //The ball
    public Text countDown;                  //The game object containing the countdown
    public Text score1Text;                 //The score for player1 text
    public Text score2Text;                 //The score for player2 text

    int score1;                              //The player 1's score
    int score2;                              //The player 2's score


    void Awake() {
        //Ensure that there is only one manager
        if (current == null)
            current = this;
        else
            Destroy(gameObject);
    }

    void Start() {
        ball.SetActive(false);
        Initialize();
    }

    void Update() {
        if (countDown.GetComponent<myTimer>().myCoolTimer < 0) {
            countDown.text = "";
            GameStart();
        }

        //Set the GUI to relfect the current score and high score
        score1Text.text = score1.ToString();
        score2Text.text = score2.ToString();
    }

    void GameStart() {
        //Deactivate the title and activate the player
        //countDown.enabled = false;
        ball.SetActive(true);
    }

    public void RoundEnded() {
        ball.SetActive(false);
        countDown.GetComponent<myTimer>().myCoolTimer = 3;
    }

    public void GameOver() {
        //Call the save method
        ReInitialize();
        //Activate the title
        countDown.enabled = true;
    }

    public bool IsPlaying() {

        //if the title is active, then the player isn't playing
        return countDown.isActiveAndEnabled == false;
    }

    private void Initialize() {
        //Reset the score and get the high score from the playerprefs
        score1 = 0;
        score2 = 0;
    }

    public void AddPointToPlayer1(int point) {
        //Add points to the player's score
        score1 += point;
    }

    public void AddPointToPlayer2(int point) {
        //Add points to the player's score
        score2 += point;
    }

    public void ReInitialize() {
        //Re initialize the score
        Initialize();
    }
}