using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchingTilePiece : MonoBehaviour {
    private int tileTypes = 4;
    private int[] tileSpaces = new int[4];
    private Image[] tileImages;

    public void setRandomTile(ArrayList correctTiles) {
        tileImages = gameObject.GetComponentsInChildren<Image>();

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
        setTileImages();
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
    
    private void setTileImages() {
        for(int i = 0; i < tileSpaces.Length; i++) {
            switch(Random.Range(0, 3)) {
                case 0:
                    tileImages[0].sprite = Resources.Load("Symbol Tiles/Symbol 1") as Sprite;
                    break;
                case 1:
                    tileImages[0].sprite = Resources.Load("Symbol Tiles/Symbol 2") as Sprite;
                    break;
                case 2:
                    tileImages[0].sprite = Resources.Load("Symbol Tiles/Symbol 3") as Sprite;
                    break;
                default:
                    tileImages[0].sprite = Resources.Load("Symbol Tiles/Symbol 4") as Sprite;
                    break;
            }
        }
        
        tileImages[2].transform.localScale = new Vector3(-1, 1);
        tileImages[3].transform.localScale = new Vector3(-1, -1);
        tileImages[4].transform.localScale = new Vector3(1, -1); 
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
