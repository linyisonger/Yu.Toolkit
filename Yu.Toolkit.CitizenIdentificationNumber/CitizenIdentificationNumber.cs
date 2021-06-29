using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Yu.Toolkit
{
    /// <summary>
    /// 性别
    /// </summary>
    [Flags]
    public enum CitizenIdentificationNumberGenderEnum
    {
        /// <summary>
        /// 男
        /// </summary>
        Male = 1 << 0,
        /// <summary>
        /// 女
        /// </summary>
        Female = 1 << 1,
    }
    /// <summary>
    /// 订制报错
    /// </summary>
    public class CitizenIdentificationNumberException : Exception
    {
        public CitizenIdentificationNumberException(string message) : base(message)
        {
        }
    }
    /// <summary>
    ///  代码字符集 
    /// </summary>
    internal class CodeCharacterSetDto
    {
        /// <summary>
        /// 代码字符
        /// </summary>
        public string CharCode { get; set; }
        /// <summary>
        /// 代码字符数值 
        /// </summary>
        public int CharValue { get; set; }
    }
    /// <summary>
    /// 公民身份号码
    /// </summary>
    public class CitizenIdentificationNumberDto
    {
        /// <summary>
        /// 性别 男女
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }
        /// <summary>
        /// 城市名称
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 省份名称
        /// </summary>
        public string ProvinceName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
    }



    /// <summary>
    /// 公民身份号码
    /// http://c.gb688.cn/bzgk/gb/showGb?type=online&hcno=080D6FBF2BB468F9007657F26D60013E
    /// </summary>
    public static class CitizenIdentificationNumber
    {

        static readonly string _defalutRegionCode = "410783";
        static readonly string _defalutDateBirth = "19980101";
        /// <summary>
        /// 代码字符集配置文件地址
        /// </summary>
        public const string CodeCharacterSetJsonFilePath = "Yu.Toolkit.CitizenIdentificationNumberStaticFiles/CitizenIdentificationNumberCodeCharacterSet.json";

        static List<CodeCharacterSetDto> _codeCharacterSetList = null;
        /// <summary>
        /// 代码字符集
        /// </summary>
        static List<CodeCharacterSetDto> CodeCharacterSetList => _codeCharacterSetList ?? GetCodeCharacterSetList();
        /// <summary>
        /// 获取代码字符集
        /// </summary>
        /// <returns></returns>
        static List<CodeCharacterSetDto> GetCodeCharacterSetList()
        {
            _codeCharacterSetList = new List<CodeCharacterSetDto>();
            var json = File.ReadAllText(CodeCharacterSetJsonFilePath);
            _codeCharacterSetList = JsonConvert.DeserializeObject<List<CodeCharacterSetDto>>(json);
            return _codeCharacterSetList;
        }
        /// <summary>
        /// 获取校验码
        /// </summary>
        /// <param name="str">前17位</param>
        /// <returns></returns>
        public static string GetCheckCode(string str)
        {
            str = str.Substring(0, 17);
            var sum = str.ToCharArray().Select((c, i) => int.Parse(c.ToString()) * Math.Pow(2, 17 - i) % 11).Sum();
            var cv = sum % 11;
            return CodeCharacterSetList.First(ccs => ccs.CharValue == cv)?.CharCode;
        }

        /// <summary>
        /// 生成身份证号码
        /// </summary> 
        /// <param name="regionCode">区域代码 6 位</param>
        /// <param name="dateBirth">出生日期 8 位</param>
        /// <param name="gender">性别</param>
        /// <returns></returns>
        public static string Gengenerate(string regionCode = "", string dateBirth = "", CitizenIdentificationNumberGenderEnum gender = CitizenIdentificationNumberGenderEnum.Male | CitizenIdentificationNumberGenderEnum.Female)
        {
            // 结果
            var result =
                (string.IsNullOrWhiteSpace(regionCode) ? _defalutRegionCode : regionCode) + // 是否使用默认地区码
                (string.IsNullOrWhiteSpace(dateBirth) ? _defalutDateBirth : dateBirth); // 是否使用默认出生日期
            // 顺序码
            int sequenceCode;
            bool hasMale = (gender & CitizenIdentificationNumberGenderEnum.Male).ToString() != "0";
            bool hasFemale = (gender & CitizenIdentificationNumberGenderEnum.Female).ToString() != "0";
            // 奇数为
            do
                sequenceCode = new Random().Next(1, 1000);
            while ((!hasMale && sequenceCode % 2 == 1) || (!hasFemale && sequenceCode % 2 == 0));
            // 追加顺序码
            result += sequenceCode.ToString("000");
            // 追加验证码
            result += GetCheckCode(result);
            return result;
        }
        /// <summary>
        /// 验证身份证号码标识符是否正确
        /// </summary>
        /// <param name="code">身份证号码标识符</param>
        public static void Verification(string code)
        {
            if (code.Length != 18) throw new CitizenIdentificationNumberException("Length must be 18");
            if (!Regex.IsMatch(code, "^[1-9]\\d{5}(18|19|([23]\\d))\\d{2}((0[1-9])|(10|11|12))(([0-2][1-9])|10|20|30|31)\\d{3}[0-9Xx]$")) throw new CitizenIdentificationNumberException("Does not meet the regular rules");
            if (code[17].ToString() != GetCheckCode(code)) throw new CitizenIdentificationNumberException("Check code error");
        }

        /// <summary>
        /// 解析内容
        /// </summary>
        /// <param name="code">身份证号码标识符</param>
        /// <returns></returns>
        public static CitizenIdentificationNumberDto Parse(string code)
        {
            Verification(code);
            var regionCode = code.Substring(0, 6);
            var year = int.Parse(code.Substring(6, 4));
            var month = int.Parse(code.Substring(10, 2));
            var day = int.Parse(code.Substring(12, 2));
            var birthday = new DateTime(year, month, day);
            var gender = int.Parse(code.Substring(14, 3)) % 2 == 0 ? "女" : "男";
            return new CitizenIdentificationNumberDto()
            {
                ProvinceName = RegionCode.GetProvinceNameByCode(regionCode),
                CityName = RegionCode.GetCityNameByCode(regionCode),
                AreaName = RegionCode.GetAreaNameByCode(regionCode),
                Gender = gender,
                Birthday = birthday
            };
        }

    }
}
