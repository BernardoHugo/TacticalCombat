using System;
using Catchy.Multiplayer.Chat;
using Catchy.Multiplayer.Chat.Client;
using Catchy.Multiplayer.Chat.Common;
using Catchy.Multiplayer.GameClient;
using UnityEngine;
using UnityEngine.UI;

public class ChatComponent : MonoBehaviour
{

    [SerializeField]
    private Client client;

    [SerializeField]
    private InputField username;

    [SerializeField]
    private Text chat;

    [SerializeField]
    private InputField messageBox;

    private ChatClientHandler _chatClientHandler;

    private void Awake()
    {
        messageBox.onEndEdit.AddListener(SendChatMessage);
    }

    private void Start()
    {
        _chatClientHandler = new ChatClientHandler(client);
        _chatClientHandler.OnChatMessageReceived += OnChatMessageReceived;
    }

    private void OnChatMessageReceived(ChatMessage chatMessage)
    {
        chat.text += string.Format("\n[{0}]: \n{1}", chatMessage.username, chatMessage.message);
    }

    private void SendChatMessage(string message)
    {
        if (string.IsNullOrEmpty(username.text) && string.IsNullOrEmpty(message))
        {
            _chatClientHandler.SendChatMessage(username.text, message);
            messageBox.text = "";
        }
    }
}
