using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;

public enum RPSMove
{
  Sword,
  Bow,
  Mage,
  None // Added for initialization
}

public class RPSGameManager : NetworkBehaviour
{
  [SerializeField] private TMP_Text resultText;
  [SerializeField] private GameObject SkillUi;

  public NetworkVariable<RPSMove> player1Move = new NetworkVariable<RPSMove>(RPSMove.None, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
  public NetworkVariable<RPSMove> player2Move = new NetworkVariable<RPSMove>(RPSMove.None, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

  void Start()
  {
    // Ensure that the result text is initially empty
    resultText.text = "";
  }

  void Update()
  {
    CheckResolveGame();
  }

  public void SetPlayerMove(RPSMove move)
  {
    if (!IsServer)
    {
      Debug.LogError("SetPlayerMove called on client.");
      return;
    }

    player1Move.Value = move;
    Debug.Log("Player 1 move set: " + move);


  }

  private void CheckResolveGame()
  {
    // Check if both players have made their moves and the game is not already resolved
    if (player1Move.Value != RPSMove.None && player2Move.Value != RPSMove.None)
    {
      ResolveGame();
    }
  }

  public void Sword()
  {

    if (IsHost)
    {
      SetPlayerMove(RPSMove.Sword);
    }
    else
    {
      // Call the command to set player 2's move
      SetPlayer2MoveServerRpc(RPSMove.Sword);
    }

    SkillUi.SetActive(false);
  }

  public void Bow()
  {

    if (IsHost)
    {
      SetPlayerMove(RPSMove.Bow);
    }
    else
    {
      // Call the command to set player 2's move
      SetPlayer2MoveServerRpc(RPSMove.Bow);
    }

    SkillUi.SetActive(false);
  }

  public void Mage()
  {

    if (IsHost)
    {
      SetPlayerMove(RPSMove.Mage);
    }
    else
    {
      // Call the command to set player 2's move
      SetPlayer2MoveServerRpc(RPSMove.Mage);
    }

    SkillUi.SetActive(false);
  }

  private void ResolveGame()
  {
    // Compare the moves to determine the winner
    if (player1Move.Value == player2Move.Value)
    {
      resultText.text = "It's a tie!";
    }
    else if ((player1Move.Value == RPSMove.Sword && player2Move.Value == RPSMove.Mage) ||
             (player1Move.Value == RPSMove.Bow && player2Move.Value == RPSMove.Sword) ||
             (player1Move.Value == RPSMove.Mage && player2Move.Value == RPSMove.Bow))
    {
      resultText.text = "Player 1 wins!";
    }
    else
    {
      resultText.text = "Player 2 wins!";
    }

    // Synchronize the game result across all clients
    RpcBroadcastResultServerRPC(resultText.text);
  }

  [ServerRpc(RequireOwnership = false)]
  private void RpcBroadcastResultServerRPC(string result)
  {
    resultText.text = result;
    // Ensure that the result text is activated (in case it was deactivated previously)
    resultText.gameObject.SetActive(true);
  }

  [ServerRpc(RequireOwnership = false)]
  private void SetPlayer2MoveServerRpc(RPSMove move)
  {
    player2Move.Value = move;
  }
}
