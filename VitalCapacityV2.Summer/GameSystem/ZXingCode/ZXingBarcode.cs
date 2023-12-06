using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Common;
using ZXing;
using System.Drawing;
using ZXing.QrCode;

namespace VitalCapacityV2.Summer.GameSystem.ZXingCode
{
    /// <summary>
    /// 条形码
    /// </summary>
    public class ZXingBarcode
    {
        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="barcodeContent">需要生成条码的内容</param>
        /// <param name="barcodeWidth">条码宽度</param>
        /// <param name="barcodeHeight">条码长度</param>
        /// <returns>返回条码图形</returns>
        public static Bitmap GetBarcodeBitmap(string barcodeContent, int barcodeWidth, int barcodeHeight)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.CODE_39;//设置编码格式
            EncodingOptions encodingOptions = new EncodingOptions();
            encodingOptions.Width = barcodeWidth;//设置宽度
            encodingOptions.Height = barcodeHeight;//设置长度
            encodingOptions.Margin = 2;//设置边距
            barcodeWriter.Options = encodingOptions;
            Bitmap bitmap = barcodeWriter.Write(barcodeContent);
            return bitmap;
        }

        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="qrCodeContent">要生成二维码的内容</param>
        /// <param name="qrCodeWidth">二维码宽度</param>
        /// <param name="qrCodeHeight">二维码高度</param>
        /// <returns>返回二维码图片</returns>
        public static Bitmap GetQRCodeBitmap(string qrCodeContent, int qrCodeWidth, int qrCodeHeight)
        {
            BarcodeWriter barcodeWriter = new BarcodeWriter();
            barcodeWriter.Format = BarcodeFormat.QR_CODE;
            QrCodeEncodingOptions qrCodeEncodingOptions = new QrCodeEncodingOptions();
            qrCodeEncodingOptions.DisableECI = true;
            qrCodeEncodingOptions.CharacterSet = "UTF-8";//设置编码
            qrCodeEncodingOptions.Width = qrCodeWidth;//设置二维码宽度
            qrCodeEncodingOptions.Height = qrCodeHeight;//设置二维码高度
            qrCodeEncodingOptions.Margin = 0;//设置二维码边距

            barcodeWriter.Options = qrCodeEncodingOptions;
            Bitmap bitmap = barcodeWriter.Write(qrCodeContent);//写入内容
            return bitmap;
        }
    }
}