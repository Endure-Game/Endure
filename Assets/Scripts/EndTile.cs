using UnityEngine;
using System.Collections;

public class EndTile : MonoBehaviour {
	public GameObject dialogue;

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.tag == "Player") {
			Instantiate (dialogue);
			Destroy (PlayerController.instance.gameObject);
			Application.LoadLevel (4);
		}
	}
}
