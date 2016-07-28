using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchingPuzzle : MonoBehaviour {
    private ArrayList correctTileSpaces = new ArrayList();
    private const int AMOUNT_OF_CORRECT_TILES = 8;
    private int currentTileSpace = 0;
    Button[] inputButtons;

    // Use this for initialization
    void Start () {
        inputButtons = transform.FindChild("Input").GetComponentsInChildren<Button>();
        setButtons();
    }

    private void setButtons() {
        currentTileSpace = 0;
        correctTileSpaces.Clear();
        
        for (int i = 0; i < inputButtons.Length; i++) {
            MatchingTilePiece tilePiece = inputButtons[i].GetComponent<MatchingTilePiece>();
            tilePiece.setRandomTile(correctTileSpaces);

            if(i < AMOUNT_OF_CORRECT_TILES) {
                correctTileSpaces.Add(tilePiece.TileSpaces);
            }

            inputButtons[i].onClick.RemoveAllListeners();

            inputButtons[i].onClick.AddListener(() => {
                compareTile(tilePiece.TileSpaces);
            });
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
                    setButtons();
                }
            }
        } else {
            setButtons();
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
