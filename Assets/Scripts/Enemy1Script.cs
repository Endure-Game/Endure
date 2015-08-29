using UnityEngine;
using System.Collections;

public class Enemy1Script : MonoBehaviour {

	private float aggro = (float) 0.5;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D () {
		//PlayerController.instance.IncrementCounter ();
		//Destroy (this.gameObject);
		//print ("Enemy has vision of player");
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.tag == "Player") {
			if(PlayerController.checkAggro (this.aggro, this.gameObject)){
				print ("Enemy has aggroed onto the player!");
			} else {
				print ("no aggro.... yet");
			}
		}
	}
}
