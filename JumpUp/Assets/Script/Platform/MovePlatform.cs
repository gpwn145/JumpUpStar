using UnityEngine;
using GameMgr;
using static UnityEngine.GraphicsBuffer;

public class MovePlatform : MonoBehaviour
{
    private Vector2 _target;
    private Vector2 _originPos;
    private int _speed = 1;
    private StageManager _stageManager;


    public Vector2 OriginPos { get { return _originPos; } set { _originPos= value; } }

    private void Awake()
    {
        transform.position = new Vector2(0,2.5f);
        _originPos = transform.position;   // 원래 위치 저장
    }

    private void Start()
    {
        _stageManager = GameMgr.GameManager.Instance.StageManager;
        Debug.Log(_stageManager);
    }

    private void Update()
    {
        Move();
    }

    public void Move()
    {
        if (GameMgr.GameManager.Instance.IsScrollGo)
        {
            _target = new Vector2(transform.position.x, -5.5f);

            transform.position = Vector2.Lerp(
                transform.position,
                _target,
                _speed * Time.deltaTime);

            if (Distance() <= 0.05f)
            {
                Debug.Log($"목표지점 도달");
                transform.position = _target;
                GameMgr.GameManager.Instance.IsScrollGo = false;
                _stageManager.StopScroll();

                //Vector2 vector2 = GameMgr.GameManager.Instance.Player.GetComponent<Player>().PlayerCurrentPos.transform.position;
                //vector2.y += 0.40f;
                //GameMgr.GameManager.Instance.Player.transform.position = vector2;
            }
        }
    }
    

    private float Distance()
    {
        float _range = transform.position.y - _target.y;
        float ab_range = Mathf.Abs(_range);
        return ab_range;
    }

    public void MoveOriginPos()
    {
        transform.position = _originPos;
    }
}
