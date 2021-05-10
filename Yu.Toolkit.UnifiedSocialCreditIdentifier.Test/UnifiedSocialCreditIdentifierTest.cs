using System;
using Xunit;

namespace Yu.Toolkit.Test
{
    public class UnifiedSocialCreditIdentifierTest
    {
        [Fact(DisplayName = "校验码测试")]
        public void GetCheckCodeTest()
        {
            Assert.True(UnifiedSocialCreditIdentifier.GetCheckCode("91350100M000100Y4") == "3");
            Assert.True(UnifiedSocialCreditIdentifier.GetCheckCode("91350100M000100Y43") == "3");
        }

        [Fact(DisplayName = "检测机构代码测试")]
        public void CheckOrganizationCodeTest()
        {
            Assert.Equal("Institution code does not exist", Assert.Throws<UnifiedSocialCreditIdentifierException>(() => { UnifiedSocialCreditIdentifier.CheckOrganizationCode("96"); }).Message);
            Assert.Equal("The registration management department code does not exist", Assert.Throws<UnifiedSocialCreditIdentifierException>(() => { UnifiedSocialCreditIdentifier.CheckOrganizationCode("B3"); }).Message);
            Assert.Equal("Length must be 2", Assert.Throws<UnifiedSocialCreditIdentifierException>(() => { UnifiedSocialCreditIdentifier.CheckOrganizationCode("B23"); }).Message);
        }


        [Fact(DisplayName = "验证符合是否规则测试")]
        public void VerificationTest()
        {
            Assert.Equal("Does not meet the regular rules", Assert.Throws<UnifiedSocialCreditIdentifierException>(() => { UnifiedSocialCreditIdentifier.Verification("AbcDefjHjklmdnOpqr"); }).Message);
            Assert.Equal("Check code error", Assert.Throws<UnifiedSocialCreditIdentifierException>(() => { UnifiedSocialCreditIdentifier.Verification("92141100XYWDG8M3BB"); }).Message);
            Assert.Equal("Length must be 18", Assert.Throws<UnifiedSocialCreditIdentifierException>(() => { UnifiedSocialCreditIdentifier.Verification("B23"); }).Message);
        }
    }
}
