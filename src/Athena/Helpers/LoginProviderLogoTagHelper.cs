using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Athena.Helpers
{
    [HtmlTargetElement("login-provider-logo", TagStructure = TagStructure.WithoutEndTag)]
    public class LoginProviderLogoTagHelper : TagHelper
    {
        private static readonly Dictionary<string, string> ProviderMap = new Dictionary<string, string>
        {
            {GoogleDefaults.AuthenticationScheme, "~/images/signin_google.png"}
        };

        private readonly IUrlHelperFactory _urlHelper;

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }
        
        [HtmlAttributeName("provider")]
        public string Provider { get; set; }

        public LoginProviderLogoTagHelper(IUrlHelperFactory urlHelper) =>
            _urlHelper = urlHelper ?? throw new ArgumentNullException(nameof(urlHelper));
        
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ProviderMap.TryGetValue(Provider ?? "", out var img))
            {
                var path = _urlHelper.GetUrlHelper(ViewContext).Content(img);
                output.TagName = "img";
                output.Attributes.SetAttribute("src", path);
                output.Attributes.SetAttribute("class", "center-align center-block");
                output.TagMode = TagMode.SelfClosing;
            }
            else
            {
                output.TagName = "span";
                output.Attributes.SetAttribute("class", "center-align center-block");
                output.Content.SetHtmlContent($"<i class='material-icons'>account_box</i> Login With {Provider ?? "Unknown Provider"}");
                output.TagMode = TagMode.StartTagAndEndTag;
            }
        }
    }
}