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
    public float lifetime = 4;

    [Range(0, 1)]
    public float threshold;

    SpriteRenderer img;

    // Start is called before the first frame update
    void Start()
    {
        NoteManager.Instance.beat_change += PerBeat;

        UpdatePosition(true);

        img = GetComponent<SpriteRenderer>();
        img.color = new Color(1, 1, 1, 0);
    }

    public Gradient appearance;
    public Gradient damage;

    public AnimationCurve eliminate;
    public bool done;
    float done_time = 0;
    public bool is_head = false;

    delegate void Slot();
    Slot fire_at_damage = () => {
        CamTrack.Instance.UpdateShake(15f);
        NoteManager.Instance.health -= 1;
    };
    Slot fire_at_heal = () => NoteManager.Instance.health += 0.05f;


    // Update is called once per frame
    void Update()
    {
        if (lifetime < -1) Destroy(gameObject);

        if (lifetime >= -0.25 && lifetime <= 1.25f && is_head)
        {
            // clickable
            transform.localScale = Vector3.one;

            if (Input.GetKeyUp(key))
            {
                done = true;
                CamTrack.Instance.UpdateShake(2f);
            }
            if (lifetime < 0.0f)
            {
                img.color = Color.white;
            }
        } else
        {
           transform.localScale = 0.8f * Vector3.one;
        }


        if (lifetime < 0)
        {
            if (!done)
            {
                if (fire_at_damage != null)
                {
                    fire_at_damage();
                    fire_at_damage = null;
                }
                img.color = damage.Evaluate(-lifetime);
            }
            else
            {
                if (fire_at_heal != null)
                {
                    fire_at_heal();
                    fire_at_heal = null;
                }
                // img.sprite = null;
            }
        } else
        {
            img.color = appearance.Evaluate(1 - lifetime / 4);
        }

        if (done)
        {
            Color c = img.color;
            c.a = Mathf.Pow((1 - done_time), 3);
            img.color = c;
        }

        UpdatePosition();

        lifetime -= Time.deltaTime * NoteManager.Instance.bpm / 60f;
        if (done) done_time += Time.deltaTime * NoteManager.Instance.bpm / 60f;
    }

    private void PerBeat()
    {
    }

    // public float smoothness = 0.1f;
    private void FixedUpdate()
    {
        // UpdatePosition();
        transform.localPosition = eq_position;
    }

    Vector3 eq_position;
    private void UpdatePosition(bool force = false)
    {
        int halt = 1;
        if(lifetime > halt)
        {
            eq_position = new Vector3(
                (lifetime - halt) * trans_mult.x,
                (lifetime - halt) * trans_mult.y, 
                0) + trans_mult;
        } else
        {

            eq_position = trans_mult;
        }
        
        if (lifetime < 0)
        {
            float t = - lifetime;
            eq_position += 0.1f * new Vector3(Mathf.PerlinNoise(t * 4, 0), 0, 0);
        }

        if (force) { transform.localPosition = eq_position; }
    }

    private void OnDestroy()
    {
        // NoteManager.Instance.beat_change -= PerBeat;

        if (!done && !NoteManager.Instance.invincible)
        {
        } else
        {
        }
    }

    Vector3 trans_mult = Vector3.zero;

}
