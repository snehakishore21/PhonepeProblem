using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem.Models
{
    public class CustomerAgent : ICustomerAgent
    {
        public string AgentEmail { get; }
        public string AgentName { get; }

        public IList<IssueType> IssueType { get; }

        public AgentStatus AgentStatus { get; set; }

        public ICustomerIssue CurrentIssue { get; set; }

        public CustomerAgent(string agentEmail, string agentName, IList<IssueType> issueType)
        {
            this.AgentEmail = agentEmail;
            this.AgentName = agentName;
            this.IssueType = issueType;
            this.AgentStatus = AgentStatus.FREE;
        }
    }
}
