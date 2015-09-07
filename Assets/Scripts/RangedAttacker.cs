using UnityEngine;
using System.Collections;

public class RangedAttacker : MonoBehaviour {
	public int damage = 3;
	public float knockback = 0.5f;
	public float delay = 0.5f;
	
	public GameObject arrow;
	public GameObject bullet;
	private float elapsed;
	
	private float untilUnlocked;
	private bool locked = false;
	
	public bool Locked {
		get {
			return locked;
		}
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		this.untilUnlocked -= Time.deltaTime;
		
		if (this.untilUnlocked <= 0) {
			this.locked = false;
		}
	}
	
	public void Attack (Vector3 towards) {

		if (PlayerController.instance.inventory [PlayerController.instance.InventoryIndex].name == "BowAndArrow") {

			if (!this.locked && PlayerController.instance.arrows > 0) {
				this.elapsed = 0;
				this.untilUnlocked = this.delay;
				this.locked = true;

				GameObject weapon = Instantiate (this.arrow, this.transform.position, Quaternion.identity) as GameObject;
				weapon.GetComponent<Ouch> ().spawner = this.transform;

				Vector3 dir = towards - this.transform.position;
				float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
				weapon.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);

				PlayerController.instance.arrows --;
				
				Ouch ouch = weapon.GetComponent<Ouch> ();
				ouch.damage = this.damage;
				ouch.destroyOnTouch = true;
			}
		} else {
			if (!this.locked && PlayerController.instance.bullets > 0) {
				this.elapsed = 0;
				this.untilUnlocked = this.delay;
				this.locked = true;
				
				GameObject weapon = Instantiate (this.bullet, this.transform.position, Quaternion.identity) as GameObject;
				weapon.GetComponent<Ouch> ().spawner = this.transform;
				
				Vector3 dir = towards - this.transform.position;
				float angle = Mathf.Atan2 (dir.y, dir.x) * Mathf.Rad2Deg;
				weapon.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
				
				PlayerController.instance.bullets --;
				
				Ouch ouch = weapon.GetComponent<Ouch> ();
				ouch.damage = this.damage;
				ouch.destroyOnTouch = true;
			}
		}
	}
	
	public Vector2 Size {
		get {
			return this.GetComponent<BoxCollider2D> ().size;
		}
	}
}
