using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DesertTile : BiomeTile
{
	public GameObject cactus;
	public RoomManager.Count cactusCount = new RoomManager.Count(10, 30);

	// randomization constants
	public int bloomNum = 100;
	public RoomManager.Count bloomSize = new RoomManager.Count(3, 7);

	public const int BiomeNumber = 1;

	public override void RandomBlocking(List<Tile> region) {

		base.RandomBlocking(region);

		// Add Boulders
//		for (int num = 0; num < bloomNum; num++) {
//
//			Tile randomTile = region[Random.Range(0, region.Count)];
//			BlockingExplosion(randomTile.x,
//			                  randomTile.y,
//			                  Random.Range (this.bloomSize.minimum, this.bloomSize.maximum + 1),
//			                  new TilePlacer(this.placeBlockingTile));
//		}
		this.PerlinGenerator(region,
		                     new TilePlacer(this.placeBlockingTile),
		                     .75f,
		                     .2f);

		// Add cactuses
		int cactusNum = Random.Range(this.cactusCount.minimum, this.cactusCount.maximum);
		for (int num = 0; num < cactusNum; num++) {
			Tile cactusTile = region[Random.Range(0, region.Count)];
			while (cactusTile.item != null) {
				cactusTile = region[Random.Range(0, region.Count)];
			}
			this.GetComponent<RoomManager>().PlaceItem(cactus, cactusTile.x, cactusTile.y);
		}

	}

	public override int getBiomeNumber() {
		return 1;
	}
}