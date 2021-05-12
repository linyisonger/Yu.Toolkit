using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Yu.Toolkit
{
    /// <summary>
    /// 地区 
    /// </summary>
    public class RegionDto
    {
        /// <summary>
        /// 地区代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 地区名称
        /// </summary>
        public string Name { get; set; }
        public RegionDto()
        {

        }
        /// <summary>
        ///  地区
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <param name="name">地区名称</param>
        public RegionDto(string code, string name)
        {
            Code = code;
            Name = name;
        }

    }
    /// <summary>
    /// 地区代码 
    /// 行政区划代码 http://www.mca.gov.cn/article/sj/xzqh/2020/    
    /// </summary>
    public static class RegionCode
    {
        /// <summary>
        /// 地区代码Json文件的地址
        /// </summary>
        public static string RegionCodeJsonFilePath = "RegionCode.json";

        static List<RegionDto> _regions = null;
        static List<RegionDto> _provinces = null;
        static List<RegionDto> _cities = null;
        static List<RegionDto> _areas = null;
        /// <summary>
        /// 地区
        /// </summary>
        public static List<RegionDto> Regions => _regions ?? GetRegions();
        /// <summary>
        /// 省份或直辖市
        /// </summary>
        public static List<RegionDto> Provinces => _provinces ?? GetProvinces();
        /// <summary>
        /// 城市
        /// </summary>
        public static List<RegionDto> Cities => _cities ?? GetCities();
        /// <summary>
        /// 区域
        /// </summary>
        public static List<RegionDto> Areas => _areas ?? GetAreas();
        /// <summary>
        /// 获取地区码
        /// </summary>
        /// <returns></returns>
        static List<RegionDto> GetRegions()
        {
            _regions = new List<RegionDto>();
            // 行政区划代码 http://www.mca.gov.cn/article/sj/xzqh/2020/
            // var html = File.ReadAllText(@"RegionCode.html");
            // var trs = Regex.Matches(html, @"<tr height=""19"" style=""mso-height-source:userset;height:14.25pt"">[\s\S]*?</tr>");
            // foreach (Match tr in trs)
            // {
            //     string name = Regex.Match(tr.Value, @"[\u4e00-\u9fa5]+")?.Groups[0]?.Value;
            //     string code = Regex.Match(tr.Value, @"<td.*>([0-9]+)</td>")?.Groups[1]?.Value;
            //     _regions.Add(new RegionDto(code, name));
            // }  
            var json = File.ReadAllText(RegionCodeJsonFilePath);
            _regions = JsonConvert.DeserializeObject<List<RegionDto>>(json);
            return _regions;
        }
        /// <summary>
        /// 获取省份或直辖市
        /// </summary>
        /// <returns></returns>
        static List<RegionDto> GetProvinces()
        {
            _provinces = Regions.Where(r => r.Code.EndsWith("0000")).ToList();
            System.Console.WriteLine(_provinces.Count);
            return _provinces;
        }
        /// <summary>
        /// 获取城市
        /// </summary>
        /// <returns></returns>
        static List<RegionDto> GetCities()
        {
            _cities = Regions.Except(Provinces).Where(r => r.Code.EndsWith("00")).ToList();
            return _cities;
        }
        /// <summary>   
        /// 获取区域
        /// </summary>
        /// <returns></returns>
        static List<RegionDto> GetAreas()
        {
            _areas = Regions.Except(Provinces).Except(Cities).ToList();
            return _areas;
        }

        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static RegionDto GetRegionByCode(string code)
        {
            return Regions.FirstOrDefault(r => r.Code == code);
        }
        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <returns></returns>
        public static RegionDto GetRegionByName(string name)
        {
            return Regions.FirstOrDefault(r => r.Name == name);
        }
        /// <summary>
        /// 获取名称
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static string GetRegionNameByCode(string code)
        {
            return GetRegionByCode(code)?.Name;
        }
        /// <summary>
        /// 获取代码
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <returns></returns>
        public static string GetRegionCodeByName(string name)
        {
            return GetRegionByName(name)?.Code;
        }

        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static RegionDto GetProvinceByCode(string code)
        {
            return Provinces.FirstOrDefault(r => r.Code == code);
        }
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <returns></returns>
        public static RegionDto GetProvinceByName(string name)
        {
            return Provinces.FirstOrDefault(r => r.Name == name);
        }
        /// <summary>
        /// 获取省份名称
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static string GetProvinceNameByCode(string code)
        {
            return GetProvinceByCode(code)?.Name;
        }
        /// <summary>
        /// 获取省份地区代码
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <returns></returns>
        public static string GetProvinceCodeByName(string name)
        {
            return GetProvinceByName(name)?.Code;
        }
        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static RegionDto GetCityByCode(string code)
        {
            return Cities.FirstOrDefault(r => r.Code == code);
        }
        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <returns></returns>
        public static RegionDto GetCityByName(string name)
        {
            return Cities.FirstOrDefault(r => r.Name == name);
        }
        /// <summary>
        /// 获取城市名称
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static string GetCityNameByCode(string code)
        {
            return GetCityByCode(code)?.Name;
        }
        /// <summary>
        /// 获取城市地区代码
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <returns></returns>
        public static string GetCityCodeByName(string name)
        {
            return GetCityByName(name)?.Code;
        }

        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static RegionDto GetAreaByCode(string code)
        {
            return Areas.FirstOrDefault(r => r.Code == code);
        }
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <returns></returns>
        public static RegionDto GetAreaByName(string name)
        {
            return Areas.FirstOrDefault(r => r.Name == name);
        }
        /// <summary>
        /// 获取区域名称
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static string GetAreaNameByCode(string code)
        {
            return GetAreaByCode(code)?.Name;
        }
        /// <summary>
        /// 获取区域地区代码
        /// </summary>
        /// <param name="name">地区名称</param>
        /// <returns></returns>
        public static string GetAreaCodeByName(string name)
        {
            return GetAreaByName(name)?.Code;
        }
        /// <summary>
        /// 获取城市
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static List<RegionDto> GetCitiesByProvinceCode(string code)
        {
            return Cities.Where(c => c.Code.StartsWith(code.Substring(0, 2))).ToList();
        }
        /// <summary>
        /// 获取区域
        /// </summary>
        /// <param name="code">地区代码</param>
        /// <returns></returns>
        public static List<RegionDto> GetAreasByCityCode(string code)
        {
            return Areas.Where(c => c.Code.StartsWith(code.Substring(0, 4))).ToList();
        }
    }
}
