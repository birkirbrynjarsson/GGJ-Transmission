using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {
    // simple start button
    public void LoadMain() {
        SceneManager.LoadScene("Main");
    }
}

