using UnityEngine;
using System.Collections;

public class Upgrade : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D (Collision2D newOther) {
		Collider2D other = newOther.collider;
		if (other.tag == "Player") {
			PlayerController.instance.AddUpgrade (this.gameObject.GetComponent<ItemController>().name);
		}
	}
}
