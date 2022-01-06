using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CamTrack : MonoBehaviour
{
    CinemachineVirtualCamera vc;
    CinemachineBasicMultiChannelPerlin mcp;

    public static CamTrack Instance { get { return _Instance; } }
    private static CamTrack _Instance;

    private void Awake()
    {
        _Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        vc = GetComponent<CinemachineVirtualCamera>();
        mcp = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    float ext = 0;
    // Update is called once per frame
    Vector3 offset = new Vector3(0, 0, -10);
    void Update()
    {
        ext = 0.5f * ext;
        mcp.m_AmplitudeGain = ext;

        transform.position = PlayerController.Instance.transform.position + offset;
    }

    public void UpdateShake(float magnitude)
    {
        ext = Mathf.Max(ext, magnitude);
    }
}
