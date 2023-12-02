using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string username;
    public int maxMessages = 25;
    public GameObject chatPanel, textObject, userIconPrefab;
    public InputField chatBox;
    public Color playerMessage, info;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    private bool touchDetected = false;

    void Start()
    {
        // Subscribe to the onEndEdit event
        chatBox.onEndEdit.AddListener(OnChatBoxEndEdit);
    }

    void Update()
    {
        // Check if the input field is focused
        if (chatBox.isFocused)
        {
            // Handle touch input only when the input field is focused
            if (!touchDetected && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                SendMessageToChat("Touch detected on Android!", Message.MessageType.Info);
                Debug.Log("Touch");
                touchDetected = true; // Set the flag to true after the first touch
            }
        }
    }

    void OnChatBoxEndEdit(string text)
    {
        // This function will be called when the user presses "Enter" on the keyboard
        if (!string.IsNullOrWhiteSpace(text)) // Check if the input is not empty or contains only whitespace
        {
            SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.UserMessage);
            chatBox.text = "";
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;
        newMessage.senderUsername = username;
        newMessage.userIconPrefab = userIconPrefab;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);
        newMessage.messageType = messageType; // Assign the message type

        if (messageType == Message.MessageType.UserMessage)
        {
            // Instantiate the user icon next to the message
            GameObject userIcon = Instantiate(userIconPrefab, chatPanel.transform);
            RectTransform iconRect = userIcon.GetComponent<RectTransform>();
            iconRect.anchoredPosition = new Vector2(-150f, newText.GetComponent<RectTransform>().anchoredPosition.y);
        }

        messageList.Add(newMessage);

        // Ensure the InputField is activated after sending a message
        chatBox.ActivateInputField();
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch (messageType)
        {
            case Message.MessageType.UserMessage:
                color = playerMessage;
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;
    public string senderUsername; // New field for sender's username
    public GameObject userIconPrefab; // New field for user icon prefab

    public enum MessageType
    {
        UserMessage,
        Info
    }
}
