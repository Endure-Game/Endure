using UnityEngine;
using System.Collections;

public class TilePlacer : MonoBehaviour
{
	private Tile[,] tileMap;
	private int width;
	private int height;
	
	void Awake() {
		this.tileMap = this.GetComponent<RoomManager>().tileMap;
		this.height = this.tileMap.GetLength(0);
		this.width = this.tileMap.GetLength(1);
	}
}

