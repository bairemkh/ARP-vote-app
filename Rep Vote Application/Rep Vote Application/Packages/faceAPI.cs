using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rep_Vote_Application.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;

namespace Rep_Vote_Application.Packages
{
    public class faceAPI
    {
        private string APIKEY = "85b409dd53444c8a8d117c9800efa4ed";
        private string ENDPOINT = "https://votefaceapi.cognitiveservices.azure.com/";
        private FaceClient faceClient;
        public static IEnumerable<DetectedFace> faceApiResponseList;

        public faceAPI()
        {
            InitFaceClient();
        }

        void InitFaceClient()
        {
            ApiKeyServiceClientCredentials credentials = new ApiKeyServiceClientCredentials(APIKEY);
            faceClient = new FaceClient(credentials);
            faceClient.Endpoint = ENDPOINT;
            FaceOperations faceOperations = new FaceOperations(faceClient);
        }


        //Take picture (sourceImage)
        Stream ImgSource;

        Command takeImage;
        public Command TakeImage
        {
            get { return takeImage ?? (takeImage = new Command(async () => await TakeImageAsync())); }
        }
        public async Task TakeImageAsync()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available.", "OK");
                   
                }

                MediaFile photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    CompressionQuality = 50,
                    PhotoSize = PhotoSize.Small,
                    Name = "photo.jpg",
                    DefaultCamera = CameraDevice.Front,
                    SaveToAlbum = false,
                    SaveMetaData = false,
                    
                });
                
                ImgSource = photo.GetStream();
                
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"ERROR: {ex.Message}", "OK");
            }

        }
        

        //Face detection
        private static async Task<List<DetectedFace>> DetectFaceRecognize(IFaceClient faceClient, Stream image, string recognition_model)
        {
            IList<DetectedFace> detectedFace = await faceClient.Face.DetectWithStreamAsync(image, recognitionModel: recognition_model, detectionModel: DetectionModel.Detection03);
            Console.WriteLine($"{detectedFace.Count} face(s) detected from image");
            return detectedFace.ToList();
        }

        //Face recognition
        public static async Task FindSimilar(IFaceClient client, Stream Camera_photo, string recognition_model)
        {
            Console.WriteLine("Face Recognition Started");
            Console.WriteLine();

            //Target image => Byte to stream
            Stream targetImageFile = new MemoryStream(Getters.CurrentUser.UserImage);

            Stream sourceImageFile = Camera_photo;

            IList<Guid?> targetFaceIds = new List<Guid?>();
            
            // Detect faces from target image
            var faces = await DetectFaceRecognize(client, targetImageFile, recognition_model);

            // Add detected faceId to list of GUIDs.
            targetFaceIds.Add(faces[0].FaceId.Value);

            // Detect faces from source image.
            IList<DetectedFace> detectedFaces = await DetectFaceRecognize(client, sourceImageFile, recognition_model);
            Console.WriteLine();

            // Find a similar face.
            IList<SimilarFace> similarResults = await client.Face.FindSimilarAsync(detectedFaces[0].FaceId.Value, null, null, targetFaceIds);
           
            foreach (var similarResult in similarResults)
            {
                Console.WriteLine($"Face ID:{similarResult.FaceId}  similar with confidence: {similarResult.Confidence}.");
            }
            Console.WriteLine();
        }
    }
}
