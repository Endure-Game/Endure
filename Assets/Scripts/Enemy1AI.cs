using UnityEngine;
using System.Collections;

public class Enemy1AI : Enemy1Script {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.tag == "Player") {
			if(PlayerController.checkAggro (aggro, this.gameObject)){
				print ("Enemy has aggroed onto the player!");
				
			} else {
				print ("no aggro.... yet");
			}
		}
	}
	
	void OnTriggerExit2D (Collider2D other){
		if (other.tag == "Player") {
			print ("AWWW SHIT PLAY RAN AWAY :(");
		}
	}


	public void DoSomething (){
		print ("doing something");
	}
}
