using System;
using UnityEngine;

public class MoveLv1Platform : NormalPlatform
{
    protected bool isRight = false;
    protected System.Random RL = new System.Random();


    protected new void Awake()
    {
        _platformID = PlatformType.moveLevel1;
        //leftGoal.x = ;
        //rightGoal.x =;
        speed = 1;
    }
    protected new void OnEnable()
    {
        isStep = true;

        if (RL.Next(0, 2) == 0)
        { isRight = true; }
    }

    protected new void Update()
    {
        //현재 위치가 목표보다 작으면 = 왼쪽 끝을 넘어가면
        if (transform.position.x <= -3f)
        {
            isRight = true;
            Debug.Log($"왼쪽 끝, 방향전환");
        }

        //현재 위치가 목표보다 크면 = 오른쪽 끝을 넘어가면
        else if (transform.position.x >= 3f)
        {
            isRight = false;
            Debug.Log($"오른쪽 끝, 방향전환");
        }

        if (isRight)
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
            Debug.Log($"오른쪽으로 이동");
        }
        else
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
            Debug.Log($"왼쪽으로 이동");
        }


    }
}
