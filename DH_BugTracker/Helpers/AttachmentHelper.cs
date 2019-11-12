using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DH_BugTracker.Helpers
{
    public class AttachmentHelper
    {
        public static string GetIcon(string fileName)
        {
            var imgPath = "";
            var ext = Path.GetExtension(fileName);
            switch (ext)
            {
                case ".pdf":
                    imgPath = "/images/pdf.png";
                    break;
                case ".doc":
                    imgPath = "/images/doc.png";
                    break;
                case ".docx":
                    imgPath = "/images/docx.png";
                    break;
                case ".zip":
                case ".7z":
                    imgPath = "/images/zip.png";
                    break;
                case ".rar":
                    imgPath = "/images/rar.png";
                    break;
                case ".xls":
                    imgPath = "/images/xls.png";
                    break;
                case ".xlsx":
                    imgPath = "/images/xlsx.png";
                    break;
                case ".txt":
                    imgPath = "/images/txt.png";
                    break;
                case ".rtf":
                    imgPath = "/images/rtf.png";
                    break;
                case ".xml":
                    imgPath = "/images/xml.png";
                    break;
                case ".html":
                    imgPath = "/images/html.png";
                    break;
                case ".gif":
                case ".jpeg":
                case ".bmp":
                case ".png":
                case ".icon":
                case ".tiff":
                    imgPath = fileName;
                    break;
                default:
                    imgPath = "/images/blank.png";
                    break;
            }
            return imgPath;
        }

    }
}