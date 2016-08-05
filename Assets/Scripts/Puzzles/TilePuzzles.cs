using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TilePuzzles : MonoBehaviour {
    protected ArrayList correctTileSpaces = new ArrayList();
    protected int amountOfInputTiles;
    protected int amountOfCorrectTiles;
    protected int tilesPerCorrectRow;

    // correct tile placement
    protected int startingCorrectXLoc;
    protected int startingCorrectYLoc;
    protected int correctTileXDistance;
    protected int correctTileYDistance;

    // correct colors
    protected Color CORRECT_DEFAULT_COLOR = new Color(.498f, .506f, .306f);
    protected Color CORRECT_SUCCESS_COLOR = new Color(.839f, .875f, .204f);
    
    protected ArrayList inputButtons = new ArrayList();
    protected ArrayList correctTileImages = new ArrayList();

    // distance between generate tiles
    protected int TILE_DISTANCE = 50;
    protected ArrayList tileLocations;

    // Use this for initialization
    protected virtual void Start() {
        setInitialValues();
        setButtons();
        randomizePuzzle();
    }
    
    // sets buttons and images for the 
    protected void setButtons() {
        int correctTileX = startingCorrectXLoc;
        int correctTileY = startingCorrectYLoc;

        for (int i = 0; i < amountOfInputTiles; i++) {
            // sets the input buttons
            GameObject matchingPiece = (GameObject)GameObject.Instantiate(Resources.Load("Symbol Tiles/Matching Tile"));
            matchingPiece.transform.SetParent(transform.FindChild("Input"), false);
            inputButtons.Add(matchingPiece);

            if(i == 0) {
                matchingPiece.GetComponent<Image>().color = Color.red;
            } else if(i == 1) {
                matchingPiece.GetComponent<Image>().color = Color.yellow;
            } else if(i == 2) {
                matchingPiece.GetComponent<Image>().color = Color.green;
            }

            // sets the correct images
            if (i < amountOfCorrectTiles) {
                if (i % tilesPerCorrectRow == 0 && i != 0) {
                    correctTileX = startingCorrectXLoc;
                    correctTileY += correctTileYDistance;
                }

                GameObject correctPiece = (GameObject)GameObject.Instantiate(Resources.Load("Symbol Tiles/Correct Tile"));
                correctPiece.transform.SetParent(transform.FindChild("Correct"), false);
                correctTileImages.Add(correctPiece);
                correctPiece.transform.localPosition = new Vector3(correctTileX, correctTileY);
                correctTileX += correctTileXDistance;
            }
        }

        foreach (GameObject matchingPiece in inputButtons) {
            matchingPiece.transform.SetSiblingIndex(Random.Range(0, inputButtons.Count));
        }
    }

    // randomizes buttons for input
    protected void randomizePuzzle() {
        tileLocations = new ArrayList();

        for (int i = 0; i < inputButtons.Count; i++) {
            GameObject matchingPiece = inputButtons[i] as GameObject;
            Image[] allTileImages = matchingPiece.GetComponentsInChildren<Image>();

            foreach (Image allTileImage in allTileImages) {
                allTileImage.enabled = true;
            }

            MatchingTilePiece tilePiece = matchingPiece.GetComponent<MatchingTilePiece>();
            tilePiece.setRandomTile(correctTileSpaces);

            if (i < amountOfCorrectTiles) {
                setCorrectImageTile(((GameObject)correctTileImages[i]).GetComponent<MatchingTilePiece>(), tilePiece.TileSpaces);
            }
            
            setMatchingListeners(matchingPiece, tilePiece.TileSpaces);
        }

        randomizeTileLocations();
    }

    // attaches listeners to the buttons
    protected void setMatchingListeners(GameObject matchingPiece, int[] tileSpaces) {
        Button matchingPieceButton = matchingPiece.GetComponent<Button>();
        matchingPieceButton.onClick.RemoveAllListeners();
        matchingPieceButton.onClick.AddListener(() => {
            tileHasBeenSelected(matchingPiece);
        });
    }

    protected void setCorrectImageTile(MatchingTilePiece correctTilePiece, int[] tileImages) {
        correctTilePiece.setCorrectTile(tileImages);
        correctTileSpaces.Add(tileImages);
        correctTilePiece.gameObject.GetComponent<Image>().color = CORRECT_DEFAULT_COLOR;
    }

    // places tiles over the puzzle
    protected virtual void randomizeTileLocations() {
        foreach (GameObject matchingPiece in inputButtons) {
            bool foundSpot;
            Vector3 newLocation;
            int spotIndex = 0;

            do {
                foundSpot = true;

                float randomXLoc = Random.Range(-322, 330);
                float randomYLoc = Random.Range(-200, -470);
                newLocation = new Vector3(randomXLoc, randomYLoc);

                //for (int i = 0; i < inputButtons.Count; i++) {
                foreach (Vector3 tileLocation in tileLocations) {
                    //GameObject comparedMatchingPiece = inputButtons[i] as GameObject;
                    if (Vector3.Distance(newLocation, tileLocation) < TILE_DISTANCE) {
                        foundSpot = false;
                        spotIndex++;
                        break;
                    }
                }
            } while (!foundSpot && spotIndex < 100);

            matchingPiece.transform.localPosition = newLocation;
            tileLocations.Add(newLocation);
        }
    }

    // compares the selected tile to the correct tile
    protected void compareTiles(int[] inputTileToCheck, int[] correctTileToCheck) { 
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

        if (correctMatch) {
            print("Yay!");
            correctSelection();
        } else {
            incorrectSelection();
        }
    }
    
    protected int[] shiftArray(int[] arrayToShift) {
        int[] shiftedArray = new int[arrayToShift.Length];

        for (int i = 0; i < arrayToShift.Length; i++) {
            if (i < arrayToShift.Length - 1) {
                shiftedArray[i] = arrayToShift[i + 1];
            } else {
                shiftedArray[i] = arrayToShift[0];
            }
        }

        return shiftedArray;
    }

    protected virtual void setInitialValues() { }

    protected virtual void tileHasBeenSelected(GameObject clickedTile) { }

    protected virtual void correctSelection() { }

    protected virtual void incorrectSelection() { }
}