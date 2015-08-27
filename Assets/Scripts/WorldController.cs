using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public int size = 3;
	public GameObject[] rooms;
	private PlayerController player;
	private CameraController camera;
	// Use this for initialization
	void Start () {
		this.player = PlayerController.instance;
		this.camera = CameraController.instance;

		GameObject nextRoom = rooms[0];
		float width = nextRoom.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		float scale = (float) nextRoom.transform.localScale.x;		
		for (int i = 0; i < this.size; i++) {
			for (int j = 0; j < this.size; j++){
				Instantiate (rooms[0], new Vector3(i*width*scale, j*width*scale, 0), Quaternion.identity);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		float newX;
		float newY;
		float playerX = this.player.transform.position.x;
		float playerY = this.player.transform.position.y;
		float cameraX = this.camera.transform.position.x;
		float cameraY = this.camera.transform.position.y;

		GameObject nextRoom = rooms[0];
		float width = nextRoom.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
		float scale = (float) nextRoom.transform.localScale.x;		

		//print (this.camera.getWidth ());
		//print (this.camera.getHeight ());

		//print (playerX);

		if (playerX <= (this.size * (width*scale)-this.camera.getWidth ()) - ((width*scale - this.camera.getWidth ()) / 2) &&
			playerX >= -(width*scale - this.camera.getWidth ()) / 2) {
			newX = playerX;
		} else {
			newX = cameraX;
		}
		if (playerY <= (this.size * (width*scale)-this.camera.getHeight ()) - ((width*scale - this.camera.getHeight ()) / 2) &&
		//if (playerY <= (width*scale - this.camera.getHeight ()) / 2 &&
		    playerY >= -(width*scale - this.camera.getHeight ()) / 2) {
			newY = playerY;
		} else {
			newY = cameraY;
		}
		this.camera.transform.position = new Vector3 (newX,
		                                              newY,
		                                   			  this.camera.transform.position.z);
	}

}
