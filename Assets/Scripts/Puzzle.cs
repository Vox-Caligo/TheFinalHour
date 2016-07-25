using UnityEngine;
using System.Collections;

public class Puzzle : MonoBehaviour {
    private GameObject puzzleUI;
    private CanvasGroup puzzleGroup;

	// Use this for initialization
	void Start () {
        puzzleUI = GameObject.Find("Puzzle Canvas");

        // randomly choose puzzle
    }
	
	// Update is called once per frame
	void Update () {
        //puzzleUI.GetComponent<CanvasGroup>().alpha = 1;
    }

    public void showPuzzle() {
        puzzleUI.GetComponent<CanvasGroup>().alpha = 1;
        puzzleUI.transform.FindChild("Puzzle 1").GetComponent<CanvasGroup>().alpha = 1;
    }

    public void hidePuzzle() {
        puzzleUI.GetComponent<CanvasGroup>().alpha = 0;
        puzzleUI.transform.FindChild("Puzzle 1").GetComponent<CanvasGroup>().alpha = 0;
    }
}
