using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        [SerializeField] private GameObject _playerPrefab;
        private GameObject _player;
        private StageManager _stageManager;
        private MovePlatform _movePlatform;
        private PlayUI _playUI;

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

        //스크롤
        private bool _isScrollGo = false;

        //초기화 위치
        private GameObject _pos;

        private bool isCreatePalyer = false;

        #region 프로퍼티
        public StageManager StageManager { get { return _stageManager; } }
        public MovePlatform MovePlatform { get { return _movePlatform; } }
        public GameObject Player { get { return _player; } }
        public GameState GameState { get { return _gameState; } set { _gameState = value; } }
        public LevelType ChoiceLevelType { get { return _levelType; } set { _levelType = value; } }
        public int Life { get { return _life; } }
        public bool IsScrollGo { get { return _isScrollGo; } }
        public GameObject Pos { get { return _pos; } }
        #endregion

        //싱글톤
        //StageFlatform 빈 오브젝트 생성(FaltformMgr + 스크립트 추가 시키기) / 혹은 프리팹으로 미리 스크립트 넣어두기
        //플레이어 위치 지정


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += ChangeScene;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= ChangeScene;
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

        // 목숨 감소 메서드
        public void DecreaseHP()
        {
            //트루면 hp차감
            _life -= 1;
            //Debug.Log("HP -1");

            // 목숨 남아있을 시
            if (_life > 0)
            {
                Vector2 vector2 = _pos.transform.position;
                vector2.y += 0.5f;
                _player.transform.position = vector2;
                _player.gameObject.SetActive(true);
            }
            else if (_life < 1)
            {
                _gameState = GameState.GameOver;
                _player.gameObject.SetActive(false);
            }
        }

        //씬 전환시 실행
        private void ChangeScene(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "GameScene")
            {
                Init();
                _gameState = GameState.Playing;
                _player = GameObject.Find("Player");
                _stageManager = GameObject.Find("StageMgr").GetComponent<StageManager>();
                _movePlatform = GameObject.Find("PlatformGroup").GetComponent<MovePlatform>();
                _playUI = GameObject.Find("PlayUI").GetComponent<PlayUI>();

                if (isCreatePalyer == false)
                {
                    _player = Instantiate(_playerPrefab);
                    _player.tag = "Player";
                    isCreatePalyer = true;
                }
                if (isCreatePalyer == true)
                {
                    _player.GetComponent<Player>().StartPos = StageManager.Platform[0].transform.position;
                    _player.GetComponent<Player>().transform.position = _player.GetComponent<Player>().CurrentPos();

                    _player.SetActive(true);
                }
            }
        }

        //점수
        public void ComboCal(int _comboN = 0)
        {
            _combo = (Combo)_comboN;

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
            _playUI.ScoreInfo(_score);
            _playUI.HeartColor(_life);
        }
        //public void PlayerPos()
        //{
        //    PlayerCurrentPos
        //}

        public void CurrentPlayerPosInfo(GameObject gameObject)
        {
            _pos = gameObject;
        }

        public void IsScroll (bool go)
        {
            _isScrollGo = go;
            //Debug.Log("2. 스크롤 활성화");
        }
    }
}
