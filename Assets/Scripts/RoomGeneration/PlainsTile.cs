using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class PlainsTile : BiomeTile
{

	// randomization constants
	public int bloomNum = 100;
	public RoomManager.Count bloomSize = new RoomManager.Count(3, 7);
	
	public const int BiomeNumber = 2;
	
	public override void RandomBlocking(List<Tile> region) {
		for (int num = 0; num < bloomNum; num++) {
			
			Tile randomTile = region[Random.Range(0, region.Count)];
			BlockingExplosion(randomTile.x,
			                  randomTile.y,
			                  Random.Range (this.bloomSize.minimum, this.bloomSize.maximum + 1));
		}
	}
	
	private void BlockingExplosion(int x, int y, int level) {

		if (level < 1 || x < 0 || y < 0 || x >= width || y >= height) {
			return;
		}
		
		Tile tile = this.tileMap[x, y];
		
		if (tile.biome != PlainsTile.BiomeNumber || tile.blocking == true) {
			return;
		}
		
		if (tile.item == null) {
			tile.blocking = true;
			this.GetComponent<RoomManager>().PlaceItem(this.getBlockingTile(), x, y);
		}
		
		for (int i = -1; i <= 1; i++) {
			for (int j = -1; j <= 1; j++) {
				if (Random.Range(0, 10) > 3 && (j != 0 || i == 1)) {
					BlockingExplosion(x + i, y + j, level - 1);
				}
			}
		}
	}

	public override int getBiomeNumber() {
		return 2;
	}
}