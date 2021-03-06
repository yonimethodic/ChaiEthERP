﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public  class Termination : IEntity
    {
        public Termination()
        {
            this.TerminationReasons = new List<TerminationReason>();
           

        }
        public int Id { get; set; }
        public virtual HRM.Employee Employee { get; set; }
        public DateTime TerminationDate { get; set; }
        public DateTime LastDateOfEmployee { get; set; }
        public string ReccomendationForRehire { get; set; }
        public string TerminationReason { get; set; }

        public virtual IList<TerminationReason> TerminationReasons { get; set; }
        #region TerminationReason
        public virtual TerminationReason GetTerminationReasons(int Id)
        {

            foreach (TerminationReason TR in TerminationReasons)
            {
                if (TR.Id == Id)
                    return TR;
            }
            return null;
        }


        public virtual void RemoveTerminationReason(int Id)
        {
            foreach (TerminationReason TR in TerminationReasons)
            {
                if (TR.Id == Id)
                {
                    TerminationReasons.Remove(TR);
                    break;
                }
            }
        }
        #endregion

    }
}
