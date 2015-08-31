using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthAnimation : MonoBehaviour {
	public float duration = 0.5f;
	public float moveSpeed = .05f;
	public int healthChange = 1;

	private float elapsed = 0;

	// Use this for initialization
	void Start () {

		Text display = this.GetComponentInChildren<Text> ();
		string text = "";

		if (healthChange >= 0) {
			text = "+" + healthChange;
			display.color = Color.green;
		} else {
			text = "" + healthChange;
		}

		display.text = text;
		display.CrossFadeAlpha (0, this.duration, false);
	}
	
	// Update is called once per frame
	void Update () {
		this.elapsed += Time.deltaTime;

		this.transform.Translate (0, (float) (Mathf.Sqrt (this.elapsed - Time.deltaTime) * this.moveSpeed), 0);

		if (this.elapsed > this.duration) {
			Destroy (this.gameObject);
		}
	}
}
