using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MyWeb.Business;
using MyWeb.Models;

namespace MyWeb.HtmlHelpers
{
    public static class PagingHelpers
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl,
            bool showFirstPage = true, bool showPreviousPage = true, bool showNextPage = true, bool showLastPage = true)
        {
            var result = new StringBuilder();
            if (pagingInfo.TotalPages > 1)
            {
                if (showFirstPage)
                {
                    if (pagingInfo.CurrentPage == 1)
                        result.Append(
                            "<li class=\"prev disabled\"><a href=\"javascript:;\">← <span class=\"hidden-480\">Trang đầu</span></a></li>");
                    else
                        result.Append("<li class=\"prev\"><a href=\"" + pageUrl(1) +
                                      "\">← <span class=\"hidden-480\">Trang đầu</span></a></li>");
                    // end first page 
                }
                if (showPreviousPage)
                {
                    if (pagingInfo.CurrentPage - 1 <= 0)
                        result.Append(
                            "<li class=\"disabled\"><a href=\"javascript:;\"><span class=\"hidden-480\">‹</span></a></li>");
                    else
                        result.Append("<li><a href=\"" + pageUrl(pagingInfo.CurrentPage - 1) +
                                      "\"><span class=\"hidden-480\">‹</span></a></li>");
                    //end Previous page
                }
                for (int i = 1; i <= pagingInfo.TotalPages; i++)
                {
                    var item = new TagBuilder("li");
                    var target = new TagBuilder("a");
                    if (i == pagingInfo.CurrentPage)
                    {
                        item.AddCssClass("active");
                        target.MergeAttribute("href", "javascript:;");
                    }
                    else
                        target.MergeAttribute("href", pageUrl(i));

                    target.InnerHtml = Convert.ToString(i);
                    item.InnerHtml += target.ToString();
                    result.Append(item);
                }
                if (showNextPage)
                {
                    if (pagingInfo.CurrentPage + 1 > pagingInfo.TotalPages)
                        result.Append(
                            "<li class=\"disabled\"><a href=\"javascript:;\"><span class=\"hidden-480\">›</span></a></li>");
                    else
                        result.Append("<li><a href=\"" + pageUrl(pagingInfo.CurrentPage + 1) +
                                      "\"><span class=\"hidden-480\">›</span></a></li>");
                    // end next page 
                }
                if (showLastPage)
                {
                    if (pagingInfo.CurrentPage == pagingInfo.TotalPages)
                        result.Append(
                            "<li class=\"next disabled\"><a href=\"javascript:;\"><span class=\"hidden-480\">Trang cuối</span> →</a></li>");
                    else
                        result.Append("<li class=\"next\"><a href=\"" + pageUrl(pagingInfo.TotalPages) +
                                      "\"><span class=\"hidden-480\">Trang cuối</span> →</a></li>");
                }
            }
            return MvcHtmlString.Create(result.ToString());
        }

        public static MvcHtmlString BuildPageLink(string baseLink, int numOfPage, int currentPage = 1)
        {
            string str = @"
<div class='pagination'>
	<ul style='display: block;float: right;margin-right: -2px;' class=''>
		<li {5}><a style='tfoot' href='{0}?page=1'>&lt;&lt;</a></li>
		<li {5}><a style='tfoot' href='{0}?page={1}'>&lt;</a></li>
		{2}
		<li {6}><a style='tfoot' href='{0}?page={3}'>&gt;</a></li>
		<li {6}><a style='tfoot' href='{0}?page={4}'>&gt;&gt;</a></li>
	</ul>
	<span style='float: right;margin: 5px;'>Trang {7}/{4}</span>
</div>
";
            int prePage = Math.Max(1, currentPage - 1);
            int nextPage = Math.Min(numOfPage, currentPage + 1);
            string links = "";
            int start = Math.Max(1, currentPage - 2);
            int end = start + 4;
            if (end > numOfPage)
            {
                start = Math.Max(1, numOfPage - 4);
                end = numOfPage;
            }

            for (int i = start; i <= end; i++)
            {
                if (i == currentPage)
                {
                    links += string.Format("<li style='opacity: 1 !important;' class='disabled'><a style='tfoot' href='{1}?page={0}' class='currentPage'>{0}</a></li>", i, baseLink);
                }
                else
                {
                    links += string.Format("<li><a style='tfoot' href='{1}?page={0}'>{0}</a></li>", i, baseLink);
                }
            }

            string firstStyle = currentPage == 1 ? "class='disabled'" : "";
            string endStyle = currentPage == numOfPage ? "class='disabled'" : "";

            string res = string.Format(str, baseLink, prePage, links, nextPage, numOfPage, firstStyle, endStyle, currentPage);
            return new MvcHtmlString(res);
        }
    }
}