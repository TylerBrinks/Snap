using System.Collections.Generic;
using Snap.Tests.Interceptors;

namespace Snap.Tests.Fakes
{
    public interface IOrderedCode
    {
        void RunInOrder();
        void RunInExplicitOrder();
        void RunInAttributedOrder();
    }

    public class OrderedCode : IOrderedCode
    {
        public static List<string> Actions = new List<string>();
   
        [First]
        [Second]
        public void RunInOrder()
        {

        }

        [First]
        [Second]
        [Third]
        public void RunInExplicitOrder()
        {
        }

        [First(Order = 1)]
        [Second(Order = 2)]
        [Third(Order = 0)]
        public void RunInAttributedOrder()
        {
        }
    }
}
