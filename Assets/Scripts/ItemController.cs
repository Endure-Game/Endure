using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	public string name;

	// Use this for initialization
	private float aggro = (float) 0.18;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D (Collision2D newOther) {
		Collider2D other = newOther.collider;
		print ("Is touching");
		if (other.tag == "Player") {
			Destroy (this.gameObject);
		}
		//PlayerController.instance.IncrementCounter ();
		//Destroy (this.gameObject);
	}
}
