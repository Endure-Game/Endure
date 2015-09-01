using UnityEngine;
using System.Collections;

public class Ouch : MonoBehaviour {
	public int damage = 3;
	public float knockback = 0.5f;
	public Transform spawner;
	public bool destroyOnTouch = false;

	// Use this for initialization
	void Start () {
		BoxCollider2D collider = this.gameObject.GetComponent<BoxCollider2D> ();
		if (collider == null) {
			collider = this.gameObject.AddComponent<BoxCollider2D> ();
			collider.isTrigger = true;
		}
	}

	void OnTriggerEnter2D (Collider2D collider) {
		if (!collider.isTrigger && collider.transform != this.spawner) {
			Health target = collider.GetComponent<Health> ();
			if (target != null) {
				target.ChangeHealth (-damage);
			}

			collider.transform.position += (collider.transform.position - this.transform.position).normalized * this.knockback;
			if (this.destroyOnTouch) {
				Destroy (this.gameObject);
			}
		}
	}
}
