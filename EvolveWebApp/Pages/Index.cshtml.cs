﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EvolveWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private AppSettings _appSettings { get; set; }
        public IndexModel(ILogger<IndexModel> logger, IOptions<AppSettings> settings)
        {
            _logger = logger;
            _appSettings = settings.Value;           
        }

        public void OnGet()
        {
            ViewData["successMsg"] = null;
            ViewData["UserRequest"] = null;
        }

        public async Task<IActionResult> OnPostAsync(string employeeID)
        {
            ViewData["successMsg"] = null;
            ViewData["UserRequest"] = null;
            if (!string.IsNullOrEmpty(employeeID))
            {
                var Url = _appSettings.AzureFunctionURL;

                dynamic content = new { employeeId = employeeID };

                //CancellationToken cancellationToken;


                using (var client = new HttpClient())
                using (var request = new HttpRequestMessage(HttpMethod.Post, Url))
                using (var httpContent = CreateHttpContent(content))
                {
                    request.Content = httpContent;

                    using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                        .ConfigureAwait(false))
                    {

                        var resualtList = response.Content.ReadAsAsync<List<UserRequest>>();


                        ViewData["UserRequest"] = resualtList.Result;

                        return Page();
                    }
                }
            }
            else
            {
                ViewData["UserRequest"] = null;
                ViewData["successMsg"] = "Lab created successfully, An email was sent with instructions for accessing lab !!";
                return Page();
            }

        }

        public static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
    }
}
