using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 1;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis ("Horizontal") * Time.deltaTime * this.speed;
		float vertical = Input.GetAxis ("Vertical") * Time.deltaTime * this.speed;
		this.transform.Translate (horizontal, vertical, 0);
	}
}
