using GameMgr;
using System;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

//장애물종류
enum ObstacleType
{
    Prickle, Rock, Bees
}

public class StageManager : MonoBehaviour
{
    //플렛폼 프리팹
    [SerializeField] private GameObject[] _onePlatformPrefab = new GameObject[3];
    [SerializeField] private GameObject[] _twoPlatformPrefab = new GameObject[3];
    [SerializeField] private GameObject[] _triPlatformPrefab = new GameObject[3];

    [SerializeField] private GameObject _platformGroup;

    //장애물 프리팹 3개
    [SerializeField] private GameObject[] _Obstacle = new GameObject[3];


    private ObstacleType _obstacleType;

    private Vector2 _pos;
    System.Random rand = new System.Random();
    private float speed = 3;
    //생성된 플렛폼
    private GameObject[] _platform = new GameObject[9];
    private int cycleN = 0;


    private MovePlatform _movePlatform;

    #region 프로퍼티
    public GameObject[] Platform { get { return _platform; } }
    public GameObject PlatformGroup { get { return _platformGroup; } }

    #endregion
    private void Awake()
    {
        //Debug.Log("스테이지플렛폼 Awake");
        CreatePlatform();
        OverlapPlatform();
    }

    private void Start()
    {
        _movePlatform = GameMgr.GameManager.Instance.MovePlatform;
    }

    private void Update()
    {
    }

    public void StopScroll()
    {
        for (int i = 0; i < _platform.Length; i++)
        {
            _platform[i].transform.parent = null;
            Debug.Log("해제됨");
        }
        
        ReArrange();

       
        //GameMgr.GameManager.Instance.Player.transform.parent = null;
    }

    //최초생성
    private void CreatePlatform()
    {
        float hight = -5.5f;
        for (int i = 0; i < 9; i++)
        {
            PlatformPosSet(i, hight, 3);

            if (_platform[i] != null)
            {
                Debug.Log(_platform[i].name);
            }

            hight += 2;
        }

        _platform[0].GetComponent<BoxCollider2D>().enabled = true;
        _platform[1].GetComponent<BoxCollider2D>().enabled = true;
        _platform[4].tag = "Block";
    }

    private void PlatformPosSet(int num, float hight, int prefabN)
    {
        _pos = new Vector2(rand.Next(-3, 4), hight);
        _platform[num] = Instantiate(_onePlatformPrefab[rand.Next(0, prefabN)], _pos, transform.rotation, PlatformGroup.transform);
        _platform[num].name = $"Block-{num}";

        _platform[num].layer = 3;
    }

    private void PlatformPosReSet(int num, float hight)
    {
        //가로 랜덤
        _pos = new Vector2(rand.Next(-3, 4), hight);

        _platform[num].transform.position = _pos;
    }


    public void PlatColActive()
    {
        GameObject _playerCPos = GameMgr.GameManager.Instance.Pos;
        int num = 0;

        for (int i = 0; i < _platform.Length; i++)
        {
            if (_playerCPos == _platform[i])
            {
                num = i;
                break;
            }
        }

        if (num < 8)
        {
            _platform[num + 1].GetComponent<BoxCollider2D>().enabled = true;
            Debug.Log($"{num + 1}-{_platform[num + 1].name} 켰음");
        }
        if(num == 8)
        {
            _platform[0].GetComponent<BoxCollider2D>().enabled = true;
            Debug.Log($"0-{_platform[0].name} 켰음");
        }
    }

    public void ReArrange()
    {
        float hight = 4.5f;

        for (int i = 0; i < _platform.Length; i++)
        {
            if (_platform[i].transform.position.y < -7f)
            {
                PlatformPosReSet(i, hight);
                _platform[i].GetComponent<BoxCollider2D>().enabled = false;
                hight += 2;
            }
        }

        _movePlatform.MoveOriginPos();

        for (int i = 0; i < _platform.Length; i++)
        {
            _platform[i].transform.parent = _platformGroup.transform;
            if(_platform[i].transform.position.y ==1)
            {
                _platform[i].tag = "Block";
                Debug.Log($"{i}-{_platform[i].name}에 태그");
            }
        }
    }

    private NormalPlatform current;
    private NormalPlatform next;
    //발판 중복 체크
    private void OverlapPlatform()
    {
        if (_platform != null)
        {
            int j = 1;

            for (int i = 0; i < _platform.Length - 1; i++)
            {
                if (_platform[i].transform.position.y >= -8 && _platform[i].transform.position.y <= 0)
                {
                    current = _platform[i].GetComponent<NormalPlatform>();
                }

                if (_platform[j].transform.position.y >= -8 && _platform[j].transform.position.y <= 0)
                {
                    next = _platform[j].GetComponent<NormalPlatform>();
                }

                //노말발판일 경우 + 타입 비교
                if (current.Type == PlatformType.Normal && current.Type == next.Type)
                {
                    //기준 발판과 타입이 같으면 포지션 맞추기
                    Vector2 pos = _platform[j].gameObject.transform.position;
                    pos.x = _platform[i].gameObject.transform.position.x;

                    _platform[j].gameObject.transform.position = pos;
                }
                j++;
            }
        }
        else
        {
            // Debug.Log("플렛폼 배열 비었음");
        }
    }
}

