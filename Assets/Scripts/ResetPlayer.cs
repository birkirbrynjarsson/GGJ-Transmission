using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour {

	float x = 22;
	float z = 18;
	float y = 4;

	public Transform player1;
	public Transform player2;

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.name == "Pelvis") {
			Instantiate (player1, new Vector3 (x, y, z), Quaternion.identity);
			Destroy (other.gameObject.transform.root.gameObject);
			GameManager.gm.Player2Score += 1;
		}
		if (other.gameObject.name == "Bip002 Pelvis") {
			Instantiate (player2, new Vector3 (x+2, y, z+2), Quaternion.identity);
			Destroy (other.gameObject.transform.root.gameObject);
			GameManager.gm.Player1Score += 1;
		}
	}
}
