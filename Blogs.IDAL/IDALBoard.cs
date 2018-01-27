using Blogs.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Blogs.IDAL
{
    public interface IDALBoard 
    {
        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        blog_tb_Board GetEntity(string id);

        /// <summary>
        /// 插入Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(blog_tb_Board entity);

        /// <summary>
        /// 更新Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(blog_tb_Board entity);

      
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Delete(string id);

        /// <summary>
        /// 查询留言板
        /// </summary>
        /// <param name="state">-1 则查询所有状态</param>
        /// <returns></returns>
        List<blog_tb_Board> Query(int state);
    }
}
