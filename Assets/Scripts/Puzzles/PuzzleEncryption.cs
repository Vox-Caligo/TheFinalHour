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

    private int[] finalLetterLocations;
    private string[] finalEncryptions;

	public PuzzleEncryption(string passage, int[] startingLetterLocations, int encryptionSteps) {
        this.passage = passage.ToUpper();
        this.startingLetterLocations = startingLetterLocations;

        finalLetterLocations = new int[startingLetterLocations.Length];
        finalEncryptions = new string[startingLetterLocations.Length];

        encryptPassage(encryptionSteps);
        decryptLetterLocations();
    }

    private void encryptPassage(int encryptionSteps) {
        for(int i = 0; i < startingLetterLocations.Length; i++) {
            string finalEncryption = "";
            int currentLocation = startingLetterLocations[i]; // maybe move to top

            for(int j = 0; j < encryptionSteps; j++) {
                if (j + 1 < encryptionSteps) {
                    switch (Random.Range(0, 3)) {
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
                    
                    finalEncryption += "-";
                } else {
                    finalEncryption += shift();
                }
            }

            finalEncryptions[i] = finalEncryption;
        }
    }

    private string randomizeOptions(string optOne, string optTwo) {
        if (Random.Range(0, 2) == 0) {
            return optOne;
        }
        return optTwo;
    }

    private string shift() {
        string shiftBuild = Random.Range(0, SHIFTABLE_SPACES).ToString();
        shiftBuild += randomizeOptions("R", "L");
        shiftBuild += Random.Range(1, SHIFTABLE_EVERY_OTHER_MAX);
        return shiftBuild;
    }

    private string jump() {
        string jumpBuild = Random.Range(1, JUMPABLE_SENTENCES).ToString();
        jumpBuild += randomizeOptions("F", "B");
        jumpBuild += alphabet[Random.Range(0, alphabet.Length)];
        return jumpBuild;
    }

    private string proceedToLetter() {
        string proceedBuild = alphabet[Random.Range(0, alphabet.Length)];
        proceedBuild += randomizeOptions("N", "P");
        proceedBuild += Random.Range(0, PROCEED_LETTERS);
        return proceedBuild;
    }

    private void decryptLetterLocations() {
        for(int i = 0; i < finalLetterLocations.Length; i++) {
            string[] separatedIntoComponents = finalEncryptions[i].Split("-"[0]);
            int finalLocation = startingLetterLocations[i];

            for (int j = 0; j < separatedIntoComponents.Length; j++) {
                string currentCode = separatedIntoComponents[j].Replace(" ", string.Empty);
                char codeIdentifier = separatedIntoComponents[j][1];

                // take the current letter and decrypt it by doing the steps in this string
                if (codeIdentifier == 'F' || codeIdentifier == 'B') {
                    finalLocation = simulateJump(finalLocation, currentCode);
                } else if (codeIdentifier == 'N' || codeIdentifier == 'P') {
                    finalLocation = simulateProceedToLetter(finalLocation, currentCode);
                } else if (codeIdentifier == 'R' || codeIdentifier == 'L') {
                    finalLocation = simulateShift(finalLocation, currentCode);
                }
            }

            finalLetterLocations[i] = finalLocation;
        }
    }

    private int simulateShift(int currentLocation, string encryptionCode) {
        int distance = (int)char.GetNumericValue(encryptionCode[0]) * (int)char.GetNumericValue(encryptionCode[2]);
        int letterShift = 0;
        int direction = (encryptionCode[1] == 'R') ? 1 : -1;

        while (letterShift < distance) {
            if ((direction > 0 && currentLocation + direction < passage.Length) ||
                (direction < 0 && currentLocation + direction >= 0)){
                currentLocation += direction;
            } else if(direction > 0) {
                currentLocation = 0;
            } else {
                currentLocation = passage.Length - 1;
            }
            
            if (char.IsLetter(passage[currentLocation])) {
                letterShift++;
            }
        }

        return currentLocation;
    }

    private int simulateJump(int currentLocation, string encryptionCode) {
        int direction = (encryptionCode[1] == 'F') ? 1 : -1;
        int startingPeriodLocation = findPeriodLocation(currentLocation, direction);
        int endingPeriodLocation = findPeriodLocation(startingPeriodLocation, direction);
        int letterLocation = -1;

        if (startingPeriodLocation > 0 && endingPeriodLocation > 0) {
            for (int i = startingPeriodLocation; i != endingPeriodLocation; i += direction) {
                if(passage[i] == encryptionCode[2]) {
                    return i;
                }
            }
        } else if(startingPeriodLocation < currentLocation) {
            letterLocation = runThroughUntilPeriod(passage, encryptionCode[2], startingPeriodLocation, passage.Length - 2, direction); // end of the passage
        } else {
            letterLocation = runThroughUntilPeriod(passage, encryptionCode[2], startingPeriodLocation, 0, direction); // start of the passage
        }

        if (letterLocation >= 0) {
            return letterLocation;
        }

        return currentLocation;
    }
    
    private int simulateProceedToLetter(int currentLocation, string encryptionCode) {
        ArrayList soughtLetterLocations = findNeededLettersInPassage(encryptionCode[0]);
        int numberOfTimes = (int)char.GetNumericValue(encryptionCode[2]);
        int direction = (encryptionCode[1] == 'N') ? 1 : -1;
        int startingLocation = currentLocation;

        ArrayList testList = new ArrayList();

        if (numberOfTimes != 0 && soughtLetterLocations.Count > 0) {
            currentLocation = findStartingLocation(soughtLetterLocations, direction, currentLocation);

            for (int i = 1; i < numberOfTimes; i++) {
                int nextLocation = currentLocation + direction;

                if (nextLocation < soughtLetterLocations.Count && nextLocation > 0) {
                    currentLocation += direction;
                } else if (direction > 0) {
                    currentLocation = 0;
                } else {
                    currentLocation = soughtLetterLocations.Count - 1;
                }
            }

            return (int)soughtLetterLocations[currentLocation];
        }

        return startingLocation;
    }

    private int findPeriodLocation(int startingLocation, int incrementor) {
        for (int i = startingLocation + incrementor; i >= 0 && i < passage.Length; i += incrementor) {
            if (passage[i] == '.') {
                return i;
            }
        }

        return -1;
    }

    private int runThroughUntilPeriod(string passageToUse, char soughtLetter, int currentLocation, int startingPoint, int incrementor) {
        while (startingPoint >= 0 && startingPoint < passageToUse.Length) {
            char currentChar = passageToUse[startingPoint];
            
            if (currentChar == soughtLetter) {
                return startingPoint;
            } else if (currentChar == '.') {
                return -1;
            } else {
                startingPoint += incrementor;
            }
        }

        return -1;
    }

    private ArrayList findNeededLettersInPassage(char searchedForLetter) {
        ArrayList soughtLetterLocations = new ArrayList();

        for (int i = 0; i < passage.Length; i++) {
            if (passage[i] == searchedForLetter) {
                soughtLetterLocations.Add(i);
            }
        }

        return soughtLetterLocations;
    }

    private int findStartingLocation(ArrayList soughtLetterLocations, int forward, int currentLocation) {
        int startingLocation = 0;

        for (int i = 0; i < soughtLetterLocations.Count; i++) {
            if(forward > 0 && (int)soughtLetterLocations[i] > currentLocation) {
                return i;
            } else if((int)soughtLetterLocations[i] < currentLocation) {
                startingLocation = i;
            }
        }

        return startingLocation;
    }

    public string retrieveEncryptionCode(int codeIndex) {
        return finalEncryptions[codeIndex];
    }

    public char retrieveCorrectLetter(int currentLetter) {
        return passage[finalLetterLocations[currentLetter]];
    }
}
