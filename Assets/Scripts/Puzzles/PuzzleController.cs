using UnityEngine;
using System.Collections;

public class PuzzleController : MonoBehaviour {

    private ArrayList availablePuzzles;

	// Use this for initialization
	void Start () {
        availablePuzzles = new ArrayList() { "maze", "matching", "stacking", "decipher" };
	}
	
	public string getAvailablePuzzle() {
        int randomPuzzleChoice = Random.Range(0, availablePuzzles.Count);
        string puzzleChoice = availablePuzzles[randomPuzzleChoice] as string;
        availablePuzzles.RemoveAt(randomPuzzleChoice);

        return puzzleChoice;
    }
}
