﻿using System;
using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using Pizza.Framework.ValueInjection.CustomInjections;
using Pizza.Persistence;

namespace Pizza.Framework.ValueInjection
{
    // TODO: probably not necessary if all types of mapping will use only one Injection
    public static class ObjectInjectionExtensions
    {
        /* Persistence Model to Model mapping.
            * 1) Flatten
            * 2) strings, numbers, enums - all by same name and same type
        */

        // TODO: add generic constraints
        public static TModel InjectFromPersistenceModel<TModel, TPersistenceModel>(this TModel model, TPersistenceModel persistenceModel)
            where TPersistenceModel : IPersistenceModel
        {
            CheckArgument(persistenceModel, "persistenceModel");
            CheckArgument(model, "model");

            model
                .InjectFrom<FlatLoopInjection>(persistenceModel)
                .InjectFrom<NullableValueInjection>(persistenceModel)
                .InjectFrom<VersionInjection>(persistenceModel); 

            return model;
        }

        /* Model to Persistence Model mapping:
            * 1) Unflatten
            * 2) strings, numbers, enums - all by same name and same type
            * 3) don't map Editable(false) properties
        */

        // TODO: add generic constraints
        public static TPersistenceModel InjectFromViewModel<TPersistenceModel, TModel>(this TPersistenceModel persistenceModel, TModel model)
            where TPersistenceModel : IPersistenceModel
        {
            CheckArgument(persistenceModel, "persistenceModel");
            CheckArgument(model, "model");

            persistenceModel
                .InjectFrom(new ViewModelToPersistenceModelInjection(typeof(TModel)), model)
                //.InjectFrom<ViewModelToPersistenceModelInjection>(model)
                .InjectFrom<NullableValueInjection>(model);
            
            return persistenceModel;
        }

        private static void CheckArgument(object item, string name)
        {
            if (item == null)
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}