using NUnit.Framework;

namespace Beamable.Samples.ABC
{
    public class ABCHelperTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void GetRoundedTime_ResultIs101_WhenValueIs1011()
        {
            // Arrange
            float value = 10.11f;
            
            // Act
            string result = ABCHelper.GetRoundedTime(value);
            
            // Assert
            Assert.That(result, Is.EqualTo("10.1"));

        }

    }
}
