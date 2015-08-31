using UnityEngine;
using System.Collections;

public class MeleeAttacker : MonoBehaviour {
	public int damage = 3;

	private GameObject weapon;
	private float elapsed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (this.weapon != null) {
			this.elapsed += Time.deltaTime;
			if (this.elapsed > 0.1f) {
				Destroy (this.weapon);
				this.weapon = null;
			}
		}
	}

	private void CreateOuch (bool horizontal, int direction) {
		Destroy (this.weapon);
		this.elapsed = 0;

		this.weapon = new GameObject ();
		this.weapon.name = "meleeWeapon";
		this.weapon.transform.parent = this.gameObject.transform;
		this.weapon.transform.position = this.gameObject.transform.position;

		BoxCollider2D collider = this.weapon.AddComponent<BoxCollider2D> ();
		collider.isTrigger = true;

		Vector2 playerSize = this.GetComponent<BoxCollider2D> ().size;
		collider.size = playerSize;

		float width = 0.6f;

		if (horizontal) {
			collider.size = new Vector2 (collider.size.x, width);
			collider.transform.Translate (0, (playerSize.y / 2 + width / 2) * direction, 0);
		} else {
			collider.size = new Vector2 (width, collider.size.y);
			collider.transform.Translate ((playerSize.x / 2 + width / 2) * direction, 0, 0);
		}

		Ouch ouch = this.weapon.AddComponent<Ouch> ();
		ouch.damage = this.damage;
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

	private IEnumerator WaitAndDestroy (float seconds, GameObject obj) {
		print ("Waiting");
		yield return new WaitForSeconds (seconds);
		print ("Wait over");
		Destroy (obj);
	}
}
