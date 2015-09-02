using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
	
	public float speed = 4;
	public static PlayerController instance;
	public int ammo = 0;

	private int counter = 0;
	private Rigidbody2D rb2d;
	private Animator animator;
	private MeleeAttacker meleeAttacker;
	private RangedAttacker rangedAttacker;
	private float playerRadius;

	private class InventoryItem
	{
		public string name;
		public Sprite sprite;
		public InventoryItem (string n, Sprite s) {
			this.name = n;
			this.sprite = s;
		}
	};

	public GameObject inventoryDisplay;

	private List<InventoryItem> inventory = new List<InventoryItem> ();
	private List<string> upgrades = new List<string> ();

	private string currentMeleeWeapon = "";

	// Use this for initialization
	void Start () {
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.animator = this.GetComponent<Animator> ();
		this.meleeAttacker = this.GetComponent<MeleeAttacker> ();
		this.rangedAttacker = this.GetComponent<RangedAttacker> ();

		// Give player starting items
		//this.inventory.Add("sword");
	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		Vector2 playerSpeed = new Vector2 (horizontal, vertical);
		float magnitude = playerSpeed.magnitude;
		if(magnitude == 0){
			magnitude = 1;
		}


		if (!meleeAttacker.Locked) {
			this.rb2d.velocity = this.speed * (playerSpeed / magnitude);

			if (horizontal > 0) {
				this.animator.SetInteger ("Direction", 3);
				this.animator.SetBool ("Idle", false);
			} else if (horizontal < 0) {
				this.animator.SetInteger ("Direction", 1);
				this.animator.SetBool ("Idle", false);
			} else if (vertical > 0) {
				this.animator.SetInteger ("Direction", 2);
				this.animator.SetBool ("Idle", false);
			} else if (vertical < 0) {
				this.animator.SetInteger ("Direction", 0);
				this.animator.SetBool ("Idle", false);
			} else {
				this.animator.SetBool ("Idle", true);
			}

			// combat controls

			// ranged attack
			if (Input.GetMouseButtonDown (0) && this.rangedAttacker.damage > 0) {
				Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				pz.z = 0;

				this.rangedAttacker.Attack(pz);
			}

			// blocking
			if (Input.GetKey (KeyCode.B)) {
				this.Health.Block = true;
			} else {
				this.Health.Block = false;
			}

			// melee attack
			if (Input.GetKeyDown (KeyCode.Space) && !this.Health.Block && this.meleeAttacker.damage > 0) {
				this.animator.SetBool ("Idle", true);
				this.animator.SetTrigger ("Sword");
				int direction = this.animator.GetInteger ("Direction");

				if (direction == 0) {
					this.meleeAttacker.AttackSouth ();
				} else if (direction == 1) {
					this.meleeAttacker.AttackWest ();
				} else if (direction == 2) {
					this.meleeAttacker.AttackNorth ();
				} else if (direction == 3) {
					this.meleeAttacker.AttackEast ();
				}
			}
		} else {
			this.rb2d.velocity = Vector2.zero;
		}

	}
	//called before start
	void Awake () {
		if (PlayerController.instance == null) {
			PlayerController.instance = this;
		}

	}
	public void IncrementCounter () {
		this.counter ++;
		print (this.counter);
	}

	public Health Health {
		get {
			return this.GetComponent<Health> ();
		}
	}

	public void AddWeaponOrTool (string name, Sprite icon) {
		switch (name) {

		case "RustyMachete":
			this.inventory.Add(new InventoryItem(name, icon));
			GameObject invItem = new GameObject();
			invItem.name = "InventoryItem";
			invItem.transform.parent = this.inventoryDisplay.transform;
			invItem.AddComponent<Image> ().sprite = icon;
			if (this.currentMeleeWeapon.Length == 0) {
				this.currentMeleeWeapon = name;
				this.meleeAttacker.damage = 2;
			}
			break;

		default: 
			print ("Error: not a valid weapon or tool name");
			break;
		}
	}

	public void AddUpgrade (string name) {

		switch (name) {

		case "sneakers":
			this.speed *= 2;
			this.upgrades.Add(name);
			break;

		case "painKillers":
			this.Health.maxHealth *= 2;
			this.Health.ChangeHealth(this.Health.maxHealth);
			this.upgrades.Add(name);
			break;
			
		default: 
			print ("Error: not a valid upgrade name");
			break;
		}
	}
}
