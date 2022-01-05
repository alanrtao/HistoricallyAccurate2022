using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool dead = false;

    SpriteRenderer sr;

    public SpriteRenderer hint;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float dist = (transform.position - PlayerController.Instance.transform.position).magnitude;

        if (dead) return;

        if (dist > 2)
        {
            hint.color = new Color(1, 1, 1, 0);
        } else if (dist > 1.25)
        {
            hint.color = new Color(1, 1, 1, 0.15f);
        } else
        {
            hint.color = new Color(1, 1, 1, 0.8f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !dead)
        {
            // close enough, succeed
            if (dist < 1.25)
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
    }
}
