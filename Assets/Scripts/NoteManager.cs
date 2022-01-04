using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    public float beat_time
    {
        get { return _beat_time; }
    }
    public float beat_time_whole { get { return whole_beat; } }

    private float _beat_time = 0;

    public float bpm = 120;

    public float head = 8; // the 

    public NoteManager Instance { get { return _Instance; } }
    private NoteManager _Instance;

    List<Note> notes;
    GameObject note_prototype;

    double dsp0;

    // Start is called before the first frame update
    void Start()
    {
        dsp0 = AudioSettings.dspTime;

        GetComponent<AudioSource>().Play();
    }

    int whole_beat = 0;

    public delegate void OnWholeBeatChangeDelegate();
    public OnWholeBeatChangeDelegate beat_change = () =>
    {
        // add new note

    };

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        double dsp = AudioSettings.dspTime - dsp0;

        _beat_time += Time.deltaTime / 60f * bpm;

        if (Mathf.FloorToInt(_beat_time) > whole_beat)
        {
            beat_change();
        }
        whole_beat = Mathf.FloorToInt(_beat_time);
    }

    KeyCode[] keys = new KeyCode[] { 
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };

    Note MakeNote()
    {
        GameObject go = GameObject.Instantiate(note_prototype, transform);
        Note n = go.GetComponent<Note>();
        n.key = keys[Mathf.FloorToInt(Random.value * keys.Length)];
        return n;
    }
}
