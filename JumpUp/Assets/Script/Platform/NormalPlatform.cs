using UnityEngine;

//플렛폼종류
public enum PlatformType
{
    Normal, moveLevel1, moveLevel2
}

public class NormalPlatform : MonoBehaviour
{
    protected PlatformType _platformID;
    protected float speed;
    protected Vector2 leftGoal;
    protected Vector2 rightGoal;

    public PlatformType Type { get { return _platformID; } }

    protected void Awake()
    {
        _platformID = PlatformType.Normal;
        speed = 0;
    }

    protected void OnEnable()
    {

    }

    protected void Update()
    {
        
    }

}
