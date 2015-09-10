using UnityEngine;
using System.Collections;

public class ToolUser : MonoBehaviour {
	public string toolType = "";
	public float range = 0.6f;

	private float left = 0.05f;
	private GameObject tool;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (tool != null) {
			left -= Time.deltaTime;
			if (left <= 0) {
				left = 0.1f;
				Destroy (this.tool);
				this.tool = null;
			}
		}
	}

	private float toolAnimationDelay = .18f;
	IEnumerator CreateTool(bool horizontal, int direction) {

		if (this.toolType.Length > 0) {
			Destroy (this.tool);

			yield return new WaitForSeconds(toolAnimationDelay);
			this.tool = new GameObject ();
			tool.name = this.toolType;
			tool.tag = this.toolType;
			tool.transform.parent = this.gameObject.transform;
			tool.transform.position = this.gameObject.transform.position;

			BoxCollider2D collider = tool.AddComponent<BoxCollider2D> ();
			collider.isTrigger = true;

			Vector2 playerSize = this.Size;
			collider.size = playerSize;

			if (horizontal) {
				collider.size = new Vector2 (collider.size.x, this.range);
				collider.transform.Translate (0, (playerSize.y / 2 + this.range / 2) * direction, 0);
			} else {
				collider.size = new Vector2 (this.range, collider.size.y);
				collider.transform.Translate ((playerSize.x / 2 + this.range / 2) * direction, 0, 0);
			}
		}
	}


	public void UseNorth () {
		StartCoroutine(CreateTool (true, 1));
	}

	public void UseEast () {
		StartCoroutine(CreateTool (false, 1));
	}

	public void UseWest () {
		StartCoroutine(CreateTool (false, -1));
	}

	public void UseSouth () {
		StartCoroutine(CreateTool (true, -1));
	}

	public Vector2 Size {
		get {
			return this.GetComponent<BoxCollider2D> ().size;
		}
	}
}
