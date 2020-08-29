using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public GameObject prefab;

    public static int N = 25;
    public static float initialSpeed = 0.5f;

    public static GameObject[] Boids = new GameObject[N];
    private Rigidbody2D _rb;

    public static int size = 20;
    public static float spacing;

    // Start is called before the first frame update
    void Start()
    {  

        // Declaring variables
        float x, y, vx, vy;
        
        // Generating N GameObjects
        spacing = 3f; //size/Mathf.Pow(N, 0.5);

        for(int i = 0; i < N; i++)
        {
            x = Random.Range(-size/2, size/2);
            y = Random.Range(-size/2, size/2);

            vx = Random.Range(-initialSpeed, initialSpeed);
            vy = Random.Range(-initialSpeed, initialSpeed);
        
            Generate(i, x, y, vx, vy, 1, false);
        }

        

    }

    void Generate(int i, float x, float y, float vx, float vy, float mass, bool frozen)
    {
        //GameObject[] objects = new GameObject[N];
        // make the object at (x,y,z)
        Boids[i] = Instantiate(prefab, new Vector2(x, y), Quaternion.identity);

        // Edit the rigidbody components of the new object
        _rb = Boids[i].GetComponent<Rigidbody2D>();
        _rb.velocity = new Vector2(vx, vy);
        // if (frozen)
        // {
        //     _rb.constraints = Rigidbody2DConstraints.FreezePosition;
        // }

        _rb.mass = mass;
        //set this as input parameter??
        _rb.angularDrag = 10f;
    }
}
