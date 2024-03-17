using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillGamePlay : MonoBehaviour
{
  [SerializeField] LoginManager loginManager;
  PlayerSkill playerSkill;
  public List<PlayerSkill> playerSkillsOrder;
  public string carreer;
  int clientId;
  void Start()
  {
    clientId = loginManager.playerNumber;
    playerSkillsOrder = new List<PlayerSkill>();
  }
  void Update()
  {
    if (clientId != loginManager.playerNumber)
    {
      clientId = loginManager.playerNumber;
    }
  }
  public void Sword()
  {
    carreer = "sword";
    AddCarrier(clientId, carreer);
  }
  public void Bow()
  {
    carreer = "bow";
    AddCarrier(clientId, carreer);
  }
  public void Mage()
  {
    carreer = "mage";
    AddCarrier(clientId, carreer);
  }
  private void AddCarrier(int clientId, string carreer)
  {
    playerSkill = new PlayerSkill(clientId, carreer);
    playerSkillsOrder.Add(playerSkill);
  }
}
