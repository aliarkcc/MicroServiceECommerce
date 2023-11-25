namespace EventBus.Base
{
    public class SubscriptionInfo
    {
        public Type HandlerType { get;}

        public SubscriptionInfo(Type handlerTypeName)
        {
            HandlerType= handlerTypeName;
        }

        public static SubscriptionInfo Typed(Type handlerType)
        {
            return new SubscriptionInfo(handlerType);
        }
    }
}
