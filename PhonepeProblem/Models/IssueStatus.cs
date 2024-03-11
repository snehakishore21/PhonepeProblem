using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem.Models
{
    public enum IssueStatus
    {
        OPEN,
        IN_PROGRESS,
        RESOLVED
    }

    public enum IssueType
    {
        PAYMENT,
        MUTUAL_FUND,
        GOLD,
        INSURANCE   
    }

    public enum AgentStatus
    {
        FREE,
        WORKING
    }  
}
