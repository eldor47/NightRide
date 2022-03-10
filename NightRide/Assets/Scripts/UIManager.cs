using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;

    public GameObject settingsMenu;

    public GameObject messageMenu;

    public Slider sliderX;
    public Text sensXText;

    public Slider sliderY;
    public Text sensYText;

    public InputField message;
    public ScrollRect scrollRect;

    public InputField usernameField;

    public bool settingsActive = false;
    public bool messageActive = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            settingsMenu.SetActive(false);
            messageMenu.SetActive(false);
            sliderX.interactable = false;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying objects!");
            Destroy(this);
        }
    }

    public void Start()
    {
        //Adds a listener to the main slider and invokes a method when the value changes.
        sliderX.onValueChanged.AddListener(delegate { ValueChangeCheckX(); });
        sliderY.onValueChanged.AddListener(delegate { ValueChangeCheckY(); });

    }

    public void FixedUpdate()
    {
        // Check children here and compare to current ones spawned in

        //If children dont match then add missing ones.

        //Keep queue of like 50
    }

    public void toggleMessageActive()
    {
        messageMenu.SetActive(!messageActive);
        message.text = "";
        message.interactable = !messageActive;
        messageActive = !messageActive;
        if (messageActive)
        {
            message.Select();
        }
    }

    public void SendMessage(String message)
    {
        ClientSend.SendMessage(message);
    }

    public void toggleSettingsActive()
    {
        settingsMenu.SetActive(!settingsActive);
        sliderX.interactable = !settingsActive;
        settingsActive = !settingsActive;
        if (settingsActive)
        {
            sliderX.value = GameManager.players[Client.instance.myId].sensitivityX;
            sensXText.text = "SensX: " + sliderX.value.ToString();

            sliderY.value = GameManager.players[Client.instance.myId].sensitivityY;
            sensYText.text = "SensY: " + sliderY.value.ToString();
        }
    }

    // Invoked when the value of the slider changes.
    public void ValueChangeCheckX()
    {
        Debug.Log(sliderX.value);
        GameManager.players[Client.instance.myId].sensitivityX = sliderX.value;
        sensXText.text = "SensX: " + sliderX.value.ToString("F2");
    }
    public void ValueChangeCheckY()
    {
        Debug.Log(sliderX.value);
        GameManager.players[Client.instance.myId].sensitivityY = sliderY.value;
        sensYText.text = "SensY: " + sliderY.value.ToString("F2");
    }


    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;

        Client.instance.ConnectToServer();

    }
    public void DisconnectFromServer()
    {
        Destroy(GameManager.players[Client.instance.myId].gameObject);
        GameManager.players.Remove(Client.instance.myId);

        //Remove platforms
        GameManager.platforms.Clear();

        Client.instance.Disconnect();

        // SOmehow reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
