using System;
using System.Collections.Generic;
using System.Text;

namespace SuperShortLink.Api.Models
{
    internal class ShortLinkGenerateDto : ShortLinkBaseDto
    {
        public string generate_url { get; set; }
    }
}
