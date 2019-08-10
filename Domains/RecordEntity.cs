using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Domains
{
    /// <summary>
    /// 带着Create/Modify信息的实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RecordEntity : Entity, IRecordEntity
    {
        /// <summary>
        /// 记录的当前状态
        /// </summary>
        public RecordStates RecordStatus { get; set; }

        #region 创建create
        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 快速设置创建者
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="operation"></param>
        public void CreatedBy(IUser creator, string action = "Create") {
            this.LastOperation = action;
            this.ModifierId = this.CreatorId = creator.Id;
            this.ModifierName = this.CreatorName = creator.Name;
            this.ModifierJSON = this.CreatorJSON = creator.ToJSON();
            this.ModifyTime = this.CreateTime = DateTime.Now;
        }

        #region 创建者信息
        /// <summary>
        /// 记录创建者Id
        /// </summary>
        public Guid CreatorId { get; set; }

        /// <summary>
        /// 记录创建者名称(一般是DisplayName)
        /// </summary>
        public string CreatorName { get; set; }

        /// <summary>
        /// 记录创建者对象序列化成JSON后存储在这个字段
        /// </summary>
        public string CreatorJSON { get; set; }



        public UserEntity Creator { get; set; }

        #endregion creator

        #endregion create

        #region 修改modify
        /// <summary>
        /// 记录修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }

        /// <summary>
        /// 快速设置修改者
        /// </summary>
        /// <param name="Modifier"></param>
        /// <param name="operation"></param>
        public void ModifiedBy(IUser modifier, string action = "Modify") {
            this.LastOperation = action;
            this.ModifierId = modifier.Id;
            this.ModifierName = modifier.Name;
            this.ModifierJSON = modifier.ToJSON();
            this.ModifyTime = DateTime.Now;
        }

        /// <summary>
        /// 最近一次操作(操作名称)
        /// </summary>
        public string LastOperation { get; set; }

        #region 记录修改者信息 Modifier
        /// <summary>
        /// 记录修改者Id
        /// </summary>
        public Guid ModifierId { get; set; }

        /// <summary>
        /// 记录修改者名称
        /// </summary>
        public string ModifierName { get; set; }

        /// <summary>
        /// 记录修改者对象序列化成JSON后存储在这个字段
        /// </summary>
        public string ModifierJSON { get; set; }

        public UserEntity Modifier { get; set; }

        #endregion Modifier

        #endregion modify

    }
}
