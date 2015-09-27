using Pizza.Contracts.Presentation;
using Pizza.Framework.Operations.Requests;
using Pizza.Framework.Operations.Results;

namespace Pizza.Framework.Operations
{
    public interface IGridServiceBase<TGridModel, TDetailsModel, TEditModel, TCreateModel> 
        where TGridModel : IGridModelBase
        where TDetailsModel : IDetailsModelBase
        where TEditModel : IEditModelBase
        where TCreateModel : ICreateModelBase
    {
        DataPageResult<TGridModel> GetDataPage(DataRequest<TGridModel> request);
        TCreateModel GetCreateModel();
        TEditModel GetEditModel(int id);
        TDetailsModel GetDetailsModel(int id);
        int Create(TCreateModel createModel);
        void Update(TEditModel editModel);
        void Delete(int id);
    }
}