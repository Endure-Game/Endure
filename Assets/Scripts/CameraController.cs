using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public static CameraController instance;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Awake (){
		if (CameraController.instance == null) {
			CameraController.instance = this;
		}
	}

	public float getHeight (){
		return Camera.main.orthographicSize * 2;
	}

	public float getWidth (){
		return Camera.main.orthographicSize * Camera.main.aspect * 2;
	}
}
