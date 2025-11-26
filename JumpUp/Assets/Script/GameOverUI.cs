using GameMgr;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject _GameOverPanel;
    [SerializeField] private Text _score;

    private void Start()
    {
        _score.text = $"{GameMgr.GameManager.Instance.Score}";
    }

    public void OnClick(int n)
    {
        switch(n)
        {
            case 0:
                SceneManager.LoadScene("GameScene");
                break;

            case 1:
                SceneManager.LoadScene("Menu");
                break;
        }
    }
}
