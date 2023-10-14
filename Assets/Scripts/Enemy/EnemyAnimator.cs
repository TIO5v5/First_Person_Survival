using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    // Змінна типу Animator
    private Animator anim;

    public const string WalkParameter = "Walk";
    public const string RunParameter = "Run";
    public const string AttackParameter = "Attack";
    public const string DeadParameter = "Dead";
    void Awake()
    {
        // отримуємо Animator в якості значення для змінної anim
        anim = GetComponent<Animator>();
    }

    // встановлюємо значення для параметра Walk 
    public void Walk(bool walk)
    {
        anim.SetBool(WalkParameter, walk);
    }

    // встановлюємо значення для параметра Run
    public void Run(bool run)
    {
        anim.SetBool(RunParameter, run);
    }

    // встановлюємо значення для параметра Attack
    public void Attack()
    {
        anim.SetTrigger(AttackParameter);
    }

    // встановлюємо значення для параметра Dead
    public void Dead()
    {
        anim.SetTrigger(DeadParameter);
    }


}
