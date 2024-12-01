using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformPointExample : MonoBehaviour
{

	[Range(-20, 20)]
	[SerializeField] private float X;
	[Range(-20, 20)]
	[SerializeField] private float Z;


	public Transform Map;
	public Transform Point;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		var localPos = new Vector3(X, 0, Z);

		var globalPos = Map.TransformPoint(localPos);

		Point.position = globalPos;

	}
}
