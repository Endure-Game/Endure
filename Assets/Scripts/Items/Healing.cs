using UnityEngine;
using System.Collections;
using System;

public class Healing : MonoBehaviour {

	public int amount = 3;
	public Func<int> Woooooo;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D (Collision2D newOther) {
		Collider2D other = newOther.collider;
		if (other.tag == "Player") {
			PlayerController.instance.Health.ChangeHealth(this.amount);
		}

	}
}
