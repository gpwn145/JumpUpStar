using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GameMgr
{
    public enum LevelType
    {
        Easy, Normal, Hard, Infinite
    }
    public enum GameState
    {
        MainPage, LevelChoice, Playing, StageChange, GameOver
    }
    public enum Combo
    {
        Fail, Base, Combo, Double, Triple
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject _player;
        [SerializeField] private StageManager _stageManager;
        [SerializeField] private UI_Manager _uiManager;

        public static GameManager Instance;
        private GameState _gameState;
        private LevelType _levelType;


        //콤보상태
        private Combo _combo;

        //점수
        private int _score = 0;

        //목숨
        private int _life = 5;
        private bool _hpDecre = false;
        private int _partHpN = 0;

        //초기화 위치
        private Vector2 _stageInitialPos;


        #region 프로퍼티
        public StageManager StageManager { get { return _stageManager; } }
        public GameState GameState { get { return _gameState; } set { _gameState = value; } }
        public LevelType ChoiceLevelType { get { return _levelType; } set { _levelType = value; } }
        #endregion

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
        private void OnEnable()
        {
            Init();
        }

        public void Init()
        {
            //콤보상태
            _combo = Combo.Base;

            //점수
            _score = 0;

            //목숨
            _life = 5;
            _hpDecre = false;
            _partHpN = 0;

        }


        private void Update()
        {
            switch (_gameState)
            {
                case GameState.MainPage:
                    break;
                case GameState.Playing:
                    HpParticle();
                    break;
                case GameState.StageChange:
                    break;
                case GameState.GameOver:
                    break;
            }
        }

        // 목숨 감소 메서드
        public void DecreaseHP()
        {
            //트루면 hp차감
            _life -= 1;
            Debug.Log("HP -1");
            _player.gameObject.SetActive(false);

            // 목숨 남아있을 시
            if (_life > 0)
            {
                Vector2 vector2 = _player.GetComponent<Player>().PlatformPos.transform.position;
                _player.transform.position = vector2;
                _player.gameObject.SetActive(true);
            }
            else if (_life < 1)
            {
                _gameState = GameState.GameOver;
                _player.gameObject.SetActive(false);
            }
        }

        public void Playing()
        {
            _stageManager.gameObject.SetActive(true);
            Instantiate(_player, _stageManager.Platform[0].transform);

            //스타트 버튼 누르면 플레이 상태
            //메인UI 내리기
            //플레이 UI 띄우기(스테이지 정보, 현재위치, 점수, 목숨, 콤보) 

            //점수/체력 계산         
        }

        private void StageChange()
        {

        }

        //점수
        public void ComboCal(int _comboN = 0)
        {
            _combo = (Combo)_comboN;
            Debug.Log(_combo);

            switch (_combo)
            {
                case Combo.Fail:
                    break;
                case Combo.Base:
                    _score += 10;
                    break;
                case Combo.Combo:
                    _score += 50;
                    if (_partHpN < 5)
                    {
                        _partHpN += 1;
                    }
                    break;
                case Combo.Double:
                    _score += 150;
                    if (_partHpN < 3)
                    {
                        _partHpN += 3;
                    }
                    break;
                case Combo.Triple:
                    _score += 300;
                    if (_life < 5)
                    {
                        _life += 1;
                    }
                    break;
            }
            _uiManager.ScoreInfo(_score);
            _uiManager.HeartColor(_life);
        }

        private void HpParticle()
        {
            if (_partHpN > 4 && _life < 5)
            {
                _partHpN -= 5;
                _life += 1;
            }
        }

        public void IsStep(GameObject obj)
        {
            if (obj.GetComponent<NormalPlatform>().IsStep == true)
            {
                obj.GetComponent<NormalPlatform>().IsStep = false;
                if (_player.GetComponent<Player>().IsSuc < 4)
                {
                    _player.GetComponent<Player>().IsSuc++;
                }
                Debug.Log("새땅");
            }
            else
            {
                _player.GetComponent<Player>().IsSuc = 0;
                Debug.Log("이미 밟았음");
            }
            ComboCal(_player.GetComponent<Player>().IsSuc);
        }
    }
}
