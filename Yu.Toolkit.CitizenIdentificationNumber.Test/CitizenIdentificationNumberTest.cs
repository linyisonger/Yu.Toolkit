using System;
using Xunit;

namespace Yu.Toolkit.Test
{
    public class CitizenIdentificationNumberTest
    {
        [Fact(DisplayName = "校验码测试")]
        public void GetCheckCodeTest()
        {
            Assert.True(CitizenIdentificationNumber.GetCheckCode("11010519491231002") == "X");
            Assert.True(CitizenIdentificationNumber.GetCheckCode("11010519491231002X") == "X");
        }

        [Fact(DisplayName = "验证符合是否规则测试")]
        public void VerificationTest()
        {
            Assert.Equal("Does not meet the regular rules", Assert.Throws<CitizenIdentificationNumberException>(() => { CitizenIdentificationNumber.Verification("AbcDefjHjklmdnOpqr"); }).Message);
            Assert.Equal("Check code error", Assert.Throws<CitizenIdentificationNumberException>(() => { CitizenIdentificationNumber.Verification("110105194912310029"); }).Message);
            Assert.Equal("Length must be 18", Assert.Throws<CitizenIdentificationNumberException>(() => { CitizenIdentificationNumber.Verification("B23"); }).Message);
        }
    }
}
