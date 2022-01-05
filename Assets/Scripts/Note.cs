using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public KeyCode key { get { return _key; } set
        {
            _key = value;
            switch (_key)
            {
                case KeyCode.UpArrow:
                    {
                        trans_mult.y = 1;
                        break;
                    }
                case KeyCode.DownArrow:
                    {
                        trans_mult.y = -1;
                        break;
                    }
                case KeyCode.LeftArrow:
                    {
                        trans_mult.x = -1;
                        break;
                    }
                case KeyCode.RightArrow:
                    {
                        trans_mult.x = 1;
                        break;
                    }
            }

            img = GetComponent<SpriteRenderer>();
            img.sprite = NoteManager.Instance.GetSprite(_key);
        }
    }
    KeyCode _key;

    // private RectTransform r;

    public bool left;

    public float time { get { return lifetime; } }
    float lifetime = 4;

    [Range(0, 1)]
    public float threshold;

    SpriteRenderer img;

    // Start is called before the first frame update
    void Start()
    {
        NoteManager.Instance.beat_change += PerBeat;

        UpdatePosition();
    }

    public Gradient appearance;
    public Gradient damage;

    public AnimationCurve eliminate;
    bool done;

    // Update is called once per frame
    void Update()
    {
        if (lifetime < -0.5) Destroy(gameObject);

        if (Input.GetKeyDown(key) && lifetime >= 0 && lifetime <= 1)
        {
            lifetime = -float.Epsilon;
            if (lifetime > threshold)
            {
                // failure
                done = false;
            } else
            {
                // success
                done = true;
            }
        }

        if (lifetime < 0)
        {
            if (!done)
                img.color = damage.Evaluate(-2 * lifetime);
            else
            {
                Color c = img.color;
                c.a = 1 - 2 * lifetime;
                img.color = c;
            }
        } else
        {
            img.color = appearance.Evaluate(1 - lifetime / 4);
        }
    }

    private void PerBeat()
    {
    }


    private void FixedUpdate()
    {
        UpdatePosition();
        lifetime -= Time.deltaTime * NoteManager.Instance.bpm / 60f;
    }

    private void UpdatePosition()
    {
        if(lifetime > threshold)
        {
            transform.localPosition = new Vector3(
                (lifetime - threshold) * trans_mult.x,
                (lifetime - threshold) * trans_mult.y, 
                0) + trans_mult;
        } else
        {

            transform.localPosition = trans_mult;
        }
        
        if (lifetime < 0)
        {
            float t = 2 * lifetime;
            transform.localPosition += 0.1f * new Vector3(Mathf.PerlinNoise(t * 4, 0), 0, 0);
        }
    }

    private void OnDestroy()
    {
        NoteManager.Instance.beat_change -= PerBeat;
    }

    Vector3 trans_mult = Vector3.zero;

}
