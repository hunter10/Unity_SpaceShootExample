using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterCtrl : MonoBehaviour {

    public enum MonsterState {idle, trace, attack, die};
    public MonsterState monsterState = MonsterState.idle;

    private Transform monsterTr;
    private Transform playerTr;
    private NavMeshAgent nvAgent;
    private Animator animator;

    public float traceDist = 10.0f;
    public float attackDist = 2.0f;

    private bool isDie = false;

    public GameObject bloodEffect;
    public GameObject booodDecal;
    
	// Use this for initialization
	void Start () {
        monsterTr = this.gameObject.GetComponent<Transform>();
        playerTr = GameObject.FindWithTag("Player").GetComponent<Transform>();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        animator = this.gameObject.GetComponent<Animator>();
        // nvAgent.destination = playerTr.position;

        StartCoroutine(this.CheckMonsterState());

        StartCoroutine(this.MonsterAction());
    }
	
	IEnumerator CheckMonsterState()
    {
        while(!isDie)
        {
            yield return new WaitForSeconds(0.2f);

            // 몬스터와 플레이어 사이의 거리측정
            float dist = Vector3.Distance(playerTr.position, monsterTr.position);

            if(dist <= attackDist) // 공격거리 범위 이내로 들어왔는지 확인
            {
                monsterState = MonsterState.attack;
            }
            else if(dist <= traceDist) // 추적거리 범위 이내로 들어왔는지 확인
            {
                monsterState = MonsterState.trace;
            }
            else
            {
                monsterState = MonsterState.idle; 
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while(!isDie)
        {
            switch (monsterState)
            {
                case MonsterState.idle:
                    nvAgent.Stop();
                    animator.SetBool("IsTrace", false);
                    break;

                case MonsterState.trace:
                    nvAgent.destination = playerTr.position;
                    nvAgent.Resume();
                    animator.SetBool("IsTrace", true);
                    animator.SetBool("IsAttack", false);
                    break;

                case MonsterState.attack:
                    nvAgent.Stop();
                    animator.SetBool("IsAttack", true);
                    break;

            }

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "BULLET")
        {
            CreateBloodEffect(coll.transform.position);
            Destroy(coll.gameObject);
            animator.SetTrigger("IsHit");
        }
    }

    void CreateBloodEffect(Vector3 pos)
    {
        GameObject blood1 = (GameObject)Instantiate(bloodEffect, pos, Quaternion.identity);
        Destroy(blood1, 2.0f);
    }
}
