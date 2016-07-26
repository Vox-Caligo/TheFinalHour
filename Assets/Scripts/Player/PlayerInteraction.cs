using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {
    private PlayerMovement playerMovement;
    private CameraMovement cameraMovement;
    private Camera playerCamera;
    private Ray ray;
    private RaycastHit hit;
    private bool inPuzzle = false;
    private bool mouseHasBeenUp = true;
    private Puzzle currentPuzzle;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        playerCamera = this.GetComponentInChildren<Camera>();
        cameraMovement = playerCamera.GetComponent<CameraMovement>();
        playerMovement = this.GetComponent<PlayerMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward); //Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 2)) {
            if (hit.collider.gameObject.tag == "Puzzle") {
                if (Input.GetKeyDown(KeyCode.E)) {
                    inPuzzle = !inPuzzle;
                    playerMovement.AbleToMove = !inPuzzle;
                    cameraMovement.AbleToMove = !inPuzzle;

                    if (inPuzzle) {
                        currentPuzzle = hit.collider.gameObject.GetComponent<Puzzle>();
                        currentPuzzle.showPuzzle();
                    } else {
                        currentPuzzle.hidePuzzle();
                    }
                }
            } else if (hit.collider.gameObject.tag == "Item") {
                print("Checking: " + hit.collider.name);
            } else if (hit.collider.gameObject.tag == "Hiding") {
                print("Checking: " + hit.collider.name);
            }
        }

        if(inPuzzle && !Cursor.visible) {
            Cursor.visible = true;
        } else if(!inPuzzle && Cursor.visible) {
            Cursor.visible = false;
        }

        Cursor.visible = true; // FOR SANITY
    }

    void OnMouseOver() {
        //print(gameObject.name);
    }

    void OnMouseDown() {
        print("hi");
        Cursor.lockState = CursorLockMode.Locked;
    }
}
