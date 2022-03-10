using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message
{

    public static Dictionary<int, Message> messages = new Dictionary<int, Message>();

    public int messageId;

    private static int nextMessageId = 1;

    public Message()
    {
        // hasPlatform = false;
        messageId = nextMessageId;
        nextMessageId++;
    }
}
