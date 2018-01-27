using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour {

    [SerializeField] public GameObject pausePanel;

	// Use this for initialization
	void Awake () {
        pausePanel.SetActive(false);
	}

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Debug.Log("IF");
            if (!pausePanel.activeInHierarchy) {
                Debug.Log("not active");
                Pause();
            }
            else {
                Debug.Log("active again");
                Play();
            }
        }
    }

    public void Pause () {
        Time.timeScale = 0;
        pausePanel.SetActive(true);
    }

    public void Play () {
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }
}
