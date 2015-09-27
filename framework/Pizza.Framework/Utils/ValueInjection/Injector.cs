using System.Collections.Generic;
using System.Linq;
using Pizza.Contracts.Persistence;
using Pizza.Contracts.Presentation;

namespace Pizza.Framework.Utils.ValueInjection
{
    public static class Injector
    {
        public static IEnumerable<TTarget> CreateViewModelsFromPersistenceModels<TSource, TTarget>(IEnumerable<TSource> sourceItems)
            where TTarget : IViewModelBase, new() 
            where TSource : IPersistenceModel
        {
            return sourceItems.Select(CreateViewModelFromPersistenceModel<TSource, TTarget>).ToList();
        }

        public static TViewModel CreateViewModelFromPersistenceModel<TPersistenceModel, TViewModel>(TPersistenceModel persistenceModel)
            where TViewModel : IViewModelBase, new() 
            where TPersistenceModel : IPersistenceModel
        {
            var model = new TViewModel();
            model.InjectFromPersistenceModel(persistenceModel);
            return model;
        }
    }
}