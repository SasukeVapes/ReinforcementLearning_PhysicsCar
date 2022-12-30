using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{

    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform Obstacle;
    [SerializeField] private Transform Obstacle2;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private Material warningMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    private CarController CarController;
  //  public RaycastHit hit;
  //  public float dist;
    public Rigidbody CarRB;
    private RayPerceptionSensorComponent3D raySensors;
  //  public float rayHitDistance;

    private void Awake()
    {

        CarController = GetComponent<CarController>();

    }
    
    public override void OnEpisodeBegin()
    {

        CarController.FullStop();
        Obstacle.localPosition = new Vector3(Random.Range(+85f, +12f), 5.34f, 68.5f);
        Obstacle2.localPosition = new Vector3(Random.Range(+85f, +12f), 5.34f, 40.9f);


    }
    public override void CollectObservations(VectorSensor sensor)
    {
        /*
         dist = Vector3.Distance(CarRB.transform.localPosition, hit.point);
        
        print(dist);
        print(hit.point);
        */
        sensor.AddObservation(CarRB.transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
   
        // sensor.AddObservation(hit.distance);
        // sensor.AddObservation(CarRB.transform.InverseTransformDirection(CarRB.velocity));
        // sensor.AddObservation(CarRB.transform.InverseTransformDirection(CarRB.velocity));
    }
  
    public override void OnActionReceived(ActionBuffers actions)
    {
        
        float turnAmount = 0f;
        float forwardAmount = 0f;
        bool brake = false;

        // float moveSpeed = 1f;
        //transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        
        switch (actions.DiscreteActions[0])
        {
            case 0: forwardAmount = 0f; break;
            case 1: forwardAmount = +1f; break;
            case 2: forwardAmount = -1f; SetReward(-5f); break;
            case 3: brake = true; SetReward(-3f); break;

        }
        switch (actions.DiscreteActions[1])
        {
            case 0: turnAmount = 0f; break;
            case 1: turnAmount = +1f; break;
            case 2: turnAmount = -1f; break;

        }
        CarController.SetInput( turnAmount, forwardAmount, brake);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetKey(KeyCode.W)) forwardAction = 1;
        if (Input.GetKey(KeyCode.S)) forwardAction = 2;
        if (Input.GetKey(KeyCode.Space)) forwardAction = 3;

        int turnAction = 0;
        if (Input.GetKey(KeyCode.D)) turnAction = 1;
        if (Input.GetKey(KeyCode.A)) turnAction = 2;

        ActionSegment<int> discreteActions = actionsOut.DiscreteActions; 
        discreteActions[0] = forwardAction;
        discreteActions[1] = turnAction;
    }
    private void OnTriggerEnter(Collider other)

    {
        if (other.TryGetComponent<Goal> (out Goal goal))
        {
            SetReward(+100f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();

        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-50f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();

 
        }
        if (other.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            SetReward(-50f);
            floorMeshRenderer.material = warningMaterial;
            EndEpisode();


        }

        if (other.TryGetComponent<Cube>(out Cube cube))
        {
            SetReward(-50f);
            floorMeshRenderer.material = warningMaterial;
            EndEpisode();


        }


    }
}
