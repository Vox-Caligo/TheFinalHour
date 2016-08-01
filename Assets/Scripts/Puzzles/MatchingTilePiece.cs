using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MatchingTilePiece : MonoBehaviour {
    private int tileTypes = 4;
    private int[] tileSpaces = new int[4];
    private Image[] tileImages;

    public void setCorrectTile(int[] correctSpaces) {
        tileSpaces = correctSpaces;
        tileImages = gameObject.GetComponentsInChildren<Image>();
        setTileImages();
    }

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
            Image tileImage = tileImages[i + 1];
            Sprite testSprite;

            switch (tileSpaces[i]) {
                case 0:
                    testSprite = Resources.Load<Sprite>("Symbol Tiles/Symbol_1");
                    break;
                case 1:
                    testSprite = Resources.Load<Sprite>("Symbol Tiles/Symbol_2");
                    break;
                case 2:
                    testSprite = Resources.Load<Sprite>("Symbol Tiles/Symbol_3");
                    break;
                default:
                    testSprite = Resources.Load<Sprite>("Symbol Tiles/Symbol_4");
                    break;
            }

            tileImage.sprite = testSprite;
        }
        
        tileImages[2].transform.localScale = new Vector3(-1, 1);
        tileImages[3].transform.localScale = new Vector3(-1, -1);
        tileImages[4].transform.localScale = new Vector3(1, -1); 
    }
    
    public int[] TileSpaces {
        get { return tileSpaces; }
    }
}
