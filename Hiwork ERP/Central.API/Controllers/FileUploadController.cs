

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using HiWork.Utils;
using System.Collections.Generic;
using System.Configuration;


namespace Central.API.Controllers
{
    //[Authorize]
    public class FileUploadController : ApiController
    {

        public struct DataFileInformation
        {
            public string FileName;
            public string OriginalFileName;
            public long FileSize;
            public string DownloadURL;
            public DateTime UploadDate;
            public string Extension;
        }



        [Route("fileUpload/uploadPhotos")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadPhotos(long userID, string culture)
        {
            try
            {
                if (this.ModelState.IsValid)
                {

                    string fileName = "";
                    var root = HttpContext.Current.Server.MapPath("~/App_Data/UploadFiles");
                    //var root = @"C:\Inetpub\FileUploader";
                    Directory.CreateDirectory(root);
                    var provider = new MultipartFormDataStreamProvider(root);
                    var result = await Request.Content.ReadAsMultipartAsync(provider);

                    if (this.ModelState.IsValid)
                    {
                        string subFolderParentName = "AllPhotos";
                        string subFolderName = string.Empty;
                        var UploadPath = "~/Upload/";
                        if (subFolderParentName != string.Empty)
                        {
                            UploadPath = UploadPath + "/" + subFolderParentName + "/";
                        }
                        if (subFolderName != string.Empty)
                        {
                            UploadPath = UploadPath + subFolderName + "/";
                        }
                        UploadPath = HttpContext.Current.Server.MapPath(UploadPath);
                        Utility.CreateDirectory(UploadPath);
                        foreach (var fileData in result.FileData)
                        {
                            //TODO: Do something with uploaded file.  
                            fileName = fileData.Headers.ContentDisposition.FileName;
                            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                            {
                                fileName = fileName.Trim('"');
                            }
                            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                            {
                                fileName = Path.GetFileName(fileName);
                            }
                            //string f = Path.GetFileNameWithoutExtension(fileName);
                            //string e = Path.GetExtension(fileName);
                            if (File.Exists(UploadPath + fileName))
                            {
                                File.Delete(UploadPath + fileName);
                                File.Copy(fileData.LocalFileName, Path.Combine(UploadPath, fileName));
                            }
                            else
                            {
                                File.Copy(fileData.LocalFileName, Path.Combine(UploadPath, fileName));
                            }
                        }
                        //Utility.ClearFolder(root);

                        return Request.CreateResponse(HttpStatusCode.OK, Path.Combine(fileName));
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("fileUpload/uploadAdvertisingPhotos")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadAdvertisingPhotos(long userID, string culture)
        {
            try
            {
                if (this.ModelState.IsValid)
                {

                    string fileName = "";
                    string DownloadURL = null;
                    var root = HttpContext.Current.Server.MapPath("~/App_Data/UploadFiles");
                    //var root = @"C:\Inetpub\FileUploader";
                    Directory.CreateDirectory(root);
                    var provider = new MultipartFormDataStreamProvider(root);
                    var result = await Request.Content.ReadAsMultipartAsync(provider);

                    if (this.ModelState.IsValid)
                    {
                        string subFolderParentName = "AdvertisingPhotos";
                        string subFolderName = "";
                        var UploadPath = "~/Upload/AllPhotos/";
                        string UploadPhysicalPath, FinalFilePath;

                        if (subFolderParentName != "")
                        {
                            UploadPath = UploadPath + "/" + subFolderParentName + "/";
                        }
                        if (subFolderName != "")
                        {
                            UploadPath = UploadPath + subFolderName + "/";
                        }

                        UploadPhysicalPath = HttpContext.Current.Server.MapPath(UploadPath);
                        Utility.CreateDirectory(UploadPhysicalPath);

                        foreach (var fileData in result.FileData)
                        {
                            //TODO: Do something with uploaded file.  
                            fileName = fileData.Headers.ContentDisposition.FileName;
                            if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                            {
                                fileName = fileName.Trim('"');
                            }
                            if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                            {
                                fileName = Path.GetFileName(fileName);
                            }

                            FinalFilePath = Path.Combine(UploadPhysicalPath, fileName);
                            {
                                fileName = Path.GetFileNameWithoutExtension(FinalFilePath);
                                fileName = DocumentScanner.AddDateIntoFileName(fileName);
                                fileName = DocumentScanner.RefractorFileName(fileName) + Path.GetExtension(FinalFilePath);
                                FinalFilePath = Path.Combine(UploadPhysicalPath, fileName);
                            }
                            File.Copy(fileData.LocalFileName, FinalFilePath, true);
                        }
                        //Utility.ClearFolder(root);

                        //return Request.CreateResponse(HttpStatusCode.OK, Path.Combine(fileName));
                        DownloadURL = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/Upload/AllPhotos/AdvertisingPhotos/" + fileName;
                        return Request.CreateResponse(HttpStatusCode.OK, DownloadURL);
                    }
                    else
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Route("fileUpload/uploadDataFile")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadDataFile(long userID, string culture)
        {
            HttpResponseMessage response;
            DataFileInformation FileDataStructure;
            List<DataFileInformation> FileDataList = new List<DataFileInformation>();
            FileInfo OSFileInfo;

            try
            {
                if (this.ModelState.IsValid)
                {

                    string fileName, OriginalFileName;
                    var root = HttpContext.Current.Server.MapPath("~/App_Data/UploadFiles");
                    Directory.CreateDirectory(root);
                    var provider = new MultipartFormDataStreamProvider(root);
                    var result = await Request.Content.ReadAsMultipartAsync(provider);
                    
                    string subFolderParentName = "DataFiles";
                    string subFolderName = "";
                    string UploadPath = "~/Upload/";
                    string UploadPhysicalPath;
                    string FinalFilePath;

                    if (subFolderParentName != "")
                    {
                        UploadPath = UploadPath + "/" + subFolderParentName + "/";
                    }
                    if (subFolderName != "")
                    {
                        UploadPath = UploadPath + subFolderName + "/";
                    }

                    UploadPhysicalPath = HttpContext.Current.Server.MapPath(UploadPath);
                    Utility.CreateDirectory(UploadPhysicalPath);

                    foreach (MultipartFileData fileData in result.FileData)
                    {
                        fileName = fileData.Headers.ContentDisposition.FileName;
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }
                        OriginalFileName = fileName;
                        FinalFilePath = Path.Combine(UploadPhysicalPath, fileName);
                        {
                            fileName = Path.GetFileNameWithoutExtension(FinalFilePath);
                            fileName = DocumentScanner.AddDateIntoFileName(fileName);
                            fileName = DocumentScanner.RefractorFileName(fileName) + Path.GetExtension(FinalFilePath);
                            fileName = HttpUtility.UrlEncode(fileName);
                            FinalFilePath = Path.Combine(UploadPhysicalPath, fileName);
                        }
                        File.Copy(fileData.LocalFileName, FinalFilePath, true);
                        OSFileInfo = new FileInfo(FinalFilePath);
                        FileDataStructure = new DataFileInformation();
                        FileDataStructure.FileName = fileName;
                        FileDataStructure.OriginalFileName = OriginalFileName;
                        FileDataStructure.DownloadURL = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/Upload/DataFiles/" + fileName;
                        FileDataStructure.UploadDate = DateTime.Now;
                        FileDataStructure.FileSize = OSFileInfo.Length;
                        FileDataStructure.Extension = OSFileInfo.Extension;
                        FileDataList.Add(FileDataStructure);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, FileDataList);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }


        [Route("fileUpload/uploadDocument")]
        [HttpPost]
        public async Task<HttpResponseMessage> UploadDocuments(long userID, string culture)
        {
            DocumentScanner descriptor;
            HttpResponseMessage response;
            DocumentData docData;
            List<DocumentData> docList = new List<DocumentData>();

            try
            {
                if (this.ModelState.IsValid)
                {

                    string fileName, OriginalFileName;
                    var root = HttpContext.Current.Server.MapPath("~/App_Data/UploadFiles");
                    Directory.CreateDirectory(root);
                    var provider = new MultipartFormDataStreamProvider(root);
                    var result = await Request.Content.ReadAsMultipartAsync(provider);

                    string subFolderParentName = "Documents";
                    string subFolderName = "";
                    string UploadPath = "~/Upload/";
                    string UploadPhysicalPath;
                    string FinalFilePath;

                    if (subFolderParentName != "")
                    {
                        UploadPath = UploadPath + "/" + subFolderParentName + "/";
                    }
                    if (subFolderName != "")
                    {
                        UploadPath = UploadPath + subFolderName + "/";
                    }

                    UploadPhysicalPath = HttpContext.Current.Server.MapPath(UploadPath);
                    Utility.CreateDirectory(UploadPhysicalPath);

                    foreach (MultipartFileData fileData in result.FileData)
                    {
                        fileName = fileData.Headers.ContentDisposition.FileName;
                        if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                        {
                            fileName = fileName.Trim('"');
                        }
                        if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                        {
                            fileName = Path.GetFileName(fileName);
                        }
                        OriginalFileName = fileName;
                        FinalFilePath = Path.Combine(UploadPhysicalPath, fileName);
                        {
                            fileName = Path.GetFileNameWithoutExtension(FinalFilePath);
                            fileName = DocumentScanner.AddDateIntoFileName(fileName);
                            fileName = DocumentScanner.RefractorFileName(fileName) + Path.GetExtension(FinalFilePath);
                            FinalFilePath = Path.Combine(UploadPhysicalPath, fileName);
                        }
                        File.Copy(fileData.LocalFileName, FinalFilePath, true);

                        // Start parsing operation and count number of words
                        // In the document file with the help of a DocumentDescriptor objct
                        docData = new DocumentData();
                        docData.FileName = fileName;
                        docData.OriginalFileName = OriginalFileName;
                        docData.DownloadURL = Request.RequestUri.GetLeftPart(UriPartial.Authority) + "/Upload/Documents/" + fileName;
                        docData.UploadDate = DateTime.Now;
                        FileInfo fileinfo = new FileInfo(FinalFilePath);
                        docData.FileSize = fileinfo.Length.ToString();
                        docData.Extension = fileinfo.Extension;
                        descriptor = new DocumentScanner();
                        descriptor.Document = FinalFilePath;
                        descriptor.ParseDocument();
                        descriptor.RemoveExcessWhitespaceLinebreak();
                        descriptor.EstimateDocument(ref docData);
                        docList.Add(docData);
                    }
                    response = Request.CreateResponse(HttpStatusCode.OK, docList);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }

        [Route("fileUpload/DeleteDocument")]
        [HttpPost]
        public HttpResponseMessage DeleteDocuments(string FileName)
        {
            HttpResponseMessage response;
            try
            {
                if (this.ModelState.IsValid)
                {
                    string subFolderParentName = "Documents";
                    string subFolderName = "";
                    string UploadPath = "~/Upload/";
                    string UploadPhysicalPath;


                    if (subFolderParentName != "")
                    {
                        UploadPath = UploadPath + "/" + subFolderParentName + "/";
                    }
                    if (subFolderName != "")
                    {
                        UploadPath = UploadPath + subFolderName + "/";
                    }

                     UploadPhysicalPath =  HttpContext.Current.Server.MapPath(UploadPath);

                    if (File.Exists(UploadPhysicalPath + FileName))
                    {
                        File.Delete(UploadPhysicalPath + FileName);
                       //  await Task.Delay(10);
                    }

                    response = Request.CreateResponse(HttpStatusCode.OK, "Success");
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;

        }

        [Route("transpro/manuscript")]
        [HttpPost]
        public HttpResponseMessage GetManuscriptEstimated(TransproManuscript Data)
        {
            HttpResponseMessage response;
            DocumentData Estimation = new DocumentData();
            DocumentScanner Scanner = new DocumentScanner();
            Scanner.DocumentContent = Data.PlainText;
            Scanner.RemoveExcessWhitespaceLinebreak();
            Scanner.EstimateDocument(ref Estimation);
            response = Request.CreateResponse(HttpStatusCode.OK, Estimation);
            return response;
        }

        public class TransproManuscript
        {
            public string PlainText { get; set; }
        }
    }
}



