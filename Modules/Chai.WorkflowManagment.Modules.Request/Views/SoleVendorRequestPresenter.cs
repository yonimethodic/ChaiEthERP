﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using Microsoft.Practices.CompositeWeb;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.Shared;
using Chai.WorkflowManagment.CoreDomain.Requests;
using Chai.WorkflowManagment.Modules.Admin;
using Chai.WorkflowManagment.CoreDomain.Users;
using Chai.WorkflowManagment.Modules.Setting;
using Chai.WorkflowManagment.Enums;
using Chai.WorkflowManagment.Shared.MailSender;

namespace Chai.WorkflowManagment.Modules.Request.Views
{
    public class SoleVendorRequestPresenter : Presenter<ISoleVendorRequestView>
    {

        // NOTE: Uncomment the following code if you want ObjectBuilder to inject the module controller
        //       The code will not work in the Shell module, as a module controller is not created by default
        //
        private RequestController _controller;
        private AdminController _adminController;
        private SettingController _settingController;
        private SoleVendorRequest _soleVendorRequest;
        public SoleVendorRequestPresenter([CreateNew] RequestController controller, AdminController adminController, SettingController settingController)
        {
            _controller = controller;
            _adminController = adminController;
            _settingController = settingController;
        }
        public override void OnViewLoaded()
        {
            if (View.GetSoleVendorRequestId > 0)
            {
                _controller.CurrentObject = _controller.GetSoleVendorRequest(View.GetSoleVendorRequestId);
            }
            CurrentSoleVendorRequest = _controller.CurrentObject as SoleVendorRequest;
        }
        public override void OnViewInitialized()
        {
            if (_soleVendorRequest == null)
            {
                int id = View.GetSoleVendorRequestId;
                if (id > 0)
                    _controller.CurrentObject = _controller.GetSoleVendorRequest(id);
                else
                    _controller.CurrentObject = new SoleVendorRequest();
            }
        }
        public SoleVendorRequest CurrentSoleVendorRequest
        {
            get
            {
                if (_soleVendorRequest == null)
                {
                    int id = View.GetSoleVendorRequestId;
                    if (id > 0)
                        _soleVendorRequest = _controller.GetSoleVendorRequest(id);
                    else
                        _soleVendorRequest = new SoleVendorRequest();
                }
                return _soleVendorRequest;
            }
            set
            {
                _soleVendorRequest = value;
            }
        }
        public IList<ItemAccount> GetItemAccounts()
        {
            return _settingController.GetItemAccounts();

        }
        public IList<SoleVendorRequest> GetSoleVendorRequests()
        {
            return _controller.GetSoleVendorRequests();
        }
        public int GetLastSoleVendorRequestId()
        {
            return _controller.GetLastSoleVendorRequestId();
        }
        private void SaveSoleVendorRequestStatus()
        {
            if (GetApprovalSetting(RequestType.SoleVendor_Request.ToString().Replace('_', ' '), 0) != null)
            {
                int i = 1;
                foreach (ApprovalLevel AL in GetApprovalSetting(RequestType.SoleVendor_Request.ToString().Replace('_', ' '), 0).ApprovalLevels)
                {
                    SoleVendorRequestStatus SVRS = new SoleVendorRequestStatus();
                    SVRS.SoleVendorRequest = CurrentSoleVendorRequest;
                    if (AL.EmployeePosition.PositionName == "Superviser/Line Manager")
                    {
                        if (CurrentUser().Superviser != 0)
                            SVRS.Approver = CurrentUser().Superviser.Value;
                        else

                            SVRS.ApprovalStatus = ApprovalStatus.Approved.ToString();
                    }
                    else if (AL.EmployeePosition.PositionName == "Program Manager")
                    {
                        if (CurrentSoleVendorRequest.SoleVendorRequestDetails[0].Project != null)
                        {
                            SVRS.Approver = GetProject(CurrentSoleVendorRequest.SoleVendorRequestDetails[0].Project.Id).AppUser.Id;
                        }
                    }
                    else
                    {
                        if (Approver(AL.EmployeePosition.Id) != null)
                            SVRS.Approver = Approver(AL.EmployeePosition.Id).Id;
                        else
                            SVRS.Approver = 0;
                    }
                    SVRS.WorkflowLevel = i;
                    i++;
                    CurrentSoleVendorRequest.SoleVendorRequestStatuses.Add(SVRS);
                }
            }
        }
        private void GetCurrentApprover()
        {
            if (CurrentSoleVendorRequest.SoleVendorRequestStatuses != null)
            {
                foreach (SoleVendorRequestStatus SVRS in CurrentSoleVendorRequest.SoleVendorRequestStatuses)
                {
                    if (SVRS.ApprovalStatus == null)
                    {
                        SendEmail(SVRS);
                        CurrentSoleVendorRequest.CurrentApprover = SVRS.Approver;
                        CurrentSoleVendorRequest.CurrentLevel = SVRS.WorkflowLevel;
                        CurrentSoleVendorRequest.ProgressStatus = ProgressStatus.InProgress.ToString();
                        break;
                    }
                }
            }
        }
        public void SaveOrUpdateSoleVendorRequest(int PRID)
        {
            PurchaseRequest thePurchaseRequest = _controller.GetPurchaseRequestbyPuID(PRID).PurchaseRequest;
            SoleVendorRequest soleVendorRequest = CurrentSoleVendorRequest;
            soleVendorRequest.PurchaseRequest = thePurchaseRequest;
            soleVendorRequest.RequestNo = View.GetRequestNo;
            soleVendorRequest.RequestDate = Convert.ToDateTime(DateTime.Today);
            soleVendorRequest.Comment = View.GetComment;
            soleVendorRequest.AppUser = _adminController.GetUser(CurrentUser().Id);
            soleVendorRequest.ProgressStatus = ProgressStatus.InProgress.ToString();

            if (CurrentSoleVendorRequest.SoleVendorRequestStatuses.Count == 0)
                SaveSoleVendorRequestStatus();
            GetCurrentApprover();

            _controller.SaveOrUpdateEntity(soleVendorRequest);
            _controller.CurrentObject = null;
            //Notify the Purchase requester that bid process is initiated
            SendEmailToRequester();
        }
        public void SaveOrUpdateSoleVendorRequest(SoleVendorRequest SoleVendorRequest)
        {
            _controller.SaveOrUpdateEntity(SoleVendorRequest);
        }
        public IList<SoleVendorSupplier> GetSoleVendorSuppliers()
        {
            return _settingController.GetSoleVendorSuppliers();
        }
        public void CancelPage()
        {
            _controller.Navigate(String.Format("~/Request/Default.aspx?{0}=3", AppConstants.TABID));
        }
        public void DeleteSoleVendorRequest(SoleVendorRequest SoleVendorRequest)
        {
            _controller.DeleteEntity(SoleVendorRequest);
        }
        public PurchaseRequest GetPurchaseRequest(int purchaseRequestId)
        {
            return _controller.GetPurchaseRequest(purchaseRequestId);
        }

