using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(_packet);
        }
    }

    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }

    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i < Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets

    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);

            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            // This only happens once so important (USE TCP)
            SendTCPData(_toClient, _packet);
        }
    }

    public static void SpawnPlatform(int _toClient, Platform _platform)
    {
        using (Packet _packet = new Packet((int)ServerPackets.spawnPlatform))
        {
            _packet.Write(_platform.platformId);
            _packet.Write(_platform.transform.position);
            _packet.Write(_platform.transform.rotation);
            _packet.Write(_platform.transform.localScale);

            // This only happens once so important (USE TCP)
            SendTCPData(_toClient, _packet);
        }
    }

    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.GetComponent<Rigidbody>().velocity);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_packet);
        }
    }

    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_playerId, _packet);
        }
    }

    public static void PlatformRotation(Platform _platform)
    {
        using (Packet _packet = new Packet((int)ServerPackets.platformRotation))
        {
            _packet.Write(_platform.platformId);
            _packet.Write(_platform.transform.rotation);

            SendUDPDataToAll(_platform.platformId, _packet);
        }
    }

    public static void PlatformPosition(Platform _platform)
    {
        using (Packet _packet = new Packet((int)ServerPackets.platformPosition))
        {
            _packet.Write(_platform.platformId);
            _packet.Write(_platform.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    public static void MessageReceived(int senderId, string _msg, int _msgId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.messageReceived))
        {
            _packet.Write(senderId);
            _packet.Write(_msg);
            _packet.Write(_msgId);

            SendTCPDataToAll(_packet);
        }
    }

    #endregion
}
