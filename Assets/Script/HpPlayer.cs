using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HpPlayer : NetworkBehaviour
{
  public NetworkVariable<int> hpP1 = new NetworkVariable<int>(10,
  NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

  public NetworkVariable<int> hpP2 = new NetworkVariable<int>(10,
  NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

  public void PlayerHpDam(int dam)
  {
    if (IsOwnedByServer)
    {
      if (hpP1.Value > 0)
      {
        hpP1.Value = hpP1.Value - dam;
      }
      else
      {
        
      }

    }
    else
    {
      if (hpP1.Value > 0)
      {
        hpP2.Value = hpP2.Value - dam;
      }
      else
      {

      }

    }

  }
  void Update()
  {

  }
}
