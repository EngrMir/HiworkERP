using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Web;
using System.Data;
using System.Drawing.Imaging;
using System.Globalization;
using HiWork.Utils.Infrastructure;

namespace HiWork.Utils
{
    public class Utility
    {

        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        //#region Filtering & Sorting
        //public List<T> FilterData<T>(List<T> data, List<Filter> filters)
        //{
        //    List<T> filteredData = new List<T>();
        //    if (filters != null && filters.Count > 0)
        //    {
        //        foreach (var f in filters)
        //        {
        //            if (f.value != null)
        //            {
        //                if (f.condition == "match")
        //                {
        //                    var v = Convert.ToString(f.value);
        //                    filteredData = (from n in data
        //                                    where GetDynamicSortProperty(n, f.model) == v
        //                                    select n).ToList();
        //                }
        //                if (f.condition == "like")
        //                {
        //                    var v = Convert.ToString(f.value);
        //                    filteredData = (from n in data
        //                                    where (GetDynamicSortProperty(n, f.model) as string).Contains(v)
        //                                    select n).ToList();
        //                }
        //                if (f.condition == "less")
        //                {
        //                    var v = Convert.ToInt64(f.value);
        //                    filteredData = (from n in data
        //                                    where Convert.ToInt64((GetDynamicSortProperty(n, "ID"))) < v
        //                                    select n).ToList();
        //                }
        //                if (f.condition == "more")
        //                {
        //                    var v = Convert.ToInt64(f.value);
        //                    filteredData = (from n in data
        //                                    where Convert.ToInt64((GetDynamicSortProperty(n, "ID"))) > v
        //                                    select n).ToList();
        //                }
        //                if (f.condition == "equal")
        //                {
        //                    var v = Convert.ToInt64(f.value);
        //                    filteredData = (from n in data
        //                                    where Convert.ToInt64((GetDynamicSortProperty(n, "ID"))) == v
        //                                    select n).ToList();
        //                }
        //                if (f.condition == "between")
        //                {
        //                    var o = JObject.Parse(f.value.ToString());
        //                    var m = Convert.ToInt64(o.Property("more").Value.ToString());
        //                    var l = Convert.ToInt64(o.Property("less").Value.ToString());
        //                    filteredData = (from n in data
        //                                    where (Convert.ToInt64((GetDynamicSortProperty(n, "ID"))) > m && Convert.ToInt64((GetDynamicSortProperty(n, "ID"))) < l)
        //                                    select n).ToList();
        //                }

        //            }
        //        }
        //    }
        //    return filteredData;
        //}
        //public static void ClearFolder(string FolderName)
        //{
        //    DirectoryInfo dir = new DirectoryInfo(FolderName);

        //    foreach (FileInfo fi in dir.GetFiles())
        //    {
        //        fi.Delete();
        //    }

        //    foreach (DirectoryInfo di in dir.GetDirectories())
        //    {
        //        ClearFolder(di.FullName);
        //        di.Delete();
        //    }
        //}
        //public List<T> Sort_List<T>(string sortDirection, string sortExpression, List<T> data)
        //{

        //    List<T> data_sorted = new List<T>();

        //    if (sortDirection == "ASC")
        //    {
        //        data_sorted = (from n in data
        //                       orderby GetDynamicSortProperty(n, sortExpression) ascending
        //                       select n).ToList();
        //    }
        //    else if (sortDirection == "DESC")
        //    {
        //        data_sorted = (from n in data
        //                       orderby GetDynamicSortProperty(n, sortExpression) descending
        //                       select n).ToList();

        //    }

        //    return data_sorted;

