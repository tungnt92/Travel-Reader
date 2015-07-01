using AppTourDuLich.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The data model defined by this file serves as a representative example of a strongly-typed
// model.  The property names chosen coincide with data bindings in the standard item templates.
//
// Applications may use this model as a starting point and build on it, or discard it entirely and
// replace it with something appropriate to their needs. If using this model, you might improve app 
// responsiveness by initiating the data loading task in the code behind for App.xaml when the app 
// is first launched.

namespace AppTourDuLich.Data
{
    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem
    {
        public SampleDataItem(String uniqueId, String title, String date, String description, String link, String imagepath)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Date = date;
            this.Description = description;
            this.ImagePath = imagepath;
            this.Link = link;
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public string Date { get; private set; }
        public string Description { get; private set; }
        public string Link { get; private set; }
        public string ImagePath { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Generic group data model.
    /// </summary>
    public class SampleDataGroup
    {
        public SampleDataGroup(String uniqueId, String title)
        {
            this.UniqueId = uniqueId;
            this.Title = title;
            this.Items = new ObservableCollection<SampleDataItem>();
        }

        public string UniqueId { get; private set; }
        public string Title { get; private set; }
        public ObservableCollection<SampleDataItem> Items { get; private set; }

        public override string ToString()
        {
            return this.Title;
        }
    }

    /// <summary>
    /// Creates a collection of groups and items with content read from a static json file.
    /// 
    /// SampleDataSource initializes with data read from a static json file included in the 
    /// project.  This provides sample data at both design-time and run-time.
    /// </summary>
    public sealed class SampleDataSource
    {
        string apiUrl1 = "http://www.vietravel.com.vn/vn/rss/tour-hot-khuyen-mai.aspx";
        string apiUrl2 = "http://www.vietravel.com.vn/vn/rss/tour-du-lich-moi-nhat.aspx";
        string apiUrl3 = "http://www.vietravel.com.vn/vn/rss/am-thuc-kham-pha.aspx";
        private static SampleDataSource _sampleDataSource = new SampleDataSource();

        private ObservableCollection<SampleDataGroup> _groups = new ObservableCollection<SampleDataGroup>();
        public ObservableCollection<SampleDataGroup> Groups
        {
            get { return this._groups; }
        }

        public static async Task<IEnumerable<SampleDataGroup>> GetGroupsAsync()
        {
            await _sampleDataSource.GetSampleDataAsync();

            return _sampleDataSource.Groups;
        }

        public static async Task<SampleDataGroup> GetGroupAsync(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groups.Where((group) => group.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        public static async Task<SampleDataItem> GetItemAsync(string uniqueId)
        {
            await _sampleDataSource.GetSampleDataAsync();
            // Simple linear search is acceptable for small data sets
            var matches = _sampleDataSource.Groups.SelectMany(group => group.Items).Where(item => item.UniqueId.Equals(uniqueId));
            if (matches.Count() == 1) return matches.First();
            return null;
        }

        private async Task GetSampleDataAsync()
        {
            if (this._groups.Count != 0)
                return;
            try
            {
                RssReader rss = new RssReader();
                HttpClient Client = new HttpClient();
                Client.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                var xml = await Client.GetStringAsync(apiUrl1 +"?cache="+Guid.NewGuid().ToString());
                var ketqua = rss.KetQua(xml);
                int i = 0;
                SampleDataGroup group = new SampleDataGroup("Group-1", "Tour Hot Khuyến Mãi");
                foreach (var k in ketqua)
                {
                    if (!k.Link.Contains("http:") && !k.Link.Contains("https:"))
                    {
                        k.Link = "http:" + k.Link;
                    }
                    string ip = k.Description.Substring(k.Description.LastIndexOf("src") + 5, k.Description.IndexOf("'", k.Description.LastIndexOf("src") + 6) - 5 - k.Description.LastIndexOf("src"));
                    if(!ip.Contains("http:")&&!ip.Contains("https:"))
                    {
                        ip = "http:" + ip;
                    }
                    group.Items.Add(new SampleDataItem(i.ToString(), k.Title, k.Date, k.Description.Substring(k.Description.LastIndexOf("</a>") + 4, k.Description.Length - k.Description.LastIndexOf("</a>") - 4), k.Link, ip));
                    i++;
                }
                this.Groups.Add(group);
                xml = await Client.GetStringAsync(apiUrl2 + "?cache=" + Guid.NewGuid().ToString());
                ketqua = rss.KetQua(xml);
                group = new SampleDataGroup("Group-2", "Tour Du Lịch Mới Nhất");
                foreach (var k in ketqua)
                {
                    if (!k.Link.Contains("http:") && !k.Link.Contains("https:"))
                    {
                        k.Link = "http:" + k.Link;
                    }
                    string ip = k.Description.Substring(k.Description.LastIndexOf("src") + 5, k.Description.IndexOf("'", k.Description.LastIndexOf("src") + 6) - 5 - k.Description.LastIndexOf("src"));
                    if (!ip.Contains("http:") && !ip.Contains("https:"))
                    {
                        ip = "http:" + ip;
                    }
                    group.Items.Add(new SampleDataItem(i.ToString(), k.Title, k.Date, k.Description.Substring(k.Description.LastIndexOf("</a>") + 4, k.Description.Length - k.Description.LastIndexOf("</a>") - 4),k.Link, ip));
                    i++;
                }
                this.Groups.Add(group);
                xml = await Client.GetStringAsync(apiUrl3 + "?cache=" + Guid.NewGuid().ToString());
                ketqua = rss.KetQua(xml);
                group = new SampleDataGroup("Group-3", "Ẩm Thực Khám Phá");
                foreach (var k in ketqua)
                {
                    if (!k.Link.Contains("http:") && !k.Link.Contains("https:"))
                    {
                        k.Link = "http:" + k.Link;
                    }
                    string ip = k.Description.Substring(k.Description.LastIndexOf("src") + 5, k.Description.IndexOf("'", k.Description.LastIndexOf("src") + 6) - 5 - k.Description.LastIndexOf("src"));
                    if (!ip.Contains("http:") && !ip.Contains("https:"))
                    {
                        ip = "http:" + ip;
                    }
                    group.Items.Add(new SampleDataItem(i.ToString(), k.Title, k.Date, k.Description.Substring(k.Description.LastIndexOf("</a>") + 4, k.Description.Length - k.Description.LastIndexOf("</a>") - 4), k.Link, ip));
                    i++;
                }
                this.Groups.Add(group);
                Uri dataUri = new Uri("ms-appx:///DataModel/AboutUs.json");
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
                string jsonText = await FileIO.ReadTextAsync(file);
                JsonObject jsonObject = JsonObject.Parse(jsonText);
                JsonArray jsonArray = jsonObject["Items"].GetArray();
                group = new SampleDataGroup("Group-4", "Mô Tả");
                foreach (JsonValue itemValue in jsonArray)
                {
                    JsonObject itemObject = itemValue.GetObject();
                    group.Items.Add(new SampleDataItem(i.ToString(),
                                                       itemObject["Title"].GetString(),
                                                       itemObject["Type"].GetString(),
                                                       "description",
                                                       itemObject["Target"].GetString(),
                                                       itemObject["Icon"].GetString()));
                    i++;
                }
                this.Groups.Add(group);
            }
            catch (Exception ex)
            {
                MessageDialogHelper msg = new MessageDialogHelper();
                msg.Show("Vui Lòng kiểm tra kết nối mạng", "Lỗi kết nối!");
            }
        }
    }
}