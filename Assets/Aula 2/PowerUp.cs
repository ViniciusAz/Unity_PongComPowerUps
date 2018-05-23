using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {

    //which kind of power up it is
    public string powerUpType;

    //time counter
    private float timeCounter;

    private void Awake()
    {
        timeCounter = 0;
    }

    // Update is called once per frame
    void Update () {
        //power up leaves after 5 seconds
        timeCounter += Time.deltaTime;

        if(timeCounter >= 5)
        {
            DestroyPowerUp();
        }
	}

    //do stuff according the powerup type
    public void DoStuff()
    {
        //if type is paddles
        if(powerUpType == "paddles")
        {
            //bigger paddles!
            BiggerPaddles();
        }else if (powerUpType == "score")
        {
            //double score!
            DoubleScore();
        }
    }

    //make the paddles bigger
    private void BiggerPaddles()
    {
        //find the paddles
        GameObject[] paddles = GameObject.FindGameObjectsWithTag("Paddle");

        //for each of them
        foreach(GameObject pad in paddles)
        {
            //change the z scale
            pad.transform.localScale = new Vector3(1, 1, 6);

            //start the paddle timer
            pad.GetComponent<PlayerPaddle>().StartTimer();
        }

        //power up done, destroy this object
        DestroyPowerUp();
    }

    //double the score to grant
    private void DoubleScore()
    {
        //find the game manager
        GameManager gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();

        //change the point to grant
        gm.pointsToGrant = 20;

        //start the timer
        gm.StartTimer();

        //power up done, destroy this object
        DestroyPowerUp();
    }

    //just destroy this
    private void DestroyPowerUp()
    {
        Destroy(gameObject);
    }
}
