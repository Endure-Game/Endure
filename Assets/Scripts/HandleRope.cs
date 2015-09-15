using UnityEngine;
using System.Collections;

public class HandleRope : MonoBehaviour {

  public static GameObject roped;
  public static GameObject rope;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

  void OnTriggerEnter2D (Collider2D collider) {
    if (collider.tag == "Rope") {
	  Sounds.instance.Rope ();

      if (HandleRope.roped != null) {
        HandleRope.roped.GetComponent<BoxCollider2D>().enabled = true;

      } else{
        RoomManager roomManager = WorldController.instance.GetComponent<RoomManager>();
        GameObject rope = roomManager.GetComponent<ElevationTile>().ropeLadder;

        HandleRope.rope = Instantiate (rope,
                                       new Vector3(0f, 0f, 0f),
                                       Quaternion.identity) as GameObject;
      }

      Vector3 playerPos = PlayerController.instance.transform.position;
      if (Mathf.Abs(playerPos.x - this.transform.position.x) > Mathf.Abs(playerPos.y - this.transform.position.y)) {
        HandleRope.rope.transform.eulerAngles = new Vector3(0f, 0f, 90f);
      } else {
        HandleRope.rope.transform.eulerAngles = new Vector3(0f, 0f, 0f);
      }

      HandleRope.rope.transform.position = this.transform.position;

      this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
      HandleRope.roped = this.gameObject;
    }
  }
}
