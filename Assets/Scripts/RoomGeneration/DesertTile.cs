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

		// Add Boulders
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

		// Add sink hole with treasure sometimes
		Tile openTile = this.GetOpenArea(region);
		if (openTile != null) {
			for (int x = -1; x <= 1; x++) {
				for (int y = -1; y <= 1; y++) {
					this.tileMap[openTile.x + x, openTile.y + y].elevation--;
				}
			}
			this.GetComponent<RoomManager>().PlaceItem(this.GetComponent<ElevationTile>().randomItem, openTile.x, openTile.y);
		}

		this.GetComponent<ElevationTile>().SmoothElevation(region);

		base.RandomBlocking(region);

	}

	public override int getBiomeNumber() {
		return 1;
	}
}