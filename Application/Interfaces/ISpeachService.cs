using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISpeachService
    {
        public Task<byte[]> Speech(string query, string languageCode); // Получение аудио-файла озвученного слова (через api Amazon Polly)
    }
}
