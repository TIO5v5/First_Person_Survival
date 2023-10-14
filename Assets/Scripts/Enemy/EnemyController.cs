using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// біблеотека для Nav Mesh
using UnityEngine.AI;


// режим поведінки
public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    // Для зміни анімації
    private EnemyAnimator enemyAnim;
    // Для налаштування переміщення
    private NavMeshAgent navAgent;
    // для визначення стану ворога
    private EnemyState enemyState;

    public EnemyState State
    {
        get => enemyState;
    }

    //швидкість ворога
    public float walkSpeed = 0.5f;
    public float runSpeed = 4f;

    // радіус в межах якого ворог прислідує головного героя
    public float chaseDistance = 7f;
    // поточна дистанція до головного героя 
    private float currentChaseDistance;
    // радіус в межах якого ворог атакує головного героя
    public float attackDistance = 1.8f;
    // відстань між ворогом та головним героєм, що починає тікати  
    public float chaseAfterAttackDistance = 2f;

    // мінімальний та максимальний радіус 
    public float patrolRadiusMin = 20f, patrolRadiusMax = 60f;
    // періодичність зміни позиції гравця
    public float patrolForThisTime = 15f;
    // таймер часу
    private float patrolTimer;

    // час для атаки 
    public float waitBeforeAttack = 2f;
    private float attackTimer;

    // для отримання тега об'єкта
    private Transform target;
    // тег об'єкта Player
    string Player = "Player";

    //Ігровий об'єкт який буде вмикатися від час атаки, та в його радіусі будемо шукати ігрока, для нанесення урону
    public GameObject attackPoint;

    private EnemyAudio enemyAudio;

    void Awake()
    {
        //отримуємо компонент EnemyAnimator
        enemyAnim = GetComponent<EnemyAnimator>();

        //отримуємо компонент NavMeshAgent
        navAgent = GetComponent<NavMeshAgent>();

        //отримуємо компонент Transform головного героя
        target = GameObject.FindWithTag(Player).transform;

        enemyAudio = GetComponentInChildren<EnemyAudio>();
    }

    // Використовується для ініціалізації
    void Start()
    {
        // Перший стан ворога - патрулювання ігрової карти
        enemyState = EnemyState.PATROL;
        patrolTimer = patrolForThisTime;

        // коли ворог вперше потрапляє на гравця
        // атакувати відразу
        attackTimer = waitBeforeAttack;

        // запам’ятати значення відстані переслідування
        // щоб ми могли повернути його назад
        currentChaseDistance = chaseDistance;
    }

    // Оновлення викликається один раз на кадр
    void Update()
    {
        // якщо ворог має стан патрулювання, то викликаємо метод патрулювання
        if (enemyState == EnemyState.PATROL)
            Patrol();
        // якщо ворог має стан переслідування, то викликаємо метод переслідування
        if (enemyState == EnemyState.CHASE)
            Chase();
        // якщо ворог має стан атаки, то викликаємо метод атакування
        if (enemyState == EnemyState.ATTACK)
            Attack();
    }

    void Patrol()
    {
        // кажимо nav agent, що він може рухатися
        navAgent.isStopped = false;
        // задаємо швидкість руху
        navAgent.speed = walkSpeed;
        // додати до таймера патрулювання відлік часу час
        patrolTimer += Time.deltaTime;
        // Якщо час зміни прийшов, то змінюємо позицію
        if (patrolTimer > patrolForThisTime)
        {
            // викликаємо метод переміщення "Cannibal"
            SetNewRandomDestination();
            // обнуляємо таймер
            patrolTimer = 0f;
        }
        // анімація "Cannibal", якщо він рухається
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnim.Walk(true);
        }
        else // анімація "Cannibal", якщо він не рухається
        {
            enemyAnim.Walk(false);
        }
        // перевірити відстань між гравцем і ворогом
        if (Vector3.Distance(transform.position, target.position) <= chaseDistance)
        {
            enemyAnim.Walk(false);
            enemyState = EnemyState.CHASE;

            enemyAudio.PlayScreamSound();
        }
    }

    void Chase()
    {
        // дозволити агенту рухатися знову
        navAgent.isStopped = false;
        // задаємо швидкість руху
        navAgent.speed = runSpeed;

        // точка в яку рухається “Cannibal” буде головний герой
        navAgent.SetDestination(target.position);

        // анімація "Cannibal", якщо він рухається
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnim.Run(true);
        }
        else
        {
            enemyAnim.Run(false);
        }

        // Якщо відстань між ворогом та гравцем менше дистанції атаки
        if (Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            // зупинити поточну анімацію
            enemyAnim.Run(false);
            enemyAnim.Walk(false);

            // перейти в стан атаки
            enemyState = EnemyState.ATTACK;

            // скинути відстань погоні до попередньої
            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
        // гравець втікає від ворога
        else if (Vector3.Distance(transform.position, target.position) > chaseDistance)
        {
            // перестаємо бігти
            enemyAnim.Run(false);

            /// перейти в стан патрулювання
            enemyState = EnemyState.PATROL;

            // скидаємо дистанцію для переміщення
            patrolTimer = patrolForThisTime;

            // скинути відстань погоні до попередньої
            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
    }

    void Attack()
    {
        // задаємо нульовий вектор для агента
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        // таймер атаки гравця
        attackTimer += Time.deltaTime;

        // якщо настав час атакувати 
        if (attackTimer > waitBeforeAttack)
        {
            // викликаємо анімацію атаки
            enemyAnim.Attack();
            // обнуляємо таймер
            attackTimer = 0f;

            enemyAudio.PlayAttackSound();
        }

        // Player тікає
        if (Vector3.Distance(transform.position, target.position) >
        attackDistance + chaseAfterAttackDistance)
        {
            enemyState = EnemyState.CHASE;
        }

        RotateTowards(target);
    }

    void RotateTowards(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7);
    }

    void SetNewRandomDestination()
    {
        // випадковий радіус(randRadius) із заданого діапазону
        float randRadius = Random.Range(patrolRadiusMin, patrolRadiusMax);
        // випадкова точка із сфери радіусом randRadius
        Vector3 randDir = Random.insideUnitSphere * randRadius;

        randDir += transform.position;
        NavMeshHit navHit;
        //переміщення "Cannibal" в нову точку 
        NavMesh.SamplePosition(randDir, out navHit, randRadius, -1);
        // новий пункт призначення
        navAgent.SetDestination(navHit.position);
    }

    void TurnOnAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }
}
