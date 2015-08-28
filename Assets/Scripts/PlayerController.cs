using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	public float speed = 4;
	public static PlayerController instance;


	private int counter = 0;
	private Rigidbody2D rb2d;
	private Animator animator;
	private float playerRadius;
	
	// Use this for initialization
	void Start () {
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.animator = this.GetComponent<Animator> ();
		CircleCollider2D myCollider = transform.GetComponent <CircleCollider2D>();
		this.playerRadius = myCollider.radius;
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");
//		this.transform.Translate (horizontal, vertical, 0);
		Vector2 playerSpeed = new Vector2 (horizontal, vertical);
		float magnitude = Vector2.SqrMagnitude (playerSpeed);
		if(magnitude == 0){
			magnitude = 1;
		}
		print (magnitude);
		//print ("mAGNITUDE" + Vector2.SqrMagnitude(playerSpeed));

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

	public static bool checkAggro(float aggro, GameObject other){
		print (other.transform.position.x);
		float distance = Vector2.Distance (instance.transform.position, other.transform.position);
		
		float enemyRadius = instance.transform.localScale.x * instance.playerRadius * aggro;
		if (distance <= enemyRadius) {
			return true;
		} else {
			return false;
		}
	}
}
