using UnityEngine;
using System.Collections;

public class MoveAndDie : MonoBehaviour {
	public float speed = 10;
	public float life = 2;

	private float lived = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.lived += Time.deltaTime;
		Vector3 move = new Vector3 (this.speed * Time.deltaTime, 0, 0);
		this.transform.position += this.transform.rotation * move;

		if (this.lived > this.life) {
			Destroy (this.gameObject);
		}
	}
}
