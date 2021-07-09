using System;
using Xunit;

namespace Yu.Toolkit.Test
{
    public class BankCardNumberTest
    {
        [Fact(DisplayName = "У�������")]
        public void GetCheckCodeTest()
        {
            Assert.True(BankCardNumber.GetCheckCode("621700243002250449") == "3");
            Assert.True(BankCardNumber.GetCheckCode("621226170200684888") == "1");
        }

        [Fact(DisplayName = "��֤�����Ƿ�������")]
        public void VerificationTest()
        {
            Assert.Equal("Does not meet the regular rules", Assert.Throws<BankCardNumberException>(() => { BankCardNumber.Verification("AbcDefjHjklmdnOpqr"); }).Message);
            Assert.Equal("Does not meet the regular rules", Assert.Throws<BankCardNumberException>(() => { BankCardNumber.Verification("6212261702006848882A"); }).Message);
            Assert.Equal("Does not meet the regular rules", Assert.Throws<BankCardNumberException>(() => { BankCardNumber.Verification("621226182"); }).Message);
            Assert.Equal("Does not meet the regular rules", Assert.Throws<BankCardNumberException>(() => { BankCardNumber.Verification("6212261822222222222222222222222222222"); }).Message);
            Assert.Equal("Check code error", Assert.Throws<BankCardNumberException>(() => { BankCardNumber.Verification("6212261702006848882"); }).Message);
        }
    }
}
