using SuperShortLink.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShortLink
{
    public interface ILogService
    {
        /// <summary>
        /// 插入访问记录
        /// </summary>
        Task Insert(long id);
    }
}
