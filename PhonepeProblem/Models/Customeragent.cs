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

        public IList<string> IssueType { get; }

        public AgentStatus AgentStatus { get; set; }
        public CustomerAgent(string agentEmail, string agentName, IList<string> issueType)
        {
            this.AgentEmail = agentEmail;
            this.AgentName = agentName;
            this.IssueType = issueType;
        }
    }
}
