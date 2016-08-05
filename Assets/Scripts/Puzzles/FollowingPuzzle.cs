using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FollowingPuzzle : TilePuzzles {
    private const int ROUNDS_OF_MEMORY = 4;
    private int currentRound = 0;
    private int baseRoundTiles = 3;
    private bool showingChoices = true;
    private bool showFlashingTiles = true;
    private ArrayList roundPattern = new ArrayList();
    private int roundPatternIndex = 0;

    // timer variables
    private float timerTicker;
    private int highlightTileIndex = -1;
    private const float DELAY_BETWEEN_TILES = 1;
    private const int MIN_TIME_DELAY = 2;
    private const int MAX_TIME_DELAY = 5;
    private const int MIN_TIME_SHOWN = 2;
    private const int MAX_TIME_SHOWN = 2;

    // colors
    private ArrayList highlightColors = new ArrayList() {
        Color.red, Color.magenta, Color.yellow, Color.cyan, Color.green
    };

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
        highlightTileIndex = 0;
        roundPatternIndex = 0;

        int tileAmount = currentRound + baseRoundTiles;

        foreach (GameObject inputButton in inputButtons) {
            inputButton.GetComponent<Image>().color = Color.white;
        }

        foreach (GameObject correctTileImage in correctTileImages) {
            correctTileImage.GetComponent<Image>().color = CORRECT_DEFAULT_COLOR;
        }

        for (int i = 0; i < tileAmount; i++) {
            int randomTileChoice = Random.Range(0, correctTileImages.Count);
            roundPattern.Add(correctTileImages[randomTileChoice]);

            if (i == 0) {
                ((GameObject)inputButtons[randomTileChoice]).GetComponent<Image>().color = Color.red;
            } else if (i == 1) {
                ((GameObject)inputButtons[randomTileChoice]).GetComponent<Image>().color = Color.yellow;
            } else if (i == 2) {
                ((GameObject)inputButtons[randomTileChoice]).GetComponent<Image>().color = Color.green;
            }
        }
    }

    protected override void correctSelection() {
        print("Selected");
        roundPatternIndex++;

        if (currentRound >= ROUNDS_OF_MEMORY) {
            print("Completed");
        } else if (roundPatternIndex >= roundPattern.Count) {
            print("New Round");
            showingChoices = true;
            showFlashingTiles = true;
            currentRound++;
            setRoundPattern();
            randomizeTileLocations();
        }
    }

    protected override void incorrectSelection() {
        showingChoices = true;
        showFlashingTiles = true;
        currentRound = 0;
        roundPatternIndex = 0;
        randomizeTileLocations();
    }

    protected override void tileHasBeenSelected(GameObject clickedTile) {
        showingChoices = false;
        showFlashingTiles = false;

        int[] inputTileToCheck = ((GameObject)clickedTile).GetComponent<MatchingTilePiece>().TileSpaces;
        int[] correctTileToCheck = ((GameObject)roundPattern[roundPatternIndex]).GetComponent<MatchingTilePiece>().TileSpaces;

        compareTiles(inputTileToCheck, correctTileToCheck);
    }

    void Update() {
        if (showFlashingTiles) {
            timerTicker -= Time.deltaTime;

            if (timerTicker <= 0) {
                if (showingChoices) {
                    highlightTileIndex++;
                    timerTicker = Random.Range(MIN_TIME_SHOWN, MAX_TIME_SHOWN);

                    if (highlightTileIndex < roundPattern.Count) {
                        ((GameObject)roundPattern[highlightTileIndex]).GetComponent<Image>().color = ((Color)highlightColors[currentRound]);

                        if (highlightTileIndex > 0) {
                            ((GameObject)roundPattern[highlightTileIndex - 1]).GetComponent<Image>().color = CORRECT_DEFAULT_COLOR;
                        }
                    } else {
                        ((GameObject)roundPattern[roundPattern.Count - 1]).GetComponent<Image>().color = CORRECT_DEFAULT_COLOR;
                        timerTicker = Random.Range(MIN_TIME_DELAY, MAX_TIME_DELAY);
                        showingChoices = false;
                    }
                } else {
                    showingChoices = true;
                    highlightTileIndex = -1;
                }
            }
        } else {
            foreach(GameObject roundPatternTile in roundPattern) {
                roundPatternTile.GetComponent<Image>().color = CORRECT_DEFAULT_COLOR;
            }
        }
    }
}
