﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ayudacard_api.Entities
{
    public class MstService
    {
        public Int32 Id { get; set; }
        public String Service { get; set; }
        public String Description { get; set; }
        public Int32 ServiceDepartmentId { get; set; }
        public String ServiceDepartment { get; set; }
        public Int32 ServiceGroupId { get; set; }
        public String ServiceGroup { get; set; }
        public String DateEncoded { get; set; }
        public String DateExpiry { get; set; }
        public Decimal LimitAmount { get; set; }
        public Decimal BudgetAmount { get; set; }
        public Boolean IsMultipleUse { get; set; }
        public Int32 StatusId { get; set; }
        public String Status { get; set; }
        public Boolean IsLocked { get; set; }
        public Int32 CreatedByUserId { get; set; }
        public String CreatedByUser { get; set; }
        public String CreatedDateTime { get; set; }
        public Int32 UpdatedByUserId { get; set; }
        public String UpdatedByUser { get; set; }
        public String UpdatedDateTime { get; set; }
    }
}