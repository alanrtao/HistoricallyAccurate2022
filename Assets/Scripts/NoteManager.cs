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

    public float bpm0 = 120;
    public float bpm { get { return _bpm; } }
    float _bpm;

    public int engage_beat = 200;

    public static NoteManager Instance { get { return _Instance; } }
    private static NoteManager _Instance;

    public GameObject note_prototype;

    float raw_time;

    public UnityEngine.UI.Slider hp_bar, beat_bar;

    [Range(0, 1)]
    public float size_delta;
    public AnimationCurve size_within_beat;

    public bool invincible; // for testing

    private void Awake()
    {
        _Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        raw_time = 0;

        GetComponent<AudioSource>().Play();

        beat_change += () =>
        {
            // add new note
            if (Random.value < 0.8f && !cd)
                MakeNote();
        };

        _bpm = bpm0;
        health = max_health;
        hp_bar.maxValue = health;
    }

    int whole_beat = 0;

    public delegate void Delegate();
    public Delegate beat_change;

    public Delegate note_removal = () => { };

    // Update is called once per frame
    void Update()
    {

        // double dsp = AudioSettings.dspTime - dsp0;

        _beat_time += Time.deltaTime / 60f * bpm;

        raw_time += Time.deltaTime;

        if (Mathf.FloorToInt(_beat_time) > whole_beat)
        {
            if (beat_change != null) beat_change();
        }
        whole_beat = Mathf.FloorToInt(_beat_time);

        health = Mathf.Min(health, max_health);
        hp_bar.value = (health < 1 ? (health <= 0 ? 0 : 1) : health);
        // beat_bar.value = _beat_time * 10 % 10;

        float t = _beat_time % 1;
        Vector3 ui_size = Vector3.one * (1 + size_delta * size_within_beat.Evaluate(t));

        hp_bar.transform.localScale = ui_size;
        beat_bar.transform.localScale = .8f * ui_size;

        _bpm = bpm0 + whole_beat / 50 * 10;

        Prune(up_notes);
        Prune(down_notes);
        Prune(left_notes);
        Prune(right_notes);

        if (health <= 0)
        {
            LoseGame();
        }
    }

    public void PreWinGame()
    {
        beat_change = null;
        foreach (Note n in FindObjectsOfType<Note>())
        {
            n.done = true; Destroy(n.gameObject);
        }
    }

    public void WinGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    public void LoseGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public float max_health = 3;
    public float health;

    private void FixedUpdate()
    {
    }

    KeyCode[] keys = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow
    };

    public Sprite[] key_sprites;

    Note MakeNote()
    {
        StartCoroutine(CoolDown(.5f * 60f / bpm));

        GameObject go = GameObject.Instantiate(note_prototype, PlayerController.Instance.transform);
        Note n = go.GetComponent<Note>();
        n.key = keys[Mathf.FloorToInt(Random.value * 4)];

        switch(n.key)
        {
            case KeyCode.UpArrow: up_notes.Add(n); break;
            case KeyCode.DownArrow: down_notes.Add(n); break;
            case KeyCode.LeftArrow: left_notes.Add(n); break;
            case KeyCode.RightArrow: right_notes.Add(n); break;
        }

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

    List<Note> up_notes = new List<Note>();
    List<Note> down_notes = new List<Note>();
    List<Note> left_notes = new List<Note>();
    List<Note> right_notes = new List<Note>();

    void Prune(List<Note> ls)
    {
        if (ls.Count > 0)
        {
            if (ls[0].done || ls[0].lifetime < -0.25)
            {
                ls.RemoveAt(0);
            }
        }
        if (ls.Count > 0)
        {
            ls[0].is_head = true;
        }
    }
}
