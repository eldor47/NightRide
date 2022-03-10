using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Stores all player information
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, Platform> platforms = new Dictionary<int, Platform>();
    public static Dictionary<int, Message> messages = new Dictionary<int, Message>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject messagePrefab;

    public GameObject platformPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void FixedUpdate()
    {
        if (UIManager.instance.messageActive)
        {
            // nbumber of messages currently spawned
            int numMessagesCurr = GameObject.FindGameObjectWithTag("scrollView").GetComponent<Canvas>().transform.childCount;
            if (numMessagesCurr < messages.Count)
            {
                Canvas renderCanvas = GameObject.FindGameObjectWithTag("scrollView").GetComponent<Canvas>();
                Debug.Log(messages.Count);
                int i = 0;
                foreach (KeyValuePair<int, Message> message in messages.Reverse())
                {

                    message.Value.transform.SetParent(renderCanvas.transform, false);
                    message.Value.GetComponent<RectTransform>().localPosition = new Vector3(0f, 20 * i - 240, 0f);
                    string username = GameManager.players[message.Value.senderId].username;
                    message.Value.transform.GetChild(0).GetComponent<Text>().text = "[" + username +  "]: " + message.Value.message;

                    //Need to figure out spacing
                    i++;
                }
            }
        }
    }

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if(_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        } else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;
        _player.GetComponent<PlayerManager>().sensitivityX = 10f;
        _player.GetComponent<PlayerManager>().sensitivityY = 10f;
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnPlatform(int _platformId, Vector3 _position, Quaternion _rotation, Vector3 _scale)
    {
        GameObject _platform = Instantiate(platformPrefab, _position, _rotation);
        _platform.transform.localScale = _scale;
        _platform.GetComponent<Platform>().Initialize(_platformId);
        platforms.Add(_platformId, _platform.GetComponent<Platform>());

    }
    public void SpawnMessage(int _userId, string _message, int _messageId)
    {
        Debug.Log("Test receiveign the message");
        Debug.Log(_message);
        // NEed to properly add this to the scene with the text
        GameObject message = Instantiate(messagePrefab, new Vector3(0, 0 ,0), Quaternion.identity);
        message.GetComponent<Message>().Initialize(_messageId, _message, _userId);
        messages.Add(_messageId, message.GetComponent<Message>());
    }

}
