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
			print (this.gameObject.tag);
			print ("Enemy is attempting to move");
			//Rigidbody2D rb2d = this.GetComponent<Rigidbody2D> ();
		
			//Vector2 playerSpeed = new Vector2 (1, 1);
			Vector2 heading = player.transform.position - this.transform.position;
			//float horizontal = 1;
			//float vertical = -1;
		
			//Vector2 enemySpeed = new Vector2 (horizontal, vertical);
		
			//print (speed * heading.normalized);
			rb2d.velocity = speed * heading.normalized;
			//rb2d.velocity = new Vector2 (0, 10);
			//print (rb2d.velocity);
		}
	}


	public void enemyActive (bool a){
		this.active = a;
		//print (speed);
		//print (enemy.transform.position.x);
		if (active) {
			print ("Enemy is active!");
		} else {
			print ("Enemy disactivated");
		}

	}
}
