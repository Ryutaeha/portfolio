using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PlayerNearby : MonoBehaviour
{
    public GameObject Monster;
    private Animator animator;
    private GameObject Player;
    private NavMeshAgent navMeshAgent;
    private MonsterRoar monsterRoar;
    private Vector3 InitialPosition;
    private float speed;
    private bool isClosed;
    [HideInInspector] public bool isAttack;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        monsterRoar = GetComponentInChildren<MonsterRoar>();
        speed = navMeshAgent.speed;
        isAttack = false;
    }

    private void Start()
    {
        InitialPosition = transform.position;        
        StartCoroutine(PlayerChase());
    }

    private void Update()
    {
        if (Time.timeScale > 0.01f) //슬로우, 
        {
            navMeshAgent.speed = speed * (1 / Mathf.Pow(Time.timeScale, 2));
            navMeshAgent.angularSpeed = 300 * (1 / Mathf.Pow(Time.timeScale, 2));
            animator.speed = 1 / Mathf.Pow(Time.timeScale, 2);
        }
        else
        {
            animator.speed = 10f;
            navMeshAgent.angularSpeed = 300;
            navMeshAgent.speed = speed * 10f;
        }
        InHome();
    }

    private IEnumerator PlayerChase() //navmesh 경로 설정 
    {
        while (true)
        {
            if (Player != null && !isAttack)
            {
                navMeshAgent.SetDestination(Player.transform.position);

                NavMeshPath path = new NavMeshPath();
                if (navMeshAgent.CalculatePath(Player.transform.position, path) && isClosed)
                {
                    print("트루");

                    if (path.corners.Length >= 1 && path.corners[path.corners.Length - 1] != navMeshAgent.destination) // 갈 수 없는 곳에 플레이어가 있는 경우
                    {
                        print("못감");
                        navMeshAgent.SetDestination(InitialPosition);
                    }
                    else
                    {
                        print("잡으러감");
                        animator.SetBool("See", true);
                    }
                }
                else if(!navMeshAgent.CalculatePath(Player.transform.position, path))
                {
                    navMeshAgent.SetDestination(InitialPosition);
                }

                CheckPlayerDistance();
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player = other.gameObject;
            Player.GetComponent<PlayerEventController>().OnDeath += ComeBackHome;
        }
    }
    private void CheckPlayerDistance()
    {
        if ((Player.transform.position - transform.position).magnitude < 4 && isClosed) //가까워지면 공격
        {
            animator.SetBool("See", false);
            isAttack = true;
            navMeshAgent.ResetPath();
            MonsterIsAttack();
            StartCoroutine(StopAttack());
        }

        if ((Player.transform.position - transform.position).magnitude > 20) //거리가 너무 멀어졌는지 체크
        {
            ComeBackHome();
        }
        else
        {
            isClosed = true;
        }
    }
    private IEnumerator StopAttack() //공격시 딜레이
    {
        yield return new WaitForSecondsRealtime(2.1f / Mathf.Pow(Time.timeScale, 2));
        isAttack = false;
        animator.SetBool("See", true);
    }

    private void InHome() //제자리로 복귀한 경우
    {
        
        if (Mathf.Approximately(transform.position.x, InitialPosition.x)&& Mathf.Approximately(transform.position.z, InitialPosition.z))
        {
            animator.SetBool("See", false);
            isAttack = false;
        }
    }

    private void ComeBackHome()
    {
        isClosed = false;
        navMeshAgent.SetDestination(InitialPosition);
        Player = null;
    }

    private void MonsterIsAttack()
    {
        transform.LookAt(Player.transform.position);
        animator.SetTrigger("Attack");
        monsterRoar.MonsterAttackSound();
    }
}