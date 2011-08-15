using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Snap.Tests.Bug
{
    [TestFixture]
    public class IncludeNamespaceBug
    {
        [Test]
        public void Wildcard_at_root_works_as_expected()
        {
            var namespaces = new List<string> {"SnapTests*"};
            var types = new List<Type> {typeof (SnapTests.Fakes.IBadCode)}.ToArray();

            Assert.AreEqual(typeof(SnapTests.Fakes.IBadCode), types.FirstMatch(namespaces));
        }

        [Test]
        public void Wildcard_at_second_level_works_as_expected()
        {
            var namespaces = new List<string> { "SnapTests.Fakes*" };
            var types = new List<Type> { typeof(SnapTests.Fakes.IBadCode) }.ToArray();

            Assert.AreEqual(typeof(SnapTests.Fakes.IBadCode), types.FirstMatch(namespaces));
        }

        [Test]
        public void Explicity_naming_a_type_as_an_namespace_to_include_works_but_perhaps_should_not()
        {
            var namespaces = new List<string> { "SnapTests.Fakes.IBadCode" };
            var types = new List<Type> { typeof(SnapTests.Fakes.IBadCode) }.ToArray();

            Assert.AreEqual(typeof(SnapTests.Fakes.IBadCode), types.FirstMatch(namespaces));
        }

        [Test]
        public void Explicitly_naming_a_namespace_does_not_work_but_should()
        {
            var namespaces = new List<string> { "SnapTests.Fakes" };
            var types = new List<Type> { typeof(SnapTests.Fakes.IBadCode) }.ToArray();

            Assert.AreEqual(typeof(SnapTests.Fakes.IBadCode), types.FirstMatch(namespaces));
            
        }

    }
}