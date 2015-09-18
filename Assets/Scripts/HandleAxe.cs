using UnityEngine;
using System.Collections;

public class HandleAxe : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "Axe") {
			Sounds.instance.AxeHit ();
			Destroy (this.gameObject);
		}
	}
}
