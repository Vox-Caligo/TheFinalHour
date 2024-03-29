﻿using UnityEngine;
using System.Collections;

public class PuzzleController : MonoBehaviour {

    private ArrayList availablePuzzles;

	// Use this for initialization
	void Start () {
        //availablePuzzles = new ArrayList() { "maze", "matching", "following", "decipher" };
	}
	
	public string getAvailablePuzzle() {
        if(availablePuzzles == null) {
            availablePuzzles = new ArrayList() { "maze", "matching", "following", "decipher" };
        }

        int randomPuzzleChoice = Random.Range(0, availablePuzzles.Count);
        string puzzleChoice = availablePuzzles[randomPuzzleChoice] as string;
        availablePuzzles.RemoveAt(randomPuzzleChoice);

        return puzzleChoice;
    }
}
