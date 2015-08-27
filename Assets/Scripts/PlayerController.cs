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

		if (vertical > 0) {
			this.animator.SetInteger ("Direction", 2);
		} else if (vertical < 0) {
			this.animator.SetInteger ("Direction", 0);
		} else if (horizontal > 0) {
			this.animator.SetInteger ("Direction", 3);
		} else if (horizontal < 0) {
			this.animator.SetInteger ("Direction", 1);
		}

		print (this.animator.GetInteger ("Direction"));
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
}
