using UnityEngine;
using System.Collections;

public class EnemyCowardAI : MonoBehaviour {

	public float cowardDistance = 2f;
	public float maxRunDistance = 6f;
	public float speed = 3f;

	private Rigidbody2D rb2d;
	private bool feared = false;
	private PlayerController player;
	private Animator animator;


	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.animator = this.GetComponent<Animator> ();
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

		// Make sure enemy is on the right layer
		this.transform.position = new Vector3(this.transform.position.x, 
		                                      this.transform.position.y, 
		                                      (float)(this.transform.position.y + 16));

		if (this.rb2d.velocity.x > 0) {
			animator.SetBool("moving", true);
			transform.localScale = new Vector3(1f, 1f, 1f);
		} else if (this.rb2d.velocity.x < 0) {
			animator.SetBool("moving", true);
			transform.localScale = new Vector3(-1f, 1f, 1f);
		} else {
			animator.SetBool("moving", false);
		}
	}
}
