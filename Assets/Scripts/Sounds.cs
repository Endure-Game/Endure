using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	//Instantiates Sounds once so it can be called repeatedly by other scripts
	public static Sounds instance;
	//enum function to allow Health to have a dropdown box for death sounds
	public enum DeathSound {
		wihelmScream,
		manDeath1,
		bearDeath1,
		slimeDeath1,
		breath
	}

	//Instantiates the sounds
	private AudioSource axeHit;
	private AudioSource shovelHit;
	private AudioSource sword;
	//private AudioSource death;
	private AudioSource swoosh;
	private AudioSource pickup;
	private AudioSource hit;
	private AudioSource rope;
	private AudioSource lockpick;
	//public enum AudioSource {axeHit, shovelHit, sword, death1};

	//Death Sounds
	private AudioSource wilhelmScream;
	private AudioSource manDeath1;
	private AudioSource bearDeath1;
	private AudioSource slimeDeath1;
	private AudioSource breath;
	// Use this for initialization
	void Start () {
		//Attaches game objects to each sound
		this.axeHit = gameObject.AddComponent<AudioSource> ();
		this.shovelHit = gameObject.AddComponent<AudioSource> ();
		this.sword = gameObject.AddComponent<AudioSource> ();
		this.swoosh = gameObject.AddComponent<AudioSource> ();
		this.pickup = gameObject.AddComponent<AudioSource> ();
		this.hit = gameObject.AddComponent<AudioSource> ();
		this.rope = gameObject.AddComponent<AudioSource> ();
		this.lockpick = gameObject.AddComponent<AudioSource> ();

		//Loads resources here.
		//You can modify the source of these sounds to change the sound effect
		this.axeHit.clip = Resources.Load ("Sounds/Chop1") as AudioClip;
		this.shovelHit.clip = Resources.Load ("Sounds/Shovel1A") as AudioClip;
		this.sword.clip = Resources.Load ("Sounds/SwordSwing1") as AudioClip;
		this.swoosh.clip = Resources.Load ("Sounds/Swoosh2") as AudioClip;
		this.pickup.clip = Resources.Load ("Sounds/Pickup2") as AudioClip;
		this.hit.clip = Resources.Load ("Sounds/Hurt2") as AudioClip;
		this.rope.clip = Resources.Load ("Sounds/Rope") as AudioClip;
		this.lockpick.clip = Resources.Load ("Sounds/Lockpick") as AudioClip;

		//Death Sounds
		this.wilhelmScream = gameObject.AddComponent<AudioSource> ();
		this.wilhelmScream.clip = Resources.Load ("Sounds/WilhelmScream") as AudioClip;
		this.manDeath1 = gameObject.AddComponent<AudioSource> ();
		this.manDeath1.clip = Resources.Load ("Sounds/ManDeath1") as AudioClip;
		this.bearDeath1 = gameObject.AddComponent<AudioSource> ();
		this.bearDeath1.clip = Resources.Load ("Sounds/BearDeath1") as AudioClip;
		this.slimeDeath1 = gameObject.AddComponent<AudioSource> ();
		this.slimeDeath1.clip = Resources.Load ("Sounds/SlimeDeath1") as AudioClip;
		this.breath = gameObject.AddComponent<AudioSource> ();
		this.breath.clip = Resources.Load ("Sounds/Breath") as AudioClip;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	//called before start
	void Awake () {
		if (Sounds.instance == null) {
			Sounds.instance = this;
		}
	}

	//Public functions to allow sounds to be called easily
	//They can be called via Sounds.instance.AxeHIt() for example
	public void AxeHit(){
		this.axeHit.Play ();
	}
	public void ShovelHit(){
		this.shovelHit.Play ();
	}
	public void Sword(){
		this.sword.Play ();
	}
	public void Swoosh(){
		this.swoosh.Play ();
	}
	public void Pickup(){
		this.pickup.Play ();
	}
	public void Hit(){
		this.hit.Play ();
	}
	public void Rope (){
		this.rope.Play ();
	}
	public void Lockpick (){
		this.lockpick.Play ();
	}
	public void WihelmScream(){
		this.wilhelmScream.Play ();
	}
	public void ManDeath1(){
		this.manDeath1.Play ();
	}
	public void BearDeath1() {
		this.bearDeath1.Play ();
	}
	public void SlimeDeath1() {
		this.slimeDeath1.Play ();
	}
	public void Breath(){
		this.breath.Play ();
	}
	//Depending on dropdown box, one of these sounds will be played
	public void PlayDeathSound(DeathSound play){
		switch (play)
		{
		case DeathSound.wihelmScream:
			this.WihelmScream();
			break;
		case DeathSound.manDeath1:
			this.ManDeath1();
			break;
		case DeathSound.bearDeath1:
			this.BearDeath1();
			break;
		case DeathSound.slimeDeath1:
			this.SlimeDeath1();
			break;
		case DeathSound.breath:
			this.Breath();
			break;
		default:
			this.WihelmScream();
			break;
		}
	}
}
