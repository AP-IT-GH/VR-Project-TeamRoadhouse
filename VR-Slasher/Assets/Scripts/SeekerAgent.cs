using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class SeekerAgent : Agent
{
    [SerializeField] private float moveSpeed = 10;
    [SerializeField] private float rotationSpeed = 350;
    [SerializeField] private MonitorTool monitorTool;
    [SerializeField] private float maxTime = 60f;
    [SerializeField] private float maxTimeNotMoved = 3f;
    private bool Collided = false;
    private bool notMoved = false;
    
    private Rigidbody rb;
    private Environment env;
    private float timer = 0f;
    private float movedTimer = 0f;

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        env = GetComponentInParent<Environment>();
        timer = maxTime;
        movedTimer = maxTimeNotMoved;

    }

    private void Update()
    {
        // If agent falls, give negative reward and end episode
        if(transform.position.y < transform.parent.position.y - 3)
        {
            SetReward(-1f);
            monitorTool.FailsCount += 1;
            EndEpisode();
        }

        // Create timer to give the agent a maximum time to find the player
        if(timer <= 0f)
        {
            SetReward(-1f);
            monitorTool.FailsCount += 1;
            EndEpisode();
            timer = maxTime; // Reset timer
        }
        timer -= Time.deltaTime; // Take time elapsed from timer

        if (Collided)
        {
            print("hitting wall");
            SetReward(-0.5f);
        }

        if (notMoved)
        {
            movedTimer -= Time.deltaTime;
            if (movedTimer <= 0)
            {
                print("not moved int 3 sec");
                SetReward(-0.3f);
            }
        }
        else
        {
            movedTimer = maxTimeNotMoved;
        }
    }

    public override void OnEpisodeBegin()
    {
        monitorTool.EpisodesCount += 1;

        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        env.ResetEnvironment();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position); //3
        sensor.AddObservation(transform.rotation); // 4
        sensor.AddObservation(env.players[0].transform.position); //3
        
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var vectorAction = actions.DiscreteActions;

        // Add negative reward when agent doesn't move
        if (vectorAction[0] == 0 && vectorAction[1] == 0)
        {
            notMoved = true;
            return;
        }
        else
            notMoved = false;
        
        // 0 = IDLY , 1 = BACKWARDS , 2 = FORWARDS
        if (vectorAction[0] != 0)
        {
            Vector3 translation = transform.forward * moveSpeed * (vectorAction[0] * 2 - 3) * Time.deltaTime;
            transform.Translate(translation, Space.World);
        }

        // 0 = IDLY , 1 = LEFT , 2 = RIGHT
        if (vectorAction[1] != 0) 
        {
            float rotation = rotationSpeed * (vectorAction[1] * 2 - 3) * Time.deltaTime;
            transform.Rotate(0, rotation, 0);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Stop Episode when Agent finds player - SET REWARD TO 10
        if (collision.transform.CompareTag("Player"))
        {
            SetReward(1f);
            monitorTool.SuccesCount += 1;
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("Collidable"))
        {
            Collided = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Collided = false;
    }

    public override void Heuristic(in ActionBuffers actionBuffers)
    {
        base.Heuristic(actionBuffers);

        var outputAction = actionBuffers.DiscreteActions;
        outputAction[0] = 0;
        outputAction[1] = 0;

        if (Input.GetKey(KeyCode.UpArrow)) // Moving forwards
            outputAction[0] = 2;
        else if (Input.GetKey(KeyCode.DownArrow)) // Moving backwards
            outputAction[0] = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) // Turning left
            outputAction[1] = 1;
        else if (Input.GetKey(KeyCode.RightArrow)) // Turning right
            outputAction[1] = 2;
    }
}
