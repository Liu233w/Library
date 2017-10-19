using Library.Net.MimeTypes;

namespace Library.Configuration
{
    public class AllowExt
    {
        public static string GetMimeType(string FileName)
        {
            string fileExtension = System.IO.Path.GetExtension(FileName).ToLower();    //还没测试
            switch (fileExtension) {
                case ".jpg": return MimeTypeNames.ImageJpeg ;
                case ".png": return MimeTypeNames.ImagePng ;
                case ".gif": return MimeTypeNames.ImageGif ;
                case ".jpeg": return MimeTypeNames.ImageJpeg ;
                case ".psd": return MimeTypeNames.ApplicationOctetStream ;
                case ".ttf": return MimeTypeNames.ApplicationXFontTtf ;
                case ".doc": return MimeTypeNames.ApplicationMsword ;
                case ".xls": return MimeTypeNames.ApplicationVndMsExcel ;
                case ".ppt": return MimeTypeNames.ApplicationVndMsPowerpoint ;
                case ".docx": return MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentWordprocessingmlDocument ;
                case ".xlsx": return MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet ;
                case ".pptx": return MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentPresentationmlPresentation ;
                case ".pdf": return MimeTypeNames.ApplicationPdf ;
                case ".xps": return MimeTypeNames.ApplicationOctetStream ;
                case ".txt": return MimeTypeNames.TextPlain ;
                case ".htm": return MimeTypeNames.TextHtml ;
                case ".html": return MimeTypeNames.TextHtml ;
                case ".css": return MimeTypeNames.TextCss ;
                case ".js": return MimeTypeNames.ApplicationJavascript ;
                case ".xml": return MimeTypeNames.ApplicationXmlDtd ;
                case ".shtml": return MimeTypeNames.TextHtml ;
                case ".svg": return MimeTypeNames.ImageSvgXml ;
                case ".rar": return MimeTypeNames.ApplicationXRarCompressed ;
                case ".zip": return MimeTypeNames.ApplicationZip ;
                case ".gz": return MimeTypeNames.ApplicationXGzip ;
                case ".gizp": return MimeTypeNames.ApplicationXGzip ;
                case ".tar": return MimeTypeNames.ApplicationXTar ;
                case ".mp3": return MimeTypeNames.AudioMpeg ;
                case ".ogg": return MimeTypeNames.AudioOgg ;
                case ".wav": return MimeTypeNames.AudioVndWave ;
                case ".amv": return MimeTypeNames.AudioXMsWma ;
                case ".rm": return MimeTypeNames.AudioVndRnRealaudio ;
                case ".mp4": return MimeTypeNames.VideoMp4 ;
                case ".mpg": return MimeTypeNames.VideoMpeg ;
                case ".flv": return MimeTypeNames.VideoXFlv ;
                case ".f4v": return MimeTypeNames.VideoXFlv ;
                case ".mkv": return MimeTypeNames.VideoXMatroska ;
                case ".mov": return MimeTypeNames.VideoQuicktime ;
                case ".mpeg": return MimeTypeNames.VideoMpeg ;
                case ".swf": return MimeTypeNames.ApplicationXShockwaveFlash ;
                case ".webm": return MimeTypeNames.VideoWebm ;
                case ".wmv": return MimeTypeNames.VideoXMsWmv ;
                case ".fla": return MimeTypeNames.ApplicationOctetStream ;
                case ".ssa": return MimeTypeNames.TextPlain ;
                case ".ass": return MimeTypeNames.TextPlain ;
                case ".srt": return MimeTypeNames.TextPlain ;
                case ".ipa": return MimeTypeNames.ApplicationOctetStream ;
            }

            return "application/octet-stream";
        }
    }
}
