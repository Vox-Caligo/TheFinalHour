using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    private float horizontalSpeed = 2.0f;
    private float verticalSpeed = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private const float MIN_Y = -20;
    private const float MAX_Y = 25;

    private Transform player;
    private bool ableToMove = true;

    void Start() {
        player = this.transform.parent.transform;
    }

    void Update() {
        if (ableToMove) {
            yaw += horizontalSpeed * Input.GetAxis("Mouse X");
            pitch -= verticalSpeed * Input.GetAxis("Mouse Y");

            if (pitch < MIN_Y) {
                pitch = MIN_Y;
            } else if (pitch > MAX_Y) {
                pitch = MAX_Y;
            }

            // rotate the player when the mouse moves horizontally, and rotate the camera when the mouse moves vertically
            Quaternion cameraRotation = transform.rotation;
            transform.rotation = new Quaternion();
            player.rotation = cameraRotation;

            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
    }

    public bool AbleToMove {
        set { ableToMove = value; }
    }
}
