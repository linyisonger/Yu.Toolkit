using System;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;

namespace Yu.Toolkit
{
    /// <summary>
    /// 身份证图像
    /// 仅用于学习与测试
    /// </summary>
    public static class CitizenIdentificationImage
    {
        static readonly string frontTemplateImagePath = "YuToolkitStaticFiles/CitizenIdentificationImageFrontTemplateImage.jfif";
        static readonly string frontTemplateAvatarPhotoPath = "YuToolkitStaticFiles/CitizenIdentificationImageFrontTemplateAvatarPhoto.png";
        static readonly string backTemplateImagePath = "YuToolkitStaticFiles/CitizenIdentificationImageBackTemplateImage.jfif";

        static FontFamily _ocr = null;
        /// <summary>
        /// 格式居中
        /// </summary>
        static StringFormat StringFormatCenter => new StringFormat()
        {
            // 居中
            Alignment = StringAlignment.Center
        };
        /// <summary>
        /// 黑色笔刷
        /// </summary>
        static SolidBrush SolidBrushBlack => new SolidBrush(Color.Black);
        /// <summary>
        /// 字体
        /// </summary>
        static FontFamily Ocr => _ocr ?? GetOcr();
        static FontFamily Black => new FontFamily("黑体");
        static FontFamily WinBlack => new FontFamily("微软雅黑");
        static FontFamily GetOcr()
        {
            var font = new PrivateFontCollection();
            font.AddFontFile("YuToolkitStaticFiles/OCR-B 10 BT.ttf");
            _ocr = new FontFamily(font.Families[0].Name, font);
            return _ocr;
        }
        /// <summary>
        /// 渲染字体
        /// </summary>
        /// <param name="gp">字体</param>
        /// <param name="str">地址</param>
        /// <param name="x">位置</param>
        /// <param name="y">位置</param>
        /// <param name="characterHeight">字符高度</param>
        /// <param name="characterWidth">字符宽度</param>
        /// <param name="maximumNumberOfWords">一行最大字数</param>
        /// <param name="lineHeight">行高</param>
        /// <param name="wordSpacing">字间距</param>
        /// <param name="fontStyle">字体样式</param>
        static void DrawString(this Graphics gp, string str, int x, int y, int characterHeight, int characterWidth, int maximumNumberOfWords, int lineHeight, float wordSpacing, FontStyle fontStyle, FontFamily familyName)
        {
            // 每个字符范围
            var rectangleF = new RectangleF(x, y, characterWidth, characterHeight);
            for (int i = 0; i < str.Length; i++)
            {
                gp.DrawString(str[i].ToString(), new Font(familyName, characterWidth, fontStyle), SolidBrushBlack, rectangleF, StringFormatCenter);
                rectangleF.X += characterWidth + wordSpacing;
                if (i % maximumNumberOfWords == 0 && i != 0)
                {
                    rectangleF.X = x;
                    rectangleF.Y += lineHeight;
                }
            }
        }
        /// <summary>
        /// 删除白色背景
        /// </summary>
        /// <param name="im">图片</param>
        /// <returns></returns>
        static Image RemoveWhiteBackground(this Image im)
        {
            var bm = new Bitmap(im);
            for (int i = 0; i < bm.Width; i++)
                for (int j = 0; j < bm.Height; j++)
                {
                    var pixel = bm.GetPixel(i, j);
                    if (pixel.R > 245 && pixel.G > 245 && pixel.B > 245)
                        bm.SetPixel(i, j, Color.FromArgb(0, pixel));
                }
            return bm;
        }
        /// <summary>
        /// 生成正面
        /// </summary>
        /// <param name="issuingAuthority">签发机关    例:郑州市公安局 </param>
        /// <param name="validPeriod">有效期限   例:2021.05.21-2041.05.21/长期</param>
        /// <returns></returns>
        public static Bitmap GengenerateFront(string issuingAuthority, string validPeriod = "")
        {
            Bitmap bm = new Bitmap(frontTemplateImagePath);
            Graphics gp = Graphics.FromImage(bm);
            gp.DrawString(issuingAuthority, new Font(Black, 13, FontStyle.Regular), SolidBrushBlack, 130, 153);
            gp.DrawString(validPeriod, new Font(WinBlack, 11, FontStyle.Regular), SolidBrushBlack, 130, 178);
            return bm;
        }
        /// <summary>
        /// 生成背面
        /// </summary>
        /// <param name="name">姓名 例:林一怂儿</param>
        /// <param name="ethnic">民族 例:汉</param>
        /// <param name="address">住址 例:杭州市西湖区莫干山路200号</param>
        /// <param name="citizenIdentificationNumber">身份证号码  例:410783199909099994</param>
        /// <param name="avatarPhoto">照片  像素尺寸358*441白底</param>
        /// <returns></returns>
        public static Bitmap GengenerateBack(string name, string ethnic, string address, string citizenIdentificationNumber, Image avatarPhoto = null)
        {
            var citizenIdentificationNumberDto = CitizenIdentificationNumber.Parse(citizenIdentificationNumber);
            var year = citizenIdentificationNumberDto.Birthday.Year.ToString();
            var month = citizenIdentificationNumberDto.Birthday.Month.ToString();
            var day = citizenIdentificationNumberDto.Birthday.Day.ToString();
            var gender = citizenIdentificationNumberDto.Gender;
            Bitmap bm = new Bitmap(backTemplateImagePath);
            Graphics gp = Graphics.FromImage(bm);
            gp.DrawString(name, new Font(Black, 13, FontStyle.Regular, GraphicsUnit.Point), SolidBrushBlack, 63, 31.3f);
            gp.DrawString(gender, new Font(Black, 11, FontStyle.Regular), SolidBrushBlack, 63, 58.3f);
            gp.DrawString(ethnic, new Font(Black, 11, FontStyle.Regular), SolidBrushBlack, 130f, 58.3f);
            gp.DrawString(year, new Font(WinBlack, 11, FontStyle.Regular), SolidBrushBlack, 62.5f, 81.3f);
            gp.DrawString(month, new Font(WinBlack, 11, FontStyle.Regular), SolidBrushBlack, 121f, 81.3f, StringFormatCenter);
            gp.DrawString(day, new Font(WinBlack, 11, FontStyle.Regular), SolidBrushBlack, 149.5f, 81.3f, StringFormatCenter);
            gp.DrawString(address, 63, 110, 14, 11, 10, 18, 1.1f, FontStyle.Regular, Black);
            gp.DrawString(citizenIdentificationNumber, 110, 176, 18, 14, 20, 18, -4f, FontStyle.Regular, Ocr);
            gp.DrawImage(avatarPhoto ?? Image.FromFile(frontTemplateAvatarPhotoPath).RemoveWhiteBackground(), 199, 20, 358 / 3.2f, 441 / 3.2f);
            return bm;
        }

    }
}
