using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour {
    public Text Player1Text;
    public Text Player2Text;
    private uint p1Score;
    private uint p2Score;

    private void Awake()
    {
        p1Score = GameManager.gm.Player1Score;
        p2Score = GameManager.gm.Player2Score;
        Player1Point();
        Player2Point();
    }

    private void Update()
    {
        if (p1Score != GameManager.gm.Player1Score) {
            p1Score = GameManager.gm.Player1Score;
            Player1Point();
        }
        if (p2Score != GameManager.gm.Player2Score) {
            p2Score = GameManager.gm.Player2Score;
            Player2Point();
        }
    }

    void Player1Point() {
        Player1Text.text = "score: " + p1Score.ToString();
    }

    void Player2Point() {
        Player2Text.text = "score: " + p2Score.ToString();
    }
}
