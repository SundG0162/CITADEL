using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float ShakeAmount;

    float ShakeTime;

    Vector3 initialPosition;

    private void Start()
    {
        initialPosition = new Vector3(0f, 0f, -5f);
    }

    private void Update()
    {
        if (ShakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * ShakeAmount + initialPosition;
            ShakeTime -= Time.deltaTime;
        }
        else
        {
            ShakeTime = 0.0f;
            transform.position = initialPosition;
        }
    }

    public void VibrateForTime(float time , float amount)
    {
        ShakeAmount = amount;
        ShakeTime = time;
    }
}
