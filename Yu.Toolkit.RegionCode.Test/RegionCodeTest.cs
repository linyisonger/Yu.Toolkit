using System;
using Xunit;

namespace Yu.Toolkit.Test
{
    [Collection("����������")]
    public class RegionCodeTest
    {
        [Fact(DisplayName = "���������ȡ����")]
        public void RegionLoading()
        {
            Assert.True(RegionCode.Regions.Count > 0);
        }
        [Fact(DisplayName = "ʡ�������")]
        public void ProvinceTest()
        {
            Assert.True(RegionCode.Provinces.Count > 0);
            Assert.True("����ʡ" == RegionCode.GetProvinceNameByCode("420000"));
            Assert.True("420000" == RegionCode.GetProvinceCodeByName("����ʡ"));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetProvinceNameByCode("420100")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetProvinceNameByCode("420106")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetProvinceCodeByName("�人��")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetProvinceCodeByName("�����")));
        }
        [Fact(DisplayName = "�д������")]
        public void CityTest()
        {
            Assert.True(RegionCode.Citys.Count > 0);
            Assert.True("�人��" == RegionCode.GetCityNameByCode("420100"));
            Assert.True("420100" == RegionCode.GetCityCodeByName("�人��"));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetCityNameByCode("420000")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetCityNameByCode("420106")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetCityCodeByName("����ʡ")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetCityCodeByName("�����")));
        }
        [Fact(DisplayName = "���������")]
        public void AreaTest()
        {
            Assert.True(RegionCode.Areas.Count > 0);
            Assert.True("�����" == RegionCode.GetAreaNameByCode("420106"));
            Assert.True("420106" == RegionCode.GetAreaCodeByName("�����"));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetAreaNameByCode("420000")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetAreaNameByCode("420100")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetAreaCodeByName("����ʡ")));
            Assert.True(string.IsNullOrEmpty(RegionCode.GetAreaCodeByName("�人��")));
        }

    }
}
