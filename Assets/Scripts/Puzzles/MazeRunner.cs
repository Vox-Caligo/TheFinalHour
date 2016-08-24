using UnityEngine;
using System.Collections;

public class MazeRunner : MonoBehaviour {

    private Transform mazeRunner;
    private const float MAZE_RUNNER_SPEED = 2;

    // winning areas
    private const float RIGHT_BOUND = 200;
    private const float LEFT_BOUND = 200;
    private const float TOP_BOUND = 200;

    // preventing pushing through a wall
    private int currentDirection = -1;

    // checks forward position
    private Vector2 startPoint;
    private Vector2 origin;

    private RaycastHit2D detector;
    private Vector3 currentMovement;
    private Vector2 viewDirection;
    private bool validDirection = false;

    // Use this for initialization
    void Start () {
        mazeRunner = this.gameObject.transform;
        this.gameObject.GetComponent<Rigidbody2D>().freezeRotation = true;

        RectTransform colliderSizer = this.gameObject.GetComponent<RectTransform>();
        this.gameObject.GetComponent<BoxCollider2D>().size = new Vector2(100, 100);
    }
	
	// Update is called once per frame
	public void updateRunner () {
        bool keyPressed = false;

        if (Input.GetKey(KeyCode.W)) {
            keyPressed = true;
            currentMovement = new Vector3(mazeRunner.localPosition.x, mazeRunner.localPosition.y + MAZE_RUNNER_SPEED);
            currentDirection = 1;
        } else if (Input.GetKey(KeyCode.S)) {
            keyPressed = true;
            currentMovement = new Vector3(mazeRunner.localPosition.x, mazeRunner.localPosition.y - MAZE_RUNNER_SPEED);
            currentDirection = 3;
        }

        if (Input.GetKey(KeyCode.A)) {
            keyPressed = true;
            currentMovement = new Vector3(mazeRunner.localPosition.x - MAZE_RUNNER_SPEED, mazeRunner.localPosition.y);
            currentDirection = 0;
        } else if (Input.GetKey(KeyCode.D)) {
            keyPressed = true;
            currentMovement = new Vector3(mazeRunner.localPosition.x + MAZE_RUNNER_SPEED, mazeRunner.localPosition.y);
            currentDirection = 2;
        }
        
        if(keyPressed) {
            mazeRunner.localPosition = currentMovement;
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        Vector3 collidedWallPosition = col.collider.gameObject.transform.localPosition;
        float colliedWallWidth = col.collider.gameObject.GetComponent<RectTransform>().rect.width * .25f;

        switch (currentDirection) {
            case 0:
                currentMovement = new Vector3(collidedWallPosition.x + colliedWallWidth, mazeRunner.localPosition.y);
                break;
            case 1:
                currentMovement = new Vector3(mazeRunner.localPosition.x, collidedWallPosition.y - colliedWallWidth);
                break;
            case 2:
                currentMovement = new Vector3(collidedWallPosition.x - colliedWallWidth, mazeRunner.localPosition.y);
                break;
            case 3:
                currentMovement = new Vector3(mazeRunner.localPosition.x, collidedWallPosition.y + colliedWallWidth);
                break;
            default:
                break;
        }

        mazeRunner.localPosition = currentMovement;
    }

    void OnTriggerEnter2D(Collider2D finale) {
        print("Win");
    }

    public void setLocation(Vector3 mazeRunnerStartingLocation) {
        gameObject.transform.localPosition = mazeRunnerStartingLocation;
    }
}
