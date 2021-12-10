using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        static string _assemblyDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string _staticFilesDirectory => _assemblyDirectory + "/YuToolkitStaticFiles/";
        static string _codeCharacterSetJsonFilePath = _staticFilesDirectory + "UnifiedSocialCreditIdentifierCodeCharacterSet.json";
        static string _registrationManagementDepartmentCodeJsonFilePath = _staticFilesDirectory + "UnifiedSocialCreditIdentifierRegistrationManagementDepartmentCode.json";

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
            var json = File.ReadAllText(_registrationManagementDepartmentCodeJsonFilePath);
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
            var json = File.ReadAllText(_codeCharacterSetJsonFilePath);
            _codeCharacterSetList = JsonConvert.DeserializeObject<List<CodeCharacterSetDto>>(json);
            return _codeCharacterSetList;
        }


        /// <summary>
        /// 获取组织机构校验码
        /// </summary>
        /// <param name="oibcBefore">组织机构代码前半段</param>
        /// <returns></returns>
        public static string GetOIBCCheckCode(string oibcBefore)
        {
            /** 因数 */
            var factor = new List<int> { 3, 7, 9, 10, 5, 8, 4, 2 };
            /** 校验码 */
            var c9 = 0;
            for (var i = 0; i < factor.Count; i++)
            {
                //把字符串中的字符一个一个的解码
                var tmp = (byte)oibcBefore[i];
                if (tmp >= 48 && tmp <= 57) tmp -= 48;
                else if (tmp >= 65 && tmp <= 90) tmp -= 55;
                //乘权重后加总
                c9 += factor[i] * tmp;
            }
            c9 = 11 - (c9 % 11);
            return c9 == 11 ? "0" : c9 == 10 ? "X" : (c9 + "");
        }
        /// <summary>
        /// 获取校验码
        /// </summary>
        /// <param name="str">前17位</param>
        /// <returns></returns>
        public static string GetCheckCode(string str)
        {
            str = str.Substring(0, 17);
            var sum = str.ToCharArray().Select((c, i) => CodeCharacterSetList.First(cv => cv.CharCode == c.ToString()).CharValue * Math.Pow(3, i) % 31).Sum() % 31;
            var cv = sum == 0 ? 0 : 31 - sum;
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
        /// 检查组织机构代码合法性
        /// </summary>
        /// <param name="usci">统一社会信用代码</param>
        public static void CheckOrganizingInstitutionBarCode(string usci)
        {
            /** 组织机构代码 */
            var organizingInstitutionBarCode = usci.Substring(8, 8) + '-' + usci.Substring(16, 1);
            
            /** 行政区域代码校验不合格 */
            if (!Regex.IsMatch(organizingInstitutionBarCode, "^([0-9A-Z]{8}\\-[\\d{1}|X])$")) throw new UnifiedSocialCreditIdentifierException("Does not meet the regular rules");
            /** 组织机构代码前部分 */
            var oibcBefore = usci.Substring(8, 8);
            /** 组织机构代码后部分 */
            var oibcAfter = usci.Substring(16, 1);
            // 组织机构代码验证
            if (GetOIBCCheckCode(oibcBefore) != oibcAfter) throw new UnifiedSocialCreditIdentifierException("Check code error");
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
            // 00 000000 XXXXXXXX0 0  主体标识码(组织机构代码)  
            for (int i = 0; i < 8; i++)
                result += CodeCharacterSetList[new Random().Next(0, CodeCharacterSetList.Count)].CharCode;
            // 00 000000 00000000X 0   组织机构代码校验码
            result += GetOIBCCheckCode(result.Substring(8, 8));
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
            CheckOrganizingInstitutionBarCode(code);
            if (code[17].ToString() != GetCheckCode(code)) throw new UnifiedSocialCreditIdentifierException("Check code error");
        }


        /// <summary>
        /// 解析内容
        /// </summary>
        /// <param name="code">统一的社会信用标识符</param>
        /// <returns></returns>
        public static UnifiedSocialCreditIdentifierDto Parse(string code)
        {
            CheckOrganizationCode(code.Substring(0, 2));
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
