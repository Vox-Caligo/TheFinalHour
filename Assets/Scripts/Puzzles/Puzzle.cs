using UnityEngine;
using System.Collections;

public class Puzzle : MonoBehaviour {
    private GameObject puzzleUI;
    private CanvasGroup puzzleGroup;

	// Use this for initialization
	void Start () {
        // randomly choose puzzle
        string puzzleType = GameObject.Find("Puzzle Controller").GetComponent<PuzzleController>().getAvailablePuzzle();
        switch (puzzleType) {
            case "maze":
                puzzleUI = Instantiate(Resources.Load("Puzzles/Maze Puzzle")) as GameObject;
                puzzleUI.AddComponent<MazePuzzle>();
                break;
            case "matching":
                puzzleUI = Instantiate(Resources.Load("Puzzles/Matching Puzzle")) as GameObject;
                puzzleUI.AddComponent<MatchingPuzzle>();
                break;
            case "following":
                puzzleUI = Instantiate(Resources.Load("Puzzles/Following Puzzle")) as GameObject;
                puzzleUI.AddComponent<FollowingPuzzle>();   // check if issue arises when it adds here instead of on prefab itself
                break;
            case "decipher":
                puzzleUI = Instantiate(Resources.Load("Puzzles/Decipher Puzzle")) as GameObject;
                puzzleUI.AddComponent<DecipherPuzzle>();
                break;
            default:
                break;
        }
        
        puzzleGroup = puzzleUI.GetComponent<CanvasGroup>();
    }

    public void showPuzzle() {
        print(puzzleUI.name);
        puzzleGroup.alpha = 1;
    }

    public void hidePuzzle() {
        puzzleGroup.alpha = 0;
    }
}
