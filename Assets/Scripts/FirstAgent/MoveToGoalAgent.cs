using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;

    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        float speed = 3f;
        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        meshRenderer.material = loseMaterial;
        SetReward(-2f);
        EndEpisode();
    }


    private void OnTriggerEnter(Collider other)
    {
        meshRenderer.material = winMaterial;
        SetReward(1f);
        EndEpisode();
    }
}
