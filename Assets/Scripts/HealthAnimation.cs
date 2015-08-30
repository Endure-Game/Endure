using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthAnimation : MonoBehaviour {
	public float duration = 1;
	public float moveSpeed = 2;
	public int healthChange = 1;

	private float elapsed = 0;

	// Use this for initialization
	void Start () {
		string text = "";

		if (healthChange > 0) {
			text = "+" + healthChange;
		} else {
			text = "-" + healthChange;
		}

		Text display = this.GetComponentInChildren<Text> ();
		display.text = text;
		display.CrossFadeAlpha (0, this.duration, false);
	}
	
	// Update is called once per frame
	void Update () {
		this.elapsed += Time.deltaTime;

		this.transform.Translate (0, this.moveSpeed * Time.deltaTime, 0);

		if (this.elapsed > this.duration) {
			Destroy (this.gameObject);
		}
	}
}
