using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public float speed = 4;
	public float sneak = 1f;

	public static PlayerController instance;
	public int bullets = 0;
	public int arrows = 0;

	public int maxBullets = 50;
	public int maxArrows = 50;

	public Region currentRegion;
	public bool locked = true;

	public GameObject itemAnimation;
	public GameObject hud;
	public GameObject miniMap;

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

		StartCoroutine (StartAnimations());
		// Give player starting items
		// this.inventory.Add("sword");
	}

	IEnumerator StartAnimations() {
		yield return new WaitForSeconds(1f);
		this.animator.SetTrigger ("GetUp");
		yield return new WaitForSeconds(1.5f);
		this.animator.SetTrigger ("ReadMap");
		yield return new WaitForSeconds(.5f);

		float stepSize = .05f;
		float alphaTimer = 0f;
		while (alphaTimer < 1f) {
			alphaTimer += stepSize;
			this.miniMap.GetComponent<CanvasGroup>().alpha = alphaTimer;
			yield return new WaitForSeconds(stepSize);
		}
		this.animator.SetTrigger ("StoreMap");
		yield return new WaitForSeconds(1f);
		this.locked = false;
	}

	// Update is called once per frame
	void Update () {

		// make sure player is at the right z distance for correct overlap
		this.transform.position = new Vector3(this.transform.position.x,
		                                      this.transform.position.y,
		                                      (float)(this.transform.position.y + 15.5));

		if (this.locked) {
			return;
		}

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
			this.Health.maxHealth = 50;
			this.Health.ChangeHealth(this.Health.maxHealth);
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			if(this.pusher){
				this.pusher = false;
			} else{
				this.pusher = true;
			}
		}


		if (!meleeAttacker.Locked && !rangedAttacker.Locked && !toolUser.Locked) {
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
			if (Input.GetKeyDown (KeyCode.Tab) || this.inventory.Count == 1) {
				if (this.selectedInventory >= this.inventory.Count - 1) {
					this.SwitchInventory (0);
				} else {
					this.SwitchInventory (this.selectedInventory + 1);
				}
			}

			var keys = new [] {
				KeyCode.Alpha1,
				KeyCode.Alpha2,
				KeyCode.Alpha3,
				KeyCode.Alpha4,
				KeyCode.Alpha5,
				KeyCode.Alpha6,
				KeyCode.Alpha7,
				KeyCode.Alpha8,
				KeyCode.Alpha9
			};

			for (var i = 0; i < keys.Length; i++) {
				if (Input.GetKeyDown (keys[i]) && i < this.inventory.Count) {
					this.SwitchInventory (i);
				}
			}

			// ranged attack
			if (Input.GetMouseButtonDown (0)) {
				if (this.rangedAttacker.damage > 0) {
					Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					pz.z = 0;
					this.rangedAttacker.Attack(pz);
					this.animator.SetBool ("Idle", true);
					if (this.inventory[selectedInventory].name == "Rifle") {
						this.animator.SetTrigger ("Gun");
					} else {
						this.animator.SetTrigger ("Arrow");
					}
				} else if (this.meleeAttacker.damage > 0) {
					this.animator.SetBool ("Idle", true);
					this.animator.SetTrigger ("Sword");
					int direction = this.animator.GetInteger ("Direction");

					Sounds.instance.Sword ();
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
					if (this.inventory[selectedInventory].name == "Axe") {
						Sounds.instance.Swoosh ();
						this.animator.SetTrigger ("Axe");
					} else if (this.inventory[selectedInventory].name == "Lockpick") {
						this.animator.SetTrigger ("Lockpick");
					} else if (this.inventory[selectedInventory].name == "Shovel") {
						this.animator.SetTrigger ("Shovel");
					} else if (this.inventory[selectedInventory].name == "Rope") {
						this.animator.SetTrigger ("Rope");
					}

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

					Sounds.instance.Sword ();
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
					this.animator.SetBool ("Idle", true);
					if (this.inventory[selectedInventory].name == "Axe") {
						Sounds.instance.Swoosh ();
						this.animator.SetTrigger ("Axe");
					} else if (this.inventory[selectedInventory].name == "Lockpick") {
						this.animator.SetTrigger ("Lockpick");
					} else if (this.inventory[selectedInventory].name == "Shovel") {
						this.animator.SetTrigger ("Shovel");
					} else if (this.inventory[selectedInventory].name == "Rope") {
						this.animator.SetTrigger ("Rope");
					}

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
					if (this.inventory[selectedInventory].name == "Rifle") {
						this.animator.SetTrigger ("Gun");
					} else {
						this.animator.SetTrigger ("Arrow");
					}

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

	void SwitchInventory (int i)
	{
		this.selectedInventory = i;
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
			this.AddToInventory (new InventoryItem(name, icon, "Melee", 3, Control.SPACE, ""));
			break;
		case "BowAndArrow":
			this.AddToInventory (new InventoryItem(name, icon, "Ranged", 3, Control.MOUSE, ""));
			this.arrows += 9;
			
			if (arrows > this.maxArrows) {
				this.arrows = this.maxArrows;
			}

			break;
		case "Axe":
			this.AddToInventory (new InventoryItem(name, icon, "Tool", 0, Control.SPACE, "Axe"));
			break;
		case "Rifle":
			this.AddToInventory (new InventoryItem(name, icon, "Ranged", 10, Control.MOUSE, ""));
			this.bullets += 3;
			if (bullets > this.maxBullets) {
				this.bullets = this.maxBullets;
			}
			break;
		case "Lockpick":
			this.AddToInventory (new InventoryItem(name, icon, "Tool", 0, Control.SPACE, "Lockpick"));
			break;
		case "Shovel":
			this.AddToInventory (new InventoryItem(name, icon, "Tool", 0, Control.SPACE, "Shovel"));
			break;
		case "Rope":
			this.AddToInventory (new InventoryItem(name, icon, "Tool", 0, Control.SPACE, "Rope"));
			break;
		default:
			print ("Error: not a valid weapon or tool name");
			break;
		}
	}

	// Only add item if there are no dupilcates, otherwise give some arrow ammo
	public void AddToInventory (InventoryItem newItem) {
		foreach (InventoryItem oldItem in this.inventory) {
			if (newItem.name == oldItem.name) {
				this.arrows += 10;
				
				if (arrows > this.maxArrows) {
					this.arrows = this.maxArrows;
				}
				return;
			}
		}
		this.inventory.Add (newItem);
	}

	public void AddUpgrade (string name) {

		string infoText = "";
		switch (name) {

		case "sneakers":
			this.speed *= 1.3f;
			this.upgrades.Add(name);
			infoText = "Speed Boost";
			break;

		case "painKillers":
			this.Health.maxHealth += 5;
			this.Health.ChangeHealth(this.Health.maxHealth);
			this.upgrades.Add(name);
			infoText = "+5 Max Health";
			break;

		case "adrenaline":
			// increase melee and ranged attack speed, but not animtion speed
			this.meleeAttacker.speed *= 1.2f;
			this.rangedAttacker.speed *= 1.2f;
			infoText = "Attack Speed Boost";
			break;

		case "bearrows":
			//changes arrows to BEARROWS
			GameObject bearrow = (GameObject)Resources.Load ("Ammo/Bearrow");
			this.rangedAttacker.arrow = bearrow;
			infoText = "The Right to Bear Arms";
			break;

		case "camo":
			this.sneak *= .8f;
			infoText = "Sneak Boost";
			break;

		case "map":
			this.hud.GetComponent<HUDController>().FillMap();
			infoText = "World Revealed";
			break;

		default:
			print ("Error: not a valid upgrade name");
			break;
		}

		GameObject itemInfo = Instantiate (itemAnimation, this.transform.position, Quaternion.identity) as GameObject;
		itemInfo.GetComponent<ItemAnimation> ().infoText = infoText;
	}

	//PUSHES THINGS OUT OF THE WAY
	void OnCollisionEnter2D (Collision2D collider) {
		if (this.pusher == true) {
			collider.transform.position += (collider.transform.position - this.transform.position).normalized * 2;
		}
	}
}
