using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Practices.CompositeWeb;
using Microsoft.Practices.CompositeWeb.Interfaces;
using Microsoft.Practices.CompositeWeb.Utility;
using Microsoft.Practices.ObjectBuilder;
using Chai.WorkflowManagment.CoreDomain;
using Chai.WorkflowManagment.CoreDomain.DataAccess;
using Chai.WorkflowManagment.CoreDomain.Admins;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Services;
using Chai.WorkflowManagment.Shared.Navigation;


using System.Data;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.CoreDomain.HRM;

namespace Chai.WorkflowManagment.Modules.HRM
{
    public class HRMController : ControllerBase
    {
        private IWorkspace _workspace;

        [InjectionConstructor]
        public HRMController([ServiceDependency] IHttpContextLocatorService httpContextLocatorService, [ServiceDependency]INavigationService navigationService)
            : base(httpContextLocatorService, navigationService)
        {
            _workspace = ZadsServices.Workspace;
        }
        #region CurrentObject
        public object CurrentObject
        {
            get
            {
                return GetCurrentContext().Session["CurrentObject"];
            }
            set
            {
                GetCurrentContext().Session["CurrentObject"] = value;
            }
        }
        #endregion
        #region Employee Search 
        public IList<Employee> ListEmployees(string EmpNo, string FullName,int project)
        {
            string filterExpression = "";
            
           filterExpression = "SELECT * FROM Employees Where 1 = Case when '" + FullName + "' = '' Then 1 When (Employees.FirstName + ' ' + Employees.lastName) = '" + FullName + "' Then 1 END ";
            // return WorkspaceFactory.CreateReadOnly().Queryable<CashPaymentRequest>(filterExpression).ToList();
            return _workspace.SqlQuery<Employee>(filterExpression).ToList();
        }
        #endregion
        #region Entity Manipulation
        public void SaveOrUpdateEntity<T>(T item) where T : class
        {
            IEntity entity = (IEntity)item;
            if (entity.Id == 0)
                _workspace.Add<T>(item);
            else
                _workspace.Update<T>(item);

            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }
        public void DeleteEntity<T>(T item) where T : class
        {
            _workspace.Delete<T>(item);
            _workspace.CommitChanges();
            _workspace.Refresh(item);
        }

        public void Commit()
        {
            _workspace.CommitChanges();
        }
        #endregion               
    }
}
