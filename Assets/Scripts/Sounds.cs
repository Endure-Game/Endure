using UnityEngine;
using System.Collections;

public class Sounds : MonoBehaviour {

	public static Sounds instance;
	public enum DeathSound {
		wihelmScream,
		death1
	}


	private AudioSource axeHit;
	private AudioSource shovelHit;
	private AudioSource sword;
	//private AudioSource death;
	private AudioSource wilhelmScream;
	private AudioSource swoosh;
	private AudioSource pickup;
	//public enum AudioSource {axeHit, shovelHit, sword, death1};
	
	// Use this for initialization
	void Start () {
		this.axeHit = gameObject.AddComponent<AudioSource> ();
		this.shovelHit = gameObject.AddComponent<AudioSource> ();
		this.sword = gameObject.AddComponent<AudioSource> ();
		this.wilhelmScream = gameObject.AddComponent<AudioSource> ();
		this.swoosh = gameObject.AddComponent<AudioSource> ();
		this.pickup = gameObject.AddComponent<AudioSource> ();

		this.axeHit.clip = Resources.Load ("Sounds/Chop1") as AudioClip;
		this.shovelHit.clip = Resources.Load ("Sounds/Shovel1A") as AudioClip;
		this.sword.clip = Resources.Load ("Sounds/SwordSwing1") as AudioClip;
		this.wilhelmScream.clip = Resources.Load ("Sounds/WilhelmScream") as AudioClip;
		this.swoosh.clip = Resources.Load ("Sounds/Swoosh2") as AudioClip;
		this.pickup.clip = Resources.Load ("Sounds/Pickup1") as AudioClip;
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
	public void PlayDeathSound(DeathSound play){
		switch (play)
		{
		case DeathSound.wihelmScream:
			this.wilhelmScream.Play ();
			break;
		default:
			this.wilhelmScream.Play ();
			break;
		}
	}
}
