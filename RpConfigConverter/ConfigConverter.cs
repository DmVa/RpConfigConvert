using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RpConfigConverter
{
    public class ConfigConverter
    {
        public void Convert(string configFile, ConfigChange changeDefinition)
        {
         
                if (!File.Exists(configFile))
                    throw new FileNotFoundException($"file {configFile} not found");
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.PreserveWhitespace = true;
                xmlDoc.Load(configFile);

                foreach (var configChange in changeDefinition.Changes)
                {
                    ApplyConfigChange(xmlDoc, configChange, changeDefinition.Variables);
                }

                xmlDoc.Save(configFile);
            
        }
        private void ApplyConfigChange(XmlDocument configDoc, Change configChange, List<Variable> variables)
        {
            XmlNode node = configDoc.SelectSingleNode(configChange.XPath);
            if (node == null)
            {
                throw new ApplicationException($"node {configChange.XPath} not found");
            }

            if (!string.IsNullOrEmpty(configChange.Attribute))
            {
                if (node.Attributes == null)
                {
                    throw new ApplicationException(
                        $"node {configChange.XPath} does not have attributes");
                }
                XmlAttribute attributeToChange = node.Attributes[configChange.Attribute];
                if (attributeToChange == null)
                {
                    throw new ApplicationException(
                        $"node {configChange.XPath} does not have attribute {configChange.Attribute}");
                }
                attributeToChange.Value = GetConfigChangeValue(configChange.Value, variables);
            }
        }

        private string GetConfigChangeValue(string value, List<Variable> variables)
        {
            string newValue = value;
            foreach(var replacement in variables)
            {
                newValue = newValue.Replace(replacement.Name, replacement.Value);
            }

            return newValue;
        }
    }
}
