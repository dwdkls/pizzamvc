using System;
using KebabManager.Contracts.Services;
using KebabManager.Contracts.ViewModels.Customers;
using KebabManager.Model.PersistenceModels;
using NHibernate;
using Pizza.Contracts.Operations.Requests;
using Pizza.Contracts.Operations.Results;
using Pizza.Framework.Operations;

namespace KebabManager.Application.Services
{
    public class CustomersCrudService
        : CrudServiceBase<Customer, CustomerGridModel, CustomerDetailsModel, CustomerEditModel, CustomerCreateModel>, ICustomersService
    {
        public CustomersCrudService(ISession session) : base(session)
        {
        }

        public override DataPageResult<CustomerGridModel> GetDataPage(DataRequest<CustomerGridModel> request)
        {
            throw new ApplicationException();
        }

        public override CrudOperationResult<int> Create(CustomerCreateModel createModel)
        {
            throw new ApplicationException();
        }

        public override CrudOperationResult Update(CustomerEditModel editModel)
        {
            throw new ApplicationException();
        }

        public override CrudOperationResult Delete(int id)
        {
            throw new ApplicationException();
        }
    }
}
