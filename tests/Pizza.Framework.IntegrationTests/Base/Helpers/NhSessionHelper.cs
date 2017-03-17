using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Cfg;
using Pizza.Framework.Persistence.Extensions;
using Pizza.Framework.Persistence.SoftDelete;
using Pizza.Persistence;

namespace Pizza.Framework.IntegrationTests.Base.Helpers
{
    public sealed class NhSessionHelper
    {
        public readonly ISessionFactory sessionFactory;

        public NhSessionHelper(Configuration nhConfiguration)
        {
            this.sessionFactory = nhConfiguration.BuildSessionFactory();
        }

        private ISession OpenSession()
        {
            var session = this.sessionFactory.OpenSession();
            session.EnableFilter(SoftDeletableFilter.FilterName);

            return session;
        }

        public TPersistenceModel GetInNewSession<TPersistenceModel>(object id)
        {
            return this.ReturnInNewSession(s => s.Get<TPersistenceModel>(id));
        }

        public TPersistenceModel SingleQueryOverInNewSession<TPersistenceModel>(object id)
            where TPersistenceModel : class
        {
            return this.ReturnInNewSession(s => s.QueryOver<TPersistenceModel>().SingleOrDefault());
        }

        public IEnumerable<TPersistenceModel> ListQueryOverInNewSession<TPersistenceModel>()
            where TPersistenceModel : class
        {
            return this.ReturnInNewSession(s => s.QueryOver<TPersistenceModel>().List());
        }

        public void UpdateInNewSession<TPersistenceModel>(TPersistenceModel model)
            where TPersistenceModel : IPersistenceModel
        {
            this.DoInNewSession(s => s.Update(model));
        }

        public void SaveInNewSession<TPersistenceModel>(TPersistenceModel model)
            where TPersistenceModel : IPersistenceModel
        {
            this.DoInNewSession(s => s.Save(model));
        }

        public void SaveManyInNewSession<TPersistenceModel>(IEnumerable<TPersistenceModel> models)
           where TPersistenceModel : IPersistenceModel
        {
            this.DoInNewSession(s => s.SaveMany(models));
        }

        public TPersistenceModel ReturnInNewSession<TPersistenceModel>(Func<ISession, TPersistenceModel> action)
        {
            TPersistenceModel result;
            using (var tempSession = this.OpenSession())
            {
                using (var transaction = tempSession.BeginTransaction())
                {
                    result = action(tempSession);
                    tempSession.Evict(result);

                    transaction.Commit();
                }
            }

            return result;
        }

        public void DoInNewSession(Action<ISession> action)
        {
            using (var tempSession = this.OpenSession())
            {
                using (var transaction = tempSession.BeginTransaction())
                {
                    action(tempSession);

                    transaction.Commit();
                }
            }
        }
    }
}