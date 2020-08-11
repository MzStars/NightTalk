using Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model
{
    /// <summary>
    /// 文件
    /// </summary>
    public class FileInfoDomain
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileInfoDomain() 
        {
            Global.InitModel(this);
        }

        /// <summary>
        /// 标识
        /// </summary>
        public Guid ID { get; set; }

        /// <summary>
        /// 后台账号唯一ID
        /// </summary>
        public int UID { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [StringLength(50)]
        public string FileName { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [StringLength(255)]
        public string FilePath { get; set; }

        /// <summary>
        /// 文件后缀
        /// </summary>
        [StringLength(10)]
        public string FileSuffix { get; set; }

        /// <summary>
        /// 文件类型
        /// </summary>
        public FileTypeEnumDomain FileType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
