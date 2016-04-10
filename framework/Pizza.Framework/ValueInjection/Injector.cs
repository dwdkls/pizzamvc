using Pizza.Contracts;
using Pizza.Persistence;

namespace Pizza.Framework.ValueInjection
{
    public static class Injector
    {
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