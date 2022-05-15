using Newtonsoft.Json;
using System;

namespace RpaTestModule
{
    public class TestClass
    {
        public void 테스트()
        {
            Console.WriteLine("테스트");
            JsonConvert.SerializeObject(new TestClass());
        }

        public int 테스트(int a, int b)
        {
            return a + b;
        }
    }
}
