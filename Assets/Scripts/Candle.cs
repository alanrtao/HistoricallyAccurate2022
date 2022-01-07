using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Candle : MonoBehaviour
{
    [SerializeField]
    private Light2D self;
    public Gradient light_col;

    float seed;

    // Start is called before the first frame update
    void Start()
    {
        seed = Random.value * 10f;
    }

    // Update is called once per frame
    void Update()
    {
        float light_factor = Mathf.PerlinNoise(Time.time, seed);
        self.color = light_col.Evaluate(light_factor);
    }
}
