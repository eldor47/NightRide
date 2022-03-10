using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        UnityEngine.Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceieved();


        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);

    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void SpawnPlatform(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        Vector3 _scale = _packet.ReadVector3();

        GameManager.instance.SpawnPlatform(_id, _position, _rotation, _scale);

        // ClientSend.PlatformReceived(_id);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        Vector3 _velocity = _packet.ReadVector3();

        //Trying to work on some sort of client frontend rigidbody rendering plus predictive rendering

       //GameManager.players[_id].transform.position = _position;
        GameManager.players[_id].lastVelocityValue = _velocity;
        GameManager.players[_id].transform.position = _position;
        GameManager.players[_id].lastPositionUpdate = Time.time;

        // Lag History Compensation Values
        GameManager.players[_id].positionHistory.Add(_position);
        GameManager.players[_id].velocityHistory.Add(_velocity);
        GameManager.players[_id].timeHistory.Add(Time.time);
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Debug.Log("Client removing player " + _id);

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlatformRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.platforms[_id].transform.rotation = _rotation;
    }

    public static void PlatformPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.platforms[_id].transform.position = _position;
    }

    public static void MessageReceived(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _message = _packet.ReadString();
        int _messageId = _packet.ReadInt();
        GameManager.instance.SpawnMessage(_id, _message, _messageId);
    }
}
