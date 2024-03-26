using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jido.Components.Pages.Autopress.Commands
{
    /*public class CommandsDataTemplateSelector : IDataTemplate
    {
        [Content]
        public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new Dictionary<string, IDataTemplate>();

        public Control? Build(object? param)
        {
            var key = param?.ToString();
            if (key is null) // If the key is null, we throw an ArgumentNullException
            {
                throw new ArgumentNullException(nameof(param));
            }
            return AvailableTemplates[key].Build(param);
        }

        public bool Match(object? data)
        {
            var key = data?.ToString();
        }
    }*/
}
