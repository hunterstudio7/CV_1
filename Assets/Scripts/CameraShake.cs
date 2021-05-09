using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Transform camTrasform;
    private float shakeDur = 1f, shakeAmount = 0.8f, decreaseFactor=1.5f;
    private Vector3 originPos;
    private void Start()
    {
        camTrasform = GetComponent<Transform>();
        originPos = camTrasform.localPosition;
    }
    private void Update()
    {
        if (shakeDur > 0)
        {
            camTrasform.localPosition = originPos + Random.insideUnitSphere * shakeAmount;
            shakeDur -= Time.deltaTime * decreaseFactor;


        }
        else
        {
            shakeDur = 0;
            camTrasform.localPosition = originPos;
        }
    }

}
