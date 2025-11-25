using UnityEngine;
using GameMgr;
using static UnityEngine.GraphicsBuffer;

public class MovePlatform : MonoBehaviour
{
    private bool _isGO;
    private Vector2 _target;
    private Vector2 _originPos;
    private int _speed = 1;
    private StageManager scroll;


    public Vector2 OriginPos { get { return _originPos; } set { _originPos= value; } }

    private void Awake()
    {
        scroll = GameMgr.GameManager.Instance.StageManager;
    }

    private void Update()
    {
        if (_isGO)
        {
            Debug.Log("4. 이동함");
            transform.position = Vector2.Lerp(
                transform.position,
                _target,
                _speed * Time.deltaTime);

            if (Distance() <= 0.1f)
            {
                _isGO = false;
                scroll.StopScroll();
            }
        }
    }

    public void IsGO(bool go)
    {
        _isGO = go;
    }

    public void TargetPos(Vector2 target)
    {
        _originPos = transform.position;   // 원래 위치 저장
        _target = new Vector2(transform.position.x, target.y);
    }
    //public void TargetPos(Vector2 target)
    //{
    //    transform.position = new Vector2(0, 2.5f);
    //    _target.y = target.y;
    //}

    private float Distance()
    {
        float _range = transform.position.y - _target.y;
        float ab_range = Mathf.Abs(_range);
        return ab_range;
    }
}
