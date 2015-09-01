using UnityEngine;
using System.Collections;

public class Enemy1Script : MonoBehaviour {

	public float aggro = 10.0f;
	private Enemy1AI enemyAI;


	// Use this for initialization
	void Start () {
		enemyAI = this.gameObject.AddComponent <Enemy1AI> ();
	}
	
	// Update is called once per frame
	void Update () {
		float distance = (PlayerController.instance.transform.position - this.transform.position).magnitude;
		print (distance);
		if (distance < this.aggro) {
			this.enemyAI.enemyActive (true);
		} else {
			this.enemyAI.enemyActive (false);
		}
	}




}
