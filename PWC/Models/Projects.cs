using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace PWC.Models
{
    public class Projects
    {
        public string name { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? dueDate { get; set; }
        public string unit { get; set; }
        public string unitSim { get; set; }
        public string manager { get; set; }
        public string staff { get; set; }
        public string avatar { get; set; }
        public string weekInWords { get; set; }
    }
    [DataContract]
    public class PieChartRange
    {
        public PieChartRange(double y, string label)
        {
            this.y = y;
            this.label = label;
        }
        //Explicitly setting the name to be used while serializing to JSON.
        [DataMember(Name = "y")]
        public Nullable<double> y = null;
        [DataMember(Name = "label")]
        public string label = null;
    }
    

}
