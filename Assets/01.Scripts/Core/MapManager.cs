using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    Transform _wallTrm;

    public void Init()
    {
        _wallTrm = transform.Find("Wall");
    }

    public void WallDisable()
    {
        _wallTrm.gameObject.SetActive(false);
    }
}
