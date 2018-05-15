using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CustomVisionHouseFinder.Library {
	public static class CustomVisionService {
		private static byte[] getImageAsByteArray( string imageFilePath ) {
			using( var fileStream = new FileStream( imageFilePath, FileMode.Open, FileAccess.Read ) ) {
				using( var binaryReader = new BinaryReader( fileStream ) ) {
					return binaryReader.ReadBytes( (int)fileStream.Length );
				}
			}
		}

		public static async Task<PredictionResult> MakePredictionRequest( string imageFilePath ) {
			using( var client = new HttpClient() ) {
				client.DefaultRequestHeaders.Add( "Prediction-Key", "put your key here." );

				// Prediction URL - replace this example URL with your valid prediction URL.
				var url =
					"https://southcentralus.api.cognitive.microsoft.com/customvision/v2.0/Prediction/7d246add-7b86-435e-9aae-a6ac9856754e/image?iterationId=af322843-65ac-4d1a-b4d0-823c2ce6f6f0";


				// Request body. Try this sample with a locally stored image.
				var byteData = getImageAsByteArray( imageFilePath );

				using( var content = new ByteArrayContent( byteData ) ) {
					content.Headers.ContentType = new MediaTypeHeaderValue( "application/octet-stream" );
					using( var response = await client.PostAsync( url, content ) ) {
						var responseString = await response.Content.ReadAsStringAsync();
						var makePredictionRequest = JsonConvert.DeserializeObject<PredictionResult>( responseString );
						return makePredictionRequest;
					}
				}
			}
		}
	}
}