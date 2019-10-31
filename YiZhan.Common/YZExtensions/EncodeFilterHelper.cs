using System;
using System.Collections.Generic;
using System.Text;

namespace YiZhan.Common.YZExtensions
{
    /// <summary>
    /// 防注入Helper
    /// </summary>
    public class EncodeFilterHelper
    {
        /// <summary>
        /// 过滤大部分html字符（替换为全角字符）
        /// </summary>
        /// <param name="content">输入的内容</param>
        /// <returns></returns>
        public static String EncodeHtml(String content)
        {
            if (content == null)
            {
                return content;
            }
            StringBuilder sb = new StringBuilder(content.Length);
            foreach (var ch in content)
            {
                switch (ch)
                {
                    case '&':
                        sb.Append("&amp;");
                        break;
                    case '<':
                        sb.Append("&lt;");
                        break;
                    case '>':
                        sb.Append("&gt;");
                        break;
                    case '"':
                        sb.Append("&quot;");
                        break;
                    case '\'':
                        sb.Append("&#x27;");
                        break;
                    case '/':
                        sb.Append("&#x2F;");
                        break;
                    case '\\':
                        sb.Append('＼');//全角斜线
                        break;
                    case '#':
                        sb.Append('＃');//全角井号
                        break;
                    case '=':
                        sb.Append("");
                        break;
                    default: sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 过滤大部分html字符（替换为""）
        /// </summary>
        /// <param name="content">输入的内容</param>
        /// <returns></returns>
        public static String EncodeHtmlToNull(String content)
        {
            if (content == null)
            {
                return content;
            }
            StringBuilder sb = new StringBuilder(content.Length);
            foreach (var ch in content)
            {
                switch (ch)
                {
                    case '&':                   
                    case '<':                       
                    case '>':                     
                    case '"':                       
                    case '\'':                       
                    case '/':                       
                    case '\\':                       
                    case '#':                       
                    case '=':
                        sb.Append("");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 为链接提供的过滤
        /// </summary>
        /// <param name="content">输入的内容</param>
        /// <returns></returns>
        public static String EncodeHtmlForLink(String content)
        {
            if (content == null)
            {
                return content;
            }
            StringBuilder sb = new StringBuilder(content.Length);
            foreach (var ch in content)
            {
                switch (ch)
                {
                    case '=':
                        sb.Append("（=）");
                        break;
                    case '&':
                        sb.Append("（&）");
                        break;
                    case '<':
                    case '>':
                    case '"':
                    case '/':
                    case '\\':
                    case '#':                   
                        sb.Append("");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

    }
}
