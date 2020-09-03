using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IClock.Tests
{
    [TestClass]
    public class ScopedClockTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScopedClock_Throws_OnNullTimeProviderArgument() => new ScopedClock((ITimeProvider)null);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScopedClock_Throws_OnNullTimeFunctionArgument() => new ScopedClock((Func<DateTimeOffset>)null);

        [TestMethod]
        public void ScopedClock_FreezesTime()
        {
            var tc = new TestClock();
            using (var target = new ScopedClock(tc))
            {
                var t1 = tc.GetTime();
                var s1 = target.GetTime();
                tc.Tick();
                var t2 = tc.GetTime();
                var s2 = target.GetTime();

                // Testclock should've advanced time
                Assert.AreNotEqual(t1, t2);

                // ScopedClock should still have snapshot of time
                Assert.AreEqual(t1, s1);    // Check against initial time
                Assert.AreEqual(s1, s2);    // Check is both instances are same time
            }
        }

    }
}
