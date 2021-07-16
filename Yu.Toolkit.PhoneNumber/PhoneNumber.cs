using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Yu.Toolkit
{
    /// <summary>
    /// 定制报错
    /// </summary>
    public class PhoneNumberException : Exception
    {
        public PhoneNumberException(string message) : base(message)
        {
        }
    }
    /// <summary>
    /// 手机号码
    /// </summary>
    public static class PhoneNumber
    {
        static string _assemblyDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string _staticFilesDirectory => _assemblyDirectory + "/YuToolkitStaticFiles/";
        static string _networkIdentificationNumberUrl = _staticFilesDirectory + "PhoneNumberNetworkIdentificationNumber.txt";

        public static List<string> _networkIdentificationNumber;
        public static List<string> NetworkIdentificationNumber => _networkIdentificationNumber ?? GetNetworkIdentificationNumber();
        static List<string> GetNetworkIdentificationNumber() => _networkIdentificationNumber = File.ReadAllLines(_networkIdentificationNumberUrl).Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();


        static Random R => new Random();


        /// <summary>
        /// 生成手机号码
        /// </summary>  
        /// <returns></returns>
        public static string Gengenerate()
        {
            var pn = NetworkIdentificationNumber[R.Next(0, NetworkIdentificationNumber.Count)];
            for (int i = 3; i < 11; i++) pn += R.Next(0, 10).ToString();
            return pn;
        }
        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="code"></param>
        public static void Verification(string code)
        {
            if (!Regex.IsMatch(code, "^([0-9]){11}$")) throw new PhoneNumberException("Does not meet the regular rules");
            if (!NetworkIdentificationNumber.Contains(code.Substring(0, 3))) throw new PhoneNumberException("Incorrect network identification number");
        }





    }
}
