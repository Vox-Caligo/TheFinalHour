using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchingPuzzle : MonoBehaviour {
    private ArrayList correctTileSpaces = new ArrayList();
    private const int AMOUNT_OF_TILES = 16;
    private const int AMOUNT_OF_CORRECT_TILES = 8;
    private const int AMOUNT_PER_CORRECT_ROW = 4;

    // correct tile placement
    private const int STARTING_CORRECT_X_LOC = -93;
    private const int STARTING_CORRECT_Y_LOC = 14;
    private const int CORRECT_TILE_X_DIST = 61;
    private const int CORRECT_TILE_Y_DIST = -80;

    // correct colors
    private Color CORRECT_DEFAULT_COLOR = new Color(.498f, .506f, .306f);
    private Color CORRECT_SUCCESS_COLOR = new Color(.839f, .875f, .204f);

    private int currentTileSpace = 0;
    ArrayList inputButtons = new ArrayList();
    ArrayList correctTileImages = new ArrayList();

    // distance between generate tiles
    private const int TILE_DISTANCE = 50;
    private ArrayList tileLocations;

    // Use this for initialization
    void Start () {
        setButtons();
        randomizePuzzle();
    }

    // sets buttons and images for the 
    private void setButtons() {
        int correctTileX = STARTING_CORRECT_X_LOC;
        int correctTileY = STARTING_CORRECT_Y_LOC;
        
        for(int i = 0; i < AMOUNT_OF_TILES; i++) {
            // sets the input buttons
            GameObject matchingPiece = (GameObject)GameObject.Instantiate(Resources.Load("Symbol Tiles/Matching Tile"));
            matchingPiece.transform.SetParent(transform.FindChild("Input"), false);
            inputButtons.Add(matchingPiece);

            // sets the correct images
            if (i < AMOUNT_OF_CORRECT_TILES) {
                if (i % AMOUNT_PER_CORRECT_ROW == 0 && i != 0) {
                    correctTileX = STARTING_CORRECT_X_LOC;
                    correctTileY += CORRECT_TILE_Y_DIST;
                }

                GameObject correctPiece = (GameObject)GameObject.Instantiate(Resources.Load("Symbol Tiles/Correct Tile"));
                correctPiece.transform.SetParent(transform.FindChild("Correct"), false);
                correctTileImages.Add(correctPiece);
                correctPiece.transform.localPosition = new Vector3(correctTileX, correctTileY);
                correctTileX += CORRECT_TILE_X_DIST;
            }
        }

        foreach(GameObject matchingPiece in inputButtons) {
            matchingPiece.transform.SetSiblingIndex(Random.Range(0, inputButtons.Count));
        }
    }

    // randomizes buttons for input
    private void randomizePuzzle() {
        tileLocations = new ArrayList();

        for (int i = 0; i < inputButtons.Count; i++) {
            GameObject matchingPiece = inputButtons[i] as GameObject;
            matchingPiece.GetComponent<Image>().enabled = true;

            MatchingTilePiece tilePiece = matchingPiece.GetComponent<MatchingTilePiece>();
            tilePiece.setRandomTile(correctTileSpaces);

            if (i < AMOUNT_OF_CORRECT_TILES) {
                setCorrectImageTile(((GameObject)correctTileImages[i]).GetComponent<MatchingTilePiece>(), tilePiece.TileSpaces);
            }

            setTileLocation(matchingPiece);
            setMatchingListeners(matchingPiece, tilePiece.TileSpaces);
        }
    }

    // attaches listeners to the buttons
    private void setMatchingListeners(GameObject matchingPiece, int[] tileSpaces) {
        Button matchingPieceButton = matchingPiece.GetComponent<Button>();
        matchingPieceButton.onClick.RemoveAllListeners();
        matchingPieceButton.onClick.AddListener(() => {
            compareTile(matchingPiece);
        });
    }

    private void setCorrectImageTile(MatchingTilePiece correctTilePiece, int[] tileImages) {
        correctTilePiece.setCorrectTile(tileImages);
        correctTileSpaces.Add(tileImages);
        correctTilePiece.gameObject.GetComponent<Image>().color = CORRECT_DEFAULT_COLOR;
    }

    // places tiles over the puzzle
    private void setTileLocation(GameObject matchingPiece) {
        bool foundSpot;
        Vector3 newLocation;
        int spotIndex = 0;

        do {
            foundSpot = true;

            float randomXLoc = Random.Range(-322, 330);
            float randomYLoc = Random.Range(-200, -470);
            newLocation = new Vector3(randomXLoc, randomYLoc);

            //for (int i = 0; i < inputButtons.Count; i++) {
            foreach(Vector3 tileLocation in tileLocations) { 
                //GameObject comparedMatchingPiece = inputButtons[i] as GameObject;
               // print("Dist for " + i + ": " + Vector3.Distance(newLocation, comparedMatchingPiece.transform.localPosition));
                if(Vector3.Distance(newLocation, tileLocation) < TILE_DISTANCE) {
                    foundSpot = false;
                    spotIndex++;
                    break;
                }
            }
        } while (!foundSpot && spotIndex < 100);
        
        matchingPiece.transform.localPosition = newLocation;
        tileLocations.Add(newLocation);

        Vector3 randomRotation = new Vector3(0, 0, Random.Range(0, 360));
        matchingPiece.transform.Rotate(randomRotation);
    }

    // compares the selected tile to the correct tile
    private void compareTile(GameObject clickedTile) {
        int[] inputTileToCheck = ((GameObject)clickedTile).GetComponent<MatchingTilePiece>().TileSpaces;
        int[] correctTileToCheck = ((GameObject)correctTileImages[currentTileSpace]).GetComponent<MatchingTilePiece>().TileSpaces;

        bool correctMatch = true;
        int[] tilesToRotate = correctTileToCheck;

        for (int i = 1; i < correctTileToCheck.Length; i++) {
            for (int j = 0; j < tilesToRotate.Length; j++) {
                if (inputTileToCheck[j] != tilesToRotate[j]) {
                    correctMatch = false;
                }
            }

            if (!correctMatch) {
                tilesToRotate = shiftArray(tilesToRotate);
            } else {
                break;
            }
        }

        if(correctMatch) {
            print("Yay!");
            ((GameObject)correctTileImages[currentTileSpace]).GetComponent<Image>().color = CORRECT_SUCCESS_COLOR;
            clickedTile.GetComponent<Image>().enabled = false;
            currentTileSpace++;
        } else {
            currentTileSpace = 0;
            randomizePuzzle();
        }

        if (currentTileSpace >= AMOUNT_OF_CORRECT_TILES) {
            print("MADE IT");
        }
    }

    private int[] shiftArray(int[] arrayToShift) {
        int[] shiftedArray = new int[arrayToShift.Length];

        for(int i = 0; i < arrayToShift.Length; i++) {
            if (i < arrayToShift.Length - 1) {
                shiftedArray[i] = arrayToShift[i + 1];
            } else {
                shiftedArray[i] = arrayToShift[0];
            }
        }

        return shiftedArray;
    }
}
