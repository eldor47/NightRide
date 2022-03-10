using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverText : MonoBehaviour
{
    public Text nameLabel;

    public void Start()
    {
        nameLabel.transform.position = new Vector3(-10, -10, -10);
        nameLabel.text = GetComponent<PlayerManager>().username;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 namePose = Camera.main.WorldToScreenPoint(new Vector3(this.transform.position.x, this.transform.position.y + 1.2f, this.transform.position.z));
        nameLabel.transform.position = namePose;
    }
}
