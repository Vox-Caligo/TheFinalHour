using UnityEngine;
using System.Collections;

public class PlayerVisibility : MonoBehaviour {

    private string visibilityLevel = "unseen";

    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Hiding") {
            if (visibilityLevel == "unseen") {
                visibilityLevel = "hiding";
            }
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Hiding") {
            if (visibilityLevel == "hiding") {
                visibilityLevel = "unseen";
            }
        }
    }

    public string VisibilityLevel {
        get { return visibilityLevel; }
    }
}
