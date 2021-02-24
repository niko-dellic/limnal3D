using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screenSaver : MonoBehaviour
{

    public float speed = 100;
    private Rigidbody2D rigidbody2d;
    private Vector2 randomVelocity;
    private Vector2 randomSpawn;
    private Vector2 randScreenSpawn;


    void Awake()    
    {
        
        randScreenSpawn = new Vector2(Random.Range((-Screen.width/2.1f), (Screen.width/2.1f)), Random.Range((-Screen.height/2.1f), (Screen.height/2.1f)));

        randomSpawn = randScreenSpawn;

        randomVelocity = Random.insideUnitCircle.normalized;

        gameObject.transform.localPosition = randomSpawn;        

        rigidbody2d = GetComponent<Rigidbody2D>();

        rigidbody2d.simulated = true;

        // rigidbody2d.velocity = randomVelocity * speed;

    }

    private void OnEnable()
    {
        rigidbody2d.velocity = randomVelocity * speed;

    }
}
