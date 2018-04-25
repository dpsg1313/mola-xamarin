using Akavache;
using Amazon.S3;
using Amazon.S3.Model;
using MolaApp.Model;
using Splat;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MolaApp.Api
{
    class ImageApi : IImageApi
    {
        const string NAME = "image";
        const string BUCKET = "mola-images";

        IAmazonS3 client;

        public ImageApi(IAmazonS3 s3Client)
        {
            client = s3Client;
        }

        private async Task<byte[]> GetRemoteAsync(string id)
        {
            // Create a GetObject request
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = BUCKET,
                Key = id + ".jpg"
            };

            // Issue request and remember to dispose of the response
            using (GetObjectResponse response = await client.GetObjectAsync(BUCKET, id + ".jpg"))
            {
                byte[] buffer;
                using (BinaryReader br = new BinaryReader(response.ResponseStream))
                {
                    buffer = br.ReadBytes((int)response.ResponseStream.Length);
                    return buffer;
                }
            }
        }

        public IObservable<ImageModel> Get(string id)
        {
            string key = NAME + "_" + id;
            var cache = BlobCache.LocalMachine;
            return Observable.Select(
                cache.GetAndFetchLatest(
                    key,
                    async () => await GetRemoteAsync(id)
                ),
                promised =>
                {
                    var model = new ImageModel(id);
                    model.Bytes = promised;
                    return model;
                }
                );
        }

        public async Task<ImageModel> GetAsync(string id)
        {
            var cachedPromise = Get(id);

            // The subscribe is necessary, otherwise the object is never fetched
            cachedPromise.Subscribe(subscribed => {});
            
            return await cachedPromise.FirstOrDefaultAsync();
        }

        public async Task<bool> PutAsync(ImageModel model)
        {
            // Create a PutObject request
            PutObjectRequest request = new PutObjectRequest
            {
                BucketName = BUCKET,
                Key = model.Id + ".jpg",
                InputStream = new MemoryStream(model.Bytes),
                ContentType = "image/jpeg"
            };

            // Put object
            PutObjectResponse response = await client.PutObjectAsync(request);
            if(response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }
    }
}
