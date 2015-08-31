using UnityEngine;
using System.Collections;

public class Enemy1AI : MonoBehaviour {
	public float speed = 3f;

	private bool active = false;
	private Rigidbody2D rb2d;
	private PlayerController player;

	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			Vector2 heading = player.transform.position - this.transform.position;
			rb2d.velocity = speed * heading.normalized;
		}
	}


	public void enemyActive (bool a){
		this.active = a;
	}
}
