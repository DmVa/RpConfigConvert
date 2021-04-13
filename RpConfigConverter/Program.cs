using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace RpConfigConverter
{
    class Program
    {
        static void Main(string[] args)
        {
			var changes = CreateChangesC1Api();
			//var changes = CreateChangesC1();
			SaveChanges(changes, args[1]);
            changes = LoadChanges(args[1]);
            List<Variable> variables = LoadVariables(args);
            if (changes.Variables == null)
                changes.Variables = new List<Variable>();
            if (changes.Changes == null)
                changes.Changes = new List<Change>();
            changes.Variables = MergeVariables(changes.Variables, variables);
            var converter = new ConfigConverter();
            converter.Convert(args[0], changes);
        }

        private static List<Variable> MergeVariables(List<Variable> initial, List<Variable> replace)
        {
            var result = new List<Variable>();
            foreach(var v in initial)
            {
                var merged = new Variable() { Name = v.Name, Value = v.Value };
                var replaceVal = replace.FirstOrDefault(x => x.Name == v.Name);
                if (replaceVal != null)
                    merged.Value = replaceVal.Value;
                result.Add(merged);
            }

            foreach (var v in replace)
            {
                var initVal = initial.FirstOrDefault(x => x.Name == v.Name);
                if (initVal == null)
                {
                    var merged = new Variable() { Name = v.Name, Value = v.Value };
                    result.Add(merged);
                }
            }
            return result;
        }

        private static List<Variable> LoadVariables(string[] args)
        {
            var result = new List<Variable>();
            if (args.Length > 2)
            {
                for(int i=2;i<args.Length;i++)
                {
                    var pair = args[i];
                    var pairParts = pair.Split(':');
                    var variable = new Variable() { Name = pairParts[0], Value = pairParts[1] };
                    result.Add(variable);
                }
            }
            return result;
        }

        private static ConfigChange LoadChanges(string fileName)
        {
            string jsonStr = File.ReadAllText(fileName);
            JavaScriptSerializer js = new JavaScriptSerializer();
            ConfigChange script = js.Deserialize<ConfigChange>(jsonStr);
            return script;
        }

        private static void SaveChanges(ConfigChange script, string stringFileName)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var jsonString = js.Serialize(script);
            jsonString = JSONFormatter.FormatOutput(jsonString);
            File.WriteAllText(stringFileName, jsonString);
        }

        private static ConfigChange CreateChangesC1()
        {
            var change = new ConfigChange();
            change.Variables = new List<Variable>();
            change.Changes = new List<Change>();
            change.Variables.Add(new Variable() { Name = "machine", Value = "DK01SV1547" });
            change.Changes.Add(new Change() { Value = "http://machine/C1Api/", Attribute = "value", XPath = "/configuration/appSettings/add[@key='api.Address']" });
			change.Changes.Add(new Change() { Value = "http://machine/CoricArchive", Attribute = "value", XPath = "/configuration/appSettings/add[@key='CoricArchivePath']" });
			change.Changes.Add(new Change() { Value = "http://machine/Help", Attribute = "value", XPath = "/configuration/appSettings/add[@key='C1HelpPageUrl']" });
			return change;
        }

		private static ConfigChange CreateChangesC1Api()
		{
			var change = new ConfigChange();
			change.Variables = new List<Variable>();
			change.Changes = new List<Change>();
			change.Variables.Add(new Variable() { Name = "machine", Value = "DK01SV1547" });
			change.Changes.Add(new Change() { Value = "http://machine/C1", Attribute = "value", XPath = "/configuration/appSettings/add[@key='C1Url']" });
			change.Changes.Add(new Change() { Value = "http://machine/ConfigurationServer/Connectivity.svc/basic", Attribute = "address", XPath = "/configuration/system.serviceModel/client/endpoint[@binding='basicHttpBinding']" });
			change.Changes.Add(new Change() { Value = "net.tcp://machine/CoricDataCommunicationService/MessageProcessor.svc/tcp", Attribute = "address", XPath = "/configuration/system.serviceModel/client/endpoint[@binding='netTcpBinding']" });
			return change;
		}

		private static ConfigChange CreateChangesArchive()
		{
			var change = new ConfigChange();
			change.Variables = new List<Variable>();
			change.Changes = new List<Change>();
			change.Variables.Add(new Variable() { Name = "machine", Value = "DK01SV1547" });
			change.Changes.Add(new Change() { Value = "http://machine/C1Api/", Attribute = "value", XPath = "/configuration/appSettings/add[@key='api.Address']" });
			return change;
		}
	}
}
