using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float sensitivityX;
    public float sensitivityY;
    public Cinemachine.CinemachineFreeLook thirdPersonCamera;
    public float lastPositionUpdate;
    public Vector3 lastVelocityValue;

    // Phsyics prediction client storage
    public List<Vector3> positionHistory = new List<Vector3>();
    public List<Vector3> rotationHistory = new List<Vector3>();
    public List<Vector3> velocityHistory = new List<Vector3>();
    public List<float> timeHistory = new List<float>();

    public Rigidbody playerRigidbody;
}
