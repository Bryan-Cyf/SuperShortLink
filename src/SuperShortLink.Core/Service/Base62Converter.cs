﻿using Microsoft.Extensions.Options;
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
        private readonly string _base62CharSet;
        private readonly int _codeLength;

        public Base62Converter(IOptionsSnapshot<ShortLinkOptions> option)
        {
            _base62CharSet = option.Value.Secrect;
            _codeLength = option.Value.CodeLength;
        }

        /// <summary>
        /// 补充0的长度
        /// </summary>
        private int ZeroLength
        {
            get
            {
                return MaxValue.ToString().Length;
            }
        }

        /// <summary>
        /// Code长度位数下能达到的最大值
        /// </summary>
        private long MaxValue
        {
            get
            {
                var max = (long)Math.Pow(62, _codeLength) - 1;
                return (long)Math.Pow(10, max.ToString().Length - 1) - 1;
            }
        }

        /// <summary>
        /// 【混淆加密】转短码
        /// 1、根据自增主键id前面补0，如：00000123
        /// 2、倒转32100000
        /// 3、把倒转后的十进制转六十二进制（乱序后）
        /// </summary>
        public string Confuse(long id)
        {
            if (id > MaxValue)
            {
                throw new Exception($"转换值不能超过最大值{MaxValue}");
            }

            var idChars = id.ToString()
                   .PadLeft(ZeroLength, '0')
                   .ToCharArray()
                   .Reverse();

            var confuseId = long.Parse(string.Join("", idChars));
            var base62Str = Encode(confuseId);
            return base62Str.PadLeft(_codeLength, _base62CharSet.First());
        }

        /// <summary>
        /// 【恢复混淆】解析短码
        /// 1、六十二进制转十进制，得到如：32100000
        /// 2、倒转00000123，得到123
        /// 3、根据123作为主键去数据库查询映射对象
        /// </summary>
        public long ReCoverConfuse(string key)
        {
            if (key.Length != _codeLength)
            {
                return 0;
                //throw new Exception($"转换值长度不等于系统设置的长度");
            }

            var confuseId = Decode(key);
            var idChars = confuseId.ToString()
                .PadLeft(ZeroLength, '0')
                .ToCharArray()
                .Reverse();

            var id = long.Parse(string.Join("", idChars));
            id = id > MaxValue ? 0 : id;
            return id;
        }

        /// <summary>
        /// 十进制 -> 62进制
        /// </summary>
        public string Encode(long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value", "value must be greater or equal to zero");

            var sb = new StringBuilder();
            do
            {
                sb.Insert(0, _base62CharSet[(int)(value % 62)]);
                value /= 62;
            } while (value > 0);

            return sb.ToString();
        }

        /// <summary>
        /// 62进制 -> 十进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public long Decode(string value)
        {
            long result = 0;
            for (int i = 0; i < value.Length; i++)
            {
                int power = value.Length - i - 1;
                int digit = _base62CharSet.IndexOf(value[i]);
                if (digit < 0)
                    throw new ArgumentException("Invalid character in base62 string", "value");
                result += digit * (long)Math.Pow(62, power);
            }

            return result;
        }

        /// <summary>
        /// 生成随机的0-9a-zA-Z字符串
        /// 62位秘钥
        /// </summary>
        /// <returns></returns>
        public string GenerateSecret()
        {
            string[] Chars = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".Split(',');
            return string.Join("", Chars.ToList().Shuffle());
        }
    }
}
