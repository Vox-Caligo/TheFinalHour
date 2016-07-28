using UnityEngine;
using System.Collections;

public class MatchingTilePiece : MonoBehaviour {
    private int tileTypes = 4;
    private int[] tileSpaces = new int[4];

    public void correctTile(int[] correctTileNumbers) {
        tileSpaces = correctTileNumbers;
    }

    public void setRandomTile(ArrayList correctTiles) {
        bool setTile;
        int[] newTileSpaces = new int[tileSpaces.Length];

        do {
            setTile = true;

            for(int i = 0; i < tileSpaces.Length; i++) {
                newTileSpaces[i] = Random.Range(0, tileTypes);
            }

            setTile = checkTileIsIncorrect(correctTiles, newTileSpaces);
        } while (!setTile);

        tileSpaces = newTileSpaces;
        setTileLocation();
    }

    private bool checkTileIsIncorrect(ArrayList correctTiles, int[] newTileSpaces) {
        foreach (int[] correctTile in correctTiles) {
            int correctTileFirstIndex = System.Array.IndexOf(newTileSpaces, correctTile[0]);

            if (correctTileFirstIndex >= 0) {
                int rearrangedTileIndex = 0;
                int[] rearrangedTileOrder = new int[newTileSpaces.Length];

                for(int i = correctTileFirstIndex; i < newTileSpaces.Length; i++) {
                    rearrangedTileOrder[rearrangedTileIndex] = newTileSpaces[i];
                    rearrangedTileIndex++;
                }

                for(int i = 0; i < correctTileFirstIndex; i++) {
                    rearrangedTileOrder[rearrangedTileIndex] = newTileSpaces[i];
                    rearrangedTileIndex++;
                }

                for(int i = 0; i < correctTile.Length; i++) {
                    if(newTileSpaces[i] != correctTile[i]) {
                        return true;
                    }
                }

                return false;
            }
        }

        return true;
    }

    private void setTileLocation() {

    }

    //private int[] tileSpaces = new int[4];
    public int[] TileSpaces {
        get { return tileSpaces; }
    }

    public string checkTileSpace() {
        string holder = "";

        for(int i = 0; i < tileSpaces.Length; i++) {
            holder += tileSpaces[i] + ", ";
        }

        return holder;
    }
}
