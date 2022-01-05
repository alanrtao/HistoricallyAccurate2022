using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool dead = false;

    SpriteRenderer sr;

    public SpriteRenderer hint, pointer;

    float pointer_radius = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = (transform.position - PlayerController.Instance.transform.position).magnitude;

        if (dead)
        {
            hint.color = new Color(1, 1, 1, 0);
            return;
        }

        float engage_dist = 3;
        float succeed_dist = 1.75f;
        if (dist > engage_dist)
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

        if (Input.GetKeyDown(KeyCode.Space) && !dead)
        {
            // close enough, succeed
            if (dist < succeed_dist)
            {
                dead = true;
                StartCoroutine(DeathAnimation());
            }
            // too far, dodge
            else
            {
                transform.position += (transform.position - PlayerController.Instance.transform.position).normalized * 3f;
            }
        }

        pointer.transform.localPosition = (transform.position - PlayerController.Instance.transform.position).normalized * pointer_radius;
        pointer.transform.right = (transform.position - PlayerController.Instance.transform.position).normalized;

        if (InScreen(Camera.main.WorldToViewportPoint(transform.position)))
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
        return vp.x > 0 && vp.x < 1 && vp.y > 0 && vp.y < 1;
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
