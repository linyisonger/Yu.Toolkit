using System;
using Xunit;

namespace Yu.Toolkit.Test
{
    [Collection("区域代码测试")]
    public class RegionCodeTest
    {
        [Fact(DisplayName = "地区代码读取正常")]
        public void RegionLoading()
        {
            Assert.True(RegionCode.Regions.Count > 0);
        }
        [Fact(DisplayName = "省代码测试")]
        public void ProvinceTest()
        {
            Assert.True(RegionCode.Provinces.Count > 0);
            Assert.True("湖北省" == RegionCode.GetProvinceNameByCode("420000"));
            Assert.True("420000" == RegionCode.GetProvinceCodeByName("湖北省"));
        }
        [Fact(DisplayName = "市代码测试")]
        public void CityTest()
        {
            Assert.True(RegionCode.Cities.Count > 0);
            Assert.True("武汉市" == RegionCode.GetCityNameByCode("420100"));
            Assert.True("420100" == RegionCode.GetCityCodeByName("武汉市"));
        }
        [Fact(DisplayName = "区代码测试")]
        public void AreaTest()
        {
            Assert.True(RegionCode.Areas.Count > 0);
            Assert.True("武昌区" == RegionCode.GetAreaNameByCode("420106"));
            Assert.True("420106" == RegionCode.GetAreaCodeByName("武昌区"));
        }

    }
}
