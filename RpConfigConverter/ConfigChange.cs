using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpConfigConverter
{
    public class ConfigChange
    {
        public List<Variable> Variables { get; set; }
        public List<Change> Changes { get; set; }
    }
}
