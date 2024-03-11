using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem.Models
{
    public interface IAgentSelectionStrategy
    {
        public (ICustomerAgent, bool) GetAgentForIssue(ICustomerIssue issue, Dictionary<string, ICustomerAgent> agents);
    }

    public class DefaultAgentSelectionStrategy: IAgentSelectionStrategy
    {
        public (ICustomerAgent, bool) GetAgentForIssue(ICustomerIssue issue, Dictionary<string, ICustomerAgent> agents)
        {
            foreach(var agent in agents)
            {
                if(agent.Value.AgentStatus == AgentStatus.FREE)
                {
                    return (agent.Value, false);
                }
            }
            return (agents.First().Value, true);
        }
    }
}
