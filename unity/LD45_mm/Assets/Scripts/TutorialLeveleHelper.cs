using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using TMPro;


public class TutorialLeveleHelper : MonoBehaviour
{
    public Transform sightPos, jumpPosition, audioPos;

    private void Start()
    {
        Pose p = new Pose(sightPos.position, Quaternion.identity);
        ModuleSystem.instance.Spawn(typeof(MonochromeModule), p);

        p = new Pose(jumpPosition.position, Quaternion.identity);
        ModuleSystem.instance.Spawn(typeof(JumpModule), p);

        p = new Pose(audioPos.position, Quaternion.identity);
        ModuleSystem.instance.Spawn(typeof(AudioModule), p);
    }
}

