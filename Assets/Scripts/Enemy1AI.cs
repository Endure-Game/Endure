using UnityEngine;
using System.Collections;

public class Enemy1AI : MonoBehaviour {
	public float speed = 3f;
	public float aggro = 2f;

	private bool active = false;
	private Rigidbody2D rb2d;
	private PlayerController player;
	private Animator animator;

	private MeleeAttacker melee;

	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.melee = this.GetComponent<MeleeAttacker> ();
		this.animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.active) {
			Vector2 heading = player.transform.position - this.transform.position;
			//print (heading.magnitude + "|" + this.aggro);
			if(heading.magnitude < this.aggro){
				if (!melee.Locked) {
					this.rb2d.velocity = speed * heading.normalized;
				} else {
					this.rb2d.velocity = Vector2.zero;
				}

				if (heading.magnitude < melee.range + 0.5f) {
					Vector2 n = heading.normalized;
					if (n.x > Mathf.Sqrt (2) / 2) {
						this.melee.AttackEast ();
					} else if (n.x < - Mathf.Sqrt (2) / 2) {
						this.melee.AttackWest ();
					} else if (n.y > Mathf.Sqrt (2) / 2) {
						this.melee.AttackNorth ();
					} else if (n.y < -Mathf.Sqrt (2) / 2) {
						this.melee.AttackSouth ();
					}
				}
			} else {
				//TODO RANDOM MOVEMENT
				print("Stopped chasing player");
				this.rb2d.velocity = Vector2.zero;
			}
		} else {
			this.rb2d.velocity = Vector2.zero;
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

	void OnTriggerEnter2D (Collider2D collided){
		if(collided.tag == "Player"){
			print ("Activated Enemy");
			this.active = true;
		}
	}

	void OnTriggerExit2D (Collider2D collided) {
		CircleCollider2D playerCollider = player.GetComponent<CircleCollider2D> ();
		Vector2 heading = player.transform.position - this.transform.position;
		print ("collider radius: " + playerCollider.radius + " headingmag: " + heading.magnitude);
		if (collided.tag == "Player" && heading.magnitude >= playerCollider.radius) {
			print ("Deactivated Enemy");
			this.active = false;
		}
	}


	public void enemyActive (bool a){
		this.active = a;
	}
}
