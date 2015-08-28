using UnityEngine;
using System.Collections;

public class ItemController : MonoBehaviour {

	// Use this for initialization
	private float aggro = (float) 0.18;


	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			if(PlayerController.checkAggro (this.aggro, this.gameObject)){
				PlayerController.instance.IncrementCounter ();
				Destroy (this.gameObject);
			}
		}
		//PlayerController.instance.IncrementCounter ();
		//Destroy (this.gameObject);
	}
}
