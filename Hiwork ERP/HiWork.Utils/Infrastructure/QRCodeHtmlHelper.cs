using System;
using System.ComponentModel;
using System.Net;
using System.Web;

namespace HiWork.Utils.Infrastructure
{
    public static class QRCodeHtmlHelper
    {
        /// <summary>
        /// Produces the markup for an image element that displays a QR Code image, as provided by Google's chart API.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="data">The data to be encoded, as a string.</param>
        /// <param name="size">The square length of the resulting image, in pixels.</param>
        /// <param name="margin">The width of the border that surrounds the image, measured in rows (not pixels).</param>
        /// <param name="errorCorrectionLevel">The amount of error correction to build into the image. Higher error correction comes at the expense of reduced space for data.</param>
        /// <param name="htmlAttributes">Optional HTML attributes to include on the image element.</param>
        /// <returns></returns>
        public static string QRCode(string data, int size = 100, int margin = 4, QRCodeErrorCorrectionLevel errorCorrectionLevel = QRCodeErrorCorrectionLevel.Low, object htmlAttributes = null)
        {
            if (data == null)
                throw new ArgumentNullException("data"); if (size < 1)
                throw new ArgumentOutOfRangeException("size", size, "Must be greater than zero.");
            if (margin < 0)
                throw new ArgumentOutOfRangeException("margin", margin, "Must be greater than or equal to zero.");
            if (!Enum.IsDefined(typeof(QRCodeErrorCorrectionLevel), errorCorrectionLevel))
                throw new InvalidEnumArgumentException("errorCorrectionLevel", (int)errorCorrectionLevel, typeof(QRCodeErrorCorrectionLevel));

            var url = string.Format("http://chart.apis.google.com/chart?cht=qr&chld={2}|{3}&chs={0}x{0}&chl={1}", size, HttpUtility.UrlEncode(data), errorCorrectionLevel.ToString()[0], margin);

            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(url);
            string base64String = Convert.ToBase64String(imageBytes);
            //string base64String = "image/jpeg;base64," + Convert.ToBase64String(imageBytes);
            //webClient.DownloadFile(url, @"C:\someImage.jpg");
            return base64String;
        }
    }

    public enum QRCodeErrorCorrectionLevel
    {
        /// <summary>Recovers from up to 7% erroneous data.</summary>
        Low,
        /// <summary>Recovers from up to 15% erroneous data.</summary>
        Medium,
        /// <summary>Recovers from up to 25% erroneous data.</summary>
        QuiteGood,
        /// <summary>Recovers from up to 30% erroneous data.</summary>
        High
    }
}