using Catchy.Multiplayer.Chat.Client;
using Catchy.Multiplayer.Chat.Common;
using Catchy.Multiplayer.Component;
using UnityEngine;
using UnityEngine.UI;

namespace Catchy.Multiplayer.Chat.Component
{
    public class ChatComponent : MonoBehaviour
    {
        [SerializeField] private ClientComponent _clientComponent;

        [SerializeField] private InputField _username;

        [SerializeField] private Text _chat;

        [SerializeField] private InputField _messageBox;

        private ChatClientHandler _chatClientHandler;

        private void Awake()
        {
            _messageBox.onEndEdit.AddListener(SendChatMessage);
        }

        private void Start()
        {
            _chatClientHandler = new ChatClientHandler(_clientComponent.Client);
            _chatClientHandler.OnChatMessageReceived += OnChatMessageReceived;
        }

        private void OnChatMessageReceived(ChatMessage chatMessage)
        {
            Debug.Log($"Message Received: Username: {chatMessage.Username}, Message: {chatMessage.Message}");
            _chat.text += $"\n[{chatMessage.Username}]: \n{chatMessage.Message}";
        }

        private void SendChatMessage(string message)
        {
            
            if (!string.IsNullOrEmpty(_username.text) && !string.IsNullOrEmpty(message))
            {
                Debug.Log($"Sending Message: Username: {_username.text}, Message: {message}");
                _chatClientHandler.SendChatMessage(_username.text, message);
                _messageBox.text = "";
            }
        }
    }
}