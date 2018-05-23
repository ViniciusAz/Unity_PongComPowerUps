using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acelerador : MonoBehaviour {

    public float increasedSpeed = 1;
    public ParticleSystem effect;

    private void OnTriggerEnter(Collider other)
    {
        RaycasterBall b = other.GetComponent<RaycasterBall>();
        if (b != null)
        {
            b.velocity += b.velocity.normalized * increasedSpeed;
            effect.transform.position = b.transform.position;
            effect.Play();
        }

        PhysicBall pb = other.GetComponent<PhysicBall>();
        if (pb != null)
        {
            var r = pb.GetComponent<Rigidbody>();
            r.AddForce(r.velocity.normalized * increasedSpeed, ForceMode.Impulse);
            effect.transform.position = pb.transform.position;
            effect.Play();
        }
    }
    
}
