using System;
using Xunit;

namespace Yu.Toolkit.Test
{
    public class NameGeneratorTest
    {
        [Fact(DisplayName = "名称生成器无参数")]
        public void NoParametersTest()
        {
            Assert.False(string.IsNullOrWhiteSpace(NameGenerator.Generate()));
        }

        [Fact(DisplayName = "名称生成器男性")]
        public void MaleTest()
        {
            Assert.False(string.IsNullOrWhiteSpace(NameGenerator.Generate(NameGeneratorGenderEnum.Male)));
        }

        [Fact(DisplayName = "名称生成器女性")]
        public void FemaleTest()
        {
            Assert.False(string.IsNullOrWhiteSpace(NameGenerator.Generate(NameGeneratorGenderEnum.Female)));
        }
    }
}
