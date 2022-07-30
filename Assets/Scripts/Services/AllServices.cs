public class AllServices
{
    private static AllServices instance;
    public static AllServices Container = instance ??= new AllServices();

    public void RegisterSingle<T>(T implementation) where T : IService =>
        Implementation<T>.ServiceInstance = implementation;

    public T Single<T>() where T : IService =>
        Implementation<T>.ServiceInstance;

    private static class Implementation<T> where T : IService
    {
        public static T ServiceInstance;
    }
}