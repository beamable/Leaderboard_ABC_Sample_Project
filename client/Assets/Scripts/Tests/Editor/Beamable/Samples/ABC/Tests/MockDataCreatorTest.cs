using NUnit.Framework;

namespace Beamable.Samples.ABC
{
    public class MockDataCreatorTest
    {
        [Test]
        public void CreateNewRandomAlias_ResultContainsTestBlah_WhenPrependNameIsTestBlah()
        {
            // Arrange
            string prependName = "testBlah";
            
            // Act
            string result = MockDataCreator.CreateNewRandomAlias(prependName);
            
            // Assert
            Assert.That(result.Contains(prependName), Is.True);
        }
        
        [Test]
        public void CreateNewRandomAlias_ResultIndexOfIs0_WhenPrependName()
        {
            // Arrange
            string prependName = "testBlah";
            
            // Act
            string result = MockDataCreator.CreateNewRandomAlias(prependName);
            
            // Assert
            Assert.That(result.IndexOf(prependName), Is.EqualTo(0));
        }
    }
}
