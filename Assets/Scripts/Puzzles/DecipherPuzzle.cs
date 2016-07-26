using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DecipherPuzzle : MonoBehaviour {
    private Text passage;
    private string chosenPassage;
    private int chosenLetterIndex = 0;
    private char[] chosenLetters = new char[4];
    private int[] typedLetters = new int[4];
    private Text[] correctTextBoxes;

	// Use this for initialization
	void Start () {
        // gets the passage for the 
        chosenPassage = getPassage();

        // chooses 5 letters at random and sorts them in ascending order
        int[] chosenLettersLocation = findChosenLetters();

        // stores the letters to be hit in order 
        string uppercasePassage = chosenPassage.ToUpper();
        for (int i = 0; i < chosenLettersLocation.Length; i++) {
            chosenLetters[i] = uppercasePassage[chosenLettersLocation[i]];
        }

        // testing purposes: shows the chosen letters
        foreach(char chosenLetter in chosenLetters) {
            print("Chosen Letter: " + chosenLetter);
        }

        // finds passage box and sets the text
        passage = transform.FindChild("Passage").GetComponent<Text>();
        passage.text = chosenPassage;

        setButtons();

        correctTextBoxes = transform.FindChild("Correct").GetComponentsInChildren<Text>();

        // set timer for each letter to flash with random timings
    }

    private int[] findChosenLetters() {
        int[] chosenLettersLocation = new int[4];
        int letterRunner = 0;

        while(letterRunner < 4) {
            int randomLetter = Random.Range(0, chosenPassage.Length);

            if(char.IsLetter(chosenPassage[randomLetter]) && System.Array.IndexOf(chosenLettersLocation, randomLetter) == -1) {
                chosenLettersLocation[letterRunner] = randomLetter;
                letterRunner++;
            }
        }

        System.Array.Sort(chosenLetters);
        return chosenLettersLocation;
    }

    private void setButtons() {
        // sets the input buttons to call checkButtonPressed with their letter when pressed
        Button[] inputButtons = transform.FindChild("Input").GetComponentsInChildren<Button>();

        for(int i = 0; i < inputButtons.Length; i++) {
            char buttonLetter = System.Convert.ToChar(inputButtons[i].GetComponentInChildren<Text>().text);

            inputButtons[i].onClick.AddListener(() => {
                checkButtonPressed(buttonLetter);
            });
        }
    }
	
	// Update is called once per frame
	void Update () {

        // accept input 

    }

    private void checkButtonPressed(char letter) {
        //print("Letter: " + letter);
        if(letter == chosenLetters[chosenLetterIndex]) {
            // fill in the main buttons
            correctTextBoxes[chosenLetterIndex].text = letter.ToString();

            chosenLetterIndex++;
            
            if(chosenLetterIndex >= chosenLetters.Length) {
                print("Yay");
                // win
            }
        } else {
            chosenLetterIndex = 0;

            // erase all the main buttons
            foreach (Text correctTextBox in correctTextBoxes) {
                correctTextBox.text = "";
            }
        }
    }

    private string getPassage() {
        string[] passages = new string[] {
            "For they are spirits of demons, performing signs, which go out to the kings of the whole world, to gather them together for the war of the great day of God, the Almighty."
        };

        return passages[Random.Range(0, passages.Length)];
    }
}
