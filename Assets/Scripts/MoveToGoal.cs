using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoal : Agent
{
    public float speed = 2f;
    public Transform targetPosition;
    public Transform startingPosition;
    public Material win;
    public Material lose;
    public Renderer platform;

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(targetPosition.position);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = startingPosition.position;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 direction = new Vector3(moveX, 0f, moveZ);

        transform.position += direction * speed * Time.deltaTime;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actionSegment = actionsOut.ContinuousActions;
        actionSegment[0] = Input.GetAxisRaw("Horizontal");
        actionSegment[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("target"))
        {
            SetReward(1f);
            platform.material = win;
        }    
        else if(other.CompareTag("wall"))
        {
            SetReward(-1f);
            platform.material = lose;
        }
        EndEpisode();
    }
}
