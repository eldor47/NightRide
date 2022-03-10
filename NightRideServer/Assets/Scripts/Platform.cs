using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int speed;
    public Rigidbody rb; 

    public static Dictionary<int, Platform> platforms = new Dictionary<int, Platform>();

    public int platformId;

    private static int nextPlatformId = 1;

    public bool platFormReceived = false;
    public void Start()
    {
        // hasPlatform = false;
        platformId = nextPlatformId;
        nextPlatformId++;

        platforms.Add(platformId, this);

        rb = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler(new Vector3(0f, 100f, 0f) * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * deltaRotation);
    }
}
