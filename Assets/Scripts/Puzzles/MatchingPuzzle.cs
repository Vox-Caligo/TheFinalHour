using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchingPuzzle : TilePuzzles {

    private int currentTileSpace = 0;
    private GameObject selectedTile;

    protected override void setInitialValues() {
        amountOfInputTiles = 16;
        amountOfCorrectTiles = 8;
        tilesPerCorrectRow = 4;

        // correct tile placement
        startingCorrectXLoc = -93;
        startingCorrectYLoc = 14;
        correctTileXDistance = 61;
        correctTileYDistance = -80;
    }

    // places tiles over the puzzle
    protected override void randomizeTileLocations() {
        base.randomizeTileLocations();

        foreach (GameObject matchingPiece in inputButtons) {
            Vector3 randomRotation = new Vector3(0, 0, Random.Range(0, 360));
            matchingPiece.transform.Rotate(randomRotation);
        }
    }

    protected override void correctSelection() {
        ((GameObject)correctTileImages[currentTileSpace]).GetComponent<Image>().color = CORRECT_SUCCESS_COLOR;

        Image[] allTileImages = selectedTile.GetComponentsInChildren<Image>();

        foreach(Image allTileImage in allTileImages) {
            allTileImage.enabled = false;
        }
        
        currentTileSpace++;

        if (currentTileSpace >= amountOfCorrectTiles) {
            print("MADE IT");
        }
    }

    protected override void incorrectSelection() {
        currentTileSpace = 0;
        randomizePuzzle();
    }

    protected override void tileHasBeenSelected(GameObject clickedTile) {
        selectedTile = clickedTile;

        int[] inputTileToCheck = ((GameObject)clickedTile).GetComponent<MatchingTilePiece>().TileSpaces;
        int[] correctTileToCheck = ((GameObject)correctTileImages[currentTileSpace]).GetComponent<MatchingTilePiece>().TileSpaces;
        compareTiles(inputTileToCheck, correctTileToCheck);
    }
}
