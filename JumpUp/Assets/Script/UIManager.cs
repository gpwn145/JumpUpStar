using GameMgr;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    [SerializeField] private GameObject _choiceLvPanel;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _mainPanel.SetActive(true);
        _choiceLvPanel.SetActive(false);
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
        SceneManager.LoadScene("GameScene");
    }

}
