﻿using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class MaintenanceRequest : IEntity
    {

        public MaintenanceRequest()
        {
            this.MaintenanceRequestStatuses = new List<MaintenanceRequestStatus>();
            this.MaintenanceRequestDetails = new List<MaintenanceRequestDetail>();
            this.MaintenanceSpareParts = new List<MaintenanceSparePart>();
            
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }        
        public DateTime RequestDate { get; set; }
        public string MaintenanceType { get; set; }
        public int Requester { get; set; }
        public string ActionTaken { get; set; }        
        public string PlateNo { get; set; }    
        public int KmReading { get; set; }
        public string MaintenanceStatus { get; set; }
        public string Remark { get; set; }
        public int CurrentApprover { get; set; }
        public int CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual Project Project { get; set; }
        public virtual Grant Grant { get; set; }
        public virtual IList<MaintenanceRequestStatus> MaintenanceRequestStatuses { get; set; }
        public virtual IList<MaintenanceRequestDetail> MaintenanceRequestDetails { get; set; }
        public virtual IList<MaintenanceSparePart> MaintenanceSpareParts { get; set; }
        [NotMapped]
        public string ReqPlateNo
        {
            get { return this.RequestNo + " - " + this.MaintenanceType + " - " + this.PlateNo; }
        }

      

        #region MaintenanceRequestStatus
        public virtual MaintenanceRequestStatus GetMaintenanceRequestStatus(int Id)
        {
            foreach (MaintenanceRequestStatus MRS in MaintenanceRequestStatuses)
            {
                if (MRS.Id == Id)
                    return MRS;

            }
            return null;
        }
        public virtual MaintenanceRequestStatus GetMaintenanceRequestStatusworkflowLevel(int workflowLevel)
        {

            foreach (MaintenanceRequestStatus MRS in MaintenanceRequestStatuses)
            {
                if (MRS.WorkflowLevel == workflowLevel)
                    return MRS;

            }
            return null;
        }
        public virtual IList<MaintenanceRequestStatus> GetMaintenanceRequestStatusByRequestId(int RequestId)
        {
            IList<MaintenanceRequestStatus> MRS = new List<MaintenanceRequestStatus>();
            foreach (MaintenanceRequestStatus MR in MaintenanceRequestStatuses)
            {
                if (MR.MaintenanceRequest.Id == RequestId)
                    MRS.Add(MR);

            }
            return MRS;
        }
        public virtual void RemoveMaintenanceRequestStatus(int Id)
        {

            foreach (MaintenanceRequestStatus MRS in MaintenanceRequestStatuses)
            {
                if (MRS.Id == Id)
                    MaintenanceRequestStatuses.Remove(MRS);
                break;
            }

        }

        #endregion
        #region MaintenanceRequestDetail
        public virtual MaintenanceRequestDetail GetMaintenanceRequestDetail(int Id)
        {

            foreach (MaintenanceRequestDetail MRS in MaintenanceRequestDetails)
            {
                if (MRS.Id == Id)
                    return MRS;

            }
            return null;
        }
        public virtual IList<MaintenanceRequestDetail> GetMaintenanceRequestDetailByMaintenanceId(int MaintenanceId)
        {
            IList<MaintenanceRequestDetail> MRS = new List<MaintenanceRequestDetail>();
            foreach (MaintenanceRequestDetail MR in MaintenanceRequestDetails)
            {
                if (MR.MaintenanceRequest.Id == MaintenanceId)
                    MRS.Add(MR);

            }
            return MRS;
        }
     
        public virtual void RemoveMaintenanceRequestDetail(int Id)
        {

            foreach (MaintenanceRequestDetail MRS in MaintenanceRequestDetails)
            {
                if (MRS.Id == Id)
                    MaintenanceRequestDetails.Remove(MRS);
                break;
            }

        }
        #endregion
        #region MaintenanceSparepart
        public virtual MaintenanceSparePart GetMaintenanceSparePart(int Id)
        {

            foreach (MaintenanceSparePart MRS in MaintenanceSpareParts)
            {
                if (MRS.Id == Id)
                    return MRS;

            }
            return null;
        }
        public virtual IList<MaintenanceSparePart> GetMMaintenanceSparePartByMaintenanceId(int MaintenanceId)
        {
            IList<MaintenanceSparePart> MRS = new List<MaintenanceSparePart>();
            foreach (MaintenanceSparePart MR in MaintenanceSpareParts)
            {
                if (MR.MaintenanceRequest.Id == MaintenanceId)
                    MRS.Add(MR);

            }
            return MRS;
        }

        public virtual void RemoveMaintenanceSparePart(int Id)
        {

            foreach (MaintenanceSparePart MRS in MaintenanceSpareParts)
            {
                if (MRS.Id == Id)
                    MaintenanceSpareParts.Remove(MRS);
                break;
            }

        }
        #endregion


    }
}
