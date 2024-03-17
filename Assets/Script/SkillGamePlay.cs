using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class SkillGamePlay : MonoBehaviour
{
  [SerializeField] LoginManager loginManager;
  playerSkill PlayerSkill;
  List<playerSkill> playerSkillsOrder;
  string carreer;
  int clientId;
  void Start()
  {

    clientId = loginManager.playerNumber;
    playerSkillsOrder = new List<playerSkill>();
  }
  void Update()
  {
    clientId = loginManager.playerNumber;
    Debug.Log("test" + clientId);
  }
  public void Sword()
  {
    carreer = "sword";
    PlayerSkill = new playerSkill(clientId, carreer);
    playerSkillsOrder.Add(PlayerSkill);
  }
  public void Bow()
  {
    carreer = "bow";
    PlayerSkill = new playerSkill(clientId, carreer);
    playerSkillsOrder.Add(PlayerSkill);
  }
  public void Mage()
  {
    carreer = "mage";
    PlayerSkill = new playerSkill(clientId, carreer);
    playerSkillsOrder.Add(PlayerSkill);
  }
}
