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
        // 检查碰撞的对象是否是盒子
        if (other.gameObject.CompareTag("topline"))
        {
            // 设置动画参数为true
            animator.SetBool("eat", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // 检查碰撞的对象是否是盒子
        if (other.gameObject.CompareTag("topline"))
        {
            // 设置动画参数为false
            animator.SetBool("eat", false);
        }
    }
}
