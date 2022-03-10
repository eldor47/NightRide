using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public int messageId;
    public string message;
    public int senderId;

    public void Initialize(int _messageId, string _message, int _senderId)
    {
        messageId = _messageId;
        message = _message;
        senderId = _senderId;
    }
}
