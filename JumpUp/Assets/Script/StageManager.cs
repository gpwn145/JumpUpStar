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
    private GameObject[] _platform = new GameObject[5];

    private bool _isScrolling = false;

    private MovePlatform _movePlatform;

    #region 프로퍼티
    public GameObject[] Platform { get { return _platform; } }
    public GameObject PlatformGroup { get { return _platformGroup; } }
    public bool IsScrolling { get { return _isScrolling; } set { _isScrolling = value; } }

    #endregion
    private void Awake()
    {
        //Debug.Log("스테이지플렛폼 Awake");
        CreatePlatform();
        OverlapPlatform();
        _movePlatform = GameMgr.GameManager.Instance.MovePlatform;
    }

    //private void Update()
    //{
    //    if (_isScrolling == false)
    //    {
    //        _isScrolling = true;
    //        PlatformScroll(GameMgr.GameManager.Instance.IsScrollGo);
    //        GameMgr.GameManager.Instance.IsScroll(false);
    //    }
    //}

    public void StopScroll()
    {
        GameMgr.GameManager.Instance.Player.transform.parent = null;
        //ReArrange();
        _isScrolling = false;

        _movePlatform.transform.position = _movePlatform.OriginPos;
    }

    //최초생성
    private void CreatePlatform()
    {
        float hight = -5.5f;

        PlatformPosSet(0, hight, 1);
        if (_platform[0] != null)
        {
            Debug.Log(_platform[0].name);
        }
        hight += 2;


        for (int i = 1; i < 5; i++)
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
        _platform[4].tag = $"Block";

    }

    private void PlatformPosSet(int num, float hight, int prefabN)
    {
        _pos = new Vector2(rand.Next(-3, 4), hight);
        _platform[num] = Instantiate(_onePlatformPrefab[rand.Next(0, prefabN)], _pos, transform.rotation, _platformGroup.transform);
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

        for(int i = 0; i < _platform.Length; i++)
        {
            if (_playerCPos == _platform[i])
            {
                num = i;
                break;
            }
        }
        if(num > 0)
        {
            _platform[num-1].GetComponent<BoxCollider2D>().enabled = false;
        }
        if (num < 4)
        {
            _platform[num+1].GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    //스테이지 전환 (5번발판 > 0번발판)
    public void PlatformScroll(bool istrue)
    {
        //Debug.Log("3. 스크롤매서드 활성화");
        //y좌표 기억 후, 아래로 스크롤
        _platformGroup.GetComponent<MovePlatform>().IsGO(istrue);
        _platformGroup.GetComponent<MovePlatform>().TargetPos(_platform[0].transform.position);
    }

    public void ReArrange()
    {
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
        if (_platform != null)
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
        else
        {
           // Debug.Log("플렛폼 배열 비었음");
        }
    }
}

