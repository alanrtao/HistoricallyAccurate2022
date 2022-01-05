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

    // RectTransform r;

    public bool left;

    public float time { get { return lifetime; } }
    float lifetime = 4;

    SpriteRenderer img;

    // Start is called before the first frame update
    void Start()
    {
        // r = transform as RectTransform;
        NoteManager.Instance.beat_change += PerBeat;

        UpdatePosition();
    }

    public Gradient appearance;
    public Gradient damage;

    // Update is called once per frame
    void Update()
    {
        if (lifetime < -0.5) Destroy(gameObject);

        if (Input.GetKeyDown(key) && lifetime >= 0 && lifetime <= 1)
        {
            if (lifetime > .5f)
            {
                // failure
            } else
            {
                // success
            }
            Destroy(gameObject);
        }

        if (lifetime < 0)
        {
            img.color = damage.Evaluate(-2 * lifetime);
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
        transform.localPosition = new Vector3(lifetime * trans_mult.x, lifetime * trans_mult.y, 0);
    }

    private void OnDestroy()
    {
        NoteManager.Instance.beat_change -= PerBeat;
    }

    Vector2 trans_mult = Vector2.zero;

}
