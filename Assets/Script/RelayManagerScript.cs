using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;
using System.IO;
using TMPro;


public class RelayManagerScript : Singleton<RelayManagerScript>
{

  [SerializeField] TMP_Text roomId;
  private async void Start()
  {
    roomId.text = "";
    await UnityServices.InitializeAsync();

    if (!AuthenticationService.Instance.IsSignedIn)
    {
      AuthenticationService.Instance.SignedIn += () =>
      {
        Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
      };
      await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }
  }
  public UnityTransport transport => NetworkManager.Singleton.GetComponent<UnityTransport>();
  public bool IsRelayEnabled =>
    transport != null && transport.Protocol == UnityTransport.ProtocolType.RelayUnityTransport;


  public async Task CreateRelay()
  {
    try
    {
      Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);
      string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

      Debug.Log("Join code" + joinCode);
      roomId.text = joinCode;
      RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
      NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
      // NetworkManager.Singleton.StartHost();
    }
    catch (RelayServiceException e) { Debug.Log(e); }
  }

  public async Task JoinRelay(string joinCode)
  {
    try
    {
      JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
      RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
      NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);
      // NetworkManager.Singleton.StartClient();
    }
    catch (RelayServiceException e) { Debug.Log(e); }
  }
}
