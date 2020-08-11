using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common.Enum
{
    public enum StateEnumDomain
    {
        /// <summary>
        /// 草稿
        /// </summary>
        [Description("草稿")]
        Drafts,
        /// <summary>
        /// 初审中
        /// </summary>
        [Description("初审中")]
        FirstReviewIng,
        /// <summary>
        /// 初审拒绝
        /// </summary>
        [Description("初审拒绝")]
        FirstReviewReject,

        /// <summary>
        /// 初审通过
        /// </summary>
        [Description("初审通过")]
        FirstReviewAccepted,
        /// <summary>
        /// 终审拒绝
        /// </summary>
        [Description("终审拒绝")]
        LastReviewReject,
        /// <summary>
        /// 终审通过
        /// </summary>
        [Description("终审通过")]
        LastReviewAccepted,
        /// <summary>
        /// 背审拒绝
        /// </summary>
        [Description("背审拒绝")]
        BackonReject
    }
}
