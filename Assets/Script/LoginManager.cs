using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LoginManager : MonoBehaviour
{
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
  private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
  {
    // The client identifier to be authenticated
    var clientId = request.ClientNetworkId;

    // Additional connection data defined by user code
    var connectionData = request.Payload;
    int byteLength = connectionData.Length;
    bool isApprove = false;
    if (byteLength > 0)
    {
      string rawData = System.Text.Encoding.ASCII.GetString(connectionData, 0, byteLength);
      string[] informationSplit = rawData.Split(":");
      // string hostData = userNameInputField.GetComponent<TMP_InputField>().text;
      // string usernameClient = informationSplit[0];
      // int passcodeClient = int.Parse(informationSplit[1]);
      // int SkinSelect = int.Parse(informationSplit[2]);
      // Debug.Log(SkinSelect);
      // isApprove = ApproveConnection(usernameClient, hostData, passcodeClient);
      // response.PlayerPrefabHash = AlternativePlayerPrefabs[SkinSelect];
    }
    else
    {
      if (NetworkManager.Singleton.IsHost)
      {
        // response.PlayerPrefabHash = AlternativePlayerPrefabs[skinSelected()];
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
    // setSpawnLocation(clientId, response);

    // If response.Approved is false, you can provide a message that explains the reason why via ConnectionApprovalResponse.Reason
    // On the client-side, NetworkManager.DisconnectReason will be populated with this message via DisconnectReasonMessage
    response.Reason = "Some reason for not approving the client";

    // If additional approval steps are needed, set this to true until the additional steps are complete
    // once it transitions from true to false the connection approval response will be processed.
    response.Pending = false;
  }
}
