using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Common
{
    public enum ControllerTypeEnumDomain
    {
        /// <summary>
        /// 文本框
        /// </summary>
        [Description("文本框")]
        TextBox,

        /// <summary>
        /// 多行文本框
        /// </summary>
        [Description("多行文本框")]
        MultiTextBox,

        /// <summary>
        /// 复选框
        /// </summary>
        [Description("复选框")]
        CheckBox,

        /// <summary>
        /// 单选框
        /// </summary>
        [Description("单选框")]
        RadioBox,

        /// <summary>
        /// 下拉框
        /// </summary>
        [Description("下拉框")]
        SelectBox,

        /// <summary>
        /// 数字选择框
        /// </summary>
        [Description("数字选择框")]
        NumberBox,

        /// <summary>
        /// 日期选择框
        /// </summary>
        [Description("日期选择框")]
        DateTimeBox,

        /// <summary>
        /// 图片上传框
        /// </summary>
        [Description("图片上传框")]
        ImageBox
    }
}
