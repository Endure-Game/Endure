using UnityEngine;
using System.Collections;

public class EndTile : MonoBehaviour {
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.collider.tag == "Player") {
			Application.LoadLevel (4);
		}
	}
}
