﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EvolveWebApp
{
    public class UserRequest
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Dept { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
    public class UserDashboard : UserRequest
    {
        public string AccountName { get; set; }
        public string ProjectName { get; set; }
        public string Location { get; set; }
        public string Grade { get; set; }
        public string CPUUsage { get; set; }
        public string MemoryUsage { get; set; }
        public string Cost { get; set; }
        public bool Status { get; set; }
    }
}
