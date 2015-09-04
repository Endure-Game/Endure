using UnityEngine;
using System.Collections;

public class EnemyRangedAI : MonoBehaviour {
	public float speed = 3f;
	public float rangedDistance = 4f;
	public float cowardDistance = 2f;

	private bool active = false;
	private Rigidbody2D rb2d;
	private PlayerController player;
	private RangedAttacker ranged;

	
	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.ranged = this.GetComponent<RangedAttacker> ();
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
				this.rb2d.velocity = this.speed * - heading.normalized; 
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
	}
	
	
	public void enemyActive (bool a){
		this.active = a;
	}
}
