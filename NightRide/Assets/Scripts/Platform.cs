using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int speed;
    public int platformId;
    public MeshRenderer itemModel;
    public Rigidbody rb;

    private bool initialized = false;

    private Vector3 basePosition;

    public void Initialize(int _platformId)
    {
        platformId = _platformId;
        itemModel.enabled = true;

        basePosition = transform.position;
        initialized = true;

        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        if (initialized)
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0f, 100f, 0f) * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

}

