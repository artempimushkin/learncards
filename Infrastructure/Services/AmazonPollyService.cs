using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AWSPollyOptions
    {
        public string awsAccessKeyId { get; set; }
        public string awsSecretAccessKey { get; set; }
    }
    public class AmazonPollyService : ISpeachService
    {
        private ApplicationDbContext Database;
        private ILanguageService _languageService;
        private readonly IOptions<AWSPollyOptions> _awsPollyOptions;
        public AmazonPollyService(ApplicationDbContext db, ILanguageService languageService, IOptions<AWSPollyOptions> awsPollyOptions)
        {
            Database = db;
            _languageService = languageService;
            _awsPollyOptions = awsPollyOptions;
        }

        public async Task<byte[]> Speech(string query, string languageCode) // Получение аудио-файла озвученного слова (через api Amazon Polly)
        {
            var lang = _languageService.GetLanguage(languageCode);

            var client = new AmazonPollyClient(_awsPollyOptions.Value.awsAccessKeyId, _awsPollyOptions.Value.awsSecretAccessKey, RegionEndpoint.USEast2);

            var synthesizeSpeechRequest = new SynthesizeSpeechRequest()
            {
                OutputFormat = OutputFormat.Mp3,
                LanguageCode = lang.LanguageCode,
                VoiceId = VoiceId.FindValue(lang.VoiceId),
                Text = query
            };

            var synthesizeSpeechResponse = await client.SynthesizeSpeechAsync(synthesizeSpeechRequest);
            var inputStream = synthesizeSpeechResponse.AudioStream;

            MemoryStream mem = new MemoryStream();
            inputStream.CopyTo(mem);
            return mem.ToArray();
        }
    }
}
