using UnityEngine;
using System.Collections;

public class EnemyCowardAI : MonoBehaviour {

	public float cowardDistance = 2f;
	public float maxRunDistance = 6f;
	public float speed = 3f;
	public float moveDuration = 0.8f;
	public float idleDuration = 0.6f;
	
	
	private bool active = false;
	private Rigidbody2D rb2d;
	private bool feared = false;
	private PlayerController player;
	private Animator animator;
	private Vector3 oldPosition;
	private float animationTime = 0f;


	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (this.active) {
			Vector2 heading = player.transform.position - this.transform.position;
			if (heading.magnitude < this.cowardDistance){
				this.feared = true;
			} else if (heading.magnitude > this.maxRunDistance){
				this.feared = false;
			}
			if(this.feared){
				this.rb2d.velocity = this.speed * - heading.normalized; 
			} else {
				if(this.oldPosition == Vector3.zero){
					this.oldPosition = this.transform.position;
				}
				
				this.animationTime += Time.deltaTime;
								
				if(this.animationTime < this.idleDuration){
					this.rb2d.velocity = Vector2.zero;
				} else if (this.animationTime >= this.idleDuration){
					if(this.animationTime > this.moveDuration + this.idleDuration){
						this.rb2d.velocity = Vector2.zero;
						this.animationTime = 0;						
					} else if (this.rb2d.velocity == Vector2.zero){
						float rx = Random.Range (-1f, 1f);
						float ry = Random.Range (-1f, 1f);
						Vector2 idleHeading = this.oldPosition - this.transform.position;
						this.rb2d.velocity = speed * (idleHeading - new Vector2(rx, ry)).normalized;
					}
				}
			}
			// Make sure enemy is on the right layer
			if(this.transform.position.z != (float)(this.transform.position.y + 16)){
				this.transform.position = new Vector3(this.transform.position.x, 
				                                      this.transform.position.y, 
				                                      (float)(this.transform.position.y + 16));
			}
			
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

	void OnTriggerEnter2D (Collider2D collided){
		if(collided.tag == "Player"){
			print ("Activated Enemy");
			this.active = true;
		}
	}
	
	void OnTriggerExit2D (Collider2D collided) {
		CircleCollider2D playerCollider = player.GetComponent<CircleCollider2D> ();
		Vector2 heading = player.transform.position - this.transform.position;
		//print ("collider radius: " + playerCollider.radius + " headingmag: " + heading.magnitude);
		if (collided.tag == "Player" && heading.magnitude >= playerCollider.radius) {
			print ("Deactivated Enemy");
			this.active = false;
		}
	}
}
