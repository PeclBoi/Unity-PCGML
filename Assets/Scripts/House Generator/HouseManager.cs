using UnityEngine;

public class HouseManager : MonoBehaviour
{

	public GameObject WallPrefab;
	public Vector2 roomSize;

	[Header("Wall")]
	[Min(0.1f)]
	public float WallWidth;
	public float WallHeight = 2.5f;
	[Range(1, 5)]
	public float WallHeightScale = 3f;
	[Range(1, 5)]
	public int Storeys = 3;

	[Header("Floor")]
	public GameObject FloorTilePrefab;
	[Min(0.1f)]
	public float FloorTileHeight;
	[Min(0.1f)]
	public float FloorTileWidth;


	[Header("Roof")]

	public GameObject RoofMiddlePart;
	public GameObject RoofCornerPart;
	public GameObject RoofSidePart;

	public float MiddlePartWidth;
	public float CornerPartWidth;
	public float SidePartWidth;

	public float SidePartHeight;



	void CreateRoof()
	{

		int rowCount = Mathf.Max(1, (int)((roomSize.x - 2 * SidePartWidth) / MiddlePartWidth));
		int colCount = Mathf.Max(1, (int)((roomSize.y - 2 * SidePartWidth) / MiddlePartWidth));

		float scaleX = (roomSize.x - 2 * SidePartWidth) / rowCount / MiddlePartWidth;
		float scaleZ = (roomSize.y - 2 * SidePartWidth) / colCount / MiddlePartWidth;

		for (int row = 0; row < rowCount; row++)
		{
			for (int col = 0; col < colCount; col++)
			{

				var tile = Instantiate(RoofMiddlePart);

				var x = -roomSize.x / 2 + MiddlePartWidth * scaleX / 2 + MiddlePartWidth * row * scaleX + SidePartWidth;
				var z = -roomSize.y / 2 + MiddlePartWidth * scaleZ / 2 + MiddlePartWidth * col * scaleZ + SidePartWidth;


				tile.transform.position = transform.position + new Vector3(x, WallHeight * WallHeightScale * Storeys + SidePartHeight, z);
				tile.transform.rotation = Quaternion.identity;
				tile.transform.localScale = new Vector3(scaleX, 1, scaleZ);
			}
		}



		var y = WallHeight * WallHeightScale * Storeys;
		int wallCount = Mathf.Max(1, (int)(roomSize.x / SidePartWidth));
		float scale = (roomSize.x - CornerPartWidth * 2) / wallCount / SidePartWidth;

		Debug.Log(wallCount);
		for (int i = 0; i < wallCount; i++)
		{
			var x = -roomSize.x / 2 + CornerPartWidth + SidePartWidth * scale / 2 + i * scale * SidePartWidth;

			var sidePiece = Instantiate(RoofSidePart);

			sidePiece.transform.position = transform.position + new Vector3(x, y, roomSize.y / 2);
			sidePiece.transform.rotation = transform.rotation;
			sidePiece.transform.localScale = new Vector3(scale, 1, 1);

			sidePiece = Instantiate(RoofSidePart);
			sidePiece.transform.position = transform.position + new Vector3(x, y, -roomSize.y / 2);
			sidePiece.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.up * 180);
			sidePiece.transform.localScale = new Vector3(scale, 1, 1);
		}


		wallCount = Mathf.Max(1, (int)(roomSize.y / SidePartWidth));
		scale = (roomSize.y - CornerPartWidth * 2) / wallCount / SidePartWidth;
		for (int i = 0; i < wallCount; i++)
		{
			var z = -roomSize.y / 2 + CornerPartWidth + (SidePartWidth * scale) / 2 + SidePartWidth * scale * i;

			var sidePiece = Instantiate(RoofSidePart);
			sidePiece.transform.position = transform.position + new Vector3(-roomSize.x / 2, y, z);
			sidePiece.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.up * 270);
			sidePiece.transform.localScale = new Vector3(scale, 1, 1);

			sidePiece = Instantiate(RoofSidePart);

