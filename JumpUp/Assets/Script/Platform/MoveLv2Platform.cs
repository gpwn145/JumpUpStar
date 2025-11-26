using UnityEngine;


public class MoveLv2Platform : MoveLv1Platform
{
    protected new void Awake()
    {
        base.Awake();
        _platformID = PlatformType.moveLevel2;
        speed = 1.5f;
    }
}
