﻿namespace Kongrevsky.Infrastructure.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EntityFramework.Triggers;
    using Kongrevsky.Utilities.EF6;
    using Nito.AsyncEx;

    public class KongrevskyUnitOfWork<T> : IKongrevskyUnitOfWork<T> where T : KongrevskyDbContext
    {
        public KongrevskyUnitOfWork(IKongrevskyDatabaseFactory<T> kongrevskyDatabaseFactory)
        {
            _kongrevskyDatabaseFactory = kongrevskyDatabaseFactory;
            _lockObject = new object();
            _lockObjectAsync = new AsyncLock();
        }

        private object _lockObject { get; }
        private AsyncLock _lockObjectAsync { get;  }
        private T Database => _database ?? (_database = _kongrevskyDatabaseFactory.Get());
        private IKongrevskyDatabaseFactory<T> _kongrevskyDatabaseFactory { get; }
        private T _database { get; set; }

        public static Action<IEnumerable<Tuple<object, object>>> AddedBeforeSaveChanges { get; set; }
        public static Action<IEnumerable<Tuple<object, object>>> RemovedBeforeSaveChanges { get; set; }
        public static Action<IEnumerable<Tuple<object, object>>> AddedAfterSaveChanges { get; set; }
        public static Action<IEnumerable<Tuple<object, object>>> RemovedAfterSaveChanges { get; set; }

        public void Commit()
        {
            lock (_lockObject)
            {
                var addedRelationships = Database.GetAddedRelationships().ToList();
                var deletedRelationships = Database.GetDeletedRelationships().ToList();

                AddedBeforeSaveChanges?.Invoke(addedRelationships);
                RemovedBeforeSaveChanges?.Invoke(deletedRelationships);

                Database.SaveChanges();

                AddedAfterSaveChanges?.Invoke(addedRelationships);
                RemovedAfterSaveChanges?.Invoke(deletedRelationships);
                // if you get an exception when try commit changes in db you can use the following try block to catch exception
                //try
                //{
                //    Database.SaveChangesWithTriggers();
                //}
                //catch (DbEntityValidationException dbEx)
                //{
                //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                //    {
                //        foreach (var validationError in validationErrors.ValidationErrors)
                //        {
                //            // check error
                //        }
                //    }
                //}
            }
        }

        public async Task CommitAsync()
        {
            using (await _lockObjectAsync.LockAsync())
            {
                var addedRelationships = Database.GetAddedRelationships().ToList();
                var deletedRelationships = Database.GetDeletedRelationships().ToList();

                AddedBeforeSaveChanges?.Invoke(addedRelationships);
                RemovedBeforeSaveChanges?.Invoke(deletedRelationships);

                await Database.SaveChangesAsync(Task.Factory.CancellationToken);

                AddedAfterSaveChanges?.Invoke(addedRelationships);
                RemovedAfterSaveChanges?.Invoke(deletedRelationships);
                // if you get an exception when try commit changes in db you can use the following try block to catch exception
                //try
                //{
                //    await Database.SaveChangesAsync(Task.Factory.CancellationToken);
                //}
                //catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
                //{
                //    foreach (var validationErrors in dbEx.EntityValidationErrors)
                //    {
                //        foreach (var validationError in validationErrors.ValidationErrors)
                //        {
                //            // check error
                //        }
                //    }
                //}
            }
        }
    }
}