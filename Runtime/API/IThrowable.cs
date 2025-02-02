using UnityEngine;
public delegate void ThisThrown(IThrowable item);
public interface IThrowable
{
    public static event ThisThrown OnThrown;
    public static void RaiseThrownEvent(IThrowable throwable) => OnThrown?.Invoke(throwable);
    public void Throw(Vector3 forward, float force);

}