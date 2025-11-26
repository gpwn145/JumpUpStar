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

        private GameObject _2comboPrefab;
        private GameObject _3comboPrefab;
        private GameObject _4comboPrefab;

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
        private int _floor = 0;

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
        public int Score { get { return _score; } }
        public bool IsScrollGo { get { return _isScrollGo; } set { _isScrollGo = value; } }
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
            _floor = 0;

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
                _player.GetComponent<Player>().PlayerCurrentPos.GetComponent<BoxCollider2D>().enabled = true;
                Debug.Log($"{_player.GetComponent<Player>().PlayerCurrentPos.name} 켰음");
                _player.gameObject.SetActive(true);
                _player.GetComponent<Player>().IsRegen = true;
                Debug.Log($"5. _isRegen = {_player.GetComponent<Player>().IsRegen}");
            }
            else if (_life < 1)
            {
                _gameState = GameState.GameOver;
                SceneManager.LoadScene("GameOver");
                Destroy(_player.gameObject);
                isCreatePalyer = false;
            }
        }

        //씬 전환시 실행
        private void ChangeScene(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "GameScene")
            {
                Init();
                _gameState = GameState.Playing;

                _stageManager = GameObject.Find("StageMgr").GetComponent<StageManager>();
                _movePlatform = GameObject.Find("PlatformGroup").GetComponent<MovePlatform>();
                _playUI = GameObject.Find("PlayUI").GetComponent<PlayUI>();
                _2comboPrefab = GameObject.Find("2Combo_0");
                _3comboPrefab = GameObject.Find("3Combo_0");
                _4comboPrefab = GameObject.Find("4Combo_0");

                _2comboPrefab.SetActive(false);
                _3comboPrefab.SetActive(false);
                _4comboPrefab.SetActive(false);

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

                    Debug.Log(_player);
                    _player.SetActive(true);

                    //리스폰? Yes
                    _player.GetComponent<Player>().IsRegen = true;
                }


                Debug.Log(_stageManager);
                Debug.Log(_movePlatform);
                Debug.Log(_playUI);
            }

            if (scene.name == "Menu")
            {
                _gameState = GameState.MainPage;
                _stageManager = null;
                _movePlatform = null;
                _playUI = null;
            }
        }

        //점수
        public void ComboCal(int _comboN = 0)
        {
            _combo = (Combo)_comboN;
            switch (_combo)
            {
                case Combo.Fail:
                    _2comboPrefab.SetActive(false);
                    _3comboPrefab.SetActive(false);
                    _4comboPrefab.SetActive(false);
                    break;
                case Combo.Base:
                    _score += 10;
                    _floor++;
                    break;
                case Combo.Combo:
                    _2comboPrefab.SetActive(true);
                    _score += 50;
                    _floor++;
                    if (_partHpN < 5)
                    {
                        _partHpN += 1;
                    }
                    break;
                case Combo.Double:
                    _3comboPrefab.SetActive(true);
                    _score += 150;
                    _floor++;
                    if (_partHpN < 3)
                    {
                        _partHpN += 3;
                    }
                    break;
                case Combo.Triple:
                    _floor++;
                    _4comboPrefab.SetActive(true);
                    _score += 300;
                    if (_life < 5)
                    {
                        _life += 1;
                    }
                    break;
            }
            if(_partHpN > 5)
            {
                _life += 1;
                _partHpN -= 5;
            }
            _playUI.ScoreInfo(_score, _floor);
            _playUI.HeartColor(_life);
            _playUI.CookieColor(_partHpN);
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
            Debug.Log("2. 스크롤 활성화");
        }
    }
}
