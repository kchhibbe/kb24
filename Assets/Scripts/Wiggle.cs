using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wiggle : MonoBehaviour
{
    // public float speed = 2f;
    // public float maxRotation = 10f;

    // void Update()
    // {
    //     transform.rotation = Quaternion.Euler(0f, 0f, maxRotation * Mathf.Sin(Time.time * speed));
    //     transform.localPosition = new Vector3(maxRotation * Mathf.Sin(Time.deltaTime * speed), 0f, 0f);
    // }

    private Quaternion originRotation;
    public float shake_decay = 0.002f;
    public float shake_intensity = .3f;

    private float temp_shake_intensity = 0;

    void Update()
    {
        if (temp_shake_intensity > 0)
        {
            transform.rotation = new Quaternion(
                originRotation.x + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.y + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.z + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f,
                originRotation.w + Random.Range(-temp_shake_intensity, temp_shake_intensity) * .2f);
            temp_shake_intensity -= shake_decay;
        }
    }

    void Start()
    {
        originRotation = transform.rotation;
        temp_shake_intensity = shake_intensity;
    }
}
