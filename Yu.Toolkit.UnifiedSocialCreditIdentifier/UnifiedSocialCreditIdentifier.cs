using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Yu.Toolkit
{
    /// <summary>
    /// 登记管理部门代码
    /// </summary>
    public class RegistrationManagementDepartmentCodeDto
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 机构代码列表
        /// </summary>
        public List<OrganizationCodeDto> OrganizationCodes { get; set; }
    }

    /// <summary>
    /// 机构代码
    /// </summary>
    public class OrganizationCodeDto
    {
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 统一的社会信用标识符
    /// </summary>
    public class UnifiedSocialCreditIdentifierDto
    {
        /// <summary>
        ///  登记管理部门名称
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        ///  签发机构名称
        /// </summary>
        public string OrganizationName { get; set; }
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
    /// 订制报错
    /// </summary>
    public class UnifiedSocialCreditIdentifierException : Exception
    {
        public UnifiedSocialCreditIdentifierException(string message) : base(message)
        {
        }
    }

    /// <summary>
    /// 统一的社会信用标识符 
    /// http://c.gb688.cn/bzgk/gb/showGb?type=online&hcno=24691C25985C1073D3A7C85629378AC0
    /// </summary>
    public static class UnifiedSocialCreditIdentifier
    {

        /// <summary>
        /// 代码字符集配置文件地址
        /// </summary>
        public const string CodeCharacterSetJsonFilePath = "YuToolkitStaticFiles/UnifiedSocialCreditIdentifierCodeCharacterSet.json";
        /// <summary>
        /// 登记管理部门代码配置文件地址
        /// </summary>
        public const string RegistrationManagementDepartmentCodeJsonFilePath = "YuToolkitStaticFiles/UnifiedSocialCreditIdentifierRegistrationManagementDepartmentCode.json";

        static readonly string[] _defalutOrganizationCodes = new string[] { "91", "92" };
        static readonly string _defalutRegionCode = "410783";

        static List<RegistrationManagementDepartmentCodeDto> _registrationManagementDepartmentCodeList = null;
        /// <summary>
        /// 登记管理部门代码
        /// </summary>
        public static List<RegistrationManagementDepartmentCodeDto> RegistrationManagementDepartmentCodeList => _registrationManagementDepartmentCodeList ?? GetRegistrationManagementDepartmentCodeList();
        /// <summary>
        /// 获取登记管理部门代码
        /// </summary>
        /// <returns></returns>
        static List<RegistrationManagementDepartmentCodeDto> GetRegistrationManagementDepartmentCodeList()
        {
            _registrationManagementDepartmentCodeList = new List<RegistrationManagementDepartmentCodeDto>();
            var json = File.ReadAllText(RegistrationManagementDepartmentCodeJsonFilePath);
            _registrationManagementDepartmentCodeList = JsonConvert.DeserializeObject<List<RegistrationManagementDepartmentCodeDto>>(json);
            return _registrationManagementDepartmentCodeList;
        }
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
            var sum = str.ToCharArray().Select((c, i) => CodeCharacterSetList.First(cv => cv.CharCode == c.ToString()).CharValue * Math.Pow(3, i) % 31).Sum();
            var cv = 31 - sum % 31;
            return CodeCharacterSetList.First(ccs => ccs.CharValue == cv)?.CharCode;
        }
        /// <summary>
        /// 检测机构代码的合法性
        /// </summary>
        /// <param name="code"></param>
        public static void CheckOrganizationCode(string code)
        {
            if (code.Length != 2) throw new UnifiedSocialCreditIdentifierException("Length must be 2");
            var registrationManagementDepartmentCode = RegistrationManagementDepartmentCodeList.FirstOrDefault(r => r.Code == code[0].ToString());
            if (registrationManagementDepartmentCode == null) throw new UnifiedSocialCreditIdentifierException("The registration management department code does not exist");
            var organizationCode = registrationManagementDepartmentCode.OrganizationCodes.FirstOrDefault(o => o.Code == code[1].ToString());
            if (organizationCode == null) throw new UnifiedSocialCreditIdentifierException("Institution code does not exist");
        }
        /// <summary>
        /// 生成统一的社会信用标识符
        /// </summary>
        /// <param name="organizationCodes">机构代码 2 位</param>
        /// <param name="regionCode">区域代码 6 位</param>
        /// <returns></returns>
        public static string Gengenerate(string[] organizationCodes = null, string regionCode = "")
        {
            if (organizationCodes == null || organizationCodes.Length == 0) organizationCodes = _defalutOrganizationCodes;
            organizationCodes.ToList().ForEach(o => CheckOrganizationCode(o));
            if (string.IsNullOrEmpty(regionCode)) regionCode = _defalutRegionCode;
            // 这里懒得引用地址包啦 仅做了长度校验
            if (regionCode.Length != 6) throw new UnifiedSocialCreditIdentifierException("Check the correctness of the area code");
            string result = "";
            // XX 000000  000000000 0   登记管理部门代码  机构类别代码
            result += organizationCodes[new Random().Next(0, organizationCodes.Length)];
            // 00 XXXXXX 000000000 0   登记管理机关行政区划码
            result += regionCode;
            // 00 000000 XXXXXXXXX 0  主体标识码(组织机构代码) 
            for (int i = 0; i < 9; i++)
                result += CodeCharacterSetList[new Random().Next(0, CodeCharacterSetList.Count)].CharCode;
            // 00 000000 0000000000 X 校验码 
            result += GetCheckCode(result);
            return result;
        }
        /// <summary>
        /// 验证统一的社会信用标识符是否正确
        /// </summary>
        /// <param name="code">统一的社会信用标识符</param>
        public static void Verification(string code)
        {
            if (code.Length != 18) throw new UnifiedSocialCreditIdentifierException("Length must be 18");
            if (!Regex.IsMatch(code, "[0-9A-HJ-NPQRTUWXY]{2}\\d{6}[0-9A-HJ-NPQRTUWXY]{10}")) throw new UnifiedSocialCreditIdentifierException("Does not meet the regular rules");
            if (code[17].ToString() != GetCheckCode(code)) throw new UnifiedSocialCreditIdentifierException("Check code error");
        }
        /// <summary>
        /// 解析内容
        /// </summary>
        /// <param name="code">统一的社会信用标识符</param>
        /// <returns></returns>
        public static UnifiedSocialCreditIdentifierDto Parse(string code)
        {
            CheckOrganizationCode(code.Substring(0,2));
            Verification(code);
            var departmentCode = code[0].ToString();
            var organizationCode = code[1].ToString();
            var regionCode = code.Substring(2, 6);
            var department = RegistrationManagementDepartmentCodeList.FirstOrDefault(a => a.Code == departmentCode);
            var organization = department.OrganizationCodes.FirstOrDefault(a => a.Code == organizationCode);
            return new UnifiedSocialCreditIdentifierDto()
            {
                DepartmentName = department?.Name,
                OrganizationName = organization?.Name,
                ProvinceName = RegionCode.GetProvinceNameByCode(regionCode),
                CityName = RegionCode.GetCityNameByCode(regionCode),
                AreaName = RegionCode.GetAreaNameByCode(regionCode),
            };
        }

    }
}
