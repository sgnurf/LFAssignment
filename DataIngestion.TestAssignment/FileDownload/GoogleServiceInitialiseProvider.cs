using DataIngestion.TestAssignment.Configuration;
using Google.Apis.Services;
using Microsoft.Extensions.Options;

namespace DataIngestion.TestAssignment.FileDownload
{
    public class GoogleServiceInitialiseProvider : IGoogleServiceInitialiseProvider
    {
        private readonly GoogleApiConfiguration googleApiConfiguration;

        public GoogleServiceInitialiseProvider(IOptions<GoogleApiConfiguration> googleApiConfiguration)
        {
            this.googleApiConfiguration = googleApiConfiguration.Value;
        }

        public BaseClientService.Initializer GetInitialiser()
        {
            return new BaseClientService.Initializer()
            {
                ApiKey = googleApiConfiguration.ApiKey,
                ApplicationName = googleApiConfiguration.ApplicationName
            };
        }
    }
}