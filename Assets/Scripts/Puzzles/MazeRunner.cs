using UnityEngine;
using System.Collections;

public class MazeRunner : MonoBehaviour {

    private Transform mazeRunner;
    private const float MAZE_RUNNER_SPEED = 2;

    // winning areas
    private const float RIGHT_BOUND = 200;
    private const float LEFT_BOUND = 200;
    private const float TOP_BOUND = 200;

    // checks forward position
    private RaycastHit2D detector;
    private Vector3 currentMovement;

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
        } else if (Input.GetKey(KeyCode.S)) {
            keyPressed = true;
            currentMovement = new Vector3(mazeRunner.localPosition.x, mazeRunner.localPosition.y - MAZE_RUNNER_SPEED);
        }

        if (Input.GetKey(KeyCode.A)) {
            keyPressed = true;
            currentMovement = new Vector3(mazeRunner.localPosition.x - MAZE_RUNNER_SPEED, mazeRunner.localPosition.y);
        } else if (Input.GetKey(KeyCode.D)) {
            keyPressed = true;
            currentMovement = new Vector3(mazeRunner.localPosition.x + MAZE_RUNNER_SPEED, mazeRunner.localPosition.y);
        }

        Vector2 viewDirection = new Vector2(currentMovement.x, currentMovement.y);
        if(validMovement(viewDirection, 5) && keyPressed) {
            mazeRunner.localPosition = currentMovement;
        }
    }

    private bool validMovement(Vector2 direction, float distance) {
        detector = Physics2D.Raycast(mazeRunner.position, direction, distance);

        if(detector.collider != null && detector.collider.name != mazeRunner.name) {
            print(detector.collider.name);
            return false;
        }

        return true;
    }

    void OnCollisionEnter2D(Collision2D col) {
        //print("Hit");
    }

    public void setLocation(Vector3 mazeRunnerStartingLocation) {
        gameObject.transform.localPosition = mazeRunnerStartingLocation;
    }
}
