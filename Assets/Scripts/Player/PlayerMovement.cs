using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    private CharacterController playerMovement;
    private Camera playerCamera;
    private Vector3 moveDirection;
    private float defaultSpeed = 10.0f;
    private bool ableToMove = true;

	// Use this for initialization
	void Start () {
        playerMovement = this.gameObject.GetComponent<CharacterController>();
        playerCamera = this.GetComponentInChildren<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        if (ableToMove) {
            moveDirection = new Vector3();
            float playerSpeed = defaultSpeed;

            if (Input.GetKey(KeyCode.W)) {
                moveDirection = transform.forward;
            } else if (Input.GetKey(KeyCode.S)) {
                moveDirection = -transform.forward;
            }

            if (Input.GetKey(KeyCode.A)) {
                moveDirection += -transform.right;
            } else if (Input.GetKey(KeyCode.D)) {
                moveDirection += transform.right;
            }

            if (Input.GetKey(KeyCode.LeftShift)) {
                playerSpeed *= 1.5f;
            } else if (Input.GetKey(KeyCode.LeftControl)) {
                playerSpeed *= 0.5f;
            }

            moveDirection.y = 0;

            playerMovement.Move(moveDirection * playerSpeed * Time.deltaTime);
        }
    }

    public bool AbleToMove {
        set { ableToMove = value; }
    }
}
