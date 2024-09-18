namespace ChartAPI.Subscription.Middleware
{
    static public class DatabaseSubscriptionMiddleware
    {
        public static void UseDatabaseSubscription<T>(this IApplicationBuilder applicationBuilder,string tableName) where  T:class,IDatabaseSubscription
        {
            var subscription = (T)applicationBuilder.ApplicationServices.GetService(typeof(T));
            subscription.Configure(tableName);
        }
    }
}
