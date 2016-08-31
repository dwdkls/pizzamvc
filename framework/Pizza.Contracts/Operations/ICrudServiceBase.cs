using System;
using System.Linq.Expressions;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Results;

namespace Pizza.Contracts.Operations
{
    public interface ICrudServiceBase<TGridModel, TDetailsModel, TEditModel, TCreateModel> 
        where TGridModel : IGridModelBase
        where TDetailsModel : IDetailsModelBase
        where TEditModel : IEditModelBase
        where TCreateModel : ICreateModelBase
    {
        DataPageResult<TGridModel> GetDataPage(DataRequest<TGridModel> request);
        CrudOperationResult<TCreateModel> GetCreateModel();
        CrudOperationResult<TEditModel> GetEditModel(int id);
        CrudOperationResult<TDetailsModel> GetDetailsModel(int id);
        CrudOperationResult<int> Create(TCreateModel createModel);
        CrudOperationResult Update(TEditModel editModel);
        CrudOperationResult Delete(int id);
    }
}