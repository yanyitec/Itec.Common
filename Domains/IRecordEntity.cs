using System;
using System.Data.Common;

namespace Itec.Domains
{
    public interface IRecordEntity:IEntity
    {
        /// <summary>
        /// 记录的当前状态
        /// </summary>
        RecordStates RecordStatus { get; set; }
        #region 创建create
        /// <summary>
        /// 记录创建时间
        /// </summary>
        DateTime CreateTime { get; set; }

        /// <summary>
        /// 快速设置创建者
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="operation"></param>
        void CreatedBy(IUser creator, string operation = "Create");

        #region 创建者信息
        /// <summary>
        /// 记录创建者Id
        /// </summary>
        Guid CreatorId { get; set; }

        /// <summary>
        /// 记录创建者名称(一般是DisplayName)
        /// </summary>
        string CreatorName { get; set; }

        /// <summary>
        /// 记录创建者对象序列化成JSON后存储在这个字段
        /// </summary>
        string CreatorJSON { get; set; }

        #endregion creator
        
        #endregion create

        #region 修改modify
        /// <summary>
        /// 记录修改时间
        /// </summary>
        DateTime ModifyTime { get; set; }

        /// <summary>
        /// 快速设置修改者
        /// </summary>
        /// <param name="Modifier"></param>
        /// <param name="operation"></param>
        void ModifiedBy(IUser Modifier, string operation = "Modify");

        /// <summary>
        /// 最近一次操作(操作名称)
        /// </summary>
        string LastOperation { get; set; }

        #region 记录修改者信息 Modifier
        /// <summary>
        /// 记录修改者Id
        /// </summary>
        Guid ModifierId { get; set; }

        /// <summary>
        /// 记录修改者名称
        /// </summary>
        string ModifierName { get; set; }

        /// <summary>
        /// 记录修改者对象序列化成JSON后存储在这个字段
        /// </summary>
        string ModifierJSON { get; set; }



        #endregion Modifier

        #endregion modify


        

        
        

        
        
    }
}