using System;

namespace Snap
{
    public class InterceptorRegistration
    {
        public InterceptorRegistration(Type interceptorType)
            : this(interceptorType, null, 0)
        {}

        public InterceptorRegistration(Type interceptorType, Type targetAttributeType, int order)
        {
            InterceptorType = interceptorType;
            TargetAttributeType = targetAttributeType;
            Order = order;

            // by default interceptor is not kept in container, and created via constructor rather than resolved from container
            KeptInContainer = false;
        }

        public Type InterceptorType { get; private set; }
        public Type TargetAttributeType { get; set; }
        public int Order { get; set; }
        public bool KeptInContainer { get; set; }
    }
}