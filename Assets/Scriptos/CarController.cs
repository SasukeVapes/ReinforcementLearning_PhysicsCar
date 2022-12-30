using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

public class CarController : MonoBehaviour
{

    private float horizontalInput;
    private float verticalInput;
    public float forwardAmount;
    public float turnAmount;
    public float forward;
    public float steerAngle;
    public bool isBreaking;
    public int i;


    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 0f;

    public Rigidbody CarRB;
   // public RaycastHit HitDist;
    public void FixedUpdate()
    {
    //    GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
      //  print((HitDist.distance));
    }
  /*
    public void GetInput()
    {
        
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
        // forwardAmount = verticalInput;
        //   turnAmount = horizontalInput;

    }
   */


    public void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
        
    }

    public void HandleMotor()
    {
        forward = verticalInput * motorForce;
        frontLeftWheelCollider.motorTorque = forward;
        frontRightWheelCollider.motorTorque = forward;
        if (i==1)
        {
            CarRB.velocity = Vector3.zero;
            CarRB.angularVelocity = Vector3.zero;
            i = 0;
        }
        brakeForce = isBreaking ? 3000f : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }


    public void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    public void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

    public void SetInput(float turnAmount, float forwardAmount, bool brake)
    {
        this.verticalInput = forwardAmount;
        this.horizontalInput = turnAmount;
        this.isBreaking = brake;
    }
  
    public void FullStop()
    {
        // Vector3 CurrentVelocity = CarRB.velocity;
        //  Vector3 CurrAngularVelocity = CarRB.angularVelocity;





              frontLeftWheelCollider.brakeTorque = float.MaxValue;
              frontRightWheelCollider.brakeTorque = float.MaxValue;
              rearLeftWheelCollider.brakeTorque = float.MaxValue;
              rearRightWheelCollider.brakeTorque = float.MaxValue;
        // CarRB.inertiaTensor = Vector3.zero;
        //  System.Threading.Thread.Sleep(600);
        // CarRB.velocity = Vector3.zero;
        //CarRB.angularVelocity = Vector3.zero;
        //  CarRB.velocity = CurrentVelocity;
        //  CarRB.angularVelocity = CurrAngularVelocity*2 ;



      
      //  frontRightWheelCollider.steerAngle = 0;
     //  frontLeftWheelCollider.steerAngle = 0;
     //  frontLeftWheelCollider.motorTorque = 0;
     //  frontRightWheelCollider.motorTorque = 0;
        
        transform.localPosition = new Vector3(52.0f, 0f, 90.0f);
        transform.rotation = Quaternion.Euler(0, 180, 0);
        i = 1;



    }
    public IEnumerator Reset()
    {

        yield return new WaitForSeconds(10);
        


    }
}