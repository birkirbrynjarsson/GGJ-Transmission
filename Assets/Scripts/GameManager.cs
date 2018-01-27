using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // using a singleton pattern for the gamemanager
    public static GameManager gm = null;
    // variables to keep track of the score for each player
    // should be updated in one script and fetched to update UI in the UpdateScore script
    public uint Player1Score;
    public uint Player2Score;
          
    void Awake() {
        if (gm == null) {
            gm = this;
            Player1Score = 0;
            Player2Score = 0;
        }

        else if (gm != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
