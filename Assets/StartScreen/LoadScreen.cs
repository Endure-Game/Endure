using UnityEngine;
using System.Collections;

public class LoadScreen : MonoBehaviour {
	public float duration = 1f;
	private float elapsed = 0;

	private CanvasGroup cg;

	// Use this for initialization
	void Start () {
		this.cg = this.GetComponent<CanvasGroup> ();
	}
	
	// Update is called once per frame
	void Update () {
		this.elapsed += Time.deltaTime;

		if (this.elapsed >= this.duration) {
			Application.LoadLevel (1);
		} else {
			this.cg.alpha = this.Smooth (this.elapsed / this.duration);
		}
	}

	float Smooth (float n) {
		return Mathf.Sin (n * Mathf.PI / 2);
	}
}
