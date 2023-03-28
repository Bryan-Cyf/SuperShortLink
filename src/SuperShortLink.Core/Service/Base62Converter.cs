using Microsoft.Extensions.Options;
using SuperShortLink.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperShortLink
{
    /// <summary>
    /// https://www.cnblogs.com/xmlnode/p/4544302.html
    /// </summary>
    public class Base62Converter
    {
        private readonly string _base62Charset;
        private readonly int _codeLength;

        public Base62Converter(IOptionsSnapshot<ShortLinkOptions> option)
        {
            _base62Charset = option.Value.Secrect;
            _codeLength = option.Value.CodeLength;
        }

        /// <summary>
        /// 注意：超过设定的长度可能会有异常数据
        /// </summary>
        private int MaxLength
        {
            get
            {
                return ConvertFromBase62(string.Empty.PadLeft(_codeLength, _base62Charset.Last())).ToString().Length;
            }
        }

        /// <summary>
        /// 【混淆加密】转短码
        /// 1、根据自增主键id前面补0，如：00000123
        /// 2、倒转32100000
        /// 3、把倒转后的十进制转六十二进制（乱序后）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Confuse(long id)
        {
            if (id.ToString().Length > MaxLength)
                throw new Exception($"转换值不能超过最大位数{MaxLength}");

            var idChars = id.ToString()
                   .PadLeft(MaxLength, '0')
                   .ToCharArray()
                   .Reverse();

            //倒转后的Id
            var confuseId = long.Parse(string.Join("", idChars));
            var base64Str = ToBase62String(confuseId);
            return base64Str.PadLeft(_codeLength, _base62Charset.First());
        }

        /// <summary>
        /// 【恢复混淆】解析短码
        /// 1、六十二进制转十进制，得到如：32100000
        /// 2、倒转00000123，得到123
        /// 3、根据123作为主键去数据库查询映射对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public long ReCoverConfuse(string key)
        {
            if (key.Length > _codeLength + 1)
                throw new Exception($"转换值不能超过最大位数{_codeLength + 1}");

            var confuseId = ConvertFromBase62(key);

            var idChars = confuseId.ToString()
                .PadLeft(MaxLength, '0')
                .ToCharArray()
                .Reverse();

            var id = long.Parse(string.Join("", idChars));
            return id;
        }

        /// <summary>
        /// 十进制 -> 62进制
        /// </summary>
        public string ToBase62String(long value)
        {
            var sb = new StringBuilder();

            do
            {
                sb.Insert(0, _base62Charset[(int)(value % 62)]);
                value /= 62;
            } while (value > 0);

            return sb.ToString();
        }

        /// <summary>
        /// 62进制 -> 十进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public long ConvertFromBase62(string value)
        {
            long result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                int power = value.Length - i - 1;
                int digit = _base62Charset.IndexOf(value[i]);
                result += digit * (long)Math.Pow(62, power);
            }

            return result;
        }

        /// <summary>
        /// 生成随机的0-9a-zA-Z字符串
        /// </summary>
        /// <returns></returns>
        public string GenerateKeys()
        {
            string[] Chars = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".Split(',');
            return string.Join("", Chars.ToList().Shuffle());
        }
    }
}
