using UnityEngine;
using System.Collections;

public class Enemy1Script : MonoBehaviour {

	public static float aggro = (float) 0.5;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*void OnTriggerEnter2D () {
		//PlayerController.instance.IncrementCounter ();
		//Destroy (this.gameObject);
		print ("Enemy has vision of player");
	}*/

	public static float getAggro(){
		return aggro;
	}
}
