using UnityEngine;
using System.Collections;

public class AngelAI : MonoBehaviour {

    protected Transform myTransform;
    protected Transform playerTransform;
    protected Transform nearestPathPoint;

    protected float moveSpeed = 5;
    protected float rotationSpeed = 5;
    protected float maxDistance = 2;

    protected void Awake() {
        myTransform = transform;
    }

    // Use this for initialization
    protected void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
	}

    // Update is called once per frame
    protected void Update () {
        movement();
    }

    protected virtual void movement() {
        myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(playerTransform.position - myTransform.position), rotationSpeed * Time.deltaTime);

        if(Vector3.Distance(playerTransform.position, myTransform.position) > maxDistance) {
            // move towards target
            myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
        }
    }
}
