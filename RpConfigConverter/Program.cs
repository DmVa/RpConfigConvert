using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpConfigConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var changes = CreateChanges();
        }

        private static ConfigChange CreateChanges()
        {
            var change = new ConfigChange();
            change.Variables = new List<Variable>();
            change.Variables.Add(new Variable() { Name = "Machine", Value = "DK03SV001547" });
            change.Changes.Add(new Change() { Value = "http://Machine/C1Api/", Attribute = "Value", XPath = "/configuration/appSettings/add[@key='api.Address']" });
            return change;
        }
    }
}
