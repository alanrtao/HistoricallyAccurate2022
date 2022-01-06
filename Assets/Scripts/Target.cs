using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool dead = false;

    SpriteRenderer sr;

    public SpriteRenderer hint, pointer, glow;
    public Animator anim;

    public Gradient damage;

    float pointer_radius = 1f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    int health = 5;
    // Update is called once per frame
    void Update()
    {
        float dist = (transform.position - PlayerController.Instance.transform.position).magnitude;

        anim.SetBool("Die", dead);
        if (dead)
        {
            hint.color = new Color(1, 1, 1, 0);
            return;
        } else
        {
            sr.color = damage.Evaluate(health / 5f);
        }

        glow.sprite = sr.sprite;

        float engage_dist = 2;
        float succeed_dist = 1f;
        int engage_beat = 200;

        if (dist > engage_dist || NoteManager.Instance.beat_time_whole < engage_beat)
        {
            hint.color = new Color(1, 1, 1, 0);
        }
        else
        {
            if (dist > succeed_dist)
            {
                hint.color = new Color(1, 1, 1, 0.15f);
            }
            else
            {
                hint.color = new Color(1, 1, 1, 0.8f);
            }

            hint.GetComponent<Sizer>().size_delta = 0.1f * (3 * (engage_dist - dist) + 1);
        }

        if (!dead && NoteManager.Instance.beat_time_whole >= engage_beat && Input.GetKeyDown(KeyCode.Space) && PlayerController.Instance.can_knife && dist < engage_dist)
        {
            // close enough, succeed
            if (dist < succeed_dist && health == 0)
            {
                dead = true;
                StartCoroutine(DeathAnimation());
            }
            // too far, dodge
            else
            {
                health--;
                transform.position += (transform.position - PlayerController.Instance.transform.position).normalized * 1f;
            }
            PlayerController.Instance.PlayKnife();
        }

        pointer.transform.localPosition = (transform.position - PlayerController.Instance.transform.position).normalized * pointer_radius;
        pointer.transform.right = (transform.position - PlayerController.Instance.transform.position).normalized;

        if (NoteManager.Instance.beat_time_whole < engage_beat && InScreen(Camera.main.WorldToViewportPoint(transform.position)))
        {
            pointer.color = new Color(1, 1, 1, 0);
        }
        else
        {
            pointer.color = Color.white;
        }
    }

    bool InScreen(Vector3 vp)
    {
        float sc_thresh = 0.35f;
        return vp.x > sc_thresh && vp.x < 1 - sc_thresh && vp.y > sc_thresh && vp.y < 1 - sc_thresh;
    }

    IEnumerator DeathAnimation()
    {
        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime;

            Color c = sr.color;
            c.a = 1 - t;
            sr.color = c;

            yield return null;
        }
        NoteManager.Instance.WinGame();
    }
}
