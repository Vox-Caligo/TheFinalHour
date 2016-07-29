using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchingPuzzle : MonoBehaviour {
    private ArrayList correctTileSpaces = new ArrayList();
    private const int AMOUNT_OF_TILES = 12;
    private const int AMOUNT_OF_CORRECT_TILES = 8;
    private int currentTileSpace = 0;
    ArrayList inputButtons = new ArrayList();

    // Use this for initialization
    void Start () {
        setButtons();
    }

    private void setButtons() {
        currentTileSpace = 0;
        correctTileSpaces.Clear();

        for(int i = 0; i < AMOUNT_OF_TILES; i++) {
            GameObject matchingPiece = (GameObject)GameObject.Instantiate(Resources.Load("Symbol Tiles/Matching Tile"));
            matchingPiece.transform.SetParent(transform.FindChild("Input"), false);

            matchingPiece.GetComponent<Button>().onClick.AddListener(() => {
                compareTile(matchingPiece.GetComponent<MatchingTilePiece>().TileSpaces);
            });

            inputButtons.Add(matchingPiece);
        }

        setMatchingListeners();
    }

    private void setMatchingListeners() {
        int currentlySelected = 0;

        foreach(GameObject matchingPiece in inputButtons) {
            MatchingTilePiece tilePiece = matchingPiece.GetComponent<MatchingTilePiece>();
            tilePiece.setRandomTile(correctTileSpaces);

            if (currentlySelected < AMOUNT_OF_CORRECT_TILES) {
                correctTileSpaces.Add(tilePiece.TileSpaces);
                currentlySelected++;
            }

            Button matchingPieceButton = matchingPiece.GetComponent<Button>();

            matchingPieceButton.onClick.RemoveAllListeners();

            matchingPieceButton.onClick.AddListener(() => {
                compareTile(matchingPiece.GetComponent<MatchingTilePiece>().TileSpaces);
            });
        }

        setTileLocations();
    }

    private void setTileLocations() {
        foreach (GameObject matchingPiece in inputButtons) {
            
            float randomXLoc = Random.Range(-322, 330);
            float randomYLoc = Random.Range(0, 235);
            Vector3 newLocation = new Vector3(randomXLoc, randomYLoc);
            
            matchingPiece.transform.localPosition = newLocation;

            Vector3 test = new Vector3(0, 0, Random.Range(0, 360));
            matchingPiece.transform.Rotate(test);
        }
    }

    private void compareTile(int[] tileToCheck) {
        int[] correctTileToCheck = correctTileSpaces[currentTileSpace] as int[];

        string tileToCheckNums = "Tile To Check Nums: ";
        string currentTileSpaceNums = "Current Tile Space Nums: ";

        for (int i = 0; i < tileToCheck.Length; i++) {
            tileToCheckNums += tileToCheck[i] + ", ";
        }

        for (int i = 0; i < tileToCheck.Length; i++) {
            currentTileSpaceNums += correctTileToCheck[i] + ", ";
        }

        print(tileToCheckNums);
        print(currentTileSpaceNums);

        int correctTileFirstIndex = System.Array.IndexOf(tileToCheck, correctTileToCheck[0]);

        if (correctTileFirstIndex >= 0) {
            int rearrangedTileIndex = 0;
            int[] rearrangedTileOrder = new int[tileToCheck.Length];

            for (int i = correctTileFirstIndex; i < tileToCheck.Length; i++) {
                rearrangedTileOrder[rearrangedTileIndex] = tileToCheck[i];
                rearrangedTileIndex++;
            }

            for (int i = 0; i < correctTileFirstIndex; i++) {
                rearrangedTileOrder[rearrangedTileIndex] = tileToCheck[i];
                rearrangedTileIndex++;
            }

            for (int i = 0; i < correctTileToCheck.Length; i++) {
                if (tileToCheck[i] != correctTileToCheck[i]) {
                    setMatchingListeners();
                }
            }
        } else {
            setMatchingListeners();
        }

        currentTileSpace++;

        if (currentTileSpace >= AMOUNT_OF_CORRECT_TILES) {
            print("MADE IT");
        }
    }

    // Update is called once per frame
    void Update () {
	    
	}
}
