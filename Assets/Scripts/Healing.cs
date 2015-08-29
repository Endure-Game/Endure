using UnityEngine;
using System.Collections;

public class Healing : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D newOther) {
		Collider2D other = newOther.collider;
		print ("Is touching");
		if (other.tag == "Player") {
			PlayerController.instance.IncrementCounter ();
			Destroy (this.gameObject);
		}
	}
}
