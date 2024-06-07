using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinBehavior : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the colliding object is a box
        if (other.gameObject.CompareTag("topline"))
        {
            // Set the animation parameter to true
            animator.SetBool("eat", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Check if the colliding object is a box
        if (other.gameObject.CompareTag("topline"))
        {
            // Set the animation parameter to false
            animator.SetBool("eat", false);
        }
    }
}
