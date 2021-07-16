using System.Collections.Generic;
using OpenTabletDriver.Web.Core.Services;
using OpenTabletDriver.Web.Core.Contracts;

namespace OpenTabletDriver.Web.Models
{
    public class HomeViewModel
    {
        public IList<IRelease> Releases { set; get; }
    }
}