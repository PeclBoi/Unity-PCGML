using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AngleToStreet : MonoBehaviour
{

	public LayerMask MarkerLayer;

	// Update is called once per frame
	void Update()
	{
		//var colliders = Physics.OverlapBox(transform.position, new Vector3(25, 10, 25), Quaternion.identity, MarkerLayer);

		//if (!colliders.Any()) { return; }

		//var sortedColliders = colliders.OrderBy(c => Vector3.Distance(transform.position, c.transform.position)).ToList();
		//var nearestTwoColliders = sortedColliders.Take(2).ToArray();

		//var rotation = Vector3.Lerp(nearestTwoColliders[0].transform.eulerAngles, nearestTwoColliders[1].transform.eulerAngles, .5f);

		//rotation += Vector3.up * 90;

		//transform.rotation = Quaternion.Euler(rotation);

	}
}
