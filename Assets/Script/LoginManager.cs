using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Relay;
using Unity.Services.Core;
using JetBrains.Annotations;

public class LoginManager : MonoBehaviour
{
  public TMP_InputField userNameInputField;
  UnityTransport transport;
  public TMP_Dropdown skinSelector;
  public GameObject loginPannel;
  public TMP_Text win;
  public GameObject skillUi;
  public GameObject leaveButton;
  // public GameObject scorePanel;
  public int playerNumber;
  public TMP_Text roomID;
  public List<GameObject> spawnPoint;
  public List<uint> AlternativePlayerPrefabs;

  void Start()
  {

    NetworkManager.Singleton.OnServerStarted += HandleServerStarted;
    NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
    NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnect;
    SetUIVisible(false);
  }
  public void SetUIVisible(bool isUserLogin)
  {
    if (isUserLogin)
    {
      loginPannel.SetActive(false);
      skillUi.SetActive(true);
      leaveButton.SetActive(true);
    }
    else
    {
      loginPannel.SetActive(true);
      skillUi.SetActive(false);
      leaveButton.SetActive(false);
    }
  }

  private void HandleClientDisconnect(ulong clientId)
  {
    Debug.Log("HandleClientDisconnect client ID = " + clientId);
    if (NetworkManager.Singleton.IsHost) { }
    else if (NetworkManager.Singleton.IsHost) { leaveButtonFunc(); }
  }

  private void HandleClientConnected(ulong clientId)
  {
    Debug.Log("HandleClientConnect client ID = " + clientId);
    if (clientId == NetworkManager.Singleton.LocalClientId)
    {

      SetUIVisible(true);
    }

  }
  public void leaveButtonFunc()
  {
    if (NetworkManager.Singleton.IsHost)
    {
      NetworkManager.Singleton.Shutdown();
      NetworkManager.Singleton.ConnectionApprovalCallback -= ApprovalCheck;
      roomID.text = "";
      win.text = "";
      resetTextServerRPC(win.text);
    }
    else if (NetworkManager.Singleton.IsClient)
    {
      NetworkManager.Singleton.Shutdown();
    }
    SetUIVisible(false);
  }

  private void HandleServerStarted()
  {
    Debug.Log("HandleServerStart");
  }

  private void OnDestroy()
  {
    if (NetworkManager.Singleton == null) { return; }
    NetworkManager.Singleton.OnServerStarted -= HandleServerStarted;
    NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
    NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnect;

  }

  public async void Host()
  {
    await UnityServices.InitializeAsync();
    if (RelayManagerScript.Instance.IsRelayEnabled)
    {
      await RelayManagerScript.Instance.CreateRelay();
    }
    NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;
    NetworkManager.Singleton.StartHost();
    Debug.Log("start host");
  }
  public TMP_InputField joinCodeInputField;
  public string joinCode;
  public async void Client()
  {
    joinCode = joinCodeInputField.GetComponent<TMP_InputField>().text;
    if (RelayManagerScript.Instance.IsRelayEnabled && !string.IsNullOrEmpty(joinCode))
    {
      await RelayManagerScript.Instance.JoinRelay(joinCode);
    }
    string userName = userNameInputField.GetComponent<TMP_InputField>().text;
    int playerSkin = skinSelected();
    NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(userName + ":" + playerSkin);
    NetworkManager.Singleton.StartClient();
    Debug.Log("start client");
  }
  private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
  {
    // The client identifier to be authenticated
    var
    clientId = request.ClientNetworkId;
    playerNumber = (int)clientId;
    Debug.Log("test" + playerNumber);

    // Additional connection data defined by user code
    var connectionData = request.Payload;
    int byteLength = connectionData.Length;
    bool isApprove = false;
    if (byteLength > 0)
    {
      string rawData = System.Text.Encoding.ASCII.GetString(connectionData, 0, byteLength);
      string[] informationSplit = rawData.Split(":");
      string hostData = userNameInputField.GetComponent<TMP_InputField>().text;
      string usernameClient = informationSplit[0];
      int SkinSelect = int.Parse(informationSplit[1]);
      Debug.Log(SkinSelect);
      isApprove = ApproveConnection(usernameClient, hostData);
      response.PlayerPrefabHash = AlternativePlayerPrefabs[SkinSelect];
    }
    else
    {
      if (NetworkManager.Singleton.IsHost)
      {
        response.PlayerPrefabHash = AlternativePlayerPrefabs[skinSelected()];
      }
    }
    // Your approval logic determines the following values
    response.Approved = isApprove;
    response.CreatePlayerObject = true;
    // The Prefab hash value of the ;, if null the default NetworkManager player Prefab is used

    // Position to spawn the player object (if null it uses default of Vector3.zero)
    response.Position = Vector3.zero;

    // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
    response.Rotation = Quaternion.identity;
    setSpawnLocation(clientId, response);

    // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
    // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
    response.Reason = "Some reason for not approving the client";

    // If additional approval steps are needed, set this to true until the additional steps are complete
    // once it transitions from true to false the connection approval response will be processed.
    response.Pending = false;
  }
  private void setSpawnLocation(ulong clientId,
  NetworkManager.ConnectionApprovalResponse response)
  {
    Vector3 spawnPos = Vector3.zero;
    Quaternion spawnRo = Quaternion.identity;
    if (clientId == NetworkManager.Singleton.LocalClientId)
    {
      GameObject selectSpawn = spawnPoint[0];
      spawnPos = selectSpawn.transform.position;
      spawnRo = selectSpawn.transform.rotation;
    }
    else if (clientId == 1)
    {
      GameObject selectSpawn = spawnPoint[1];
      spawnPos = selectSpawn.transform.position;
      spawnRo = selectSpawn.transform.rotation;
    }
    else if (clientId == 2)
    {
      GameObject selectSpawn = spawnPoint[2];
      spawnPos = selectSpawn.transform.position;
      spawnRo = selectSpawn.transform.rotation;
    }
    else if (clientId == 3)
    {
      GameObject selectSpawn = spawnPoint[1];
      spawnPos = selectSpawn.transform.position;
      spawnRo = selectSpawn.transform.rotation;
    }
    response.Position = spawnPos;
    response.Rotation = spawnRo;

  }

  public bool ApproveConnection(string clientUsername, string hostUsername)
  {
    bool isApprove = System.String.Equals(clientUsername.Trim(), hostUsername.Trim()) ? false : true;
    return isApprove;

  }
  public int skinSelected()
  {
    if (skinSelector.GetComponent<TMP_Dropdown>().value == 0)
    {
      return 0;
    }
    else if (skinSelector.GetComponent<TMP_Dropdown>().value == 1)
    {
      return 1;
    }
    else if (skinSelector.GetComponent<TMP_Dropdown>().value == 2)
    {
      return 2;
    }
    else if (skinSelector.GetComponent<TMP_Dropdown>().value == 3)
    {
      return 3;
    }
    return 0;
  }
  [ServerRpc(RequireOwnership = false)]
  public void resetTextServerRPC(string text)
  {
    win.text = text;
  }
}

