using UnityEngine;
using System.Collections;
using DG.Tweening;
using System;
using TMPro;


public class TutorialLeveleHelper : MonoBehaviour
{
    public Transform pos;

    private void Start()
    {
        Pose p = new Pose(pos.position, pos.rotation);
        ModuleSystem.instance.Spawn(typeof(MonochromeModule), p);
    }
}

