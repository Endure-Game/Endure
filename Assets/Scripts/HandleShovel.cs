using UnityEngine;
using System.Collections;

public class HandleShovel : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

  void OnTriggerEnter2D (Collider2D collider) {
    if (collider.tag == "Shovel") {

      RoomManager roomManager = WorldController.instance.GetComponent<RoomManager>();
      GameObject hole = roomManager.GetComponent<ElevationTile>().hole;
      roomManager.PlaceItem(hole,
                            (int)(this.transform.position.x + 15.5f),
                            (int)(this.transform.position.y + 15.5f));

      if (this.GetComponent<Drops>() != null) {
        this.GetComponent<Drops>().DropItem();
      }
      Destroy (this.gameObject);
    }
  }
}
