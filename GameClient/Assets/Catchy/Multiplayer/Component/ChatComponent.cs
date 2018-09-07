 using System;
using Catchy.Multiplayer.Common.Chat;
using Catchy.Multiplayer.GameClient;
using UnityEngine;
using UnityEngine.UI;

public class ChatComponent : MonoBehaviour {

    [SerializeField]
    private Client client;

    [SerializeField]
    private InputField username;

    [SerializeField]
    private Text chat;

    [SerializeField]
    private InputField messageBox;

    private ChatHandler chatHandler;

    private void Awake()
    {
        messageBox.onEndEdit.AddListener(SendChatMessage);
    }

    private void Start()
    {
        chatHandler = new ChatHandler(client);
        chatHandler.OnChatMessageReceived += OnChatMessageReceived;
    }

    private void OnChatMessageReceived(ChatMessage chatMessage)
    {
        chat.text += string.Format("\n[{0}]: \n{1}",chatMessage.username,chatMessage.message);
    }

    private void SendChatMessage(string message)
    {
        chatHandler.SendChatMessage(username.text, message);
        messageBox.text = "";
    }
}
