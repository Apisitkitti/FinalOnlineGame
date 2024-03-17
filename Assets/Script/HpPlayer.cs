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
}
