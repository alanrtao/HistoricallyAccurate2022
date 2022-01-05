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

    public static NoteManager Instance { get { return _Instance; } }
    private static NoteManager _Instance;

    public GameObject note_prototype;

    double dsp0;

    public UnityEngine.UI.Slider hp_bar, beat_bar;

    [Range(0, 1)]
    public float size_delta;
    public AnimationCurve size_within_beat;

    private void Awake()
    {
        _Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        dsp0 = AudioSettings.dspTime;

        GetComponent<AudioSource>().Play();

        beat_change += () =>
        {
            // add new note
            /*if (Random.value > 0.8f)
                MakeNote(true);*/
            if (Random.value < 0.8f && !cd)
                MakeNote(false);
        };
    }

    int whole_beat = 0;

    public delegate void Delegate();
    public Delegate beat_change;

    public Delegate note_removal = () => { };

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

        beat_bar.value = _beat_time * 10 % 10;

        float t = _beat_time % 1;
        Vector3 ui_size = Vector3.one * (1 + size_delta * size_within_beat.Evaluate(t));

        hp_bar.transform.localScale = ui_size;
        beat_bar.transform.localScale = .8f * ui_size;
    }

    KeyCode[] l_keys = new KeyCode[] { 
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D
    };

    KeyCode[] r_keys = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };

    public Sprite[] key_sprites;

    Note MakeNote(bool l)
    {
        StartCoroutine(CoolDown(0.5f));

        GameObject go = GameObject.Instantiate(note_prototype, PlayerController.Instance.transform);
        Note n = go.GetComponent<Note>();
        n.key = (l ? l_keys : r_keys)[Mathf.FloorToInt(Random.value * 4)];
        n.left = l;

        return n;
    }

    bool cd = false;
    IEnumerator CoolDown(float t)
    {
        cd = true;
        float t_ = 0;
        while (t_ < t)
        {
            t_ += Time.deltaTime;
            yield return null;
        }
        cd = false;
    }

    public Sprite GetSprite(KeyCode k)
    {
        if (k == KeyCode.W) return key_sprites[0];
        if (k == KeyCode.A) return key_sprites[1];
        if (k == KeyCode.S) return key_sprites[2];
        if (k == KeyCode.D) return key_sprites[3];
        if (k == KeyCode.UpArrow) return key_sprites[4];
        if (k == KeyCode.DownArrow) return key_sprites[5];
        if (k == KeyCode.LeftArrow) return key_sprites[6];
        if (k == KeyCode.RightArrow) return key_sprites[7];
        return null;
    }
}
