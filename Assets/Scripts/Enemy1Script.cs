using UnityEngine;
using System.Collections;

public class Enemy1Script : MonoBehaviour {

	private float aggro = (float) 1.0;
	private Enemy1AI enemyAI;


	// Use this for initialization
	void Start () {
		enemyAI = this.gameObject.AddComponent <Enemy1AI> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake () {

	}

	void OnTriggerEnter2D (Collider2D other){
		this.enemyAI.enemyActive (true);
		/*if (other.gameObject.tag == "Player") {
			if(PlayerController.checkAggro (this.aggro, this.gameObject)){
				this.enemyAI.enemyActive (true);
			} else {
				print ("no aggro.... yet");
			}
		}*/
	}

	void OnTriggerExit2D (Collider2D other){
		this.enemyAI.enemyActive (false);
	}

	void Attack (){
		print ("AM ATTACKING PLAYER AW SHIT NIGGA!");
	}




}
