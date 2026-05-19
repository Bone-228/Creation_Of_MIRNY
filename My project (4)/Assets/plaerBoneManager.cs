using System;
using UnityEngine;

public class PlayerBoneManager : MonoBehaviour
{
    public Transform playerCam;
    public Transform headBone;
    public BattleZoneHandler battleZoneHandler;

    void Update()
    {
        if (battleZoneHandler != null && battleZoneHandler.playerInside)
        {
            Vector3 headRot = headBone.eulerAngles;
            headRot.x = playerCam.eulerAngles.x;
            headBone.eulerAngles = headRot;
            Debug.Log("Head bone rotation updated to match camera.");
        }
    }
}