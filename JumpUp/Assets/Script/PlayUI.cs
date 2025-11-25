using GameMgr;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private GameObject _playingViewPanel;

    private Color heartColor;
    private Color cookieColor;
    [SerializeField] private List<Image> _heartImage = new List<Image>();
    [SerializeField] private List<Image> _cookieImage = new List<Image>();

    [SerializeField] private Text _score;


    private void Init()
    {
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
