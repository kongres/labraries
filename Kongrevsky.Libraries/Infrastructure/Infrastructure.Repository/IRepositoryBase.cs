﻿namespace Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using AutoMapper;
    using Infrastructure.Repository.Models;
    using Kongrevsky.Utilities.Enumerable.Models;
    using PagedList;

    public interface IRepositoryBase<T, DB> : IRepositoryBase<T> 
        where T : class
        where DB : DbContext
    { }

    public interface IRepositoryBase<T> where T : class 
    {
        int BulkInsert<TSource>(List<TSource> entities) where TSource : BaseEntity;
        int ClassicBulkInsert(List<T> entities);
        int BulkUpdate<TSource>(List<TSource> entities) where TSource : BaseEntity;
        int ClassicBulkUpdate(List<T> entities);
        int BulkDelete<TSource>(List<TSource> entities) where TSource : BaseEntity;
        int BulkDelete<TSource>(Expression<Func<TSource, bool>> where) where TSource : BaseEntity;
        int BulkDeleteDuplicates<Ts>(Expression<Func<T, Ts>> expression, Expression<Func<T, bool>> where = null);
        T Add(T entity);
        T Update(T entity);
        T AddOrUpdate(T entity);
        void AddOrUpdate(params T[] entities);
        T AddOrUpdate(Expression<Func<T, object>> identifierExpression, T entity);
        void AddOrUpdate(Expression<Func<T, object>> identifierExpression, params T[] entities);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        Task<T> GetByIdAsync(string id);
        T GetById(Guid id);
        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        Task<T> GetAsync(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        IPagedList<T> GetPage(Page page, Expression<Func<T, bool>> checkPermission, Expression<Func<T, bool>> where, Func<IQueryable<T>, IQueryable<T>> sortFunc, params Expression<Func<T, object>>[] includes);
        IPagedList<T> GetPage(Page page, params Expression<Func<T, object>>[] includes);

        PagingQueryable<TCast> GetPage<TCast>(PagingModel<TCast> filter, Expression<Func<T, bool>> checkPermission, List<Expression<Func<T, bool>>> where, IConfigurationProvider configurationProvider, List<Expression<Func<TCast, bool>>> postWhere = null) where TCast : class;
    }
}