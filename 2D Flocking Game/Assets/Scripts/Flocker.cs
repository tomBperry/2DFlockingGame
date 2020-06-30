using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker : MonoBehaviour
{
    // Referencing the rigid body to which the script is attatched
    public Rigidbody2D rb;

    public float attractStrength = 1f;
    public float repelStrength = 1f;
    public float repelDistance = 10f;
    public float allignDistance = 15f;

    public float allignStrength = 1f;

    public float maxSpeed = 1f;

    public  Vector2 localVeloicty;

    public bool variablesSet = false;
    

    

    // Making a list of all other Boids
    public static List<Flocker> Flockers;
    // Use this to use the list of N objects made in Generator
        // go GameObject
    //go.GetComponent<Scriptname>().listname; 

    void start()
    {
        // GameObject GeneratorObject = GameObject.Find("GeneratorObject");
        // int N = Generator.N;//GeneratorObject.GetComponent<Generator>().N;
        
        if (!variablesSet)
        {
            attractStrength = attractStrength/Generator.N;
            Debug.Log(attractStrength);

            variablesSet = true;
        }

        Debug.Log(variablesSet);

    }

    void FixedUpdate()
    {
        Debug.Log(variablesSet);
        foreach (Flocker flocker in Flockers)
        {
            if (flocker != this)
                Flock(flocker);
        }

        LimitSpeed(rb);
    }

    void Update()
    {
        Vector2 moveDirection = rb.velocity;
        if (moveDirection != Vector2.zero) {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void OnEnable ()
    {
        if (Flockers == null)
        {
            Flockers = new List<Flocker>();
        }

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
        Allign(objToFlock);               
    }

    // Attracting force
    void Attract(Flocker objToFlock)
    {
        Rigidbody2D rbToFlock = objToFlock.rb;

        Vector2 direction = rb.position - rbToFlock.position;
        // float distance = direction.magnitude;

        // if (distance < repelDistance/3)
        //     return;

        // Newtonian gravity for POC
        float forceMagnitude = 1f;//Mathf.Pow(distance, -2);
        Vector2 force = direction.normalized * forceMagnitude * attractStrength;

        rbToFlock.AddForce(force);

    }

    // Repelling force
    void Repel(Flocker objToFlock)
    {
        Rigidbody2D rbToFlock = objToFlock.rb;

        Vector2 direction = rb.position - rbToFlock.position;

        // This vector -> scalar can be optimised
        float distance = magSquare(direction);//.magnitude;

        if (distance < repelDistance*repelDistance)
        {
            Vector2 force = -direction.normalized;

            rbToFlock.AddForce(force * repelStrength);
        }
    }

    // Alligning force
    void Allign(Flocker objToFlock)
    {
        
        Rigidbody2D rbToFlock = objToFlock.GetComponent<Rigidbody2D>();

        foreach (Flocker flocker in Flockers)
        {
            if (flocker != this)
            {
                Vector2 direction = rb.position - rbToFlock.position;

                // This vector -> scalar can be optimised
                float distance = magSquare(direction);//.magnitude;

                if (distance < allignDistance*allignDistance)
                    localVeloicty = localVeloicty + rbToFlock.velocity;
            }
        }

        localVeloicty = localVeloicty.normalized;
        localVeloicty = localVeloicty*maxSpeed;

        Vector2 localVelocityDifference = localVeloicty - rb.velocity;

        Vector2 allignForce = localVelocityDifference*allignStrength;
        // Debug.Log(allignForce);

        rbToFlock.AddForce(allignForce);
        
    }

    void LimitSpeed(Rigidbody2D _rb)
    {
        _rb.velocity = _rb.velocity.normalized;
        _rb.velocity = _rb.velocity*maxSpeed;
        // Debug.Log(_rb.velocity);

        // if(magSquare(_rb.velocity) > maxSpeed*maxSpeed)
        // {
        //     //smoothness of the slowdown is controlled by the 0.99f, 
        //     //0.5f is less smooth, 0.9999f is more smooth
        //     _rb.velocity *= 0.95f;
        // }
    }

    float magSquare(Vector2 v)
    {
        return v.x*v.x + v.y*v.y;
    }
}