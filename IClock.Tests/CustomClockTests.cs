using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IClock.Tests
{
    [TestClass]
    public class CustomClockTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CustomClock_Throws_OnNullArgument() => new CustomClock(null!);

        [TestMethod]
        public void CustomClock_GetTime_Returns_CorrectValue1()
        {
            var tc = new TestClock();
            var target = new CustomClock(tc.GetTime);
            Assert.AreEqual(TestClock.DefaultTime, target.GetTime());
        }

        [TestMethod]
        public void CustomClock_GetTime_Returns_CorrectValue2()
        {
            var time = new DateTime(2020, 1, 2, 3, 4, 5, 6, DateTimeKind.Local);
            var target = new CustomClock(() => time);
            Assert.AreEqual(time, target.GetTime());
        }

    }
}
