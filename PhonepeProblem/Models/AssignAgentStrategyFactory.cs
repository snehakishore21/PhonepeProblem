using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonepeProblem.Models
{
    internal class AssignAgentStrategyFactory
    {
        public static IAgentSelectionStrategy GetStrategyToSelectAgent(ICustomerIssue customerIssue)
        {
            return new DefaultAgentSelectionStrategy();
        }
    }
}
