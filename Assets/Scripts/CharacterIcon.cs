using UnityEngine;
using System.Collections;

public class CharacterIcon : MonoBehaviour {
	public GameObject world;
	private RoomManager roomManager;
	private RectTransform rt;

	// Use this for initialization
	void Start () {
		this.roomManager = world.GetComponent<RoomManager> ();
		this.rt = this.GetComponent<RectTransform> ();
	}

	// Update is called once per frame
	void Update () {

		if (PlayerController.instance != null) {
			var roomWidth = this.roomManager.columns;
			var roomHeight = this.roomManager.rows;

			var newLocation = PlayerController.instance.transform.position;
			newLocation += new Vector3 (roomWidth / 2, roomHeight / 2);

			newLocation = Vector3.Scale (newLocation, new Vector3 (150f / (roomWidth * this.roomManager.roomSide), 150f / (roomHeight * this.roomManager.roomSide), 1));

			this.rt.anchoredPosition = newLocation;
		}

	}
}
