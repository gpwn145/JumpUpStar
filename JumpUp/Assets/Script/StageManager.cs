using GameMgr;
using System;
using UnityEngine;

//플렛폼종류
enum PlatformType
{
    Normal, moveLevel1, moveLevel2
}

//장애물종류
enum ObstacleType
{
    Prickle, Rock, Bees
}

public class StageManager : MonoBehaviour
{
    //플렛폼 프리팹
    [SerializeField] private GameObject _platformPrefab;
    //장애물 프리팹 3개
    [SerializeField] private GameObject[] _Obstacle = new GameObject[3];
    //목숨조각 프리팹
    [SerializeField] private GameObject _particleHpPrefab;
    

    private GameObject[] _platform = new GameObject[5];
    private ObstacleType _obstacleType;
    private Vector2 _pos;
    System.Random rand = new System.Random();

    #region 프로퍼티
    #endregion
    private void Awake()
    {
        CreatePlatform();
    }

    //최초생성
    private void CreatePlatform()
    {
        //빈 오브젝트 생성 : 플렛폼을 다 넣어서 관리할 것
        GameObject platforms = new GameObject("Platforms");
        
        float hight = -6.5f;

        for (int i = 0; i < 5; i++)
        {
            _pos = new Vector2(rand.Next(-3, 4), hight);
            _platform[i] = Instantiate(_platformPrefab, _pos, transform.rotation, platforms.transform);
            _platform[i].name = $"Block-{i + 1}";

            hight += 3;
        }
    }

    //스테이지별 위치 랜덤 생성

    //스테이지 구성
    public void PlatformScroll()
    {
        _platform[0].transform.position = _platform[4].transform.position;
        float hight = -3.5f;

        for (int i = 1; i < 4; i++)
        {
            _pos = new Vector2(rand.Next(-3, 4), hight);
            _platform[i].transform.position = _pos;
            hight += 3;
        }
    }
}
