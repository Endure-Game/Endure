using UnityEngine;
using System.Collections;

public class Enemy1AI : MonoBehaviour {
	public float speed = 3f;

	private bool active = false;
	private Rigidbody2D rb2d;
	private PlayerController player;

	private MeleeAttacker melee;

	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.melee = this.GetComponent<MeleeAttacker> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (true) {
			Vector2 heading = player.transform.position - this.transform.position;

			if (!melee.Locked) {
				rb2d.velocity = speed * heading.normalized;
			} else {
				rb2d.velocity = Vector2.zero;
			}

			if (heading.magnitude < melee.range + 0.5f) {
				Vector2 n = heading.normalized;
				if (n.x > Mathf.Sqrt (2) / 2) {
					melee.AttackEast ();
				} else if (n.x < - Mathf.Sqrt (2) / 2) {
					melee.AttackWest ();
				} else if (n.y > Mathf.Sqrt (2) / 2) {
					melee.AttackNorth ();
				} else if (n.y < -Mathf.Sqrt (2) / 2) {
					melee.AttackSouth ();
				}
			}
		} else {
			rb2d.velocity = Vector2.zero;
		}
	}


	public void enemyActive (bool a){
		this.active = a;
	}
}
