using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace IClock.Tests
{
    [TestClass]
    public class ForwardOnlyClockTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ForwardOnlyClock_Throws_OnNullArgument() => new ForwardOnlyClock(null);

        [TestMethod]
        public void ForwardOnlyClock_GetTime_Returns_ForwardTime()
        {
            var tc = new TestClock();
            var target = new ForwardOnlyClock(tc);

            var t1 = target.GetTime();
            tc.Add(TimeSpan.FromSeconds(-1));   // Set clock back 1 second
            var t2 = target.GetTime();
            Assert.AreEqual(t1, t2);            // Time should be same

            tc.Add(TimeSpan.FromSeconds(3));    // Add 2 seconds
            var t3 = target.GetTime();
            Assert.AreEqual(t1.AddSeconds(2), t3);  // Time should be at +2 seconds now.
        }

    }
}
