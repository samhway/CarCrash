using CarCrash.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
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
        public bool ?PedInv { get; set; }
        public bool ?MotInv { get; set; }
        public bool ?CycInv { get; set; }
        public bool ?WSRel { get; set; }
        public decimal ?lat { get; set; }
        public decimal ?lon { get; set; }
        public string ?Rnam { get; set; }
        public bool ?ImpRes { get; set; }
        public bool ?Unr { get; set; }
        public bool ?DUI { get; set; }
        public bool ?MP { get; set; }
        public bool ?AniRel { get; set; }
        public bool ?DomAniRel { get; set; }
        public bool ?IntRel { get; set; }
        public string ?Route { get; set; }
        public bool ?OveRol { get; set; }
        public bool ?TenDri { get; set; }
        public bool ?ComVeh { get; set; }
        public bool ?OldDri { get; set; }
        public bool ?Night { get; set; }
        public bool ?Single { get; set; }
        public bool ?Dist { get; set; }
        public bool ?Drows { get; set; }
        public bool ?Depart { get; set; }
        public int ?ID { get; set; }
        public string ?Sev { get; set; }
        public string ?Date { get; set; }
        public string ?Loc { get; set; }
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

            if(PageBlah.TotalPages == 25009)
            {
                var addLinks = 5;
                var subLinks = 4;
                if (PageBlah.CurrentPage < 5)
                {
                    subLinks = (PageBlah.CurrentPage - 1);
                    addLinks = (9 - subLinks);
                }
                else if (PageBlah.CurrentPage > 25005)
                {
                    addLinks = (25010 - PageBlah.CurrentPage);
                    subLinks = (9 - addLinks);

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
                if (lastLink < (PageBlah.TotalPages + 1))
                {
                    if ((PageBlah.TotalPages - lastLink) >= 1)
                    {
                        TagBuilder td = new TagBuilder("a");
                        td.Attributes.Add("readonly", "readonly");
                        td.Attributes.Add("title", "You can filter above, or click a numbered link");
                        td.InnerHtml.AppendHtml("...");
                        td.AddCssClass(PageClass);
                        td.AddCssClass(PageClassNormal);
                        final.InnerHtml.AppendHtml(td);
                    }

                    for (int i = (PageBlah.TotalPages); i < (PageBlah.TotalPages + 1); i++)
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
                }
            }
            else
            {
                var addLinks = 5;
                var subLinks = 4;
                if (PageBlah.CurrentPage < 5)
                {
                    subLinks = (PageBlah.CurrentPage - 1);
                    addLinks = (9 - subLinks);
                }
                else if (PageBlah.CurrentPage > 25005)
                {
                    addLinks = (25010 - PageBlah.CurrentPage);
                    subLinks = (9 - addLinks);

                }
                var firstLink = (PageBlah.CurrentPage - subLinks);
                var lastLink = (PageBlah.CurrentPage + addLinks);

                //Search Parameters
                var parameters = new RouteValueDictionary();
                parameters.Add("WSRel", WSRel);
                parameters.Add("PedInv", PedInv);
                parameters.Add("MotInv", MotInv);
                parameters.Add("CycInv", CycInv);
                parameters.Add("Rnam", Rnam);
                parameters.Add("lon", lon);
                parameters.Add("lat", lat);
                parameters.Add("ImpRes", ImpRes);
                parameters.Add("Unr", Unr);
                parameters.Add("DUI", DUI);
                parameters.Add("MP", MP);
                parameters.Add("AniRel", AniRel);
                parameters.Add("DomAniRel", DomAniRel);
                parameters.Add("IntRel", IntRel);
                parameters.Add("OveRol", OveRol);
                parameters.Add("Route", Route);
                parameters.Add("TenDri", TenDri);
                parameters.Add("ComVeh", ComVeh);
                parameters.Add("OldDri", OldDri);
                parameters.Add("Night", Night);
                parameters.Add("Single", Single);
                parameters.Add("Dist", Dist);
                parameters.Add("Drows", Drows);
                parameters.Add("Depart", Depart);
                parameters.Add("Sev", Sev);
                parameters.Add("ID", ID);
                parameters.Add("Loc", Loc);
                parameters.Add("Date", Date);

                parameters.Add("pageNum", 1);
                for (int i = firstLink; i < lastLink; i++)
                {
                    parameters.Remove("pageNum");
                    parameters.Add("pageNum", i);
                    TagBuilder tb = new TagBuilder("a");
                    tb.Attributes["href"] = uh.Action("SearchData", parameters); //This sets up our href, we're going to pagenumber i
                    if (PageClassesEnabled)
                    {
                        tb.AddCssClass(PageClass);
                        tb.AddCssClass(i == PageBlah.CurrentPage ? PageClassSelected : PageClassNormal); //Shortcut IF STATEMENT
                    }

                    tb.InnerHtml.Append(i.ToString());

                    final.InnerHtml.AppendHtml(tb);

                }
                if (lastLink < (PageBlah.TotalPages + 1 ))
                {
                    if((PageBlah.TotalPages - lastLink) >= 1)
                    {
                        TagBuilder td = new TagBuilder("a");
                        td.Attributes.Add("readonly", "readonly");
                        td.Attributes.Add("title", "You can filter above, or click a numbered link");
                        td.InnerHtml.AppendHtml("...");
                        td.AddCssClass(PageClass);
                        td.AddCssClass(PageClassNormal);
                        final.InnerHtml.AppendHtml(td);
                    }

                    
                    for (int i = (PageBlah.TotalPages); i < (PageBlah.TotalPages + 1); i++)
                    {
                        parameters.Remove("pageNum");
                        parameters.Add("pageNum", i);
                        TagBuilder tb = new TagBuilder("a");
                        tb.Attributes["href"] = uh.Action("SearchData", parameters); //This sets up our href, we're going to pagenumber i
                        if (PageClassesEnabled)
                        {
                            tb.AddCssClass(PageClass);
                            tb.AddCssClass(i == PageBlah.CurrentPage ? PageClassSelected : PageClassNormal); //Shortcut IF STATEMENT
                        }

                        tb.InnerHtml.Append(i.ToString());

                        final.InnerHtml.AppendHtml(tb);

                    }
                }

            }



            tho.Content.AppendHtml(final.InnerHtml);
            





        }
    }
}
