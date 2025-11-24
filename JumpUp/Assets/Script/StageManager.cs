using GameMgr;
using System;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

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

    //장애물 프리팹 3개
    [SerializeField] private GameObject[] _Obstacle = new GameObject[3];

    //생성된 프리팹
    private GameObject[] _platform = new GameObject[5];

    private ObstacleType _obstacleType;

    private Vector2 _pos;
    System.Random rand = new System.Random();
    private float speed = 3;

    private GameObject platforms;

    #region 프로퍼티
    public GameObject[] Platform { get { return _platform; } }
    public GameObject Platforms { get { return platforms; } }

    #endregion
    private void Awake()
    {
        CreatePlatform();
        OverlapPlatform();
    }

    private void Start()
    {
        platforms = GameObject.Find("Platforms");
    }

    private void Update()
    {
    }
    //최초생성
    private void CreatePlatform()
    {
        //빈 오브젝트 생성 : 플렛폼을 다 넣어서 관리할 것
        platforms = new GameObject("Platforms");
        platforms.transform.position = new Vector3(0, 2.5f);

        float hight = -5.5f;

        PlatformPosSet(0, hight, 1);
        hight += 2;


        for (int i = 1; i < 5; i++)
        {
            PlatformPosSet(i, hight, 3);
            _platform[0].GetComponent<NormalPlatform>().IsStep = false;
            hight += 2;
        }
    }

    private void PlatformPosSet(int num, float hight, int prefabN)
    {
        _pos = new Vector2(rand.Next(-3, 4), hight);
        _platform[num] = Instantiate(_onePlatformPrefab[rand.Next(0, prefabN)], _pos, transform.rotation, platforms.transform);
        _platform[num].name = $"Block-{num}";

        _platform[num].tag = $"Block{num}";
        _platform[num].layer = 3;


        _platform[num].AddComponent<BoxCollider2D>().usedByEffector = true;
        _platform[num].GetComponent<BoxCollider2D>().offset = new(0, -0.1f);
        _platform[num].GetComponent<BoxCollider2D>().size = new Vector2(0.7f, 0.1f);
        _platform[num].AddComponent<PlatformEffector2D>();
    }

    private void PlatformPosReSet(int num, float hight)
    {
        //가로 랜덤
        _pos = new Vector2(rand.Next(-3, 4), hight);

        _platform[num].transform.position = _pos;
        _platform[num].name = $"Block-{num}";
        _platform[num].tag = $"Block{num}";
    }

    //스테이지별 위치 랜덤 생성

    //스테이지 전환 (5번발판 > 0번발판)
    public void PlatformScroll()
    {
        //y좌표 기억 후, 아래로 스크롤
        float scroll = speed * Time.deltaTime;
        platforms.transform.Translate(Vector2.down * scroll);

        //4번 블럭 > 0번블럭교체
        Vector2 boxPos = new Vector2(_platform[4].transform.position.x, _platform[0].transform.position.y);
        _platform[0].transform.position = boxPos;

        //안보이는 블럭 비활성화 (1~4)
        for (int i = 1; i < 5; i++)
        {
            _platform[i].SetActive(false);
        }

        float hight = -3.5f;

        //1,2,3,4 위치 재배치 후 활성화
        for (int i = 1; i < 5; i++)
        {
            PlatformPosReSet(i, hight);
            hight += 2;
        }
    }

    //발판 중복 체크
    private void OverlapPlatform()
    {
        int j = 1;
        for (int i = 0; i < 4; i++)
        {
            NormalPlatform current = _platform[i].GetComponent<NormalPlatform>();
            NormalPlatform next = _platform[j].GetComponent<NormalPlatform>();

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
}

