using System;
using Xunit;

namespace Yu.Toolkit.Test
{
    public class PhoneNumberTest
    {
        [Fact(DisplayName = "验证符合是否规则测试")]
        public void VerificationTest()
        {
            Assert.Equal("Does not meet the regular rules", Assert.Throws<PhoneNumberException>(() => { PhoneNumber.Verification("1999999999A"); }).Message);
            Assert.Equal("Does not meet the regular rules", Assert.Throws<PhoneNumberException>(() => { PhoneNumber.Verification("199999999999"); }).Message);
            Assert.Equal("Does not meet the regular rules", Assert.Throws<PhoneNumberException>(() => { PhoneNumber.Verification("199999999"); }).Message);
            Assert.Equal("Incorrect network identification number", Assert.Throws<PhoneNumberException>(() => { PhoneNumber.Verification("20019991999"); }).Message);
        }
    }
}
