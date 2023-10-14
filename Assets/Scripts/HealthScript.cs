using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class HealthScript : MonoBehaviour
{
    // для компонентів EnemyAnimator, NavMeshAgent, EnemyController
    private EnemyAnimator enemyAnim;
    private NavMeshAgent navAgent;
    private EnemyController enemyController;

    // хп персонажа
    public float health = 100f;

    // визначення об'єкта
    public bool isPlayer, isBoar, isCannibal;

    // визначення смерті
    private bool isDead;

    // для тегів
    public const string Enemy = "Enemy";
    public const string Player = "Player";

    private EnemyAudio enemyAudio;

    private PlayerStats playerStats;

    void Awake()
    {
        if (isBoar || isCannibal)
        {
            enemyAnim = GetComponent<EnemyAnimator>();
            enemyController = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

        if (isPlayer)
        {
            playerStats = GetComponent<PlayerStats>();
        }
    }

    // завдавати шкоди
    public void ApplyDamage(float damage)
    {
        // якщо ми загинули, не виконуэться решту коду
        if (isDead)
            return;
        // якщо ми живы, то - хп
        health -= damage;

        //для ворога
        if (isBoar || isCannibal)
        {
            // якщо ми вистрілили у ворога і у нього стан патрулювання, 
            // то дистанція погоні збільшується
            // при пострілі у ворога, він починає знатися за нами
            if (enemyController.State == EnemyState.PATROL)
            {
                enemyController.chaseDistance = 50f;
            }
        }

        // якщо закінчилися хп
        if (health <= 0f)
        {
            PlayerDied();
            isDead = true;
        }

        if (isPlayer)
        {
            playerStats.DisplayHealthStats(health);
        }
    }

    void PlayerDied()
    {
        // якщо ігровий персонаж є Cannibal
        if (isCannibal)
        {
            // деактивуємо всі компоненти персонажа
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            // додаємо до поточного об'єкту Rigidbody та одразу додаємо закручування
            gameObject.AddComponent<Rigidbody>().AddTorque(transform.forward * 20f);

            enemyController.enabled = false;
            navAgent.enabled = false;
            enemyAnim.enabled = false;

            StartCoroutine(DeadSound());

            EnemyManager.instance.EnemyDied(true);
        }

        // якщо ігровий персонаж є Boar
        if (isBoar)
        {
            // деактивуємо всі компоненти  персонажа
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemyController.enabled = false;
            // викликаємо анімацію смерті
            enemyAnim.Dead();

            StartCoroutine(DeadSound());

            EnemyManager.instance.EnemyDied(false);
        }
        // якщо ігровий персонаж є Player
        if (isPlayer)
        {
            // знаходимо всіх ворогів на ігровій сцені
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Enemy);
            // зупиняємо їх 
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }
            // деактивуємо
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().
                                                 gameObject.SetActive(false);

            EnemyManager.instance.StopSpawning();
        }
        if (tag == Player)
        {
            Invoke(nameof(RestartGame), 3f);
        }
        else
        {
            Invoke(nameof(TurnOffGameObject), 3f);
        }
    }


    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.PlayDeadSound();
    }
}
