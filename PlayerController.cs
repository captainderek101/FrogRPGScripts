using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 1f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveVelocity;
    private DialogueManager manager;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        manager = FindObjectOfType<DialogueManager>();
        manager.battleBegin += SavePosition;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        animator.SetBool("Walking", moveVelocity != Vector2.zero);
        rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void SavePosition()
    {
        PlayerPrefs.SetFloat("xPos", transform.position.x);
        PlayerPrefs.SetFloat("yPos", transform.position.y);
    }

    private void OnLevelWasLoaded(int level)
    {
        transform.position = new Vector3(PlayerPrefs.GetFloat("xPos"), PlayerPrefs.GetFloat("yPos"));
    }
}
