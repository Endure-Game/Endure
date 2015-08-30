using UnityEngine;
using System.Collections;

public class MeleeAttacker : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void CreateOuch (bool horizontal, int direction) {
		GameObject weapon = new GameObject ();
		weapon.name = "meleeWeapon";
		weapon.transform.parent = this.gameObject.transform;
		weapon.transform.position = this.gameObject.transform.position;

		BoxCollider2D collider = weapon.AddComponent<BoxCollider2D> ();

		Vector2 playerSize = this.GetComponent<BoxCollider2D> ().size;
		collider.size = playerSize;

		float width = 0.4f;

		if (horizontal) {
			collider.size = new Vector2 (collider.size.x, width);
			collider.transform.Translate (0, (playerSize.y / 2) * direction, 0);
		} else {
			collider.size = new Vector2 (width, collider.size.y);
			collider.transform.Translate ((playerSize.x / 2) * direction, 0, 0);
		}

		weapon.AddComponent<Ouch> ();
	}

	public void AttackNorth () {
		CreateOuch (false, -1);
		print ("Attacking north");
	}
	
	public void AttackEast () {
		CreateOuch (true, 1);
		print ("Attacking east");
	}
	
	public void AttackWest () {
		CreateOuch (false, -1);
		print ("Attacking west");
	}
	
	public void AttackSouth () {
		CreateOuch (true, 1);
		print ("Attacking south");
	}
}
