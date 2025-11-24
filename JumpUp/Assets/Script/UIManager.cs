using GameMgr;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _playingViewPanel;
    [SerializeField] private GameObject _choiceLvPanel;

    private Color heartColor;
    private Color cookieColor;
    [SerializeField] private List<Image> _heartImage = new List<Image>();
    [SerializeField] private List<Image> _cookieImage = new List<Image>();

    [SerializeField] private Text _score;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _mainPanel.SetActive(true);
        _choiceLvPanel.SetActive(false);
        _playingViewPanel.SetActive(false);
        GameMgr.GameManager.Instance.GameState = GameState.MainPage;
    }

    //스타트 버튼
    public void OnClickStart()
    {
        //메인 끄기
        _mainPanel.SetActive(false);
        //초이스 화면 키기
        _choiceLvPanel.SetActive(true);
        //게임상태 난이도선택화면
        GameMgr.GameManager.Instance.GameState = GameState.MainPage;
        GameMgr.GameManager.Instance.Init();

    }

    //난이도선택페이지
    public void OnClickChoiceLv(int Lv)
    {
        if (Lv == 4)
        {
            Init();
            return;
        }

        //난이도 선택
        switch (Lv)
        {
            case 0:
                GameMgr.GameManager.Instance.ChoiceLevelType = LevelType.Easy;
                break;
            case 1:
                GameMgr.GameManager.Instance.ChoiceLevelType = LevelType.Normal;
                break;
            case 2:
                GameMgr.GameManager.Instance.ChoiceLevelType = LevelType.Hard;
                break;
            case 3:
                GameMgr.GameManager.Instance.ChoiceLevelType = LevelType.Infinite;
                break;
        }
        //선택하면 끄기
        _choiceLvPanel.SetActive(false);
        _playingViewPanel.SetActive(true);

        //게임상태 플레이로 바꿈
        GameMgr.GameManager.Instance.GameState = GameState.Playing;
        //플레이모드 메서드 실행
        GameMgr.GameManager.Instance.Playing();
    }

    public void ScoreInfo(int score)
    {
        _score.text = $"{score}";
    }

    public void HeartColor(int lifeN)
    {
        for (int i = 0; i < lifeN; i++)
        {
            ColorUtility.TryParseHtmlString("#FEBBEA", out heartColor);
            _heartImage[i].color = heartColor;
        }

        int N = 4;
        for (int i = 0; i < 5 - lifeN; i++)
        {
            heartColor = Color.white;
            _heartImage[N--].color = heartColor;
        }
    }
}
