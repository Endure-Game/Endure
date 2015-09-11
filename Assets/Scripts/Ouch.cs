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
		StartCoroutine (Wait ());
	}

	void OnTriggerEnter2D (Collider2D collider) {
		OnOwie (collider);
	}

	void OnColliderEnter2D (Collider2D collider) {
		OnOwie (collider);
	}

	IEnumerator Wait (){
		yield return new WaitForSeconds (5);

	}

	void Stun (Rigidbody2D loser){
		loser.Sleep ();
		loser.Sleep ();
		loser.Sleep ();
		this.Wait ();
		loser.WakeUp ();
	}

	void OnOwie (Collider2D collider) {
		if (!collider.isTrigger && collider.transform != this.spawner) {
			Health target = collider.GetComponent<Health> ();
			if (target != null) {
				target.ChangeHealth (-damage);
				collider.transform.position += (collider.transform.position - this.transform.position).normalized * this.knockback;
				//stun the reciepient for a short amount of time
				Rigidbody2D enemy = collider.GetComponent<Rigidbody2D>();
				this.Stun(enemy);
			}

			if (this.destroyOnTouch) {
				Destroy (this.gameObject);
			}
		}
	}
}
