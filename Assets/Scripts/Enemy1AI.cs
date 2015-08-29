using UnityEngine;
using System.Collections;

public class Enemy1AI : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void enemyActive (GameObject enemy, float speed){
		print (speed);
		print (enemy.transform.position.x);

		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
		
		Vector2 playerSpeed = new Vector2 (horizontal, vertical);
		float magnitude = playerSpeed.magnitude;
		if(magnitude == 0){
			magnitude = 1;
		}
		
		
		
		this.rb2d.velocity = this.speed * (playerSpeed/magnitude);
		
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
		}
	}
}
