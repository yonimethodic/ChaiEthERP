﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chai.WorkflowManagment.CoreDomain.Setting;
using Chai.WorkflowManagment.CoreDomain.Users;

namespace Chai.WorkflowManagment.CoreDomain.HRM
{
    public partial class EmployeeDetail : IEntity
    {
        public int Id { get; set; }
        public virtual Setting.JobTitle JobTitle { get; set; }
       
        public virtual Setting.Position Position { get; set; }

        public virtual Setting.Program Program { get; set; }

        public int  DutyStation { get; set; }

        public decimal Salary { get;  set;}

        public int EmploymentStatus { get; set; }
        public int Class { get; set; }
        public string HoursPerWeek { get; set; }
        public string BaseCountry { get; set; }
        public string BaseCity { get; set; }
        public string BaseState { get; set; }
        public string CountryTeam { get; set; }
        public string DescriptiveJobTitle { get; set; }
        public int Supervisor { get; set; }
        public int ReportsTo { get; set; }
        
            



        public virtual HRM.Employee Employee { get; set; }
    
    }
}
