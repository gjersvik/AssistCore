using NUnit.Framework;
using AssistCore;

namespace AssistCore.Tests
{
    public class Class1Tests
    {
        [Test]
        public void Adder(){
            Assert.That(Class1.adder(1,2), Is.EqualTo(3));
        }
    }
}
