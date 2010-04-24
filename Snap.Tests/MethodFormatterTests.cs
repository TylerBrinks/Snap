using System;
using NUnit.Framework;

namespace Snap.Tests
{
    [TestFixture]
    public class MethodFormatterTests
    {
        [Test]
        public void Method_Formatter_Translates_Calls_To_Strings()
        {
            var testClass = new MethodFormatTestClass();
            var methodInfo = testClass.GetType().GetMethod("GetDateTime", new[]{typeof(int)} );

            var signature = MethodSignatureFormatter.Create(methodInfo, new[]{"1"});
            Assert.AreEqual("Snap.Tests.MethodFormatTestClass::GetDateTime({1})", signature);
        }

        [Test]
        public void Method_Formatter_Supports_Zero_Parameters()
        {
            var testClass = new MethodFormatTestClass();
            var methodInfo = testClass.GetType().GetMethod("GetDateTime", new Type[] { });

            var signature = MethodSignatureFormatter.Create(methodInfo, new object[]{});
            Assert.AreEqual("Snap.Tests.MethodFormatTestClass::GetDateTime()", signature);
        }

        [Test]
        public void Method_Formatter_Supports_Multiple_Parameters()
        {
            var testClass = new MethodFormatTestClass();
            var methodInfo = testClass.GetType().GetMethod("GetDateTime", new[] { typeof(int), typeof(int) });

            var signature = MethodSignatureFormatter.Create(methodInfo, new[] { "1", "3" });
            Assert.AreEqual("Snap.Tests.MethodFormatTestClass::GetDateTime({1}, {3})", signature);
        }

        [Test]
        public void Method_Formatter_Supports_Multiple_Generic_Parameters()
        {
            var testClass = new MethodFormatTestClass();
            var methodInfo = testClass.GetType().GetMethod("GetGenericMultiple");

            var signature = MethodSignatureFormatter.Create(methodInfo, new[] { "1", "3" });
            Assert.AreEqual("Snap.Tests.MethodFormatTestClass::GetGenericMultiple<T, TK>({1}, {3})", signature);
        }

        [Test]
        public void Method_Formatter_Supports_Generic_Methods()
        {
            var testClass = new MethodFormatTestClass();
            var methodInfo = testClass.GetType().GetMethod("GetGeneric");
            var format = MethodSignatureFormatter.Create(methodInfo, new[] { "test" });

            Assert.AreEqual("Snap.Tests.MethodFormatTestClass::GetGeneric<T>({test})", format);
        }

        [Test]
        public void Method_Formatter_Supports_Generic_Classes()
        {
            var methodInfo = typeof(GenericClass<>).GetMethod("GetGeneric");
            var format = MethodSignatureFormatter.Create(methodInfo, new[] { "test" });

            Assert.AreEqual("Snap.Tests.GenericClass`1<T>::GetGeneric({test})", format);
        }
    }

    public interface IMethodNameTestClass
    {
        DateTime GetDateTime(int minutes);
        DateTime GetDateTime(int minutes, int seconds);
        DateTime GetDateTime();
        DateTime ShouldNotBeCached();
        string GetGeneric<T>(T value);
        string GetGenericMultiple<T, TK>(T value, TK key);
    }

    public class MethodFormatTestClass : IMethodNameTestClass
    {
        public DateTime GetDateTime(int minutes)
        {
            return DateTime.Now.AddMinutes(minutes);
        }

        public DateTime GetDateTime(int minutes, int seconds)
        {
            return DateTime.Now.AddMinutes(minutes).AddSeconds(seconds);
        }

        public DateTime GetDateTime()
        {
            return DateTime.Now;
        }

        public string GetGeneric<T>(T value)
        {
            return value.ToString();
        }

        public string GetGenericMultiple<T, TK>(T value, TK key)
        {
            return value.ToString();
        }

        public DateTime ShouldNotBeCached()
        {
            return DateTime.Now;
        }
    }

    public class GenericClass<T>
    {
        public string GetGeneric(T value)
        {
            return value.ToString();
        }
    }
}
