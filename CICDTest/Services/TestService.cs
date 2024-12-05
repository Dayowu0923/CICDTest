using CICDTest.Interfaces;

namespace CICDTest.Services
{
    public class TestService : ITestService
    {
        public TestService()
        {
            
        }

        public Task<int> test(int num1, int num2)
        {
            return Task.FromResult(num1 + num2);
        }
    }
}
