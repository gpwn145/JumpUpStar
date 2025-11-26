using GameMgr;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    //플레이어 프리팹
    [SerializeField] Transform _rayFoot;
    //이동: 플레이어인풋 사용

    //점프관련
    [SerializeField] float _jumpPower = 2f;
    private bool _isGrounded;
    private Rigidbody2D _rigid;
    private RaycastHit2D _hit;

    //위치
    private Vector2 _currentPos;
    private Vector2 _startPos;
    float _startHight = 0.5f;
    private GameObject _playerCurrentPos;
    private GameObject _playerBeforePos;
    private MovePlatform _movePlatform;

    private Animator animator;

    //점수
    private int _comboN;
    private bool _isRegen;

    //콤보시간
    private float _comboTimelimit = 1.5f;
    private float _currentTime;
    private bool _isKeepCombo = false;


    #region 프로퍼티
    public Vector2 StartPos { get { return _startPos; } set { _startPos = value; } }
    public GameObject PlayerCurrentPos { get { return _playerCurrentPos; } }
    public bool IsRegen { get { return _isRegen; } set { _isRegen = value; } }
    #endregion

    private void Awake()
    {
        Debug.Log("플레이어 Awake");
        _rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _startPos.y += 0.5f;
    }

    private void Start()
    {
        _movePlatform = GameObject.Find("PlatformGroup").GetComponent<MovePlatform>();
        Debug.Log($"1. _isRegen = {_isRegen}");
    }

    private void Update()
    {
       if(_isKeepCombo)
        {
            _currentTime  -= Time.deltaTime;
            
            if(_currentTime <= 0)
            {
                _isRegen = true;
            }
        }
    }

    private void FixedUpdate()
    {
        _hit = Physics2D.Raycast(_rayFoot.position, Vector2.down, 0.1f, LayerMask.GetMask("Ground"));

        if (_hit.collider == null)
        {
            Debug.DrawRay(_rayFoot.position, Vector2.down * 0.1f, new Color(1, 0, 0));

            _isGrounded = false;
            if (transform.parent != null && GameMgr.GameManager.Instance.IsScrollGo == false)
            {
                transform.parent = null;
            }
            if(_playerCurrentPos != null && _playerCurrentPos.GetComponent<BoxCollider2D>())
            {
                _playerCurrentPos.GetComponent<BoxCollider2D>().enabled = false;
                Debug.Log($"{_playerCurrentPos.name} 껐음");
            }
        }
    }

    public Vector2 CurrentPos()
    {
        _currentPos = _startPos;
        _currentPos.y += _startHight;

        return _currentPos;
    }

    //접촉할 때
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isTopCollision = collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f;
        if (collision.collider.gameObject.layer == 3 && isTopCollision)
        {
            _isGrounded = true;

            //새 부모지정
            transform.parent = collision.collider.gameObject.transform;
            //현재위치 플랫폼 정보
            _playerCurrentPos = collision.collider.gameObject;

            //다음발판으로 넘어가면 _isRegen=false
            if(_playerBeforePos!=null && _playerBeforePos != _playerCurrentPos)
            {
                Debug.Log($"{_playerBeforePos} > {_playerBeforePos} : 콤보 성공");
                _isRegen = false;
                Debug.Log($"2. _isRegen = {_isRegen}");
            }

            //다음발판으로 넘어가면 _isRegen=true
            _playerBeforePos = _playerCurrentPos;
            GameMgr.GameManager.Instance.CurrentPlayerPosInfo(_playerCurrentPos);

            _startPos = collision.collider.gameObject.transform.position;
            _startPos.y += _startHight;
            GameMgr.GameManager.Instance.StageManager.PlatColActive();

            //리스폰 상태 X 콤보 증가
            if (_comboN < 4 && _isRegen == false)
            {
                _comboN++; 
                Debug.Log($"콤보증가 : {_comboN}");
                Debug.Log($"3. _isRegen = {_isRegen}");
                _currentTime = _comboTimelimit;
                _isKeepCombo = true;
            }
            //리스폰 상태면 초기화
            else if (_isRegen == true)
            {
                _comboN = 0;
                Debug.Log($"콤보초기화 : {_comboN}");
                Debug.Log($"4. _isRegen = {_isRegen}");

                _currentTime = 0;
                _isKeepCombo = true;
            }

            for (int i = 0; i < GameMgr.GameManager.Instance.StageManager.Platform.Length; i++)
            {
                if (GameMgr.GameManager.Instance.StageManager.Platform[i] == collision.collider.gameObject)
                {
                    Debug.Log($"밟은 발판 index = {i},  이름 = {GameMgr.GameManager.Instance.StageManager.Platform[i].name}");
                }
            }

            if (collision.collider.gameObject.tag == "Block" && GameMgr.GameManager.Instance.IsScrollGo == false)
        {
            Debug.Log("1. 마지막 발판에 닿음");
            _playerCurrentPos = collision.collider.gameObject;
            GameMgr.GameManager.Instance.IsScroll(true);
            _playerCurrentPos.tag = "Untagged";
        }
        }

        else if (collision.collider.gameObject.layer == 6)
        {
            GameMgr.GameManager.Instance.DecreaseHP();
            _playerCurrentPos.GetComponent<BoxCollider2D>().enabled = true;
            _comboN = 0;
        }

        if ( isTopCollision && collision.collider.gameObject.layer == 3 || collision.collider.gameObject.layer == 6 )
        {
            GameMgr.GameManager.Instance.ComboCal(_comboN);
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
