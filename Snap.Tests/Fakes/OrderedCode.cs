using System.Collections.Generic;
using Snap.Tests.Interceptors;

namespace Snap.Tests.Fakes
{
    public interface IOrderedCode
    {
        void RunInOrder();
        void RunInDecoratedOrder();
    }

    public class OrderedCode : IOrderedCode
    {
        public static List<string> Actions = new List<string>();
   
        [First]
        [Second]
        public void RunInOrder()
        {

        }

        [Second]
        [First]
        public void RunInDecoratedOrder()
        {
        }
    }
}
