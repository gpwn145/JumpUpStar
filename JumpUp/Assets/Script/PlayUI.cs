using GameMgr;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class PlayUI : MonoBehaviour
{
    [SerializeField] private GameObject _playingViewPanel;
    [SerializeField] private Sprite _cookieNull;
    [SerializeField] private Sprite _cookie;



    private Color heartColor;
    private Color cookieColor;
    [SerializeField] private List<Image> _heartImage = new List<Image>();
    [SerializeField] private List<Image> _cookieImage = new List<Image>();

    [SerializeField] private Text _score;
    [SerializeField] private Text _floor;

    private void Awake()
    {

    }


    public void ScoreInfo(int score, int floor)
    {
        _score.text = $"{score}";
        _floor.text = $"{floor}";
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

    public void CookieColor(int CookieN)
    {
        for (int i = 0; i < _cookieImage.Count; i++)
        {
            _cookieImage[i].GetComponent<Image>().sprite = _cookieNull;
        }

        for (int i = 0; i < CookieN; i++)
        {
            _cookieImage[i].GetComponent<Image>().sprite = _cookie;
        }
    }
}
