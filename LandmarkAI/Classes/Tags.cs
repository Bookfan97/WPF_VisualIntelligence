using System;
using System.Collections.Generic;
using System.Text;

namespace LandmarkAI.Classes
{
    public class Predicition
    {
        public string TagID { get; set; }
        public string Tag { get; set; }
        public double Probability { get; set; }
    }

    public class CustomVision
    {
        public string ID { get; set; }
        public string Project { get; set; }
        public string Iteration { get; set; }
        public DateTime Created { get; set; }
        public IList<Predicition> Predictions { get; set; }
    }
}