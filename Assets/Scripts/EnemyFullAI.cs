using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFullAI : MonoBehaviour {

	//Basic AI Stats
	public float speed = 3f;
	public float aggro = 5f;
	public float deAggro = 10f;

	//For idle movement
	public float moveDuration = 0.8f;
	public float idleDuration = 0.6f;

	//Basic AI stats that player shouldn't reaaaally care about
	private bool active = false;
	private Rigidbody2D rb2d;
	private PlayerController player;
	private Vector3 lastPlayerPos;
	private Vector3 heading;//TODO FIGURE OUT WHY I HAVE THIS SHIT
	private Vector3 targetHeading;
	//private PathFinding pathFinding;// = GetComponent<PathFinding> ();
	private List<Vector3> path;
	//For idle movement (Privately set)
	private Animator animator;
	private Vector3 oldPosition;
	private float animationTime = 0f;

	private float hitDelay = 0.10f;
	private EnemyFullAI standStill;
	private float stunTime;
	private bool isStunned;


	//For weapons
	//private MeleeAttacker melee;
	//private RangedAttacker ranged;
	[System.Serializable]
	public class Melee {
		public bool isMelee = false;
		private MeleeAttacker meleeWeapon;
		public void setWeapon(MeleeAttacker meleeWep){
			this.meleeWeapon = meleeWep;
		}
		public MeleeAttacker getWeapon () {
			return this.meleeWeapon;
		}
	}
	public Melee melee;

	[System.Serializable]
	public class Ranged {
		public bool isRanged = false;
		public float rangedDistance = 4f;
		private RangedAttacker rangedWeapon;
		public void setWeapon(RangedAttacker rangedWep){
			this.rangedWeapon = rangedWep;
		}
		public RangedAttacker getWeapon () {
			return this.rangedWeapon;
		}
	}
	public Ranged ranged;

	[System.Serializable]
	public class Coward {
		public bool isCoward = false;
		public float cowardDistance = 2f;
		public float maxRunDistance = 6f;
		private bool feared = false;
		public bool isFeared (){
			return this.feared;
		}
		public void yesFear(){
			this.feared = true;
		}
		public void noFear (){
			this.feared = false;
		}
	}
	public Coward coward;

	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.rb2d = this.GetComponent<Rigidbody2D> ();
		if (this.melee.isMelee) {
			this.melee.setWeapon(this.GetComponent<MeleeAttacker> ());
		}
		if (this.ranged.isRanged) {
			this.ranged.setWeapon (this.GetComponent<RangedAttacker>());
		}
		this.animator = this.GetComponent<Animator> ();

	}



	void Awake (){
		//this.pathFinding = GetComponent<PathFinding> ();

	}

	// Update is called once per frame
	void Update () {
		if (this.active) {
			this.targetHeading = this.player.transform.position - this.transform.position;
			//print (heading.magnitude + "|" + this.aggro);
			if(this.targetHeading.magnitude < this.aggro){
				this.lastPlayerPos = player.transform.position;
			}
			if (this.targetHeading.magnitude < this.deAggro){
				 if (this.ranged.isRanged){
					this.RangedAttack ();
				} else if (this.coward.isCoward){
					this.CowardRun ();
				}else if(this.melee.isMelee){
					this.MeleeAttack ();
				}
				//this.MeleeAttack ();
			} else if(this.targetHeading.magnitude >= this.deAggro){
				//Double Check conditional
				if(this.coward.isCoward){
					this.CowardRun();
				} else {
					this.IdleMovement ();
				}
			}

			this.stunTime += Time.deltaTime;
			if (stunTime >= this.hitDelay && this.isStunned == true) {
				this.standStill.coward.isCoward = false;
				this.isStunned = false;
			}

			// Make sure enemy is on the right layer
			if(this.transform.position.z != (float)(this.transform.position.y + 16)){
				this.transform.position = new Vector3(this.transform.position.x,
				                                      this.transform.position.y,
				                                      (float)(this.transform.position.y + 16));
			}

			//Animation for player movement
			if (this.rb2d.velocity.x > 0) {
				animator.SetBool("moving", true);
				transform.localScale = new Vector3(1f, 1f, 1f);
			} else if (this.rb2d.velocity.x < 0) {
				animator.SetBool("moving", true);
				transform.localScale = new Vector3(-1f, 1f, 1f);
			} else {
				animator.SetBool("moving", false);
			}
		}

	}

	public void Stun (GameObject loser) {
		this.stunTime = 0f;
		this.standStill = loser.GetComponent<EnemyFullAI>();
		this.standStill.coward.isCoward = true;
		this.isStunned = true;
	}
	//moveTo should be a vector3 of position
	public void MoveTo (Vector3 moveTo){
		//if (this.path == null || this.path.Count == 0) {
			//moveTo.z = 0f;
			//Vector2 curPos = this.transform.position;
			//this.path = this.pathFinding.PathFinder (this.transform.position, moveTo);
		//}
		//Vector2 newHeading = new Vector2(path[0].x, path[0].y);
		//Vector2 newPosition = new Vector2 (this.transform.position.x, this.transform.position.y);
		//this.heading = newHeading - newPosition;
		this.heading = moveTo - this.transform.position;

		this.rb2d.velocity = this.heading.normalized * this.speed;
	}

	void MeleeAttack (){
		//heading = this.lastPlayerPos - this.transform.position;
		this.oldPosition = this.transform.position;
		if (!this.melee.getWeapon().Locked) {
			this.MoveTo (this.lastPlayerPos);
		} else {
			this.rb2d.velocity = Vector2.zero;
		}

		if (this.targetHeading.magnitude < this.melee.getWeapon().range + 0.5f) {
			Vector3 n = this.targetHeading.normalized;
			if (n.x > Mathf.Sqrt (2) / 2) {
				this.melee.getWeapon().AttackEast ();
			} else if (n.x < - Mathf.Sqrt (2) / 2) {
				this.melee.getWeapon().AttackWest ();
			} else if (n.y > Mathf.Sqrt (2) / 2) {
				this.melee.getWeapon().AttackNorth ();
			} else if (n.y < -Mathf.Sqrt (2) / 2) {
				this.melee.getWeapon().AttackSouth ();
			}
		}
	}
	void RangedAttack (){
		//heading = player.transform.position - this.transform.position;

		//if (true) {
		//print ("Distance: " + this.targetHeading.magnitude + "|cowardDistance: " + this.coward.cowardDistance);
		this.oldPosition = this.transform.position;
		if (this.coward.isCoward && this.targetHeading.magnitude < this.coward.cowardDistance) {
			this.CowardRun ();
		} else if (this.targetHeading.magnitude < this.ranged.rangedDistance){
			//print ("PRINT SOME SHIT OUT2");
			//Vector3 target = heading;
			//print (target.normalized);
			//target.z = player.transform.position.z;
			this.ranged.getWeapon().Attack (player.transform.position);
		} else if (!this.ranged.getWeapon().Locked) {
			//print ("PRINT SOME SHIT OUT");
			this.MoveTo (lastPlayerPos);
			//this.rb2d.velocity = this.speed * heading.normalized;
		} else {
			this.rb2d.velocity = Vector2.zero;
		}
		/*if (heading.magnitude < this.cowardDistance && !ranged.Locked){
			this.rb2d.velocity = this.speed * -heading.normalized;
		}*/

	}
	void CowardRun (){
		this.heading = player.transform.position - this.transform.position;

		if (heading.magnitude < this.coward.cowardDistance){
			this.coward.yesFear();
		} else if (heading.magnitude > this.coward.maxRunDistance){
			this.coward.noFear ();
		}
		if(this.coward.isFeared()){
			this.rb2d.velocity = this.speed * - heading.normalized;
			this.oldPosition = Vector3.zero;
		} else {
			IdleMovement ();
		}
	}
	void IdleMovement (){
		if(this.oldPosition == Vector3.zero){
			this.oldPosition = this.transform.position;
		}

		this.animationTime += Time.deltaTime;

		//print (this.oldPosition);

		if (this.animationTime < this.idleDuration){
			this.rb2d.velocity = Vector2.zero;
		} else {
			if (this.animationTime > this.moveDuration + this.idleDuration) {
				this.rb2d.velocity = Vector2.zero;
				this.animationTime = 0;
			} else if (this.rb2d.velocity == Vector2.zero){
				float rx = Random.Range (-1f, 1f);
				float ry = Random.Range (-1f, 1f);
				this.moveDuration = Random.Range(.6f, 2f);
				Vector3 idleHeading = this.oldPosition - this.transform.position;
				this.rb2d.velocity = speed * (idleHeading - new Vector3(rx, ry)).normalized;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D collided){
		if(collided.tag == "Player"){
			//print ("Activated Enemy");
			this.active = true;
		}
	}

	void OnTriggerExit2D (Collider2D collided) {
		CircleCollider2D playerCollider = player.GetComponent<CircleCollider2D> ();
		this.targetHeading = player.transform.position - this.transform.position;
		//print ("collider radius: " + playerCollider.radius + " headingmag: " + heading.magnitude);
		if (collided.tag == "Player" && this.targetHeading.magnitude >= playerCollider.radius) {
			//print ("Deactivated Enemy");
			this.active = false;
		}
	}


	public void enemyActive (bool a){
		this.active = a;
	}
}
