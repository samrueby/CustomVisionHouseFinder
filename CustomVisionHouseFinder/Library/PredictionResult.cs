using System;
using System.Collections.Generic;

namespace CustomVisionHouseFinder.Library {
	public class Prediction {
		public string TagId { get; set; }
		public string TagName { get; set; }
		public double Probability { get; set; }
	}

	public class PredictionResult {
		public string Id { get; set; }
		public string Project { get; set; }
		public string Iteration { get; set; }
		public DateTime Created { get; set; }
		public List<Prediction> Predictions { get; set; }
	}
}