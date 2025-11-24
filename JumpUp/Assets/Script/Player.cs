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
    private GameObject _platformPos;
    private Vector2 _stageInitialPos;
    private bool isScroll = false;

    //콤보성공여부
    private int _isSuc;

    private Animator animator;

    #region 프로퍼티
    public int IsSuc { get { return _isSuc; } set { _isSuc = value; } }
    public GameObject PlatformPos { get { return _platformPos; } }
    #endregion

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _platformPos = GameMgr.GameManager.Instance.StageManager.Platform[0];
        transform.parent = _platformPos.transform;

        _stageInitialPos.y = _platformPos.transform.position.y + 0.5f;
        Debug.Log(transform.position);
    }

    private void FixedUpdate()
    {
        _hit = Physics2D.Raycast(_rayFoot.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground", "Death"));

        if (_hit.collider != null)
        {
            if (_hit.collider.gameObject.layer == 3)
            {
                Debug.DrawRay(_rayFoot.position, Vector2.down * 0.2f, new Color(1, 0, 0));

                //platformPos = _hit.collider.gameObject;
            }
        }
        //if(IsSetPos)
        //{
        //    PlayerPosSet(platformPos);
        //    IsSetPos = false;
        //}
        if (isScroll)
        {
            transform.parent = null;
            GameMgr.GameManager.Instance.StageManager.PlatformScroll();
            transform.parent = GameMgr.GameManager.Instance.StageManager.Platform[0].transform;
            if (GameMgr.GameManager.Instance.StageManager.Platforms.transform.position.y >= -5.5f)
            {
                isScroll =false;
            }
        }
    }

    //접촉할 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == 3)
        {
            //새로 밟은 땅 체크
            GameMgr.GameManager.Instance.IsStep(collision.collider.gameObject);
            _isGrounded = true;
            //IsSetPos = true;

            //새 부모지정
            transform.parent = collision.collider.gameObject.transform;

            //가장 위 발판인지 확인
            if (collision.collider.gameObject == GameMgr.GameManager.Instance.StageManager.Platform[4])
            {
                isScroll = true;
            }
        }

        else if(collision.collider.gameObject.layer == 6)
        {
            GameMgr.GameManager.Instance.DecreaseHP();
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
            transform.parent = null;
        }
    }

    //점프 시스템
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (_isGrounded && GameMgr.GameManager.Instance.GameState == GameState.Playing)
        {
            _rigid.linearVelocity = new Vector2(_rigid.linearVelocity.x, 0);
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
        }
    }

    //private void PlayerPosSet(GameObject obj)
    //{
    //    Vector2 _stageInitialPos;

    //    //부모지정
    //    transform.parent = obj.transform;
    //    _stageInitialPos.y = obj.transform.position.y + 0.5f;

    //    //위치 지정
    //    // _stageInitialPos = transform.position;
    //    //transform.localPosition = new Vector2(0f, 0.6f);

    //    //if (_stageInitialPos.x < obj.transform.position.x + 0.01f || _stageInitialPos.x > obj.transform.position.x - 0.01f)
    //    //{
    //    //    _stageInitialPos.x = obj.transform.position.x;
    //    //}

    //    //if (_stageInitialPos.y < obj.transform.position.y + 0.01f || _stageInitialPos.y > obj.transform.position.y - 0.01f)
    //    //{
    //    //    _stageInitialPos.y = obj.transform.position.y + 0.5f;
    //    //}

    //    //위치 적용
    //    //transform.position = _stageInitialPos;
    //}
}
