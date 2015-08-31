using UnityEngine;
using System.Collections;

public class MeleeAttacker : MonoBehaviour {
	public int damage = 3;
	public float range = 0.6f;
	public float delay = 0.5f;

	private GameObject weapon;
	private float elapsed;
	private bool locked = false;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.elapsed += Time.deltaTime;

		if (this.weapon != null) {
			if (this.elapsed > 0.1f) {
				Destroy (this.weapon);
				this.weapon = null;
			}
		}

		if (this.elapsed >= this.delay) {
			this.locked = false;
		}
	}

	private void CreateOuch (bool horizontal, int direction) {
		if (!this.locked) {
			Destroy (this.weapon);
			this.elapsed = 0;
			this.locked = true;

			this.weapon = new GameObject ();
			this.weapon.name = "meleeWeapon";
			this.weapon.transform.parent = this.gameObject.transform;
			this.weapon.transform.position = this.gameObject.transform.position;

			BoxCollider2D collider = this.weapon.AddComponent<BoxCollider2D> ();
			collider.isTrigger = true;

			Vector2 playerSize = this.Size;
			collider.size = playerSize;

			if (horizontal) {
				collider.size = new Vector2 (collider.size.x, this.range);
				collider.transform.Translate (0, (playerSize.y / 2 + this.range / 2) * direction, 0);
			} else {
				collider.size = new Vector2 (this.range, collider.size.y);
				collider.transform.Translate ((playerSize.x / 2 + this.range / 2) * direction, 0, 0);
			}

			Ouch ouch = this.weapon.AddComponent<Ouch> ();
			ouch.damage = this.damage;
		}
	}

	public void AttackNorth () {
		CreateOuch (true, 1);
		print ("Attacking north");
	}
	
	public void AttackEast () {
		CreateOuch (false, 1);
		print ("Attacking east");
	}
	
	public void AttackWest () {
		CreateOuch (false, -1);
		print ("Attacking west");
	}
	
	public void AttackSouth () {
		CreateOuch (true, -1);
		print ("Attacking south");
	}

	public Vector2 Size {
		get {
			return this.GetComponent<BoxCollider2D> ().size;
		}
	}
}
