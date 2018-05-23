using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    //how many points the player gets every time it saves the ball
    public int pointsToGrant;
    //power up prefab
    public GameObject powerUpPrefab;
    //power up list
    public List<string> powerUpList;

    //score
    private int score;
    //canvas text
    private Text scoreText;
    //time counter
    private float timeCounter;
    //time counter for reset power up
    private float timeCounterReset;

    private void Awake()
    {
        //start zeroed
        score = 0;
        timeCounter = timeCounterReset = 0;
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {
        //QualitySettings.antiAliasing = 80;
	}
	
	// Update is called once per frame
	void Update () {
        //each 5 seconds, pop up a power up
        timeCounter += Time.deltaTime;

        if(timeCounter >= 2)
        {
            //reset the time counter
            timeCounter = 0;

            //pop up a new power up
            GameObject newPowerUp = Instantiate(powerUpPrefab, new Vector3(Random.Range(-14, 14), 0, Random.Range(-9, 9)), Quaternion.identity);

            //set the type as to make paddles bigger
            newPowerUp.GetComponent<PowerUp>().powerUpType = powerUpList[Random.Range(0, powerUpList.Count)];
        }

        //if timer is set, score returns to normal after 5 seconds
        if (timeCounterReset > 0)
        {
            timeCounterReset += Time.deltaTime;
            if (timeCounterReset >= 5)
            {
                timeCounterReset = 0;
                ResetScore();
            }
        }
    }

    //update the score
    public void UpdateScore()
    {
        score += pointsToGrant;

        //update text in canvas
        scoreText.text = "Score: " + score.ToString();
    }

    //timer to reset score to original value
    public void StartTimer()
    {
        timeCounterReset = Time.deltaTime;
    }

    private void ResetScore()
    {
        pointsToGrant = 10;
    }

    //getters and setters
    public int GetScore()
    {
        return score;
    }
    public void SetScore(int value)
    {
        score = value;
    }
}
