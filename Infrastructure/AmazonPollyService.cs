using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class AmazonPollyService : ISpeachService
    {
        private ApplicationDbContext Database;
        private ILanguageService languageService;
        public AmazonPollyService(ApplicationDbContext db, ILanguageService languageService)
        {
            Database = db;
            this.languageService = languageService;
        }

        public async Task<byte[]> Speech(string query, string languageCode) // Получение аудио-файла озвученного слова (через api Amazon Polly)
        {
            var lang = languageService.GetLanguage(languageCode);
            //============================================================

            IConfigurationBuilder builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            IConfigurationSection configurationSection = configuration.GetSection("AWS_Polly");

            string awsAccessKeyId = configurationSection.GetValue<string>("awsAccessKeyId");
            string awsSecretAccessKey = configurationSection.GetValue<string>("awsSecretAccessKey");

            //============================================================

            var client = new AmazonPollyClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USEast2);

            var synthesizeSpeechRequest = new SynthesizeSpeechRequest()
            {
                OutputFormat = OutputFormat.Mp3,
                LanguageCode = lang.LanguageCode,
                VoiceId = VoiceId.FindValue(lang.VoiceId), // .Joanna,
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
