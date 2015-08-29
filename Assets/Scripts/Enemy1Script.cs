using UnityEngine;
using System.Collections;

public class Enemy1Script : MonoBehaviour {

	public float speed = 3;
	private float aggro = (float) 1.0;
	private static Enemy1AI enemyAI;

	// Use this for initialization
	void Start () {
		enemyAI = this.gameObject.AddComponent <Enemy1AI> ();
		print (enemyAI);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake () {

	}
	void OnTriggerEnter2D () {
		//PlayerController.instance.IncrementCounter ()
		//Destroy (this.gameObject);
		//print ("Enemy has vision of player");
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.tag == "Player") {
			if(PlayerController.checkAggro (this.aggro, this.gameObject)){
				//print ("Enemy has aggroed onto the player!");
				
				enemyAI.enemyActive (this.gameObject, this.speed);
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

	void Attack (){
		print ("AM ATTACKING PLAYER AW SHIT NIGGA!");
	}




}
