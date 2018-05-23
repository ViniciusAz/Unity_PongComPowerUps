using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPaddle : MonoBehaviour {

    public Transform ball;
    public float speed = 2;

	void FixedUpdate ()
    {
        Vector3 dir = Vector3.zero;
        if (ball.position.z > transform.position.z)
            dir = Vector3.forward * speed * Time.fixedDeltaTime;

        if (ball.position.z < transform.position.z)
            dir = -Vector3.forward * speed * Time.fixedDeltaTime;



        //get the collider around the paddle
        Collider[] colliders = Physics.OverlapSphere(transform.position, transform.localScale.z / 2);
        //for each collider found
        foreach (Collider col in colliders)
        {
            //if it is Border Top and it is going up, stop
            if (col.gameObject.name == "Border Top" && dir.z > 0)
            {
                dir = Vector3.zero;
                break;
            }

            //if it is Border Bottom and it is going down, stop
            if (col.gameObject.name == "Border Bottom" && dir.z < 0)
            {
                dir = Vector3.zero;
                break;
            }
        }


        transform.position += dir;
    }
}
