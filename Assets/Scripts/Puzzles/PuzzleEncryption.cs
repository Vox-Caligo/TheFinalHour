using UnityEngine;
using System.Collections;

public class PuzzleEncryption : MonoBehaviour {
    private const int SHIFTABLE_SPACES = 5;
    private const int SHIFTABLE_EVERY_OTHER_MAX = 3;
    private const int JUMPABLE_SENTENCES = 1;
    private const int PROCEED_LETTERS = 5;
    private string[] alphabet = new string[] {
        "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
         "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
    };

    private string passage;
    private int[] startingLetterLocations;

    private char[] finalLetters;
    private string[] finalEncryptions;

	public PuzzleEncryption(string passage, int[] startingLetterLocations, int encryptionSteps) {
        this.passage = passage;
        this.startingLetterLocations = startingLetterLocations;

        finalLetters = new char[startingLetterLocations.Length];
        finalEncryptions = new string[startingLetterLocations.Length];

        encryptPassage(encryptionSteps);
    }

    private void encryptPassage(int encryptionSteps) {
        for(int i = 0; i < startingLetterLocations.Length; i++) {
            string finalEncryption = "";
            int currentLocation = startingLetterLocations[i]; // maybe move to top

            for(int j = 0; j < encryptionSteps; j++) {
                switch(Random.Range(0, 3)) {
                    case 0:
                        finalEncryption += shift();
                        break;
                    case 1:
                        finalEncryption += jump();
                        break;
                    case 2:
                        finalEncryption += proceedToLetter();
                        break;
                    default:
                        break;
                }

                if(j + 1 < encryptionSteps) {
                    finalEncryption += ", ";
                }
            }

            finalEncryptions[i] = finalEncryption;
            print("Final Encryption for #" + i + ": " + finalEncryption);
        }
    }

    private string shift() {
        string shiftBuild = Random.Range(0, SHIFTABLE_SPACES).ToString();

        if(headForward()) {
            shiftBuild += "R";
        } else {
            shiftBuild += "L";
        }

        shiftBuild += Random.Range(1, SHIFTABLE_EVERY_OTHER_MAX);

        return shiftBuild;
    }

    private string jump() {
        string jumpBuild = Random.Range(1, JUMPABLE_SENTENCES).ToString();

        if (headForward()) {
            jumpBuild += "F";
        } else {
            jumpBuild += "B";
        }

        return jumpBuild;
    }

    private string proceedToLetter() {
        string proceedBuild = alphabet[Random.Range(0, alphabet.Length)];

        if (headForward()) {
            proceedBuild += "N";
        } else {
            proceedBuild += "P";
        }

        proceedBuild += Random.Range(0, PROCEED_LETTERS);

        return proceedBuild;
    }

    private bool headForward() {
        if(Random.Range(0, 1) == 0) {
            return true;
        }

        return false;
    }

    //private int[] finalLetterLocations(string encryptionString) {
    //    return new int[];
    //}
}
