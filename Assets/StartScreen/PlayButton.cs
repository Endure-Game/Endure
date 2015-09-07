using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayButton : MonoBehaviour {
	// Use this for initialization
	void Start () {
		this.GetComponent<Button> ().onClick.AddListener (this.OnClick);
	}
	
	void OnClick () {
		Application.LoadLevel (1);
	}
}
