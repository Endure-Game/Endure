using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class BeachTile : MonoBehaviour
{
	public GameObject[] groundTiles;
	public GameObject[] blockingTiles;
	
	// randomization constants
	public int bloomNum = 100;
	public RoomManager.Count bloomSize = new RoomManager.Count(3, 7);
	public float bloomTightness = 0.4f;
	
	public const int BiomeNumber = 5;
	
	private Tile[,] tileMap;
	private int width;
	private int height;
	
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
	
	public void RandomBlocking(List<Tile> region) {
		for (int num = 0; num < bloomNum; num++) {
			
			Tile randomTile = region[Random.Range(0, region.Count)];
			BlockingExplosion(randomTile.x,
			                  randomTile.y,
			                  Random.Range (this.bloomSize.minimum, this.bloomSize.maximum + 1));
		}
	}
	
	private void BlockingExplosion(int x, int y, int level) {
		
		if (level == 0 || x < 0 || y < 0 || x >= width || y >= height) {
			return;
		}
		
		if (Mathf.Sqrt(Random.Range(0, level)) < 1) {
			return;
		}
		
		Tile tile = this.tileMap[x, y];
		
		if (tile.biome != BeachTile.BiomeNumber || tile.blocking == true) {
			return;
		}
		
		if (tile.item == null) {
			tile.blocking = true;
			this.GetComponent<RoomManager>().PlaceItem(this.getBlockingTile(), x, y);
		}
		
		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				if (Random.Range(0, 10) > this.bloomTightness * 10 && (j != 0 || i == 1)) {
					BlockingExplosion(x + i, y + j, level - 1);
				}
			}
		}
	}
}