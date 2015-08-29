using UnityEngine;
using System.Collections;

public class Enemy1Script : MonoBehaviour {

	public float aggro = (float) 0.5;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake () {

	}
	void OnTriggerEnter2D () {
		//PlayerController.instance.IncrementCounter ();
		//Destroy (this.gameObject);
		//print ("Enemy has vision of player");
	}


	void Attack (){
		print ("AM ATTACKING PLAYER AW SHIT NIGGA!");
	}


}
