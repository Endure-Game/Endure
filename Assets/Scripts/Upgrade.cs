﻿using UnityEngine;
using System.Collections;

public class Upgrade : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter2D (Collision2D newOther) {
		Collider2D other = newOther.collider;
		print ("Is touching");
		if (other.tag == "Player") {
			PlayerController.instance.AddWeaponOrTool (this.gameObject.GetComponent<ItemController>().name);
			Destroy (this.gameObject);
		}
	}
}
