using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SysLogViewer.Models;
using SysLogViewer.Repositories;

namespace SysLogViewer.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IOptions<AppSettings> _appSettings;


        [BindProperty]
        public IList<SysLogModel> SysLogs { get; set; }

        [BindProperty]
        public SysLogTypes SysLogType { get; set; }

        [BindProperty]
        public DateTime? StartDate { get; set; }

        [BindProperty]
        public DateTime? EndDate { get; set; }

        [BindProperty]
        public string UrlString { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IOptions<AppSettings> appSettings)
        {
            _logger = logger;
            _appSettings = appSettings;
        }

        public async Task OnGetAsync()
        {
            using var db = new PleasanterDbClient(_appSettings.Value.ConnectionString, _appSettings.Value.CommandTimeout);
            var items = await db.GetSysLogs(StartDate, EndDate, SysLogType, UrlString);

            SysLogs = items.ToList();
        }

        public async Task OnPostAsync()
        {
            using var db = new PleasanterDbClient(_appSettings.Value.ConnectionString, _appSettings.Value.CommandTimeout);
            var items = await db.GetSysLogs(StartDate, EndDate, SysLogType, UrlString);

            SysLogs = items.ToList();
        }
    }
}
