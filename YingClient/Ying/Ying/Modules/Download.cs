using KMCCC.Launcher;
using KMCCC.Modules.JVersion;
using LitJson;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ying;

namespace Ying.Modules
{
    interface IDownloadBaseUrl
    {
        string VersionListUrl { get; }
        string VersionBaseUrl { get; }
        string LibraryBaseUrl { get; }
        string MavenBaseUrl { get; }
        string JsonBaseUrl { get; }
        string AssetsBaseUrl { get; }
        string ForgeBaseUrl { get; }
    }

    class BMCLAPIBaseUrl : IDownloadBaseUrl
    {
        public string VersionListUrl => "https://bmclapi2.bangbang93.com/mc/game/version_manifest.json";
        public string VersionBaseUrl => "https://bmclapi2.bangbang93.com/";
        public string LibraryBaseUrl => "https://bmclapi2.bangbang93.com/libraries/";
        public string MavenBaseUrl => "https://bmclapi2.bangbang93.com/maven/";
        public string JsonBaseUrl => "https://bmclapi2.bangbang93.com/";
        public string AssetsBaseUrl => "https://bmclapi2.bangbang93.com/assets/";
        public string ForgeBaseUrl => "https://bmclapi2.bangbang93.com/maven/net/minecraftforge/forge/";
    }

    class OfficialBaseUrl : IDownloadBaseUrl
    {
        public string VersionListUrl => "https://launchermeta.mojang.com/mc/game/version_manifest.json";
        public string VersionBaseUrl => "https://launcher.mojang.com/";
        public string LibraryBaseUrl => "https://libraries.minecraft.net/";
        public string MavenBaseUrl => "https://files.minecraftforge.net/maven/";
        public string JsonBaseUrl => "https://launchermeta.mojang.com/";
        public string AssetsBaseUrl => "https://resources.download.minecraft.net/";
        public string ForgeBaseUrl => "https://files.minecraftforge.net/maven/net/minecraftforge/forge/";
    }

    public class DownloadInfo
    {
        public string Path { get; set; }
        public string Url { get; set; }
        public int Size { get; set; }
    }

    static class DownloadHelper
    {
        public static IDownloadBaseUrl BaseUrl { get; private set; }

        public static void SetDownloadSource(int sourceType)
        {
            switch (sourceType)
            {
                case 0:
                    BaseUrl = new OfficialBaseUrl();
                    break;

                case 1:
                    BaseUrl = new BMCLAPIBaseUrl();
                    break;
            }
        }

        public static IEnumerable<DownloadInfo> GetLostEssentials(Version version)
        {
            var lostEssentials = new List<DownloadInfo>();

            var JarPath = $"{YingApp.YCore.GameRootPath}\\versions\\{version.JarId}\\{version.JarId}.jar";
            if (!File.Exists(JarPath))
            {
                lostEssentials.Add(new DownloadInfo
                {
                    Path = JarPath,
                    Url = BaseUrl.VersionBaseUrl + version.Downloads.Client.Url.Substring(28),
                    Size = version.Downloads.Client.Size,
            });
            }

            /*foreach (var lib in version.Libraries)
            {
                JFileInfo yinfo = lib.YDownloadInfo.Artifact;
                var absolutePath = $"{YingApp.YCore.GameRootPath}\\libraries\\{yinfo.Path}";
                if (!File.Exists(absolutePath))
                {
                    lostEssentials.Add(new DownloadInfo
                    {
                        Path = absolutePath,
                        Url = (lib.IsForgeLib) ? (BaseUrl.MavenBaseUrl + yinfo.Path) : (BaseUrl.LibraryBaseUrl + yinfo.Path),
                        Size = yinfo.Size,
                    });
                }
            }*/

            foreach (var ylibrary in version.Libraries)
            {
                if (!ylibrary.isYForgeLib)
                {
                    JFileInfo yinfo = ylibrary.YDownloadInfo.Artifact;
                    var absolutePath = $"{YingApp.YCore.GameRootPath}\\libraries\\{yinfo.Path}";
                    if (!File.Exists(absolutePath))
                    {
                        lostEssentials.Add(new DownloadInfo
                        {
                            Path = absolutePath,
                            Url = BaseUrl.LibraryBaseUrl + yinfo.Path,
                            Size = yinfo.Size,
                        });
                    }
                }
                else
                {
                    //"name": "net.minecraft:launchwrapper:1.12",
                    //https://bmclapi2.bangbang93.com/maven/net/minecraft/launchwrapper/1.12/launchwrapper-1.12.jar
                    //string[] ytemp = ylibrary.NS.Split(':');

                    string ypath = $"{ylibrary.NS.Replace(".", "/")}/{ylibrary.Name}/{ylibrary.Version}/{ylibrary.Name}-{ylibrary.Version}.jar";
                    var absolutePath = $"{YingApp.YCore.GameRootPath}\\libraries\\{ypath}";
                    if (!File.Exists(absolutePath))
                    {
                        lostEssentials.Add(new DownloadInfo
                        {
                            Path = absolutePath,
                            Url = BaseUrl.MavenBaseUrl + ypath,
                        });
                    }
                }
            }

            foreach (var ynative in version.Natives)
            {
                JFileInfo yinfo = ynative.YDownloadInfo.Classifiers["natives-windows"];
                var absolutePath = YingApp.YCore.GetNativePath(ynative);//$"{YingApp.YCore.GameRootPath}\\libraries\\{native.Path}";
                if (!File.Exists(absolutePath))
                {
                    lostEssentials.Add(new DownloadInfo
                    {
                        Path = absolutePath,
                        Url = BaseUrl.LibraryBaseUrl + yinfo.Path,
                        Size = yinfo.Size,
                    });
                }
            }
            return lostEssentials;
        }

