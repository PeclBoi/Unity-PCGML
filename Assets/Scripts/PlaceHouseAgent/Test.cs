using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	[SerializeField] private GameObject House;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray, out RaycastHit hit))
		{
			Debug.Log(hit.point);
		}

		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			var houseInstance = Instantiate(House);
			houseInstance.transform.position = hit.point;
		}
	}
}
