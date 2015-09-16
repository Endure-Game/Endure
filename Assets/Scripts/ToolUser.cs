using UnityEngine;
using System.Collections;

public class ToolUser : MonoBehaviour {
	public string toolType = "";
	public float range = 0.6f;

	public float delay = 1.0f;

	private GameObject tool;

	private float untilUnlocked;
	private bool locked = false;
	private float elapsed;

	public bool Locked {
		get {
			return locked;
		}
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

		this.untilUnlocked -= Time.deltaTime;

		if (this.tool != null) {
			this.elapsed += Time.deltaTime;
			if (this.elapsed > 0.1f) {
				Destroy (this.tool);
				this.tool = null;
			}
		}

		if (this.untilUnlocked <= 0) {
			this.locked = false;
		}

	}

	private float toolAnimationDelay = .18f;
	IEnumerator CreateTool(bool horizontal, int direction) {

		if (this.toolType.Length > 0 && !this.locked) {

			Destroy (this.tool);

			this.elapsed = 0;
			this.untilUnlocked = this.delay;
			this.locked = true;

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

			float verticalAdjustment = playerSize.y / 4;
			if (this.toolType == "Shovel") {
				verticalAdjustment = -playerSize.y / 4;
			} else if (this.toolType == "Lockpick") {
				verticalAdjustment = 0f;
			}

			if (horizontal) {
				collider.size = new Vector2 (.05f, this.range);
				collider.transform.Translate (0f, (playerSize.y / 2 + this.range / 2) * direction, 0);
			} else {
				collider.size = new Vector2 (this.range, .05f);
				collider.transform.Translate ((playerSize.x / 2 + this.range / 2) * direction, verticalAdjustment, 0);
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
