using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Models
{
    public class Crashd
    {
        public float pedestrian_involved { get; set; }
        public float bicyclist_involved { get; set; }
        public float motorcycle_involved { get; set; }
        public float improper_restraint { get; set; }
        public float dui { get; set; }
        public float intersection_related { get; set; }
        public float overturn_rollover { get; set; }
        public float unrestrained { get; set; }
        public float single_vehicle { get; set; }
        public float drowsy_driving { get; set; }
        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
            pedestrian_involved, bicyclist_involved, motorcycle_involved, improper_restraint,
            dui, intersection_related, overturn_rollover, unrestrained, single_vehicle, drowsy_driving
            };
            int[] dimensions = new int[] { 1, 10 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
}
