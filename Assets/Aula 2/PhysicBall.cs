using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhysicBall : MonoBehaviour {

    public Vector3 startForce;
    public ForceMode mode;
    private GameManager gm;
    // Use this for initialization
    void Start () {
        gm = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameManager>();
        GetComponent<Rigidbody>().AddForce(startForce, mode);
	}
    private void OnTriggerExit(Collider other)
    {
        //if leaves the boundary collider, game over
        if (other.gameObject.tag == "Boundary")
        {
            //Debug.Log("Game Over!");
            //reset the scene
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transform.position = Vector3.zero;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if it is some powerup
        if (other.gameObject.tag == "PowerUp")
        {
            other.gameObject.GetComponent<PowerUp>().DoStuff();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.tag == "Paddle")
        {
            gm.UpdateScore();
        }
    }
}
