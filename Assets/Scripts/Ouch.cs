using UnityEngine;
using System.Collections;

public class Ouch : MonoBehaviour {
	public int damage = 3;

	// Use this for initialization
	void Start () {
		BoxCollider2D collider = this.gameObject.AddComponent<BoxCollider2D> ();
		collider.isTrigger = true;
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (!collider.isTrigger) {
			Health target = collider.GetComponent<Health> ();
			if (target != null) {
				target.ChangeHealth (-damage);
			}
		}
	}
}
