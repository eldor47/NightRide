using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        UnityEngine.Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            //Something bad has happened
            UnityEngine.Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }

        //Sends player into game
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlatformReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        int _platformId = _packet.ReadInt();

        Debug.Log("Platform received");

        // What to do here. How would I enable
        //Platform.platforms[_platformId].platFormReceived = true;
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }

        Vector3 _direction = _packet.ReadVector3();
        float _camY = _packet.ReadFloat();


        Server.clients[_fromClient].player.SetInput(_direction, _camY, _inputs);
    }

    public static void SendMessage(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();
        string _message = _packet.ReadString();

        Message message = new Message();

        //Probably do some check here with the senderId and the from client

        ServerSend.MessageReceived(_clientIdCheck, _message, message.messageId);
    }


}