        //}
        public static void SetDynamicPropertyValue(object item,string culture)
        {
            
            //Use reflection to get property
            PropertyInfo property;
            PropertyInfo[] properties = item.GetType().GetProperties();

            properties.ToList().ForEach(prop =>
           {
                if (null != prop && prop.CanWrite)
                {
                    if (prop.Name.Contains("Name") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("Name"); 
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                    if (prop.Name.Contains("Description") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("Description");
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                    if (prop.Name.Contains("Address") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("Address");
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                    if (prop.Name.Contains("AccountName") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("AccountName");
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                    if (prop.Name.Contains("Note") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("Note");
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                    if (prop.Name.Contains("Title") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("Title");
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                    if (prop.Name.Contains("HomeAddress") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("HomeAddress");
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                    if (prop.Name.Contains("SelfIntroduction") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("SelfIntroduction");
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                    if (prop.Name.Contains("Comment") && prop.Name.Contains("_" + culture))
                    {
                        property = item.GetType().GetProperty("Comment");
                        var value = property != null ? property.GetValue(item, null) : string.Empty;
                        prop.SetValue(item, value, null);
                    }
                   if (prop.Name.Contains("DeliveryTypeName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("DeliveryTypeName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("Address1") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("Address1");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("Address2") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("Address2");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("FirstName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("FirstName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("LastName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("LastName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("MiddleName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("MiddleName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("CityOfOverseas") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("CityOfOverseas");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }                  
                   if (prop.Name.Contains("ApartmentName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("ApartmentName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("HomeCountryAddress") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("HomeCountryAddress");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("Street") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("Street");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("MainCareer") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("MainCareer");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("SelfPR") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("SelfPR");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
   
                   if (prop.Name.Contains("CityOfOverseas") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("CityOfOverseas");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }

                   if (prop.Name.Contains("Street") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("Street");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("HomeCountryAddress") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("HomeCountryAddress");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("TownName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("TownName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                 if (prop.Name.Contains("CompanyName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("CompanyName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("CEOName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("CEOName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                if (prop.Name.Contains("ServiceName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("ServiceName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("InvoiceCompanyName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("InvoiceCompanyName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("InvoiceAddress1") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("InvoiceAddress1");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("InvoiceAddress2") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("InvoiceAddress2");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                  if (prop.Name.Contains("InchagreName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("InchagreName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                  if (prop.Name.Contains("AddressedPersonName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("AddressedPersonName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("AccountHolderName") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("AccountHolderName");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("Item") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("Item");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("Contents") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("Contents");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("Contents") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("Contents");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }
                   if (prop.Name.Contains("Response") && prop.Name.Contains("_" + culture))
                   {
                       property = item.GetType().GetProperty("Response");
                       var value = property != null ? property.GetValue(item, null) : string.Empty;
                       prop.SetValue(item, value, null);
                   }

               }
           });
          
        }

        public static object GetPropertyValue(object item,string property, string culture)
        {
            //Use reflection to get property value
            PropertyInfo[] properties = item.GetType().GetProperties();
            object objValue = null;
            List<PropertyInfo> list = properties.ToList();
            foreach(PropertyInfo s in list)
            {
                if (s.Name.Contains(property) && s.Name.Contains("_" + culture))
                {
                    objValue = item.GetType().GetProperty(s.Name).GetValue(item, null);
                }
            }
            //foreach (var s in properties)
            //{
            //    if(s.Name.Contains(property) && s.Name.Contains("_" + culture)){
            //        objValue = item.GetType().GetProperty(s.Name).GetValue(item, null);
            //    }
            //}
            return objValue;
        }

       
        //#endregion

        #region Encryption / Decryption Functions
        public static string EncryptText(string strText)
        {
            if (strText != null)
            {
                return Encrypt(strText, "&#?:*%@,");
            }
            else
            {
                return null;
            }
        }

        public static string DecryptCypher(string strText)
        {
            if (strText != null)
            {
                return Decrypt(strText, "&#?:*%@,");
            }
            else
            {
                return null;
            }
        }

        public static string Encrypt(string strText, string strEncrKey)
        {
            //------------------------------------------------------------------------
            //Encryption algorithm code
            //------------------------------------------------------------------------
            byte[] byKey = {

            };
                    byte[] IV = {
                0x12,
                0x34,
                0x56,
                0x78,
                0x90,
                0xab,
                0xcd,
                0xef
            };

            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(Microsoft.VisualBasic.Strings.Left(strEncrKey, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string Decrypt(string strText, string sDecrKey)
        {
            //------------------------------------------------------------------------
            //Decryption algorithm code
            //------------------------------------------------------------------------
            byte[] byKey = {

    };
            byte[] IV = {
        0x12,
        0x34,
        0x56,
        0x78,
        0x90,
        0xab,
        0xcd,
        0xef
    };
            byte[] inputByteArray = new byte[strText.Length + 1];

            strText = Microsoft.VisualBasic.Strings.Replace(strText, " ", "+");

            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(Microsoft.VisualBasic.Strings.Left(sDecrKey, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;

                return encoding.GetString(ms.ToArray());

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        #endregion
        public static string MD5(string input)
        {
            string hashed;
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            hashed = s.ToString();
            return hashed;
        }

        public static string buildQueryString(Dictionary<string, string> queryDictionary)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var item in queryDictionary)
            {
                query[item.Key] = item.Value;
            }
            return query.ToString();
        }

        public static string prepareURL(string baseURL, string ActionName, Dictionary<string, string> queryDictionary = null)
        {
            var builder = new UriBuilder(baseURL + ActionName);
            if (queryDictionary != null)
            {
                builder.Query = buildQueryString(queryDictionary);
            }
            return builder.ToString();
        }

        public static string EncrytionStringOld(string input)
        {
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);

            string encrptedString = Convert.ToBase64String(bs);

            return encrptedString;
        }

        public static string DecrytionStringOld(string encryptedInput)
        {
            string decrptedString;
            MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();

            byte[] bytes = Convert.FromBase64String(encryptedInput);
            decrptedString = System.Text.Encoding.UTF8.GetString(bytes);

            return decrptedString;
        }


        public static string EncrytionString(string input)
        {
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            byte[] iputBytes = UTF8Encoding.UTF8.GetBytes(input);

            string encryptedString = string.Empty;
            using (AesManaged aes = new AesManaged())
            {
                InitializeAes(saltBytes, aes);

                using (ICryptoTransform encryptTransform = aes.CreateEncryptor())
                {
                    encryptedString = Convert.ToBase64String(Crypted(iputBytes, encryptTransform));
                }

            }

            return encryptedString;
        }

        public static string DecrytionString(string encryptedInput)
        {
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            byte[] iputBytes = Convert.FromBase64String(encryptedInput);

            string decryptedString = string.Empty;

            using (AesManaged aes = new AesManaged())
            {
                InitializeAes(saltBytes, aes);

                using (ICryptoTransform decryptTransform = aes.CreateDecryptor())
                {
                    byte[] decryptBytes = Crypted(iputBytes, decryptTransform);
                    decryptedString = UTF8Encoding.UTF8.GetString(decryptBytes, 0, decryptBytes.Length);
                }
            }
            return decryptedString;
        }

        private static byte[] Crypted(byte[] iputBytes, ICryptoTransform encryptTransform)
        {
            using (MemoryStream encryptedStream = new MemoryStream())
            {
                using (CryptoStream encryptor =
                new CryptoStream(encryptedStream, encryptTransform, CryptoStreamMode.Write))
                {
                    encryptor.Write(iputBytes, 0, iputBytes.Length);
                    encryptor.Flush();
                    encryptor.Close();
                    byte[] encryptBytes = encryptedStream.ToArray();
                    return encryptBytes;
                }

            }
        }


        private static void InitializeAes(byte[] saltBytes, AesManaged aes)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes("1357", saltBytes);
            aes.BlockSize = aes.LegalBlockSizes[0].MaxSize;
            aes.KeySize = aes.LegalKeySizes[0].MaxSize;
            aes.Key = rfc.GetBytes(aes.KeySize / 8);
            aes.IV = rfc.GetBytes(aes.BlockSize / 8);
        }


        public static Guid Int2Guid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static int Guid2Int(Guid value)
        {
            byte[] b = value.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }

        public static string ImageToBase64(string imagePath)
        {
            try
            {
                using (Image image = Image.FromFile(imagePath))
                {
                    using (MemoryStream m = new MemoryStream())
                    {
                        image.Save(m, image.RawFormat);
                        byte[] imageBytes = m.ToArray();

                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static Image ScaleImage(Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        public static bool Base64ToImage(string base64String, string path)
        {
            try
            {
                //base64String = base64String.Replace("\r\n  ", "");
                int base64StringLength = base64String.Length;
                byte[] imageBytes = Convert.FromBase64String(base64String);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                Image image = Image.FromStream(ms, true);
                image = ScaleImage(image, 200, 400);
                image.Save(path);

                return true;
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
            }
        }

        public static string GenerateUserVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public static string GenerateMerchantSecurityKey()
        {
            int length = 9;
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string specialCharacters = "!@#$%*_+?:";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            result.Insert(random.Next(9), specialCharacters[random.Next(specialCharacters.Length)]);
            return result.ToString();
        }

        public static string GenerateInstitutionUserSecurityKey()
        {
            int length = 7;
            Random random = new Random();
            string characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string specialCharacters = "!@#$%*_+?:";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }

            result.Insert(random.Next(7), specialCharacters[random.Next(specialCharacters.Length)]);
            return result.ToString();
        }

        public static string GenerateMerchantCategoryCode()
        {
            lock (syncLock)
            {
                var randomPINResult = random.Next(0, 9999).ToString();
                return randomPINResult.PadLeft(4, '0');
            }
        }
        public static string GenerateMerchantCode()
        {
            lock (syncLock)
            {
                var randomPINResult = random.Next(0, 9999).ToString();
                return randomPINResult.PadLeft(4, '0');
            }
        }

        public static string GenerateTransactionId(string transactionType, string stan)
        {
            lock (syncLock)
            {
                char padLeft = Convert.ToChar(random.Next(1, 8).ToString());
                return transactionType + stan + random.Next(1, 99999999).ToString().PadLeft(8, padLeft);
            }
        }

        public static string GetRetrievalReferenceNumber(DateTime transactionDateTime, string stan)
        {
            var sbTransactionId = new StringBuilder();
            sbTransactionId.AppendFormat("{0}{1}{2}", transactionDateTime.ToString("yy").Last(), transactionDateTime.DayOfYear, transactionDateTime.ToString("HH"));
            sbTransactionId.Append(stan);
            return sbTransactionId.ToString();
        }

        //public static string GenerateEmailVerificationCode()
        //{
        //    lock (syncLock)
        //    {
        //        var randomPINResult = random.Next(0, 999999).ToString();
        //        return randomPINResult.PadLeft(6, '0');
        //    }
        //}

        //public static string GenerateReferralCode()
        //{
        //    lock (syncLock)
        //    {
        //        var randomPINResult = random.Next(0, 999999).ToString();
        //        return randomPINResult.PadLeft(6, '0');
        //    }
        //}

        //public static string GenerateMobileNumberVerificationCode()
        //{
        //    lock (syncLock)
        //    {
        //        var randomPINResult = random.Next(0, 999999).ToString();
        //        return randomPINResult.PadLeft(6, '0');
        //    }
        //}

        public static string GenerateOTP(int length)
        {
            lock (syncLock)
            {
                string _numbers = "0123456789";
                Random random = new Random();
                StringBuilder builder = new StringBuilder(6);

                for (var i = 0; i < length; i++)
                {
                    builder.Append(_numbers[random.Next(0, _numbers.Length)]);
                }
                return builder.ToString();

                //var randomPINResult = random.Next(0, 9999).ToString();
                //return randomPINResult.PadLeft(4, '0');
            }
        }

        public static string GenerateCardNumber()
        {
            int length = 12;
            Random random = new Random();
            string characters = "0123456789";
            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                result.Append(characters[random.Next(characters.Length)]);
            }
            return result.ToString();
        }

        public static DateTime PasswordChangeVerificationCodeExpiryDate()
        {
            return DateTime.UtcNow.AddMinutes(30);
        }

        public static string GenerateDNCPassword()
        {
            return "123456";
        }

        public static string GenerateETHPassword(string account)
        {
            return "123456";
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            string Encode;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                {
                    //  return encoders[j];
                    Encode = Convert.ToString(encoders[j]);
                    return encoders[j];
                }
            }
            return null;
        }

        public static string CreateDirectory(string UploadPath)
        {
            string status = "Success";


            var fileInfo = new System.IO.FileInfo(UploadPath);
            if (!fileInfo.Directory.Exists)
            {
                fileInfo.Directory.Create();
            }
            return status;
        }

        public string GetImageFile(string Path)
        {

            var val = System.Drawing.Image.FromFile(Path);

            return Convert.ToString(val);
        }

        public static void ClearFolder(string FolderName)
        {
            DirectoryInfo dir = new DirectoryInfo(FolderName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                ClearFolder(di.FullName);
                di.Delete();
            }
        }
        public static DateTime ConvertToUtc(DateTime dateTime)
        {
            switch (dateTime.Kind)
            {
                case DateTimeKind.Unspecified:
                    return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
                case DateTimeKind.Local:
                    return dateTime.ToUniversalTime();
                default:
                    return dateTime;
            }
        }

   

        public static List<SelectedItem> RegPurposeType = new List<SelectedItem>
            {
            new SelectedItem() {Id=1,Name_en="registered by client",Name_cn="由客户注册",Name_fr="Enregistré par le client",Name_jp="クライアント登録",Name_kr="고객이 등록한",Name_tl="ลงทะเบียนโดยลูกค้า" },
            new SelectedItem() {Id=2,Name_en="registered by b-cause (for record)",Name_cn="由原因登记（备案）",Name_fr="Enregistré par b-cause (pour enregistrement)",Name_jp="b-causeによって登録された（記録用）",Name_kr="b-cause에 의해 등록 (기록 용)" ,Name_tl="จดทะเบียนโดย b-cause (สำหรับบันทึก)"},
            new SelectedItem() {Id=3,Name_en="registered by b-cause (for sales)", Name_cn="由b-cause（销售）注册", Name_fr="Enregistré par b-cause (pour les ventes)", Name_jp="b-causeによって登録された（販売用）", Name_kr="b-cause에 의해 등록 (판매용)",Name_tl="จดทะเบียนโดย b-cause (สำหรับการขาย)" }
            };

        public static List<SelectedItem> ClientLocationType = new List<SelectedItem>
            {
            new SelectedItem() {Id=1,Name_en="Local", Name_cn="本地",Name_fr="Local",Name_jp="地元", Name_kr="노동 조합 지부", Name_tl="ในประเทศ" },
            new SelectedItem() {Id=2,Name_en="overseas", Name_cn="海外", Name_fr="étranger", Name_jp="海外",Name_kr="해외", Name_tl="ต่างประเทศ" }
            };

        public static List<SelectedItem> CompanyType = new List<SelectedItem>
            {
            new SelectedItem() {Id=1,Name_en="Corporate", Name_cn="企业", Name_fr="Entreprise", Name_jp="コーポレート", Name_kr="기업", Name_tl="ขององค์กร" },
            new SelectedItem() {Id=2,Name_en="Individuals", Name_cn="个人", Name_fr="Personnes", Name_jp="個人", Name_kr="개인", Name_tl="บุคคล" }
            };

        public static List<SelectedItem> getItemCultureList(List<SelectedItem> itemList, BaseViewModel model)
        {
            SelectedItem objItem = new SelectedItem();
            List<SelectedItem> newitemList = new List<SelectedItem>();
            itemList.ForEach(a =>
            {
                a.Name = Utility.GetPropertyValue(a, "Name", model.CurrentCulture) == null ? string.Empty :
                                                             Utility.GetPropertyValue(a, "Name", model.CurrentCulture).ToString();
            });

            return itemList;
        }
        public static List<SelectedItem> TradingOfficeList = new List<SelectedItem>
                {
                 new SelectedItem() {Id=4,Name_en="Japan", Name_cn="日本", Name_fr="Japon", Name_jp="日本", Name_kr="일본", Name_tl="ประเทศญี่ปุ่น" },
                 new SelectedItem() {Id=2,Name_en="Korea", Name_cn="韩国", Name_fr="Corée", Name_jp="韓国", Name_kr="대한민국", Name_tl="เกาหลี" },
                 new SelectedItem() {Id=12,Name_en="Philipines", Name_cn="菲律宾", Name_fr="Philippines", Name_jp="フィリピン", Name_kr="필리핀", Name_tl="ฟิลิปปินส์" },
                 new SelectedItem() {Id=11,Name_en="Bangladesh", Name_cn="孟加拉国", Name_fr="Bangladesh", Name_jp="バングラデシュ", Name_kr="방글라데시", Name_tl="บังคลาเทศ" }
                };

        public static List<SelectedItem> ActivityType = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="Arranging appointment(PhoneCall/e-mail)", Name_cn="安排约会（电话号码/电子邮件）", Name_fr="Organisation du rendez-vous (PhoneCall / e-mail)", Name_jp="予約の手配（PhoneCall / Eメール）", Name_kr="약속 정렬 (PhoneCall / 전자 메일)", Name_tl="นัดหมาย (PhoneCall / e-mail)" },
            new SelectedItem() {Id=2,Name_en="Meeting", Name_cn="会议", Name_fr="Réunion", Name_jp="会議", Name_kr="모임", Name_tl="การประชุม" },
            new SelectedItem() {Id=2,Name_en="Sales Appointment", Name_cn="销售预约", Name_fr="Rendez-vous de vente", Name_jp="セールスアポイントメント", Name_kr="판매 약속", Name_tl="นัดหมายการขาย" },
            new SelectedItem() {Id=2,Name_en="Handling Claim", Name_cn="处理索赔", Name_fr="Traitement de la réclamation", Name_jp="クレームの取扱い", Name_kr="손해 배상 청구 처리", Name_tl="การจัดการการอ้างสิทธิ์" }
        };

        public static List<SelectedItem> ResultofActivity = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="Failed (Absent)", Name_cn="失败（缺席）", Name_fr="Échec (Absent)", Name_jp="失敗（不在）", Name_kr="실패 (부재)", Name_tl="ล้มเหลว (ขาด)" },
            new SelectedItem() {Id=2,Name_en="Failed (Refused)", Name_cn="失败（拒绝）", Name_fr="Échec (Refusé)", Name_jp="失敗（拒否", Name_kr="실패 (거부 됨)", Name_tl="ล้มเหลว (ปฏิเสธ)"},
            new SelectedItem() {Id=3,Name_en="Failed (No Interest)", Name_cn="失败（无兴趣）", Name_fr="Échec (aucun intérêt)", Name_jp="失敗（興味なし）", Name_kr="실패 (관심 없음)", Name_tl="ล้มเหลว (ไม่มีดอกเบี้ย)" },
            new SelectedItem() {Id=4,Name_en="On hold", Name_cn="等候接听", Name_fr="En attente", Name_jp="保留", Name_kr="보류 중", Name_tl="ระงับ" },
            new SelectedItem() {Id=5,Name_en="On hold（Positive)", Name_cn="暂停（正）", Name_fr="En attente (Positif)", Name_jp="保留中（ポジティブ）", Name_kr="보류 중 (긍정적)", Name_tl="ระงับ (บวก)" },
            new SelectedItem() {Id=6,Name_en="Succeed（Appoinment)", Name_cn="成功（的会面）", Name_fr="Réussir (nomination)", Name_jp="成功する（Appoinment）", Name_kr="성공 (Appoinment)", Name_tl="ประสบความสำเร็จ (แต่งตั้ง)" },
            new SelectedItem() {Id=7,Name_en="Succeed（Quotation)", Name_cn="成功（报价）", Name_fr="Réussir (Quotation)", Name_jp="成功する（見積もり）", Name_kr="성공 (견적)", Name_tl="ประสบความสำเร็จ (Quotation)" },
            new SelectedItem() {Id=8,Name_en="Succeed（expecting Order)", Name_cn="成功（期待订单）", Name_fr="Réussir (en attendant l'ordre)", Name_jp="成功する（注文を期待する）",Name_kr="성공 (주문 예상)", Name_tl="ประสบความสำเร็จ (คาดหวังว่าจะสั่งซื้อ)" },
            new SelectedItem() {Id=9,Name_en="Succeed（Ordered)", Name_cn="成功（有序）", Name_fr="Réussir (commandé)", Name_jp="成功（秩序ある）", Name_kr="성공 (Ordered)", Name_tl="ประสบความสำเร็จ (สั่ง)" },
            new SelectedItem() {Id=10,Name_en="Succeed（GainingTrust）", Name_cn="成功（GainingTrust）", Name_fr="Réussir (GainingTrust)", Name_jp="成功する（GainingTrust", Name_kr="성공 (GainingTrust)", Name_tl="ประสบความสำเร็จ (GainingTrust)" }
        };

        public static List<SelectedItem> AffiliateType = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="Affiliate(promotion)", Name_cn="加盟（促销）", Name_fr="Affilié (promotion)", Name_jp="アフィリエイト（プロモーション）", Name_kr="제휴사 (프로모션)", Name_tl="พันธมิตร (โปรโมชั่น)" },
            new SelectedItem() {Id=2,Name_en="Post right", Name_cn="发贴权", Name_fr="Post right", Name_jp="投稿権", Name_kr="오른쪽부터", Name_tl="โพสต์ขวา" },
            new SelectedItem() {Id=3,Name_en="HR", Name_cn="HR", Name_fr="HEURE", Name_jp="HR", Name_kr="HR", Name_tl="ทรัพยากรบุคคล" },
            new SelectedItem() {Id=4,Name_en="Related company in overseas", Name_cn="相关公司在海外", Name_fr="Société associée à l'étranger", Name_jp="海外関連会社", Name_kr="해외 관련 회사", Name_tl="บริษัท ที่เกี่ยวข้องในต่างประเทศ" }
        };

        public static List<SelectedItem> PriceCalculateTypeList = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="Word", Name_cn="字", Name_fr="Mot", Name_jp="ワード", Name_kr="워드", Name_tl="คำ" },
            new SelectedItem() {Id=2,Name_en="Character",Name_cn="字符", Name_fr="Personnage", Name_jp="キャラクター", Name_kr="캐릭터",Name_tl="ตัวละคร"  },
            new SelectedItem() {Id=3,Name_en="Pages", Name_cn="网页", Name_fr="Pages", Name_jp="ページ", Name_kr="페이지", Name_tl="หน้า" }
        };


        public static List<SelectedItem> PaymentWayList = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="Credit Card", Name_cn="信用卡", Name_fr="Carte de crédit", Name_jp="クレジットカード", Name_kr="신용 카드", Name_tl="บัตรเครดิต" },
            new SelectedItem() {Id=2,Name_en="Bank", Name_cn="银行",Name_fr="Banque", Name_jp="バンク", Name_kr="은행", Name_tl="ธนาคาร" },
            new SelectedItem() {Id=3,Name_en="paypall", Name_cn="贝宝", Name_fr="Pay Pal", Name_jp="ペイパル", Name_kr="페이팔", Name_tl="เพย์พาล" }
        };


        public static List<SelectedItem> LanguageLevelList = new List<SelectedItem>
        {
            new SelectedItem() {Id=0,Name_en="None", Name_cn="没有", Name_fr="Aucun", Name_jp="なし", Name_kr="없음", Name_tl="ไม่มี" },
            new SelectedItem() {Id=1,Name_en="Native", Name_cn="本地人",Name_fr="Originaire de", Name_jp="ネイティブ", Name_kr="원주민", Name_tl="พื้นเมือง" },
            new SelectedItem() {Id=2,Name_en="Business", Name_cn="商业", Name_fr="Entreprise", Name_jp="ビジネス", Name_kr="사업", Name_tl="ธุรกิจ" },
            new SelectedItem() {Id=3,Name_en="Daily Conversation", Name_cn="每日对话",Name_fr="Conversation quotidienne", Name_jp="毎日の会話", Name_kr="매일의 대화", Name_tl="การสนทนารายวัน" },
            new SelectedItem() {Id=4,Name_en="Amature", Name_cn="电枢", Name_fr="Amature", Name_jp="アマチュア", Name_kr="Amature", Name_tl="มือสมัครเล่น" }
        };


        public static List<SelectedItem> NoticeStatusList = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="Show", Name_cn="显示", Name_fr="Montrer", Name_jp="ショー", Name_kr="보여 주다", Name_tl="แสดง" },
            new SelectedItem() {Id=2,Name_en="Hide", Name_cn="隐藏",Name_fr="Cacher", Name_jp="隠す", Name_kr="숨는 장소", Name_tl="ปิดบัง" },
            new SelectedItem() {Id=3,Name_en="ShowAfterLogin", Name_cn="登录后显示", Name_fr="Afficher après la connexion", Name_jp="ログイン後に表示", Name_kr="로그인 후 표시", Name_tl="แสดงหลังจากเข้าสู่ระบบ" },
           
        };
        public static List<SelectedItem> NoticePriorityList = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="General", Name_cn="一般", Name_fr="Général", Name_jp="一般", Name_kr="일반", Name_tl="ทั่วไป" },
            new SelectedItem() {Id=2,Name_en="Important", Name_cn="重要",Name_fr="Important", Name_jp="重要", Name_kr="중대한", Name_tl="สำคัญ" },
         
        };

        public static List<SelectedItem> CompanyTransproTypeList = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="None", Name_cn="没有", Name_fr="Aucun", Name_jp="なし", Name_kr="없음", Name_tl="ไม่มี" },
            new SelectedItem() {Id=2,Name_en="OEM", Name_cn="",Name_fr="", Name_jp="", Name_kr="", Name_tl="" },
            new SelectedItem() {Id=3,Name_en="Partially OEM", Name_cn="",Name_fr="", Name_jp="", Name_kr="", Name_tl="" },
            new SelectedItem() {Id=4,Name_en="Branch", Name_cn="科",Name_fr="Branche", Name_jp="ブランチ", Name_kr="분기", Name_tl="สาขา" },
            new SelectedItem() {Id=4,Name_en="Head office", Name_cn="",Name_fr="", Name_jp="", Name_kr="본점", Name_tl="" }
        };

        public static List<SelectedItem> PenaltyCategoryList = new List<SelectedItem>
        {
            new SelectedItem() {Id=1,Name_en="Abandonment", Name_cn="",Name_fr="", Name_jp="", Name_kr="", Name_tl="" },
            new SelectedItem() {Id=2,Name_en="Late delivery ", Name_cn="",Name_fr="", Name_jp="", Name_kr="", Name_tl="" },
            new SelectedItem() {Id=3,Name_en="Mistake", Name_cn="没有", Name_fr="Aucun", Name_jp="なし", Name_kr="없음", Name_tl="ไม่มี" },
            new SelectedItem() {Id=4,Name_en="Untranslation", Name_cn="没有", Name_fr="Aucun", Name_jp="なし", Name_kr="없음", Name_tl="ไม่มี" },


        };


    }
    public class SelectedItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Name_en { get; set; }
        public string Name_jp { get; set; }
        public string Name_kr { get; set; }
        public string Name_fr { get; set; }
        public string Name_cn { get; set; }
        public string Name_tl { get; set; }
    }
    public static class EnumHelper
    {
        /// <summary>
        /// Retrieve the description on the enum, e.g.
        /// [Description("Bright Pink")]
        /// BrightPink = 2,
        /// Then when you pass in the enum, it will retrieve the description
        /// </summary>
        /// <param name="en">The Enumeration</param>
        /// <returns>A string representing the friendly name</returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }
        
    }

    public static class PdfHelper
    {
        public static byte[] HtmlToPdf(string html, string filePath)
        {
            Byte[] bytes;
            using (var ms = new MemoryStream())
            {
                using (var doc = new iTextSharp.text.Document())
                {
                    using (var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, ms))
                    {
                        doc.Open();
                        using (var htmlWorker = new iTextSharp.text.html.simpleparser.HTMLWorker(doc))
                        {
                            using (var sr = new StringReader(html))
                            {
                                htmlWorker.Parse(sr);
                            }
                        }
                        //using (var srHtml = new StringReader(html))
                        //{
                        //    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, srHtml);
                        //}
                        //using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                        //{
                        //    using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_html)))
                        //    {
                        //        iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                        //    }
                        //}
                        doc.Close();
                    }
                }
                bytes = ms.ToArray();
            }
            return bytes;
        }
    }
}
