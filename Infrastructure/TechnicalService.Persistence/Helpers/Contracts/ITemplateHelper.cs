using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnicalService.Persistence.Helpers.Contracts
{
    public interface ITemplateHelper
    {
        string GetTemplateContent(string templateName, Dictionary<string, string> replacements);
    }
}
