using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
[CreateAssetMenu(fileName = "SecretPasswordSO")]
public class PasswordContainerSO : ScriptableObject
{
  [ReadOnly]
  public  string secretPassword = "ebcb6eb1-c67a-49ad-9550-a573f2b0d55b";
}
