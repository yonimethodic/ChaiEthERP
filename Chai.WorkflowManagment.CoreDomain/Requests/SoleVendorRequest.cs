﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;
using System.ComponentModel.DataAnnotations;
using Chai.WorkflowManagment.CoreDomain.Approval;
using Chai.WorkflowManagment.CoreDomain.Request;


namespace Chai.WorkflowManagment.CoreDomain.Requests
{
    public partial class SoleVendorRequest : IEntity
    {
        public SoleVendorRequest()
        {
            this.SoleVendorRequestDetails = new List<SoleVendorRequestDetail>();
            this.SoleVendorRequestStatuses = new List<SoleVendorRequestStatus>();
            this.SVRAttachments = new List<SVRAttachment>();
        }
        public int Id { get; set; }
        public string RequestNo { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public string Comment { get; set; }
        public int CurrentApprover { get; set; }
        public Nullable<int> CurrentLevel { get; set; }
        public string CurrentStatus { get; set; }
        public string ProgressStatus { get; set; }
        public virtual AppUser AppUser { get; set; }
        public virtual PurchaseRequest PurchaseRequest { get; set; }
        public virtual PurchaseOrderSoleVendor PurchaseOrderSoleVendor { get; set; }
        public virtual IList<SoleVendorRequestDetail> SoleVendorRequestDetails { get; set; }
        public virtual IList<SoleVendorRequestStatus> SoleVendorRequestStatuses { get; set; }
        public virtual IList<SVRAttachment> SVRAttachments { get; set; }

        #region SoleVendorRequestDetail
        public virtual SoleVendorRequestDetail GetSoleVendorRequestDetail(int Id)
        {
            foreach (SoleVendorRequestDetail TARD in SoleVendorRequestDetails)
            {
                if (TARD.Id == Id)
                    return TARD;
            }
            return null;
        }
        public virtual IList<SoleVendorRequestDetail> GetSRDetailBySoleVendorId(int tarId)
        {
            IList<SoleVendorRequestDetail> TARDs = new List<SoleVendorRequestDetail>();
            foreach (SoleVendorRequestDetail TARD in SoleVendorRequestDetails)
            {
                if (TARD.SoleVendorRequest.Id == tarId)
                    TARDs.Add(TARD);
            }
            return TARDs;
        }
        public virtual IList<SoleVendorRequestDetail> GetPendingPurchaseOrderDetails()
        {
            IList<SoleVendorRequestDetail> PendingPOSVRDs = new List<SoleVendorRequestDetail>();
            foreach (SoleVendorRequestDetail SVRD in SoleVendorRequestDetails)
            {
                if (SVRD.POStatus == "InProgress")
                    PendingPOSVRDs.Add(SVRD);
            }
            return PendingPOSVRDs;
        }
        public virtual void RemoveSoleVendorRequestDetail(int Id)
        {
            foreach (SoleVendorRequestDetail TARD in SoleVendorRequestDetails)
            {
                if (TARD.Id == Id)
                    SoleVendorRequestDetails.Remove(TARD);
                break;
            }
        }
        #endregion
        #region SoleVendorRequestStatus
        public virtual SoleVendorRequestStatus GetSoleVendorRequestStatus(int Id)
        {

            foreach (SoleVendorRequestStatus SVRS in SoleVendorRequestStatuses)
            {
                if (SVRS.Id == Id)
                    return SVRS;

            }
            return null;
        }
        public virtual SoleVendorRequestStatus GetSoleVendorRequestStatusworkflowLevel(int workflowLevel)
        {

            foreach (SoleVendorRequestStatus LRS in SoleVendorRequestStatuses)
            {
                if (LRS.WorkflowLevel == workflowLevel)
                    return LRS;

            }
            return null;
        }
        public virtual IList<SoleVendorRequestStatus> GetSoleVendorRequestStatusByRequestId(int RequestId)
        {
            IList<SoleVendorRequestStatus> VRS = new List<SoleVendorRequestStatus>();
            foreach (SoleVendorRequestStatus VR in SoleVendorRequestStatuses)
            {
                if (VR.SoleVendorRequest.Id == RequestId)
                    VRS.Add(VR);

            }
            return VRS;
        }
        public virtual void RemoveSoleVendorRequestStatus(int Id)
        {

            foreach (SoleVendorRequestStatus VRS in SoleVendorRequestStatuses)
            {
                if (VRS.Id == Id)
                    SoleVendorRequestStatuses.Remove(VRS);
                break;
            }

        }
        #endregion
        #region SVRAttachment

        public virtual void RemoveSVRAttachment(string FilePath)
        {
            foreach (SVRAttachment cpa in SVRAttachments)
            {
                if (cpa.FilePath == FilePath)
                {
                    SVRAttachments.Remove(cpa);
                    break;
                }
            }
        }
        #endregion
    }
}
