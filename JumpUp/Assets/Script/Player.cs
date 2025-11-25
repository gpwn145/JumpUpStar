using GameMgr;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //플레이어 프리팹
    [SerializeField] Transform _rayFoot;
    //이동: 플레이어인풋 사용

    //점프관련
    [SerializeField] float _jumpPower = 2f;
    private bool _isGrounded;
    private Rigidbody2D _rigid;
    RaycastHit2D _hit;

    //위치
    private Vector2 _currentPos;
    private Vector2 _startPos;
    float _startHight = 1f;
    private GameObject _playerCurrentPos;

    private Animator animator;

    #region 프로퍼티
    public Vector2 StartPos { get { return _startPos; } set { _startPos = value; } }
    public GameObject PlayerCurrentPos { get { return _playerCurrentPos; }  }
    #endregion

    private void Awake()
    {
        Debug.Log("플레이어 Awake");
        _rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _startPos.y += _startHight;
    }

    //private void FixedUpdate()
    //{
    //    _hit = Physics2D.Raycast(_rayFoot.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground", "Death"));

    //    if (_hit.collider != null)
    //    {
    //        if (_hit.collider.gameObject.layer == 3)
    //        {
    //            Debug.DrawRay(_rayFoot.position, Vector2.down * 0.2f, new Color(1, 0, 0));

    //            //platformPos = _hit.collider.gameObject;
    //        }
    //    }
    //}

    public Vector2 CurrentPos()
    {
        _currentPos = _startPos;
        _currentPos.y += _startHight;

        return _currentPos;
    }

    //접촉할 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            _isGrounded = true;

            //새 부모지정
            transform.parent = collision.collider.gameObject.transform;
            //현재위치 플랫폼 정보
            _playerCurrentPos = collision.collider.gameObject;
            GameMgr.GameManager.Instance.CurrentPlayerPosInfo(_playerCurrentPos);

            _startPos = collision.collider.gameObject.transform.position;
            _startPos.y += _startHight;
        }

        else if(collision.collider.gameObject.layer == 6)
        {
            GameMgr.GameManager.Instance.DecreaseHP();
            _playerCurrentPos.GetComponent<BoxCollider2D>().enabled = true;
        }

        if(collision.collider.gameObject.tag == "Block")
        {
            Debug.Log("1. 마지막 발판에 닿음");
            GameMgr.GameManager.Instance.IsScroll(true);
        }
    }

    //머무를때
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            _isGrounded = true;
        }
    }

    //벗어날때
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            _isGrounded = false;
            if(transform.parent != null)
            {
                transform.parent = null;
            }
            GameMgr.GameManager.Instance.StageManager.PlatColActive();
        }
    }

    //점프 시스템
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (_playerCurrentPos.tag != "Block")
        {
            if (_isGrounded && GameMgr.GameManager.Instance.GameState == GameState.Playing)
            {
                _rigid.linearVelocity = new Vector2(_rigid.linearVelocity.x, 0);
                _rigid.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
            }
        }
    }
}
