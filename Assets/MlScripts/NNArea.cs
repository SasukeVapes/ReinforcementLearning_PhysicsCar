using UnityEngine;
using Unity.MLAgents;   //need Package Manager, MLAgents package
using Unity.MLAgents.Actuators;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(DecisionRequester))]
public class NNArea : Agent
{
    [SerializeField] private Transform targetTransform;



    public float Movespeed = 30f;
    public float Turnspeed = 100f;
    
    private Rigidbody rb = null;                //use physics to move car
    private Vector3 recall_position;            //to reset car each episode, training happens in episodes/steps
    private Quaternion recall_rotation;
    private Bounds bnd;                         //so I know how far down to raycast from the car to the road

    public override void Initialize()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.drag = 1;
        rb.angularDrag = 5;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;

        this.GetComponent<MeshCollider>().convex = true;

        this.GetComponent<DecisionRequester>().DecisionPeriod = 1;

        bnd = this.GetComponent<MeshRenderer>().bounds;

        recall_position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        recall_rotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
    }
    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        this.transform.position = new Vector3(50.0f, 0f, 52.0f);
        this.transform.rotation = recall_rotation;
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Purpose: translate neural network output (actions) into the gameobject doing something

        //decisionrequestor component needed
        //  space type: discrete
        //      branches size: 2 move, turn
        //          branch 0 size: 3  fwd, nomove, back
        //          branch 1 size: 3  left, noturn, right

        if (isWheelsDown() == false)
            return;

        float mag = rb.velocity.sqrMagnitude;

        switch (actions.DiscreteActions.Array[0])   //move
        {
            case 0:
                break;
            case 1:
                rb.AddRelativeForce(Vector3.back * Movespeed * Time.deltaTime, ForceMode.VelocityChange); //back
                break;
            case 2:
                rb.AddRelativeForce(Vector3.forward * Movespeed * Time.deltaTime, ForceMode.VelocityChange); //forward 
                break;
        }

        switch (actions.DiscreteActions.Array[1]) //turn
        {
            case 0:
                break;
            case 1:
                this.transform.Rotate(Vector3.up, -Turnspeed * Time.deltaTime); //left
                break;
            case 2:
                this.transform.Rotate(Vector3.up, Turnspeed * Time.deltaTime); //right
                break;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //Purpose: to override mlagent brain, and control car myself by keyboard. Why? sanity check for OnActionReceived coded right
        actionsOut.DiscreteActions.Array[0] = 0;
        actionsOut.DiscreteActions.Array[1] = 0;

        float move = Input.GetAxis("Vertical");     //-1..0..1  WASD, arrowws
        float turn = Input.GetAxis("Horizontal");

        if (move < 0)
            actionsOut.DiscreteActions.Array[0] = 1;    //back
        else if (move > 0)
            actionsOut.DiscreteActions.Array[0] = 2;    //forward

        if (turn < 0)
            actionsOut.DiscreteActions.Array[1] = 1;    //left
        else if (turn > 0)
            actionsOut.DiscreteActions.Array[1] = 2;    //right
    }
    private void OnTriggerEnter(Collider other)

    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(+1f);

            EndEpisode();

        }
        if (other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-1f);

            EndEpisode();

        }


    }
    private bool isWheelsDown()
    {
        //raycast down from car = ground should be closely there
        return Physics.Raycast(this.transform.position, -this.transform.up, bnd.size.y * 0.55f);
    }
}
