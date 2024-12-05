using CICDTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace CICDTest.Services.Tests
{
    [TestFixture]
    public class TestServiceTests
    {


        private  readonly TestService _testService;

        public TestServiceTests()
        {
            _testService = new TestService(); // 初始化欄位
        }

        [Test]
        public async Task testTest()
        {
            // Arrange
            int num1 = 3;
            int num2 = 7;
            int expected = 10;

            // Act
            var result = await _testService.test(num1, num2);

            // Assert
            NUnit.Framework.Assert.That(result, Is.EqualTo(expected), "The sum of the numbers is incorrect.");
        }
    }
}