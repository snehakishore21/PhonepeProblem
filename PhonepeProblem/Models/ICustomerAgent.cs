using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem.Models
{
    public interface ICustomerAgent
    {
        public string AgentEmail { get; }
        public string AgentName { get; }

        public IList<IssueType> IssueType { get; }

        public AgentStatus AgentStatus { get; set; }

        public ICustomerIssue CurrentIssue { get; set; }
    }
}
