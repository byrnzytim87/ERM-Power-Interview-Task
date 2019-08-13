using System;
using System.Collections.Generic;
using System.Text;

namespace ERMPowerClientApp
{
    class LP
    {
        public string FileType = "LP";
        public string MeterPointCode { get; set; }
        public int SerialNumber { get; set; }
        public string PlantCode { get; set; }
        public DateTime Date { get; set; }
        public string DataType { get; set; }
        public float DataValue { get; set; }
        public string Units { get; set; }
        public string Status { get; set; }

        // Assumptions:
        // With the limited datasets available, I'm unsure as to the range of 
        // data that is required for each field.
        // I've used strings to ensure no data loss where I was unsure - in the real world,
        // some of these may become enums (Units, Status, DataType for example).
    }
}
