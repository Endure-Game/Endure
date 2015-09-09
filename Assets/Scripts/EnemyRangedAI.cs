using UnityEngine;
using System.Collections;

public class EnemyRangedAI : MonoBehaviour {
	public float speed = 3f;
	public float rangedDistance = 4f;
	public float cowardDistance = 2f;

	private bool active = false;
	private Rigidbody2D rb2d;
	private PlayerController player;
	private Animator animator;
	private RangedAttacker ranged;


	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.ranged = this.GetComponent<RangedAttacker> ();
		this.animator = this.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {
		if (true) {
			Vector2 heading = player.transform.position - this.transform.position;

			//if (true) {
			if (!ranged.Locked) {
				this.rb2d.velocity = this.speed * heading.normalized;
			} else {
				this.rb2d.velocity = Vector2.zero;
			}
			if (heading.magnitude < this.cowardDistance && !ranged.Locked){
				this.rb2d.velocity = this.speed * -heading.normalized;
			}
			else if (heading.magnitude < this.rangedDistance){
				//Vector3 target = heading;
				//print (target.normalized);
				//target.z = player.transform.position.z;
				this.ranged.Attack (player.transform.position);
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


	public void enemyActive (bool a){
		this.active = a;
	}
}
