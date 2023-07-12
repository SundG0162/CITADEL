using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    Transform _centerCamTrm;
    public void Init()
    {
        _centerCamTrm = transform.Find("CenterCam");
    }



    public void CenterCam()
    {
        StartCoroutine(CenterCamCoroutine());
    }

    IEnumerator CenterCamCoroutine()
    {
        var centerCam = _centerCamTrm.GetComponent<CinemachineVirtualCamera>();
        centerCam.Priority = 20;
        yield return new WaitForSeconds(5f);
        centerCam.Priority = 10;
    }
}

