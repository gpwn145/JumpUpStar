using GameMgr;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //플레이어 프리팹
    [SerializeField] Transform _rayFoot;
    //이동: 플레이어인풋 사용
    private PlayerInput _action;

    //점프관련
    [SerializeField] float _jumpPower = 4.2f;
    private bool _isGrounded;
    private Rigidbody2D _rigid;

    

    //콤보성공여부
    private bool _isSuc;

    #region 프로퍼티
    public bool IsSuc {  get { return _isSuc; } }
    #endregion

    private void Awake()
    {
        _action = GetComponent<PlayerInput>();
        _rigid = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D _hit = Physics2D.Raycast(_rayFoot.position, Vector2.down, 0.2f, LayerMask.GetMask("Ground", "Death"));
        
        if(_hit.collider != null)
        {
            if(_hit.collider.gameObject.layer == 3)
            {
                Debug.Log("땅 레이어");
                Debug.DrawRay(_rayFoot.position, Vector2.down * 0.2f, new Color(1, 0, 0));

                _isSuc = true;
            }
            //땅 못밟으면 콤보 초기화
            else if (_hit.collider.gameObject.layer == 6)
            {
                _isSuc = false;
            }

            GameMgr.GameManager.Instance.ComboMgr();
        }
    }

    //점프 시스템
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (_rigid.IsSleeping())
        {
            _rigid.AddForce(Vector3.up * _jumpPower, ForceMode2D.Impulse);
        }
    }

    
}
