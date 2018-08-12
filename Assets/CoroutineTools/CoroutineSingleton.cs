
/// <summary>
/// Class used to invoke coroutines from a static context (without an accesible GameObject).
/// </summary>
class CoroutineSingleton : Singleton<EmptyMonoBehaviour>
{
    protected CoroutineSingleton() { }
}
