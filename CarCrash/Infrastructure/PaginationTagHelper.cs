using CarCrash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarCrash.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-blah")]
    public class PaginationTagHelper : TagHelper
    {
        //Dynamically create the page links for us

        private IUrlHelperFactory uhf;
        public PaginationTagHelper(IUrlHelperFactory temp)
        {
            uhf = temp;
        }
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext vc { get; set; }

        //Different than the View Context
        public PageInfo PageBlah { get; set; } //Page info is passed in from the html and is then set equal to PageBlah
        public string PageAction { get; set; }
        public string PageClass { get; set; }
        public bool PageClassesEnabled { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }
        public override void Process(TagHelperContext thc, TagHelperOutput tho)
        {
            IUrlHelper uh = uhf.GetUrlHelper(vc);

            TagBuilder final = new TagBuilder("div");

            var addLinks = 5;
            var subLinks = 4;
            if(PageBlah.CurrentPage > 4)
            {
                TagBuilder tc = new TagBuilder("a");
                tc.Attributes.Add("readonly", "readonly");
                tc.InnerHtml.AppendHtml("...");
                tc.AddCssClass(PageClass);
                tc.AddCssClass(PageClassNormal);
                final.InnerHtml.AppendHtml(tc);
            }
            if(PageBlah.CurrentPage < 5)
            {
                subLinks = (PageBlah.CurrentPage - 1);
                addLinks = (9 - subLinks);
            }
            else if(PageBlah.CurrentPage > 25005)
            {
                addLinks = (25010 - PageBlah.CurrentPage);
                subLinks = (9-addLinks);

            }
            var firstLink = (PageBlah.CurrentPage - subLinks);
            var lastLink = (PageBlah.CurrentPage + addLinks);

            for (int i = firstLink; i < lastLink; i++)
            {
                TagBuilder tb = new TagBuilder("a");
                tb.Attributes["href"] = uh.Action(PageAction, new { pageNum = i }); //This sets up our href, we're going to pagenumber i
                if (PageClassesEnabled)
                {
                    tb.AddCssClass(PageClass);
                    tb.AddCssClass(i == PageBlah.CurrentPage ? PageClassSelected : PageClassNormal); //Shortcut IF STATEMENT
                }

                tb.InnerHtml.Append(i.ToString());

                final.InnerHtml.AppendHtml(tb);

            }
            if (PageBlah.CurrentPage < 25005)
            {
                TagBuilder td = new TagBuilder("a");
                td.Attributes.Add("readonly", "readonly");
                td.InnerHtml.AppendHtml("...");
                td.AddCssClass(PageClass);
                td.AddCssClass(PageClassNormal);
                final.InnerHtml.AppendHtml(td);
            }




            if (PageBlah.CurrentPage > 5)
            {

            }

            //final.InnerHtml.AppendHtml();

            tho.Content.AppendHtml(final.InnerHtml);

        }
    }
}
