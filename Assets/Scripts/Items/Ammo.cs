using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {

	public int amount = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D newOther) {
		Collider2D other = newOther.collider;
		if (other.tag == "Player") {
			if(this.gameObject.GetComponent<ItemController>().name == "arrows"){
				PlayerController.instance.arrows += amount;
			}
			if(this.gameObject.GetComponent<ItemController>().name == "bullets"){
				PlayerController.instance.bullets += amount;
			}
		}
		//PlayerController.instance.IncrementCounter ();
		//Destroy (this.gameObject);
	}
}
