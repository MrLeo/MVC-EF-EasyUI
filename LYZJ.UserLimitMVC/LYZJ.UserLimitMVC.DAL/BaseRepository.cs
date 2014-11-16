using System.Data;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq.Expressions;
using LYZJ.UserLimitMVC.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LYZJ.UserLimitMVC.DAL
{
    /// <summary>
    /// 实现对数据库的操作(增删改查)的基类
    /// </summary>
    /// <typeparam name="T">定义泛型，约束其是一个类</typeparam>
    public class BaseRepository<T> where T : class
    {
        //创建EF框架的上下文
        //EF上下文的实例保证线程内唯一
        //private DataModelContainer db = new DataModelContainer();

        /// <summary>
        /// 获取的是当前线程内部的上下文实例，而且保证了线程内上下文唯一
        /// </summary>
        private readonly DbContext _db = EFContextFactory.GetCurrentDbContext();

        /// <summary>
        /// 实现对数据库的添加功能,添加实现EF框架的引用
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>最后返回对象的实体类型</returns>
        public T AddEntity(T entity)
        {
            //EF4.0的写法   添加实体
            //db.CreateObjectSet<T>().AddObject(entity);
            //EF5.0的写法
            _db.Entry<T>(entity).State = EntityState.Added;
            //下面的写法统一
            //db.SaveChanges();
            return entity;
        }

        /// <summary>
        /// 实现对数据库的修改功能
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回是否执行成功，如果执行成功，返回true,负责返回false</returns>
        public bool UpdateEntity(T entity)
        {
            //EF4.0的写法
            //db.CreateObjectSet<T>().Addach(entity);
            //db.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
            //EF5.0的写法
            //db.Set<T>().Attach(entity);
            _db.Entry<T>(entity).State = EntityState.Detached;

            //return db.SaveChanges() > 0;
            return true;
        }

        /// <summary>
        /// 实现对数据库的删除功能
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns>返回是否执行成功，如果执行成功，返回true,负责返回false</returns>
        public bool DeleteEntity(T entity)
        {
            //EF4.0的写法
            //db.CreateObjectSet<T>().Addach(entity);
            //db.ObjectStateManager.ChangeObjectState(entity, EntityState.Deleted);
            //EF5.0的写法
            _db.Set<T>().Attach(entity);
            _db.Entry<T>(entity).State = EntityState.Deleted;

            // return db.SaveChanges() > 0;
            return true;
        }

        /// <summary>
        /// 实现对数据库的查询  --简单查询
        /// </summary>
        /// <param name="whereLambda">查询的简单条件</param>
        /// <returns>返回一个实体类的IQueryable集合</returns>
        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            //EF4.0的写法
            //return db.CreateObjectSet<T>().Where<T>(whereLambda).AsQueryable();
            //EF5.0的写法
            return _db.Set<T>().Where<T>(whereLambda).AsQueryable();
        }

        /// <summary>
        /// 实现对数据的分页查询
        /// </summary>
        /// <typeparam name="S">按照某个类进行排序</typeparam>
        /// <param name="pageIndex">当前第几页</param>
        /// <param name="pageSize">一页显示多少条数据</param>
        /// <param name="total">总条数</param>
        /// <param name="whereLambda">取得排序的条件</param>
        /// <param name="isAsc">如何排序，根据倒叙还是升序</param>
        /// <param name="orderByLambda">根据那个字段进行排序</param>
        /// <returns>返回一个实体类型的IQueryable集合</returns>
        public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int total, Expression<Func<T, bool>> whereLambda,
                                                 bool isAsc, Expression<Func<T, S>> orderByLambda)
        {
            //EF4.0和上面的查询一样
            //EF5.0
            var temp = _db.Set<T>().Where<T>(whereLambda);
            total = temp.Count(); //得到总的条数
            //排序,获取当前页的数据
            if (isAsc)
            {
                temp = temp.OrderBy<T, S>(orderByLambda)
                    .Skip<T>(pageSize*(pageIndex - 1)) //越过多少条
                    .Take<T>(pageSize).AsQueryable(); //取出多少条
            }
            else
            {
                temp = temp.OrderByDescending<T, S>(orderByLambda)
                    .Skip<T>(pageSize*(pageIndex - 1)) //越过多少条
                    .Take<T>(pageSize).AsQueryable(); //取出多少条
            }
            return temp.AsQueryable();
        }
    }
}
