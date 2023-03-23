using SuperShortLink.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace SuperShortLink
{
    public class ShortLinkOptions
    {

        public const string SectionName = "ShortLink";

        /// <summary>
        /// 短码长度
        /// </summary>
        [Range(1, int.MaxValue)]
        public int CodeLength { get; set; }

        /// <summary>
        /// 62位加密秘钥
        /// </summary>
        [Required]
        public string Secrect { get; set; }

        /// <summary>
        /// 数据库链接
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型
        /// DatabaseType：SqlServer/MySQL/PostgreSQL
        /// </summary>
        [Required]
        public DatabaseType DbType { get; set; }

        /// <summary>
        /// 缓存数量限制
        /// </summary>
        public int? CacheCountLimit { get; set; }

        /// <summary>
        /// 登陆账号
        /// </summary>
        public string LoginAcount { get; set; }

        /// <summary>
        /// 登陆密码
        /// </summary>
        public string LoginPassword { get; set; }

        public string AutoIncrementSQL
        {
            get
            {
                return DbType switch
                {
                    DatabaseType.MySQL => "last_insert_id()",
                    DatabaseType.PostgreSQL => "lastval()",
                    DatabaseType.SqlServer => "scope_identity()",
                };
            }
        }
        /// <summary>
        /// 生成默认数据
        /// </summary>
        public static ShortLinkOptions GenDefault()
        {
            return new ShortLinkOptions()
            {
                LoginAcount = "admin",
                LoginPassword = "123456",
                CacheCountLimit = 10000
            };
        }
    }
}
