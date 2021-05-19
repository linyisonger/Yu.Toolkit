using System;
using Xunit;

namespace Yu.Toolkit.Test
{
    public class NameGeneratorTest
    {
        [Fact(DisplayName = "�����������޲���")]
        public void NoParametersTest()
        {
            Assert.False(string.IsNullOrWhiteSpace(NameGenerator.Generate()));
        }

        [Fact(DisplayName = "��������������")]
        public void MaleTest()
        {
            Assert.False(string.IsNullOrWhiteSpace(NameGenerator.Generate(NameGeneratorGenderEnum.Male)));
        }

        [Fact(DisplayName = "����������Ů��")]
        public void FemaleTest()
        {
            Assert.False(string.IsNullOrWhiteSpace(NameGenerator.Generate(NameGeneratorGenderEnum.Female)));
        }
    }
}