        public class Assets
        {
            [JsonPropertyName("objects")]
            public Dictionary<string, Asset> Objects { get; set; }
        }

        public class Asset
        {
            [JsonPropertyName("hash")]
            public string Hash { get; set; }

            [YingJsonIgnore]
            public string HashPrefix { get => Hash.Substring(0, 2); }

            [JsonPropertyName("size")]
            public int Size { get; set; }
        }

        public static IEnumerable<DownloadInfo> GetLostAssets(Version version)
        {
            var lostAssets = new List<DownloadInfo>();

            var indexPath = $"{YingApp.YCore.GameRootPath}\\assets\\indexes\\{version.AssetsIndex.ID}.json";
            string indexJson;

            if (!File.Exists(indexPath))
            {
                try
                {
                    string indexUrl;
                    if (version.AssetsIndex.Url != null)
                    {
                        indexUrl = BaseUrl.JsonBaseUrl + version.AssetsIndex.Url.Substring(32);
                    }
                    else
                    {
                        indexUrl = $"{BaseUrl.JsonBaseUrl}indexs/{version.AssetsIndex.ID}.json";
                    }

                    var client = new System.Net.Http.HttpClient() { Timeout = new System.TimeSpan(0, 0, 5) };
                    indexJson = client.GetStringAsync(indexUrl).Result;
                    client.Dispose();
                }
                catch
                {
                    System.Windows.MessageBox.Show("获取资源列表失败!");
                    return lostAssets;
                }

                if (!Directory.Exists(Path.GetDirectoryName(indexPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(indexPath));
                }
                File.WriteAllText(indexPath, indexJson);
            }
            else
            {
                indexJson = File.ReadAllText(indexPath);
            }

            foreach(var asset in JsonMapper.ToObject<Assets>(indexJson).Objects)
            {
                var relativePath = $"{asset.Value.HashPrefix}\\{asset.Value.Hash}";
                var absolutePath = (version.AssetsIndex.ID == "legacy") ? $"{YingApp.YCore.GameRootPath}\\assets\\virtual\\legacy\\{asset.Key}" 
                                                                  : $"{YingApp.YCore.GameRootPath}\\assets\\objects\\{relativePath}";

                if (!File.Exists(absolutePath))
                {
                    lostAssets.Add(new DownloadInfo
                    {
                        Path = absolutePath,
                        Url = BaseUrl.AssetsBaseUrl + relativePath,
                        Size = asset.Value.Size,
                    });
                }
            }

            return lostAssets;
        }
    }
}
