using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class PlaceHouseAgent : Agent
{
    private int NumberOfHousesPlaced;
    private int NumberOfTries;
    [SerializeField] private int TargetNumberOfHouses;
    [SerializeField] private GameObject[] RoadSegments;
    [SerializeField] private Transform mapTransform;
    [SerializeField] private HousePlacer housePlacer;

    private GameObject lastRoadSegment;

    public bool keepInitRotation;

    private int validPlacedHouses = 0, invalidPlacesHouses = 0;

    private void Awake()
    {
        lastRoadSegment = Instantiate(RoadSegments[Random.Range(0, RoadSegments.Length)], mapTransform.TransformPoint(new Vector3(25, -5, 25)), Quaternion.identity, transform);

        var rotation = keepInitRotation ? Quaternion.identity : Quaternion.EulerRotation(0, Random.Range(0, 359), 0);
        MeshGenerator meshGenerator = lastRoadSegment.GetComponent<MeshGenerator>();
        if (meshGenerator == null)
        {
            MeshGenerator[] meshGenerators = lastRoadSegment.GetComponentsInChildren<MeshGenerator>();
            foreach (var item in meshGenerators)
            {
                item.GenerateMesh(rotation);
            }
        }
        else
        {
            meshGenerator.GenerateMesh(rotation);
        }
    }

    public override void OnEpisodeBegin()
    {

        if (NumberOfTries >= TargetNumberOfHouses)
        {
            Debug.Log($"Valid: {validPlacedHouses}, Invalid: {invalidPlacesHouses}, PlacedHouses: {NumberOfTries} out of {TargetNumberOfHouses}");

            AddReward(5 * NumberOfHousesPlaced / TargetNumberOfHouses);
            Debug.Log("Bonus: " + 5 * (float)NumberOfHousesPlaced / TargetNumberOfHouses);
            validPlacedHouses = invalidPlacesHouses = 0;
            NumberOfTries = 0;
            NumberOfHousesPlaced = 0;
            if (lastRoadSegment != null)
            {
                Destroy(lastRoadSegment);
                lastRoadSegment = null;
            }

            lastRoadSegment = Instantiate(RoadSegments[Random.Range(0, RoadSegments.Length)], mapTransform.TransformPoint(new Vector3(25, -5, 25)), Quaternion.identity, transform);
            var rotation = Quaternion.EulerRotation(0, Random.Range(0, 359), 0);
            MeshGenerator meshGenerator = lastRoadSegment.GetComponent<MeshGenerator>();

            if (meshGenerator == null)
            {
                MeshGenerator[] meshGenerators = lastRoadSegment.GetComponentsInChildren<MeshGenerator>();
                foreach (var item in meshGenerators)
                {
                    item.GenerateMesh(rotation);
                }
            }
            else
            {
                meshGenerator.GenerateMesh(rotation);
            }

            housePlacer.Clear();
        }
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Space))
        {
            validPlacedHouses = invalidPlacesHouses = 0;
            for (int i = 0; i < TargetNumberOfHouses; i++)
            {
                StartCoroutine(RequestMultipleDecisions(50));
            }
        }
    }


    private IEnumerator RequestMultipleDecisions(int count)
    {
        for (int i = 0; i < count; i++)
        {
            RequestDecision(); // Request decision from the trained model
            yield return new WaitForSeconds(0.5f); // Wait for the next frame before continuing
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        int positionX = actions.DiscreteActions[0];
        int positionZ = actions.DiscreteActions[1];

        Vector3 housePosition = new Vector3(positionX, 0, positionZ);

        Vector3 localHousePosition = mapTransform.TransformPoint(housePosition);

        if (housePlacer.PlaceHouse(localHousePosition))
        {
            validPlacedHouses++;
            NumberOfHousesPlaced++;
            AddReward(1f);
            EndEpisode();
        }
        else
        {
            invalidPlacesHouses++;
            AddReward(-0.1f);
        }

        NumberOfTries++;
    }
}
