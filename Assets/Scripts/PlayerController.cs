using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed = 4;
	public static PlayerController instance;

	private int counter = 0;
	private Rigidbody2D rb2d;
	private Animator animator;
	
	// Use this for initialization
	void Start () {
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
//		this.transform.Translate (horizontal, vertical, 0);
		this.rb2d.velocity = this.speed * new Vector2 (horizontal, vertical);

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
	//called before start
	void Awake () {
		if (PlayerController.instance == null) {
			PlayerController.instance = this;
		}

	}
	public void IncrementCounter () {
		this.counter ++;
		print (this.counter);
	}

	void OnTriggerStay2D (Collider2D enemy){
		//PlayerController.instance.IncrementCounter ();
		//Destroy (this.gameObject);
		if(enemy.gameObject.tag == "Enemy"){
			print ("Enemy is nearby player!");
			CircleCollider2D myCollider = transform.GetComponent <CircleCollider2D>();
			float enemyX = enemy.gameObject.transform.position.x;
			float enemyY = enemy.gameObject.transform.position.y;
			float playerX = this.gameObject.transform.position.x;
			float playerY = this.gameObject.transform.position.y;
			//enemy.gameObject.GetComponent <Enemy1Script>().aggro;
			float distance = Vector2.Distance (this.transform.position, enemy.transform.position);
			float enemyRadius = this.transform.localScale.x * myCollider.radius * Enemy1Script.aggro;
			//print (other.gameObject.transform.position.x);
			//print (this.transform.localScale.x * myCollider.radius * aggroRadius);
			//print (distance + "|" + enemyRadius);
			if(distance <= enemyRadius){
				print ("AGGRO'D THE ENEMY!");
			}
			//print(Vector2.Distance (this.transform.position, enemy.transform.position));
			//print (Vector2.SqrMagnitude (enemyX, enemyY, playerX));
			
		}
	}
}
