using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class gumballProjectile : NetworkBehaviour
{
    public float destroyAfter = 9;
        public Rigidbody rigidBody;
        // public float force = 1000;

        public float speed = 10f;
        public float slowAngularRotation = 3f;

        public override void OnStartServer()
        {
            Invoke(nameof(DestroySelf), destroyAfter);
        }

        // set velocity for server and client. this way we don't have to sync the
        // position, because both the server and the client simulate it.
        void Start()
        {
            // if(isLocalPlayer)
            // {
            // Transform cameraView = Camera.main.transform;
            // }

            Vector3 randomVector = new Vector3(Random.Range(-1f, 1f), Random.Range(1,1), Random.Range(-2f, 0f));
            // Vector3 randomVector = new Vector3(Random.Range(-3f, 3f), Random.Range(3f, 3f), Random.Range(-2f, 2f));
            Vector3 randomRotation = new Vector3(Random.Range(-5f, 5f), Random.Range(5f, 5f), Random.Range(-5f, 5f));
            
            rigidBody.angularVelocity = ((randomRotation*speed)/slowAngularRotation);
            rigidBody.AddForce(transform.forward * speed * 300); //(cameraView.forward * speed * 300);
            rigidBody.velocity = randomVector*speed;

            
        }


        // destroy for everyone on the server
        [Server]
        void DestroySelf()
        {
            NetworkServer.Destroy(gameObject);
        }

        // ServerCallback because we don't want a warning if OnTriggerEnter is
        // called on the client
        // [ServerCallback]
        // void OnTriggerEnter(Collider co)
        // {
        //     NetworkServer.Destroy(gameObject);
        // }
}
