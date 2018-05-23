using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaycasterBall : MonoBehaviour {

    //velocity vector
    public Vector3 velocity;

    //Game Manager script reference
    private GameManager gm;

    Ray r;
    RaycastHit rh;

    private void Awake()
    {
        //get the game manager reference
        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
    }

    // Use this for initialization
    void Start () {
        r = new Ray();
        rh = new RaycastHit();
	}

    private void OnTriggerExit(Collider other)
    {
        //if leaves the boundary collider, game over
        if (other.gameObject.tag == "Boundary")
        {
            //Debug.Log("Game Over!");
            //reset the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if it is some powerup
        if(other.gameObject.tag == "PowerUp")
        {
            other.gameObject.GetComponent<PowerUp>().DoStuff();
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        //ball position
        r.origin = transform.position;
        //direction ball is going, normalized
        r.direction = velocity.normalized;

        //if the ball is colliding with the borders or paddles
        if (Physics.Raycast(r, out rh, velocity.magnitude * Time.fixedDeltaTime + 0.01f))
        {
            //ball is in the impact point
            transform.position = rh.point;
            
            //reflect the velocity vector
            velocity = Vector3.Reflect(velocity, rh.normal);

            //update score if colliding with paddles
            if (rh.transform.gameObject.tag == "Paddle")
            {
                gm.UpdateScore();
            }
        }//otherwise, just keep moving
        else
        {
            transform.position += velocity * Time.fixedDeltaTime;
        }
    }
}
