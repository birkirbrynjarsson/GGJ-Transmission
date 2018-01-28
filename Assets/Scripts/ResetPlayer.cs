using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPlayer : MonoBehaviour {

	float x = 22;
	float z = 18;
	float y = 4;

	public Transform player1;
	public Transform player2;
	public AudioClip cheer;

	private Vector3 player1StartPos;
	private Vector3 player2StartPos;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		player1StartPos = player1.position;
		player2StartPos = player2.position;
	}

	IEnumerator PlayCheer() {
		AudioSource audio = GetComponent<AudioSource> ();

		audio.Play ();
		yield return new WaitForSeconds (audio.clip.length);
		audio.clip = cheer;
	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.name == "Bip001 Pelvis") {
			Instantiate (player1, new Vector3 (player1StartPos.x, player1StartPos.y, player1StartPos.z), Quaternion.identity);
			Destroy (other.gameObject.transform.root.gameObject);
			GameManager.gm.Player2Score += 1;
			StartCoroutine (PlayCheer ());
		}
		if (other.gameObject.name == "Bip002 Pelvis") {
			Instantiate (player2, new Vector3 (player2StartPos.x, player2StartPos.y, player2StartPos.z), Quaternion.identity);
			Destroy (other.gameObject.transform.root.gameObject);
			GameManager.gm.Player1Score += 1;
			StartCoroutine (PlayCheer ());
		}
	}
}
