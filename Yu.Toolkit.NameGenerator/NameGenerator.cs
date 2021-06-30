using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Yu.Toolkit
{
    /// <summary>
    /// 性别
    /// </summary>
    [Flags]
    public enum NameGeneratorGenderEnum
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
    ///  随机类型
    /// </summary>
    internal enum NameGeneratorTypeEnum
    {
        /// <summary>
        /// 双字
        /// </summary>
        Double = 0,
        /// <summary>
        /// 单字
        /// </summary>
        Single = 1,
        /// <summary>
        /// 单双字
        /// </summary>
        SingleDouble = 2
    }

    /// <summary>
    /// 姓名生成器
    /// </summary>
    public static class NameGenerator
    {

        static string _assemblyDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        static string _staticFilesDirectory => _assemblyDirectory + "/YuToolkitStaticFiles/";
        static string _familyNameFileUrl = _staticFilesDirectory + "NameGeneratorFamilyName.txt";
        static string _maleNameFileUrl = _staticFilesDirectory + "NameGeneratorMaleName.txt";
        static string _femaleNameFileUrl = _staticFilesDirectory + "NameGeneratorFemaleName.txt";
        static string _maleDoubleFileUrl = _staticFilesDirectory + "NameGeneratorMaleDouble.txt";
        static string _femaleDoubleFileUrl = _staticFilesDirectory + "NameGeneratorFemaleDouble.txt";

        static List<string> _familyName;
        static List<string> _maleName;
        static List<string> _femaleName;
        static List<string> _maleDouble;
        static List<string> _femaleDouble;

        public static List<string> FamilyName => _familyName ?? GetFamilyName();
        public static List<string> MaleName => _maleName ?? GetMaleName();
        public static List<string> FemaleName => _femaleName ?? GetFemaleName();
        public static List<string> MaleDouble => _maleDouble ?? GetMaleDouble();
        public static List<string> FemaleDouble => _femaleDouble ?? GetFemaleDouble();

        static List<string> GetFamilyName() => _familyName = File.ReadAllLines(_familyNameFileUrl).Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        static List<string> GetMaleName() => _maleName = File.ReadAllLines(_maleNameFileUrl).Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        static List<string> GetFemaleName() => _femaleName = File.ReadAllLines(_femaleNameFileUrl).Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        static List<string> GetMaleDouble() => _maleDouble = File.ReadAllLines(_maleDoubleFileUrl).Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();
        static List<string> GetFemaleDouble() => _femaleDouble = File.ReadAllLines(_femaleDoubleFileUrl).Select(a => a.Trim()).Where(a => !string.IsNullOrWhiteSpace(a)).ToList();

        static Random _random => new Random();

        /// <summary>
        /// 生成名字
        /// </summary>
        /// <param name="gender">性别 默认两性</param>
        /// <returns></returns>
        public static string Generate(NameGeneratorGenderEnum gender = NameGeneratorGenderEnum.Male | NameGeneratorGenderEnum.Female)
        {
            if (gender == NameGeneratorGenderEnum.Male) return GenerateMale();
            if (gender == NameGeneratorGenderEnum.Female) return GenerateFemale();
            return _random.Next(0, 2) == (int)NameGeneratorGenderEnum.Male ? GenerateMale() : GenerateFemale();
        }

        /// <summary>
        /// 生成男性名字
        /// </summary>
        /// <returns></returns>
        static string GenerateMale()
        {
            int type = _random.Next(0, 3);
            string name = type == (int)NameGeneratorTypeEnum.Double ? MaleDouble[_random.Next(0, MaleDouble.Count)] : type == (int)NameGeneratorTypeEnum.Single ? MaleName[_random.Next(0, MaleName.Count)] : MaleName[_random.Next(0, MaleName.Count)] + MaleName[_random.Next(0, MaleName.Count)];
            return $"{FamilyName[_random.Next(0, FamilyName.Count)]}{name}";
        }
        /// <summary>
        /// 生成女性名称
        /// </summary>
        /// <returns></returns>
        static string GenerateFemale()
        {
            int type = _random.Next(0, 3);
            string name = type == (int)NameGeneratorTypeEnum.Double ? FemaleDouble[_random.Next(0, FemaleDouble.Count)] : type == (int)NameGeneratorTypeEnum.Single ? FemaleName[_random.Next(0, FemaleName.Count)] : FemaleName[_random.Next(0, FemaleName.Count)] + FemaleName[_random.Next(0, FemaleName.Count)];
            return $"{FamilyName[_random.Next(0, FamilyName.Count)]}{name}";
        }
    }
}