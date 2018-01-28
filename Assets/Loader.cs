using System.Collections;
using UnityEngine;

public class Loader : MonoBehaviour {


	public GameObject gameManager;

	void Awake() {

		if (GameManager.gm == null) {
			Instantiate (gameManager);
		}
	}
}
