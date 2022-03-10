using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{

    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceieved()
    {
        using(Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlatformReceived(int _platformId)
    {
        using (Packet _packet = new Packet((int)ClientPackets.platformReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(_platformId);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(Vector3 _direction, float _camY, bool[] _inputs) 
    {
        using(Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(_direction);
            _packet.Write(_camY);
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            //Using UDP because it is sent over and over plus better speed
            SendUDPData(_packet);
        }
    }

    public static void SendMessage(string message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendMessage))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(GameManager.players[Client.instance.myId].username);
            _packet.Write(message);

            SendTCPData(_packet);
        }
    }

    #endregion
}
