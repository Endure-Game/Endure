using UnityEngine;
using System.Collections;

public class EnemyCowardAI : MonoBehaviour {

	public float cowardDistance = 2f;
	public float maxRunDistance = 6f;
	public float speed = 3f;

	private Rigidbody2D rb2d;
	private bool feared = false;
	private PlayerController player;


	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (true) {
			Vector2 heading = player.transform.position - this.transform.position;
			if (heading.magnitude < this.cowardDistance){
				this.feared = true;
			} else if (heading.magnitude > this.maxRunDistance){
				this.feared = false;
			}
			if(this.feared){
				this.rb2d.velocity = this.speed * - heading.normalized; 
			} else {
				//this.rb2d.velocity.
				this.rb2d.velocity = Vector2.zero;
			}
		}
	}
}
