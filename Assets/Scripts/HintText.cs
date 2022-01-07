using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintText : MonoBehaviour
{
    public TMPro.TextMeshPro text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = NoteManager.Instance.beat_time_whole + "/" + NoteManager.Instance.engage_beat;
        if (NoteManager.Instance.beat_time_whole > NoteManager.Instance.engage_beat) Destroy(gameObject);
    }
}
