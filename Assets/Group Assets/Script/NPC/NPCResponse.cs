using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCResponse : MonoBehaviour
{
    [HideInInspector] public int NPCPath;
    [HideInInspector] public string NPCState;
    [HideInInspector] public string NPCSetState;
    [HideInInspector] public string NPCReward;
    [HideInInspector] public string NPCTake;

    public void ResetValues()
    {
        NPCPath = 0;
        NPCState = null;
        NPCSetState = null;
        NPCReward = null;
        NPCTake = null;
    }
}
