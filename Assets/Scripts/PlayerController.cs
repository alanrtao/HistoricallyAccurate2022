using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Transform next_step;

    public static PlayerController Instance { get { return _Instance; } }
    private static PlayerController _Instance;

    [Range(0, 1)]
    public float speed;

    private void Awake()
    {
        _Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        NoteManager.Instance.beat_change += HandleMovement;
        next_step = transform.GetChild(0);
        next_step.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.1f);

        knife_delta = knife.transform.localPosition;
    }

    float step_delta = 0.2f;

    public Animator anim, knife;
    public SpriteRenderer main, next;

    Vector3 knife_delta;

    Vector3 leftward = new Vector3(-1, 1, 1), rightward = new Vector3(1, 1, 1);

    // Update is called once per frame
    void Update()
    {
        if (NoteManager.Instance.beat_time_whole >= NoteManager.Instance.engage_beat)
        {
            if (Input.GetKey(KeyCode.W))
            {
                logged_movement.y = 1;

            }
            else if (Input.GetKey(KeyCode.S))
            {
                logged_movement.y = -1;

            }
            if (Input.GetKey(KeyCode.A))
            {
                logged_movement.x = -1;

            }
            else if (Input.GetKey(KeyCode.D))
            {
                logged_movement.x = 1;

            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            // logged_movement.y = 1;

            anim.SetBool("Up", true);
            anim.SetBool("Down", false);
            anim.SetBool("Left", false);
            anim.SetBool("Right", false);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            // logged_movement.y = -1;

            anim.SetBool("Up", false);
            anim.SetBool("Down", true);
            anim.SetBool("Left", false);
            anim.SetBool("Right", false);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            // logged_movement.x = -1;

            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Left", true);
            anim.SetBool("Right", false);

            knife.transform.localScale = leftward;
            knife.transform.localPosition = new Vector3(-knife_delta.x, knife_delta.y, knife_delta.z);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // logged_movement.x = 1;

            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Left", false);
            anim.SetBool("Right", true);

            knife.transform.localScale = rightward;
            knife.transform.localPosition = new Vector3(knife_delta.x, knife_delta.y, knife_delta.z);
        }

        next.sprite = main.sprite;

        next_step_eq = logged_movement * step_delta;
        float smoothness = .5f;
        next_step.localPosition = next_step.localPosition * smoothness + next_step_eq * (1 - smoothness);
    }

    Vector3 next_step_eq;

    Vector3 logged_movement = Vector3.zero;
    private void FixedUpdate()
    {
        
    }

    private void HandleMovement()
    {
        transform.position = transform.position + logged_movement * step_delta;
        logged_movement = Vector3.zero;
    }

    public bool can_knife = true;
    public void PlayKnife()
    {
        can_knife = false;
        knife.gameObject.SetActive(true);
        StartCoroutine(UnKnife());
    }

    IEnumerator UnKnife()
    {
        yield return new WaitForSeconds(1);
        knife.gameObject.SetActive(false);
        can_knife = true;
    }
}
