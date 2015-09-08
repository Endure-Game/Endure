using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float speed = 4;
	public static PlayerController instance;
	public int bullets = 0;
	public int arrows = 0;
	public Region currentRegion;

	private int counter = 0;
	private Rigidbody2D rb2d;
	private Animator animator;
	private MeleeAttacker meleeAttacker;
	private RangedAttacker rangedAttacker;
	private ToolUser toolUser;

	private float playerRadius;

	private bool pusher = false;//FOR DEBUGGING

	public enum Control {
		SPACE,
		MOUSE
	}

	public class InventoryItem
	{
		public string name;
		public Sprite sprite;
		public string type;
		public int damage;
		public Control control;

		public string tool;

		public InventoryItem (string n, Sprite s, string t, int d, Control c, string tool) {
			this.name = n;
			this.sprite = s;
			this.type = t;
			this.damage = d;
			this.control = c;
			this.tool = tool;
		}
	};

	public List<InventoryItem> inventory = new List<InventoryItem> ();
	private int selectedInventory = 0;

	public int InventoryIndex {
		get {
			return selectedInventory;
		}
	}

	private List<string> upgrades = new List<string> ();


	// Use this for initialization
	void Start () {
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		this.animator = this.GetComponent<Animator> ();
		this.meleeAttacker = this.GetComponent<MeleeAttacker> ();
		this.rangedAttacker = this.GetComponent<RangedAttacker> ();
		this.toolUser = this.GetComponent<ToolUser> ();

		// Give player starting items
		//this.inventory.Add("sword");
	}

	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxisRaw ("Horizontal");
		float vertical = Input.GetAxisRaw ("Vertical");

		Vector2 playerSpeed = new Vector2 (horizontal, vertical);
		float magnitude = playerSpeed.magnitude;
		if (magnitude == 0) {
			magnitude = 1;
		}

		//DEBUGGING TOOL ONLY
		if (Input.GetKeyDown (KeyCode.I)){
			this.arrows = 10000;
			this.bullets = 10000;
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			if(this.pusher){
				this.pusher = false;
			} else{
				this.pusher = true;
			}
		}


		if (!meleeAttacker.Locked) {
			this.rb2d.velocity = this.speed * (playerSpeed / magnitude);
			// make sure player is at the right z distance for correct overlap
			this.transform.position = new Vector3(this.transform.position.x,
			                                      this.transform.position.y,
			                                      (float)(this.transform.position.y + 16));
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
			if (Input.GetKeyDown (KeyCode.Tab) || this.inventory.Count == 1) {
				print ("Do something");
				print ("Inv Count" + this.inventory.Count);
				if (this.selectedInventory >= this.inventory.Count - 1) {
					this.selectedInventory = 0;
				} else {
					this.selectedInventory++;
				}

				if(this.inventory[this.selectedInventory].type == "Melee"){
					this.meleeAttacker.damage = this.inventory[this.selectedInventory].damage;
					this.rangedAttacker.damage = 0;
					this.toolUser.toolType = "";
				} else if (this.inventory[this.selectedInventory].type == "Ranged") {
					this.rangedAttacker.damage = this.inventory[this.selectedInventory].damage;
					this.meleeAttacker.damage = 0;
					this.toolUser.toolType = "";
				} else if (this.inventory[this.selectedInventory].type == "Tool") {
					this.toolUser.toolType = this.inventory[this.selectedInventory].tool;
					this.rangedAttacker.damage = 0;
					this.meleeAttacker.damage = 0;
				}
				print (this.selectedInventory);
			}

			// ranged attack
			if (Input.GetMouseButtonDown (0)) {
				if (this.rangedAttacker.damage > 0) {
					Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					pz.z = 0;
					this.rangedAttacker.Attack(pz);
					this.animator.SetBool ("Idle", true);
					this.animator.SetTrigger ("Arrow");
				} else if (this.meleeAttacker.damage > 0) {
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
				} else if (this.toolUser.toolType.Length > 0) {
					int direction = this.animator.GetInteger ("Direction");

					if (direction == 0) {
						this.toolUser.UseSouth ();
					} else if (direction == 1) {
						this.toolUser.UseWest ();
					} else if (direction == 2) {
						this.toolUser.UseNorth ();
					} else if (direction == 3) {
						this.toolUser.UseEast ();
					}
				}
			}

			// blocking
			if (Input.GetKey (KeyCode.B)) {
				this.Health.Block = true;
			} else {
				this.Health.Block = false;
			}

			// melee attack
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (!this.Health.Block && this.meleeAttacker.damage > 0) {
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
				} else if (this.toolUser.toolType.Length > 0) {
					int direction = this.animator.GetInteger ("Direction");

					if (direction == 0) {
						this.toolUser.UseSouth ();
					} else if (direction == 1) {
						this.toolUser.UseWest ();
					} else if (direction == 2) {
						this.toolUser.UseNorth ();
					} else if (direction == 3) {
						this.toolUser.UseEast ();
					}
				} else if (this.rangedAttacker.damage > 0) {
					int direction = this.animator.GetInteger ("Direction");
					this.animator.SetBool ("Idle", true);
					this.animator.SetTrigger ("Arrow");

					if (direction == 0) {
						this.rangedAttacker.Attack (this.transform.position + new Vector3(0, -1));
					} else if (direction == 1) {
						this.toolUser.UseWest ();
						this.rangedAttacker.Attack (this.transform.position + new Vector3(-1, 0));
					} else if (direction == 2) {
						this.toolUser.UseNorth ();
						this.rangedAttacker.Attack (this.transform.position + new Vector3(0, 1));
					} else if (direction == 3) {
						this.toolUser.UseEast ();
						this.rangedAttacker.Attack (this.transform.position + new Vector3(1, 0));
					}
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
			this.inventory.Add(new InventoryItem(name, icon, "Melee", 3, Control.SPACE, ""));
			break;
		case "BowAndArrow":
			this.inventory.Add (new InventoryItem(name, icon, "Ranged", 3, Control.MOUSE, ""));
			this.arrows += 9;
			break;
		case "Axe":
			this.inventory.Add (new InventoryItem(name, icon, "Tool", 0, Control.SPACE, "Axe"));
			break;
		case "Rifle":
			this.inventory.Add (new InventoryItem(name, icon, "Ranged", 10, Control.MOUSE, ""));
			this.bullets += 3;
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

	//PUSHES THINGS OUT OF THE WAY
	void OnCollisionEnter2D (Collision2D collider) {
		if (this.pusher == true) {
			collider.transform.position += (collider.transform.position - this.transform.position).normalized * 2;
		}
	}
}
