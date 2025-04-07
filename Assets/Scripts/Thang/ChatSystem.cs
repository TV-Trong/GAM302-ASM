using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatSystem : NetworkBehaviour
{
    public TextMeshProUGUI textMessage;
    public TMP_InputField inputFieldMessage;
    public GameObject buttonSend;
    private Queue<string> messages = new Queue<string>();
    private const int maxMessages = 9;

    private PlayerNetworkProperties localPlayerNetworkProperties;

    public override void Spawned()
    {
        textMessage = GameObject.Find("Text Message").GetComponent<TextMeshProUGUI>();
        inputFieldMessage = GameObject.Find("InputField Message").GetComponent<TMP_InputField>();
        buttonSend = GameObject.Find("Button Send");
        buttonSend.GetComponent<Button>().onClick.AddListener(SendMessageChat);

        // Get the PlayerNetworkProperties component for the local player
        if (HasStateAuthority)
        {
            localPlayerNetworkProperties = GetComponent<PlayerNetworkProperties>();
        }
    }

    public void SendMessageChat()
    {
        var message = inputFieldMessage.text;
        if (string.IsNullOrWhiteSpace(message))
            return;

        string playerName = "Unknown Player";

        // If local player properties are available, get the player name
        if (localPlayerNetworkProperties != null)
        {
            playerName = localPlayerNetworkProperties.PlayerName;
        }

        var text = $"{playerName}: {message}";

        RPCChat(text);
        inputFieldMessage.text = "";
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPCChat(string msg)
    {
        if (messages.Count >= maxMessages)
        {
            messages.Dequeue(); // Remove the oldest message if limit is reached
        }
        messages.Enqueue(msg);
        textMessage.text = string.Join("\n", messages); // Update UI
    }
}
