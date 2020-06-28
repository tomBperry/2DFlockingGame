using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : MonoBehaviour
{
    // Referencing the rigid body to which the script is attatched
    public Rigidbody rb;

    public float repelStrength = 10f;
    public float repelDistance = 15f;

    // Making a list of all other Boids
    public static List<Flocker> Flockers;

    void FixedUpdate()
    {
        foreach (Flocker flocker in Flockers)
        {
            if (flocker != this)
                Flock(flocker);
        }

    }

    void OnEnable ()
    {
        if (Flockers == null)
            Flockers = new List<Flocker>();

        Flockers.Add(this);
    }

    void OnDisable ()
    {
        Flockers.Remove(this);
    }


    void Flock (Flocker objToFlock)
    {
        Attract(objToFlock);
        Repel(objToFlock);
               
    }

    // Attracting force
    void Attract(Flocker objToFlock)
    {
        Rigidbody rbToFlock = objToFlock.rb;

        Vector2 direction = rb.position - rbToFlock.position;
        float distance = direction.magnitude;

        if (distance == 0f)
            return;

        // Newtonian gravity for POC
        float forceMagnitude = (rb.mass * rbToFlock.mass) / Mathf.Pow(distance, 2);
        Vector2 force = direction.normalized * forceMagnitude;

        rbToFlock.AddForce(force);

    }
    // Repelling force
    void Repel(Flocker objToFlock)
    {
         Rigidbody rbToFlock = objToFlock.rb;

        Vector2 direction = rb.position - rbToFlock.position;
        float distance = direction.magnitude;

        if (distance < repelDistance)
        {
            Vector2 force = direction.normalized * repelStrength;

            rbToFlock.AddForce(force);
        }
    }

    // Alligning force
    // void Allign(Flocker objToFlock)
    // {
        
    // }
}
