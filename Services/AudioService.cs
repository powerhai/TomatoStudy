using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Media;
using Contract;
namespace Services
{
    public class AudioService : IAudioService
    {
        private const string AudioUrl =
            "http://dict.youdao.com/dictvoice?audio={0}&type=1&client=deskdict&id=7000b664c615fcd3f&vendor=unknown&in=YoudaoDict&appVer=6.3.69.5012&appZengqiang=0&abTest=4";
        public async Task PlaySound (string word) {
            try {
                var fileName = $"Audio/{word}.avi";
                var file = new FileInfo(fileName);
                if(!file.Directory.Exists)
                    file.Directory.Create();
                if(!file.Exists) {
                    var url = string.Format(AudioUrl, word.Trim());
                    var client = new HttpClient();
                    var stream = File.Open(fileName, FileMode.CreateNew);
                    var bys = await client.GetByteArrayAsync(url);
                    await stream.WriteAsync(bys, 0, bys.Length);
                    stream.Close();
                }
                var player = new MediaPlayer();
                player.Open(new Uri(fileName, UriKind.Relative));
                player.Play();
            }
            catch(Exception e) {}
        }
    }
}
