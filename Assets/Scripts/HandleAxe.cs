using UnityEngine;
using System.Collections;

public class HandleAxe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//this.axeHit = gameObject.AddComponent<AudioSource> ();
		//this.axeHit.clip = Resources.Load ("Sounds/Chop1") as AudioClip;
	}
	
	/*// Update is called once per frame
	void Update () {
		
	}*/
	
	void OnTriggerEnter2D (Collider2D collider) {
		if (collider.tag == "Axe") {
			Sounds.instance.AxeHit ();
			Destroy (this.gameObject);
		}
	}
}
