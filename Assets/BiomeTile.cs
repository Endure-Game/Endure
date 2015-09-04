using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BiomeTile : MonoBehaviour
{
	public GameObject[] groundTiles;
	public GameObject[] blockingTiles;
	
	protected Tile[,] tileMap;
	protected int width;
	protected int height;
	
	void Awake() {
		this.tileMap = this.GetComponent<RoomManager>().tileMap;
		this.height = this.tileMap.GetLength(0);
		this.width = this.tileMap.GetLength(1);
	}

	public GameObject getGroundTile() {
		return this.groundTiles[Random.Range(0, this.groundTiles.Length)];
	}
	
	public GameObject getBlockingTile() {
		return this.blockingTiles[Random.Range(0, this.blockingTiles.Length)];
	}

	public abstract void RandomBlocking(List<Tile> region);
	public abstract int getBiomeNumber();


}
