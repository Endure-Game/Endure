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
		DoOwie (collider);
	}

	void OnColliderEnter2D (Collider2D collider) {
		DoOwie (collider);
	}

	void DoOwie (Collider2D collider){
		MeleeAttacker meleeAttacker = this.gameObject.GetComponent<MeleeAttacker> ();
		print (meleeAttacker);
		if (collider.tag != this.tag) {
			if (meleeAttacker != null) {
				if (meleeAttacker.Locked == false) {
					OnOwie (collider);
					meleeAttacker.LockAttacker ();
				}
			} else {
				OnOwie (collider);
			}
		}
	}





	void OnOwie (Collider2D collider) {
		//this.death.Play ();
		if (!collider.isTrigger && collider.transform != this.spawner && this.tag != collider.tag) {
			Health target = collider.GetComponent<Health> ();
			if (target != null) {
				target.ChangeHealth (-damage);
				collider.transform.position += (collider.transform.position - this.transform.position).normalized * this.knockback;
				//stun the reciepient for a short amount of time
				/*GameObject enemy = collider.gameObject;
				if(enemy.tag != "Player"){
					EnemyFullAI script = enemy.GetComponent<EnemyFullAI>();
					script.Stun (enemy);
				}*/
				Sounds.instance.Hit();
			}

			if (this.destroyOnTouch) {
				//this.hit.Play ();
				//Destroy (this.gameObject);
				TrailRenderer trail = this.GetComponent<TrailRenderer>();
				if(trail != null) {
					trail.enabled = false;
				}
				this.GetComponent<Renderer>().enabled = false;
				this.transform.position = Vector3.one * 9999999f;
				//Destroy(gameObject, this.hit.clip.length);
			}
		}
	}
}
