using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sizer : MonoBehaviour
{
    public float size_delta = 0.1f;
    public float sharpness = 5;
    Vector3 init_size;

    // Start is called before the first frame update
    void Start()
    {
        init_size = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        float t = NoteManager.Instance.beat_time - NoteManager.Instance.beat_time_whole;
        transform.localScale = init_size * (1 + size_delta * Mathf.Max(0, Mathf.Pow(1 - sharpness * t, 3)));
    }
}
