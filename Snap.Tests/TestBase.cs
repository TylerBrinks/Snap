using System.Collections.Generic;
using NUnit.Framework;
using Snap.Tests.Fakes;

namespace Snap.Tests
{
    [TestFixture]
    public class TestBase
    {
        [SetUp]
        public void Reset_Ordered_Code()
        {
            OrderedCode.Actions = new List<string>();
        }
    }
}
