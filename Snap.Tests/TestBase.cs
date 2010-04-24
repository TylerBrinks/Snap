using System.Collections.Generic;
using NUnit.Framework;
using SnapTests.Fakes;

namespace Snap.Tests
{
    [TestFixture]
    public class TestBase
    {
        [SetUp]
        public void Reset_Ordered_Code()
        {
            // The actions are static for convenience.  Reset the list
            // after each test run.
            OrderedCode.Actions = new List<string>();
        }
    }
}
