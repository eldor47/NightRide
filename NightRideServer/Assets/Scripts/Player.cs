using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public float gravity = -9.81f;

    public float moveSpeed = 6f;
    public float jumpSpeed = 30f;
    private float yVelocity = 0f;

    private Vector3 direction;
    private float camY;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    bool[] inputs;
    private Rigidbody movingPlatform;

    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    private bool _isGrounded = true;
    private Transform _groundChecker;

    public float DashDistance = 1f;
    private bool hasDashed = false;

    Vector3 velocity;

    public Rigidbody rb;


    private void Start()
    {
        //gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        rb = GetComponent<Rigidbody>();
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
        _groundChecker = transform.GetChild(0);
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;

        inputs = new bool[2];
    }
    public void checkRay()
    {
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 8;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.5f, layerMask))
        {
           // Debug.Log("Did Hit");
            if (hit.collider.gameObject.tag == "Platform")
            {
               // Debug.Log("On top of platform");
                movingPlatform = hit.collider.gameObject.GetComponent<Rigidbody>();
            }
        }
        else
        {
           // Debug.Log("Did not Hit");
            movingPlatform = null;
        }
    }

    IEnumerator LateCall()
    {
        hasDashed = true;
        yield return new WaitForSeconds(3f);
        hasDashed = false;
    }

    public void FixedUpdate()
    {

        checkRay();

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + camY;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Passes move direction to the character controller
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;


            // Think i need to update this with rigidbody

            rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
        }

        //gravity
        if (movingPlatform)
        {
            rb.MovePosition(rb.position + movingPlatform.velocity * Time.fixedDeltaTime);
        }
        //controller.Move(velocity * Time.fixedDeltaTime);

        // Debug.Log(_groundChecker.position);
        _isGrounded = Physics.CheckSphere(_groundChecker.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);

        if (inputs[0] && _isGrounded)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpSpeed * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }

        if (inputs[1] && !hasDashed)
        {
            Debug.Log("Should dash");
            Vector3 dashVelocity = Vector3.Scale(transform.forward, DashDistance * new Vector3((Mathf.Log(1f / (Time.fixedDeltaTime * rb.drag + 1)) / -Time.fixedDeltaTime), 0, (Mathf.Log(1f / (Time.fixedDeltaTime * rb.drag + 1)) / -Time.fixedDeltaTime)));
            Debug.Log("Forward " + transform.forward);
            rb.AddForce(transform.forward * DashDistance, ForceMode.VelocityChange);
            StartCoroutine(LateCall());
        }

        if (transform.position.y <= -5)
        {
            this.transform.position = new Vector3(0f, 5f, 0f);
            rb.ResetInertiaTensor();
            rb.ResetCenterOfMass();
        }

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);

        //Move(_inputDirection);
    }

/*    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Platform")
        {
            Debug.Log("Collided plat");
            Debug.Log(col.gameObject);
            Debug.Log(col.gameObject.GetComponent<Rigidbody>().velocity);
            velocity = col.gameObject.GetComponent<Rigidbody>().velocity;
        }
    }*/



    private void Move(Vector2 _inputDirection)
    {

        Vector3 _moveDirection = transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        //Check gravity
/*        if (controller.isGrounded)
        {
            yVelocity = 0f;
            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }*/

        yVelocity += gravity;
        _moveDirection.y = yVelocity;

        // Passes move direction to the character controller
        controller.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(Vector3 _direction, float _camY, bool[] _inputs)
    {
        direction = _direction;
        camY = _camY;
        inputs = _inputs;
    }
}
