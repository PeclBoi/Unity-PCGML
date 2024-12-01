using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class PlacerAgent : Agent
{
    public Texture2D placementTexture;
    public GameObject objectPrefab;
    public float spawnRadius = 5f;
    public float placementThreshold = 0.5f;

    private Color32[] textureColors;
    private Vector2Int lastPosition;

    public override void Initialize()
    {
        textureColors = placementTexture.GetPixels32();
    }

    public override void OnEpisodeBegin()
    {
        // Reset environment for new episode
        lastPosition = Vector2Int.zero;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Get agent's position in texture coordinates
        Vector2Int agentPosition = GetTexturePosition(transform.position);

        // Add agent's position as observation
        sensor.AddObservation(agentPosition.x / (float)placementTexture.width);
        sensor.AddObservation(agentPosition.y / (float)placementTexture.height);

        // Add color values of pixels around the agent as observations
        AddTextureObservations(agentPosition, sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        // Convert action values to texture coordinates
        float x = Mathf.Clamp(actions.ContinuousActions[0], 0f, 1f) * placementTexture.width;
        float y = Mathf.Clamp(actions.ContinuousActions[1], 0f, 1f) * placementTexture.height;



        // Convert action to texture position
        Vector2 actionPosition = new Vector2(x, y);
        Debug.Log(actionPosition);



        // Check if position is within bounds
        if (IsWithinBounds(actionPosition))
        {
            // Place object if predicted to be on black part of the texture
            if (IsPositionBlack(actionPosition))
            {
                PlaceObject(actionPosition);
                SetReward(1f); // Reward for successful placement
            }
            else
            {
                SetReward(-0.1f); // Penalize for placing on white part
            }
        }
        else
        {
            SetReward(-0.1f); // Penalize for out-of-bounds placement
        }

        // End episode after each action
        EndEpisode();
    }

    private void PlaceObject(Vector2 position)
    {
        // Instantiate object at calculated position
        Debug.Log("Place");
        Instantiate(objectPrefab, GetWorldPosition(position), Quaternion.identity);
    }

    private bool IsPositionBlack(Vector2 position)
    {
        Color32 pixelColor = textureColors[(int)position.x + (int)position.y * placementTexture.width];
        return pixelColor.r < placementThreshold * 255f; // Assuming black has lower intensity
    }

    private bool IsWithinBounds(Vector2 position)
    {
        return position.x >= 0 && position.x < placementTexture.width &&
               position.y >= 0 && position.y < placementTexture.height;
    }

    private Vector2Int GetTexturePosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPosition.x / placementTexture.width),
            Mathf.RoundToInt(worldPosition.y / placementTexture.height)
        );
    }

    private Vector2Int GetTexturePosition(float[] vectorAction)
    {
        // Convert action values to texture coordinates
        float x = Mathf.Clamp(vectorAction[0], 0f, 1f) * placementTexture.width;
        float y = Mathf.Clamp(vectorAction[1], 0f, 1f) * placementTexture.height;
        return new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y));
    }

    private Vector3 GetWorldPosition(Vector2 texturePosition)
    {
        return new Vector3(
            (texturePosition.x + 0.5f) / placementTexture.width,
            0f,
            (texturePosition.y + 0.5f) / placementTexture.height
        );
    }

    private void AddTextureObservations(Vector2Int agentPosition, VectorSensor sensor)
    {
        int observationSize = 3; // Size of the observation window around the agent
        int halfSize = observationSize / 2;

        for (int y = -halfSize; y <= halfSize; y++)
        {
            for (int x = -halfSize; x <= halfSize; x++)
            {
                // Calculate position in texture coordinates
                int texX = Mathf.Clamp(agentPosition.x + x, 0, placementTexture.width - 1);
                int texY = Mathf.Clamp(agentPosition.y + y, 0, placementTexture.height - 1);

                // Get color of the pixel at this position
                Color32 pixelColor = textureColors[texX + texY * placementTexture.width];

                // Normalize color values to range [0, 1] and pass them as observations
                sensor.AddObservation(pixelColor.r / 255f);
                sensor.AddObservation(pixelColor.g / 255f);
                sensor.AddObservation(pixelColor.b / 255f);
            }
        }
    }
}
