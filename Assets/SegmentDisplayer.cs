using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentDisplayer : MonoBehaviour
{

    public GameObject floor;

    public GameObject[] RoadSegments_Level1;
    public GameObject[] RoadSegments_Level2;

    public float SPACE_BETWEEN_FLOOR = 20;

    public int rows = 3;

    // Start is called before the first frame update
    void Start()
    {

        for (int i = 0; i < RoadSegments_Level2.Length; i++) 
        {
            var floorInstance = Instantiate(floor);
            Vector3 pos = transform.position + Vector3.right * SPACE_BETWEEN_FLOOR * (i % 5) + Vector3.back * SPACE_BETWEEN_FLOOR * (int)(i / (RoadSegments_Level1.Length / rows));
            floorInstance.transform.position = pos;
            var roadSegment = Instantiate(RoadSegments_Level1[i]);
            roadSegment.transform.position = pos + Vector3.up;   
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
