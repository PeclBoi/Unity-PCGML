using System.Collections.Generic;
using UnityEngine;

public class HousePlacer : MonoBehaviour
{

	[SerializeField]
	private bool drawGizmos;

	[SerializeField]
	private GameObject[] Houses;

	[SerializeField]
	private List<GameObject> placedHouses = new List<GameObject>();



	public bool PlaceHouse(Vector3 pos)
	{
		var placedHouse = Instantiate(Houses[0], pos, Quaternion.identity);

		bool isSuccess = placedHouse.GetComponent<PlacementValidator>().Validate();

		if (isSuccess)
		{
			placedHouses.Add(placedHouse);
		}
		else
		{
			Destroy(placedHouse.GetComponent<PlacementValidator>());
			placedHouse.GetComponent<Renderer>().material.color = Color.red;
			Destroy(placedHouse);
		}


		//Debug.Log(isSuccess);

		return isSuccess;
	}

	public void Clear()
	{
		placedHouses.ForEach(house => Destroy(house));
		placedHouses.Clear();
	}
}
