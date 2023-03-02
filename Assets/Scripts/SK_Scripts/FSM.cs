using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM : MonoBehaviour
{
    bool isContact;
    int startHP = 1;
    int currentHP;
    enum EnemyState
    {
        Idle,
        Move,
        Damage,
        Die
    }

    EnemyState state = EnemyState.Idle;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(state);
        currentHP = startHP;
        StartCoroutine(state.ToString());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Idle()
    {
        //Idle상태일 경우 2초 후 Move 상태(Idle -> Move)
        yield return new WaitForSeconds(2f);
        state = EnemyState.Move;
        //노트와 충돌 시 Idle -> Damage로 변경
        if(isContact == true)
        {
            currentHP--;
            if (currentHP > 0)
            {
                state = EnemyState.Damage;
            }
            else
            {
                //*만약 충돌 시 HP가 0이라면 Move -> Die로 변경
                state = EnemyState.Die;
            }
            isContact = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Note")
        {
            isContact = true;
        }
    }

    private IEnumerator Move()
    {
        //float h = 
        //포인트 지점 도착 시 Idle로 변경(Move -> Idle)

        //노트와 충돌 시 Move -> Damage로 변경
        if (isContact == true)
        {
            currentHP--;
            if (currentHP > 0)
            {
                state = EnemyState.Damage;
            }
            else
            {
                //*만약 충돌 시 HP가 0이라면 Move -> Die로 변경
                state = EnemyState.Die;
            }
            isContact = false;
        }
        yield return null;
    }

    /*private IEnumerator Damage()
    {
        //damage를 받을 경우 현재 currentHP에서 받은 damage만큼 마이너스 / 보류

        //Damage 상태가 된 후 0.5초 후 Idle 상태로 변경(Damage -> Idle)
        yield return new WaitForSeconds(0.5f);
        state = EnemyState.Idle;
    }*/

    private IEnumerable Die()
    {
        //Die상태일 경우 오브젝트가 분리
        gameObject.SetActive(false);

        //분리된 오브젝트는 2초 후 SetActive(false);
        yield return null;
    }
}

