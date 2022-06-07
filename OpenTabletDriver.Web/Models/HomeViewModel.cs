using System.Collections.Generic;
using OpenTabletDriver.Web.Core.Contracts;
using OpenTabletDriver.Web.Core.Services;

namespace OpenTabletDriver.Web.Models
{
    public class HomeViewModel
    {
        public IList<IRelease> Releases { set; get; }
    }
}