        public IList<PurchaseRequest> GetPurchaseRequestListInProgress()
        {
            return _controller.GetPurchaseRequestsInProgress();
        }
        public SoleVendorRequest GetSoleVendorRequest(int id)
        {
            return _controller.GetSoleVendorRequest(id);
        }
        public IList<PurchaseRequest> GetPurchaseRequestList()
        {
            return _controller.GetPurchaseRequests();
        }
        public PurchaseRequestDetail GetPurchaseRequestDetail(int prDetailId)
        {
            return _controller.GetPurchaseRequestDetail(prDetailId);
        }
        public IList<PurchaseRequestDetail> ListPurchaseReqInProgress()
        {
            return _controller.ListPurchaseReqInProgress();
        }

        public IList<PurchaseRequestDetail> ListPRDetailsInProgressById(int id)
        {
            return _controller.ListPRDetailsInProgressById(id);
        }
        public IList<PurchaseRequestDetail> ListPurchaseReqbyId(int id)
        {
            return _controller.ListPurchaseReqById(id);
        }
        public IList<SoleVendorRequest> ListSoleVendorRequests(string RequestNo, string RequestDate)
        {
            return _controller.ListSoleVendorRequests(RequestNo, RequestDate);
        }
        public SoleVendorSupplier GetSoleVendorSupplier(int Id)
        {
            return _settingController.GetSoleVendorSupplier(Id);
        }
        public AppUser Approver(int Position)
        {
            return _controller.Approver(Position);
        }
        public AssignJob GetAssignedJobbycurrentuser()
        {
            return _controller.GetAssignedJobbycurrentuser();
        }
        public IList<SoleVendorSupplier> GetSuppliers()

