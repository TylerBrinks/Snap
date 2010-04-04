using System.Collections.Generic;
using LinFu.IoC.Configuration;
using Snap.Tests.Interceptors;

namespace Snap.Tests.Fakes
{
    public interface IOrderedCode
    {
        void RunInOrder();
        void RunInExplicitOrder();
        void RunInAttributedOrder();
    }

    [Implements(typeof(IOrderedCode))]  // Attribute for LinFu configuration
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
