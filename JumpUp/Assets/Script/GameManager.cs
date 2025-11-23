using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameMgr
{
    public enum GameState
    {
        MainPage, Playing, StageChange, GameOver
    }
    enum Combo
    {
        Base, Combo, Double, Triple
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private StageManager _stageManager;
        [SerializeField] GameObject _playerPrefab;
        //초기화 위치
        private Vector2 _stageInitialPos;

        public static GameManager Instance;
        public GameState _gameState;

        //콤보상태
        private Combo _combo;
        private int _comboN = 1;

        //점수
        private int _score;

        //목숨
        private int _life = 5;
        private bool _hpDecre = false;
        private int _partHpN = 0;



        //싱글톤
        //StageFlatform 빈 오브젝트 생성(FaltformMgr + 스크립트 추가 시키기) / 혹은 프리팹으로 미리 스크립트 넣어두기
        //플레이어 위치 지정

        public int Life { get { return _life; } }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            switch (_gameState)
            {
                case GameState.MainPage:

                    break;
                case GameState.Playing:
                    _stageManager.gameObject.SetActive(true);
                    Playing();
                    break;
                case GameState.StageChange:
                    _stageManager.PlatformScroll();
                    break;
                case GameState.GameOver:
                    break;
            }
        }

        // 목숨 감소 메서드
        public void DecreaseHP(bool hpDecre)
        {
            //트루면 hp차감
            if (hpDecre)
            {
                _life -= 1;
                Debug.Log("HP -1");
                _player.gameObject.SetActive(false);

                // 목숨 남아있을 시
                if (_life > 0)
                {
                    _player.transform.position = _stageInitialPos;
                    _player.gameObject.SetActive(true);
                    //플레이어는 초기 위치로(화면에 보이는 제일 아래 블럭으로 이동)
                }
                else if (_life < 1)
                {
                    _gameState = GameState.GameOver;
                    _player.gameObject.SetActive(false);
                }
            }
        }

        private void Playing()
        {
            Instantiate(_playerPrefab, _stageInitialPos, _player.transform.rotation);
            //스타트 버튼 누르면 플레이 상태
            //메인UI 내리기
            //플레이 UI 띄우기(스테이지 정보, 현재위치, 점수, 목숨, 콤보) 

            //점수/체력 계산         
            ComboCal();

        }

        private void StageChange()
        {

        }

        //점수
        private void ComboCal()
        {
            _combo = (Combo)_comboN;

            switch (_combo)
            {
                case Combo.Base:
                    _score += 10;
                    break;
                case Combo.Combo:
                    _score += 50;
                    _partHpN += 1;
                    break;
                case Combo.Double:
                    _score += 150;
                    _partHpN += 3;
                    break;
                case Combo.Triple:
                    _score += 300;
                    _life += 1;
                    break;
            }
        }

        //콤보 관리 매서드
        public void ComboMgr()
        {
            //콤보는 최대 트리플까지
            if (_player.IsSuc == true && _comboN < 3)
            {
                _comboN++;
            }

            else if (_player.IsSuc == false)
            {
                _comboN = 0;
            }
        }

    }
}