        {
            return _settingController.GetSoleVendorSuppliers();
        }
        public AssignJob GetAssignedJobbycurrentuser(int UserId)
        {
            return _controller.GetAssignedJobbycurrentuser(UserId);
        }
        public IList<AppUser> GetUsers()
        {
            return _adminController.GetUsers();
        }
        public AppUser GetUser(int id)
        {
            return _adminController.GetUser(id);
        }
        public AppUser GetSuperviser(int superviser)
        {
            return _controller.GetSuperviser(superviser);
        }
        public void DeleteSoleVendorRequestDetail(SoleVendorRequestDetail soleVendorRequestDetail)
        {
            _controller.DeleteEntity(soleVendorRequestDetail);
        }
        public SoleVendorRequestDetail GetSoleVendorRequestDetail(int id)
        {
            return _controller.GetSoleVendorRequestDetail(id);
        }
        public AppUser CurrentUser()
        {
            return _controller.GetCurrentUser();
        }
        public Project GetProject(int ProjectId)
        {
            return _settingController.GetProject(ProjectId);
        }
        public IList<Project> GetProjects()
        {
            return _settingController.GetProjects();
        }
        public IList<Grant> GetGrants()
        {
            return _settingController.GetGrants();
        }
        public ItemAccount GetItemAccount(int Id)
        {
            return _settingController.GetItemAccount(Id);
        }
        public IList<Grant> GetGrantbyprojectId(int projectId)
        {
            return _settingController.GetProjectGrantsByprojectId(projectId);
        }
        public ApprovalSetting GetApprovalSetting(string RequestType, int value)
        {
            return _settingController.GetApprovalSettingforProcess(RequestType, value);
        }
        private void SendEmail(SoleVendorRequestStatus SVRS)
        {
            if (GetSuperviser(SVRS.Approver).IsAssignedJob != true)
            {
                EmailSender.Send(GetSuperviser(SVRS.Approver).Email, "Sole Vendor Request", (CurrentSoleVendorRequest.AppUser.FullName).ToUpper() + "' has made a request for Sole Vendor Purchase with Request No '" + (CurrentSoleVendorRequest.RequestNo).ToUpper() + "'");
            }
            else
            {
                EmailSender.Send(GetSuperviser(_controller.GetAssignedJobbycurrentuser(SVRS.Approver).AssignedTo).Email, "Sole Vendor Request", (CurrentSoleVendorRequest.AppUser.FullName).ToUpper() + "' has made a request for Sole Vendor Purchase with Request No '" + (CurrentSoleVendorRequest.RequestNo).ToUpper() + "'");
            }
        }
        private void SendEmailToRequester()
        {
            EmailSender.Send(GetUser(CurrentSoleVendorRequest.PurchaseRequest.Requester).Email, "Purchase Request ", "Your Purchase Request with Purchase Request No. - '" + (CurrentSoleVendorRequest.PurchaseRequest.RequestNo).ToUpper() + "' was Completed and a bid process is initiated.");
        }
        public void Commit()
        {
            _controller.Commit();
        }


    }
}




