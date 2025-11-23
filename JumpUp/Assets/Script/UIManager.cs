using UnityEngine;
using UnityEngine.UI;
using GameMgr;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject _mainPanel;
    private Color color;

    private void Awake()
    {
       
    }
    
    public void OnClickStart()
    {
        _mainPanel.SetActive(false);
        GameMgr.GameManager.Instance._gameState = GameState.Playing;
    }
}
