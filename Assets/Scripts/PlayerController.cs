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
    }

    float step_delta = 0.2f;

    public Animator anim;
    public SpriteRenderer main, next;

    // Update is called once per frame
    void Update()
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
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            // logged_movement.x = 1;

            anim.SetBool("Up", false);
            anim.SetBool("Down", false);
            anim.SetBool("Left", false);
            anim.SetBool("Right", true);
        }

        next.sprite = main.sprite;

        next_step.localPosition = logged_movement * step_delta;
    }

    Vector3 logged_movement = Vector3.zero;
    private void FixedUpdate()
    {
        
    }

    private void HandleMovement()
    {
        transform.position = transform.position + logged_movement * step_delta;
        logged_movement = Vector3.zero;
    }
}
