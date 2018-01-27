using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Blogs.UI.Main
{
    public class CompressFilter :Stream
    {
        private Stream output;
        public CompressFilter(Stream filter)
        {
            output = filter;
        }
        public override bool CanRead
        {
            get { return output.CanRead; }
        }

        public override bool CanSeek
        {
            get { return output.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return output.CanWrite; }
        }

        public override void Flush()
        {
            output.Flush();
        }

        public override long Length
        {
            get { return output.Length; }
        }

        public override long Position
        {
            get { return output.Position; }
            set { output.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return output.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return output.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            output.SetLength(value);
        }

        StringBuilder responseHtml = new StringBuilder();
        private bool isFirstWrite = true;   //是否首次写入
        public override void Write(byte[] buffer, int offset, int count)
        {
            string strBuffer = UTF8Encoding.UTF8.GetString(buffer, offset, count);
            if (isFirstWrite)
            {
                if (!Regex.IsMatch(strBuffer, "<\\s*html", RegexOptions.IgnoreCase))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(strBuffer);
                    output.Write(bytes, 0, bytes.Length);
                }
            }

            isFirstWrite = false;

            responseHtml.Append(strBuffer);
            // ---------------------------------
            // Wait for the closing </html> tag
            // ---------------------------------
            Regex eof = new Regex("</html>", RegexOptions.IgnoreCase);
            if (eof.IsMatch(strBuffer))
            {
                string html = responseHtml.ToString();

                int startIndex = html.IndexOf("<pre");
                int endIndex = html.IndexOf("</pre>");
                if ((startIndex != -1) && (endIndex != -1))
                {
                    // String start = html.Substring(0, startIndex);
                    // String end = html.Substring(endIndex);
                    // html = start + HttpUtility.HtmlDecode(html.Substring(startIndex, endIndex - startIndex)) + end;
                    // return (start + src.Substring(startIndex, endIndex - startIndex).Replace("&nbsp;", " ").Replace("&lt;", "<").Replace("&gt;", ">").Replace("&quot;", "\"").Replace("<br/>", "\n").Trim() + end).Replace("&lt;/pre&gt;", "</pre>");
                }

                //html = Regex.Replace(html, @"(\s|\;|^|\{|\})\/\/.*\n", "");  //去掉 //注释
               // html = Regex.Replace(html, @"/\*((\n\r|.)*?)\*/", ""); //去掉 /**/注释
               // html = Regex.Replace(html, @"<!--*.*?-->", "", RegexOptions.Compiled | RegexOptions.Multiline); //去掉 <!--  -->注释
                html = Regex.Replace(html, @">(\s)*?(?=\S+)", ">", RegexOptions.Compiled | RegexOptions.Multiline); //移除空白

                // html = Regex.Replace(html, "<input type=\"hidden\" name=\"__VIEWSTATE\"(.|\n)*?/>", "", RegexOptions.Compiled | RegexOptions.Multiline);
                // html = Regex.Replace(html, "<form name=\"aspnetForm\".*id=\"aspnetForm\">", "<form name=\"form1\" id=\"form1\">", RegexOptions.IgnoreCase);

                //Regex r1 = new Regex("<input type=\"hidden\" name=\"__EVENTTARGET\".*/>", RegexOptions.IgnoreCase);
                // Regex r2 = new Regex("<input type=\"hidden\" name=\"__EVENTARGUMENT\".*/>", RegexOptions.IgnoreCase);
                byte[] bytes = Encoding.UTF8.GetBytes(html);
                output.Write(bytes, 0, bytes.Length);
            }
        }
    }
}