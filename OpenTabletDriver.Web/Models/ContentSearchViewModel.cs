using Microsoft.AspNetCore.Html;

namespace OpenTabletDriver.Web.Models
{
    public class ContentSearchViewModel
    {
        public IHtmlContent Content { set; get; }
        public string Search { set; get; }
    }
}