			sidePiece.transform.position = transform.position + new Vector3(roomSize.x / 2, y, z);
			sidePiece.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.up * 90);
			sidePiece.transform.localScale = new Vector3(scale, 1, 1);
		}

		CreateCorners(y);

	}

	private void CreateCorners(float y)
	{
		var cornerPiece = Instantiate(RoofCornerPart);

		cornerPiece.transform.position = transform.position + new Vector3(roomSize.x / 2 - CornerPartWidth / 2, y, roomSize.y / 2 - CornerPartWidth / 2);
		cornerPiece.transform.rotation = transform.rotation;
		cornerPiece.transform.localScale = Vector3.one;

		cornerPiece = Instantiate(RoofCornerPart);
		cornerPiece.transform.position = transform.position + new Vector3(roomSize.x / 2 - CornerPartWidth / 2, y, -roomSize.y / 2 + CornerPartWidth / 2);
		cornerPiece.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.up * 90);
		cornerPiece.transform.localScale = Vector3.one;

		cornerPiece = Instantiate(RoofCornerPart);
		cornerPiece.transform.position = transform.position + new Vector3(-roomSize.x / 2 + CornerPartWidth / 2, y, -roomSize.y / 2 + CornerPartWidth / 2);
		cornerPiece.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.up * 180);
		cornerPiece.transform.localScale = Vector3.one;

		cornerPiece = Instantiate(RoofCornerPart);
		cornerPiece.transform.position = transform.position + new Vector3(-roomSize.x / 2 + CornerPartWidth / 2, y, roomSize.y / 2 - CornerPartWidth / 2);
		cornerPiece.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.up * 270);
		cornerPiece.transform.localScale = Vector3.one;
	}

	void CreateFloor()
	{
		int rowCount = Mathf.Max(1, (int)(roomSize.x / FloorTileWidth));
		int colCount = Mathf.Max(1, (int)(roomSize.y / FloorTileHeight));

		float scaleX = (roomSize.x / rowCount) / FloorTileWidth;
		float scaleZ = (roomSize.y / colCount) / FloorTileHeight;



		for (int row = 0; row < rowCount; row++)
		{
			for (int col = 0; col < colCount; col++)
			{
				var x = -roomSize.x / 2 + FloorTileWidth * scaleX / 2 + row * scaleX * FloorTileWidth;
				var z = -roomSize.y / 2 + FloorTileHeight * scaleZ / 2 + col * scaleZ * FloorTileHeight + scaleZ * FloorTileHeight / 2;

				var tile = Instantiate(FloorTilePrefab);

				tile.transform.position = transform.position + new Vector3(x, 0, z);
				tile.transform.rotation = transform.rotation;
				tile.transform.localScale = new Vector3(scaleX, 1, scaleZ);
			}
		}
	}


    private void CreateWalls()
    {
        for (int storey = 0; storey < Storeys; storey++)
        {
            float y = WallHeight * WallHeightScale * storey;
            CreateHorizontalWalls(y);
            CreateVerticalWalls(y);
        }
    }

    private void CreateHorizontalWalls(float y)
    {
        int wallCount = Mathf.Max(1, (int)(roomSize.x / WallWidth));
        float scale = roomSize.x / (wallCount * WallWidth);

        for (int i = 0; i < wallCount; i++)
        {
            float x = CalculateWallPositionX(i, scale);
            PlaceWall(new Vector3(x, y, roomSize.y / 2), scale, Quaternion.identity);     // Vorderseite
            PlaceWall(new Vector3(x, y, -roomSize.y / 2), scale, Quaternion.identity);    // Rückseite
        }
    }

    private void CreateVerticalWalls(float y)
    {
        int wallCount = Mathf.Max(1, (int)(roomSize.y / WallWidth));
        float scale = (roomSize.y / wallCount) / WallWidth;

        for (int i = 0; i < wallCount; i++)
        {
            float z = CalculateWallPositionZ(i, scale);
            PlaceWall(new Vector3(-roomSize.x / 2, y, z), scale, Quaternion.Euler(Vector3.up * 90)); // Linke Seite
            PlaceWall(new Vector3(roomSize.x / 2, y, z), scale, Quaternion.Euler(Vector3.up * 90));  // Rechte Seite
        }
    }

    private float CalculateWallPositionX(int index, float scale)
    {
        return -roomSize.x / 2 + WallWidth * scale / 2 + index * scale * WallWidth;
    }

    private float CalculateWallPositionZ(int index, float scale)
    {
        return -roomSize.y / 2 + WallWidth * scale / 2 + index * scale * WallWidth;
    }

    private void PlaceWall(Vector3 position, float scale, Quaternion rotation)
    {
        var wall = Instantiate(WallPrefab);
        wall.transform.position = transform.position + position;
        wall.transform.rotation = transform.rotation * rotation;
        wall.transform.localScale = new Vector3(scale, WallHeightScale, 1);
    }


    // Update is called once per frame
    void Start()
	{
		CreateWalls();
		CreateFloor();
		CreateRoof();
	}
}
