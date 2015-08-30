using UnityEngine;
using System.Collections;

public class Enemy1AI : MonoBehaviour {

	private PlayerController player;
	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void enemyActive (GameObject enemy, float speed){
		print (speed);
		print (enemy.transform.position.x);
		Rigidbody2D rb2d = enemy.GetComponent<Rigidbody2D>();

		//Vector2 playerSpeed = new Vector2 (1, 1);
		Vector2 heading = player.transform.position - enemy.transform.position;
		//float horizontal = 1;
		//float vertical = -1;
		
		//Vector2 enemySpeed = new Vector2 (horizontal, vertical);
		float magnitude = heading.magnitude;
		if(magnitude == 0){
			magnitude = 1;
		}
		
		
		rb2d.velocity = speed * (heading/magnitude);
		
		/*this.rb2d.velocity = this.speed * (playerSpeed/magnitude);
		
		if (horizontal > 0) {
			this.animator.SetInteger ("Direction", 3);
			this.animator.SetBool ("Idle", false);
		} else if (horizontal < 0) {
			this.animator.SetInteger ("Direction", 1);
			this.animator.SetBool ("Idle", false);
		} else if (vertical > 0) {
			this.animator.SetInteger ("Direction", 2);
			this.animator.SetBool ("Idle", false);
		} else if (vertical < 0) {
			this.animator.SetInteger ("Direction", 0);
			this.animator.SetBool ("Idle", false);
		} else {
			this.animator.SetBool ("Idle", true);
		}*/
	}
}
