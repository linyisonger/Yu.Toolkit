using System;
using System.Text.RegularExpressions;

namespace Yu.Toolkit
{

    /// <summary>
    /// 定制报错
    /// </summary>
    public class BankCardNumberException : Exception
    {
        public BankCardNumberException(string message) : base(message)
        {
        }
    }
    /// <summary>
    /// 银行卡号
    /// </summary>
    public static class BankCardNumber
    {

        /// <summary>
        /// 获取校验码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetCheckCode(string str)
        {
            int sum = 0;
            for (int i = 0; i < str.Length; i++)
            {
                int n = int.Parse(str[i] + "");
                sum += (str.Length - 1 - i) % 2 == 0 ? (n * 2) / 10 + (n * 2) % 10 : n;
            }
            return 10 - sum % 10 + "";
        }

        /// <summary>
        /// 生成银行卡号
        /// </summary> 
        /// <param name="bin">BIN 6 位</param>
        /// <param name="length">总卡号长度 13-19 </param>
        /// <returns></returns>
        public static string Gengenerate(string bin = "621210", int length = 18)
        {
            var result = bin;
            var random = new Random();
            for (int i = 0; i < length - bin.Length - 1; i++) result += random.Next(0, 10);
            return result + GetCheckCode(result);
        }
        /// <summary>
        /// 验证银行卡号
        /// </summary>
        /// <param name="code"></param>
        public static void Verification(string code)
        {
            if (!Regex.IsMatch(code, "^([0-9]){13,19}$")) throw new BankCardNumberException("Does not meet the regular rules");
            if (code[^1].ToString() != GetCheckCode(code[0..^1])) throw new BankCardNumberException("Check code error");
        }
    }
}
