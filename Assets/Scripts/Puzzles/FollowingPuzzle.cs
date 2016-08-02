using UnityEngine;
using System.Collections;

public class FollowingPuzzle : TilePuzzles {
    private const int ROUNDS_OF_MEMORY = 5;
    private int currentRound = 0;
    private int baseRoundTiles = 3;
    private bool showingChoices = true;
    private ArrayList roundPattern = new ArrayList();
    private int currentRoundIndex = 0;

    protected override void Start() {
        setInitialValues();
        setButtons();
        randomizePuzzle();
        setRoundPattern();
    }
    
    protected override void setInitialValues() {
        amountOfInputTiles = 16;
        amountOfCorrectTiles = 8;
        tilesPerCorrectRow = 8;
        startingCorrectXLoc = -198;
        startingCorrectYLoc = 14;
        correctTileXDistance = 61;
        correctTileYDistance = -80;
    }

    private void setRoundPattern() {
        roundPattern.Clear();

        int tileAmount = currentRound + baseRoundTiles;

        for (int i = 0; i < tileAmount; i++) {
            int randomTileChoice = Random.Range(0, correctTileImages.Count);
            roundPattern.Add(correctTileImages[randomTileChoice]);
            print("Choice " + i + " is " + randomTileChoice);
        }
    }

    protected override void correctSelection() {
        print("Selected");
        currentRoundIndex++;
        randomizeTileLocations();

        if(currentRoundIndex >= roundPattern.Count) {
            print("New Round");
            showingChoices = true;
            currentRound++;
            setRoundPattern();
        }

        if (currentRound >= ROUNDS_OF_MEMORY) {
            print("MADE IT");
        }
    }

    protected override void incorrectSelection() {
        showingChoices = true;
        currentRound = 0;
        currentRoundIndex = 0;
        randomizeTileLocations();
    }

    protected override void tileHasBeenSelected(GameObject clickedTile) {
        showingChoices = false;

        int[] inputTileToCheck = ((GameObject)clickedTile).GetComponent<MatchingTilePiece>().TileSpaces;
        int[] correctTileToCheck = ((GameObject)correctTileImages[currentRoundIndex]).GetComponent<MatchingTilePiece>().TileSpaces;
        compareTiles(inputTileToCheck, correctTileToCheck);
    }

    void Update() {
        if(showingChoices) {
            // flash items
        }
    }
}
