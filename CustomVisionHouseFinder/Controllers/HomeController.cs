using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CustomVisionHouseFinder.Library;
using CustomVisionHouseFinder.Models;
using CustomVisionHouseFinder.Models.Home;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CustomVisionHouseFinder.Controllers {
	public class HomeController: Controller {
		private readonly IHostingEnvironment hostingEnvironment;
		private readonly IMemoryCache cache;

		public HomeController( IHostingEnvironment hostingEnvironment, IMemoryCache memoryCache ) {
			this.hostingEnvironment = hostingEnvironment;
			this.cache = memoryCache;
		}

		public async Task<IActionResult> Index() {
			var contentRootPath = hostingEnvironment.ContentRootPath;

			var model = new List<HouseModel>();
			foreach( var file in Directory.EnumerateFiles( Path.Combine( contentRootPath, "wwwroot/images/Houses" ) ) ) {
				model.Add(
					new HouseModel
						{
							Name = "A",
							HasThirdStoryWindow = processPrediction( await getPrediction( file ) ),
							ImageUrl = Url.Content( "~/images/Houses/" + Path.GetFileName( file ) )
						} );
			}

			return View( model );
		}

		private async Task<PredictionResult> getPrediction( string file ) {
			if( cache.TryGetValue( file, out PredictionResult prediction ) )
				return prediction;

			var predictionResult = await CustomVisionService.MakePredictionRequest( file );
			cache.Set( file, predictionResult );
			return predictionResult;
		}

		private bool processPrediction( PredictionResult predictionResult ) {
			return ( predictionResult.Predictions.SingleOrDefault( p => p.TagName == "has-third-story-window" )?.Probability ?? 0 ) >
			       ( predictionResult.Predictions.SingleOrDefault( p => p.TagName == "no-third-story-window" )?.Probability ?? 0 );
		}

		public IActionResult Error() {
			return View( new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier } );
		}
	}
}