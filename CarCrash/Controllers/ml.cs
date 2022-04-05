using Microsoft.AspNetCore.Mvc;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Controllers
{

    [ApiController]
    [Route("/score")]
    public class InferenceController : ControllerBase
    {
        private InferenceSession _session;

        public InferenceController(InferenceSession session)
        {
            _session = session;
        }

        [HttpPost]
        public ActionResult Score(CrashData data)
        {
            var result = _session.Run(new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor("float_input", data.AsTensor())
            });
            Tensor<float> score = result.First().AsTensor<float>();
            var prediction = new Prediction { PredictedValue = score.First() * 100000 };
            result.Dispose();
            return Ok(prediction);
        }
    }

    public class CrashData
    {
        public float pedestrian_involved { get; set; }
        public float bicyclist_involved { get; set; }
        public float motorcycle_involved { get; set; }
        public float improper_restraint { get; set; }
        public float dui { get; set; }
        public float intersection_related { get; set; }
        public float overturn_rollover { get; set; }
        public float older_driver_involved { get; set; }
        public float route_173 { get; set; }
        public float route_356700 { get; set; }


        public Tensor<float> AsTensor()
        {
            float[] data = new float[]
            {
            pedestrian_involved, bicyclist_involved, motorcycle_involved, improper_restraint,
            dui, intersection_related, overturn_rollover, older_driver_involved, route_173, route_356700
            };
            int[] dimensions = new int[] { 1, 10 };
            return new DenseTensor<float>(data, dimensions);
        }
    }
    public class Prediction
    {
        public float PredictedValue { get; set; }
    }
}