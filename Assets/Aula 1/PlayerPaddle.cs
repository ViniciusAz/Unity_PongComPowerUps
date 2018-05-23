using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour {

    public float speed = 2;

    //default speed
    private float defaultSpeed;
    //direction
    private float dir;
    //time counter
    private float timeCounter;

    private void Awake()
    {
        defaultSpeed = speed;
        timeCounter = 0;
    }

    private void Update()
    {
        dir = Input.GetAxis("Vertical");

        //reset speed
        speed = defaultSpeed;
        
        //get the collider around the paddle
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.z / 2);
        //for each collider found
        foreach (Collider col in colliders)
        {
            //if it is Border Top and it is going up, stop
            if (col.gameObject.name == "Border Top" && dir > 0)
            {
                speed = 0;
                break;
            }

            //if it is Border Bottom and it is going down, stop
            if (col.gameObject.name == "Border Bottom" && dir < 0)
            {
                speed = 0;
                break;
            }
        }

        //if timer is set, paddle returns to normal after 5 seconds
        if(timeCounter > 0)
        {
            timeCounter += Time.deltaTime;
            if(timeCounter >= 5)
            {
                timeCounter = 0;
                ResetScale();
            }
        }
    }

    void FixedUpdate()
    {
        transform.position += Vector3.forward * speed * dir * Time.fixedDeltaTime;
    }

    //timer to reset paddle to original size
    public void StartTimer()
    {
        timeCounter = Time.deltaTime;
    }

    private void ResetScale()
    {
        transform.localScale = new Vector3(1, 1, 3);
    }
}
