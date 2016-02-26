using System;
using System.Globalization;
using System.Text;
using System.Web.Mvc;
using Pizza.Contracts.Presentation.Operations.Results;

namespace Pizza.Mvc.HtmlHelpers
{
    public static class PaginatorHelper
    {
        private const int VisiblePagesOffset = 2;

        private const string StandardButtonCssClassName = "btn";
        private const string PreviousDivCssClassName = "prev";
        private const string NextDivCssClassName = "next";

        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrl)
        {
            int first = 1, last = pagingInfo.TotalPages;
            if (pagingInfo.TotalPages > 2 * VisiblePagesOffset + 1)
            {
                last = Math.Min(pagingInfo.TotalPages, pagingInfo.CurrentPageNumber + VisiblePagesOffset);
                first = Math.Max(1, last - 2 * VisiblePagesOffset);
                last += Math.Max(0, first - pagingInfo.CurrentPageNumber + VisiblePagesOffset);
            }

            var result = new StringBuilder();
            result.AppendPrevButton(pagingInfo.CurrentPageNumber, (pagingInfo.CurrentPageNumber > 1), pageUrl);

            var divOuter = new TagBuilder("div");
            divOuter.AddCssClass("allbtns");

            for (int currentPage = first; currentPage <= last; currentPage++)
            {
                divOuter.AppendPageButton(currentPage, (currentPage == pagingInfo.CurrentPageNumber), pageUrl);
            }

            result.Append(divOuter);

            result.AppendNextButton(pagingInfo.CurrentPageNumber, (pagingInfo.CurrentPageNumber < pagingInfo.TotalPages), pageUrl);

            return MvcHtmlString.Create(result.ToString());
        }

        private static void AppendPrevButton(this StringBuilder result, int pageNumber, bool isVisible, Func<int, string> pageUrl)
        {
            var divPrev = new TagBuilder("div");
            divPrev.AddCssClass(PreviousDivCssClassName);

            if (isVisible)
            {
                var anchor = new TagBuilder("a");
                anchor.MergeAttribute("href", pageUrl(pageNumber - 1));
                anchor.InnerHtml = "Prev";

                divPrev.InnerHtml = anchor.ToString();
            }

            result.Append(divPrev);
        }

        private static void AppendPageButton(this TagBuilder result, int pageNumber, bool isActive, Func<int, string> pageUrl)
        {
            var anchor = new TagBuilder("a");
            anchor.MergeAttribute("href", pageUrl(pageNumber));
            anchor.InnerHtml = pageNumber.ToString(CultureInfo.InvariantCulture);
            if (isActive)
            {
                anchor.AddCssClass("active-btn");
            }


            var div = new TagBuilder("div");
            div.AddCssClass(StandardButtonCssClassName);
            div.InnerHtml = anchor.ToString();

            result.InnerHtml += div;
        }

        private static void AppendNextButton(this StringBuilder result, int pageNumber, bool isVisible, Func<int, string> pageUrl)
        {
            var divNext = new TagBuilder("div");
            divNext.AddCssClass(NextDivCssClassName);

            if (isVisible)
            {
                var anchor = new TagBuilder("a");
                anchor.MergeAttribute("href", pageUrl(pageNumber + 1));
                anchor.InnerHtml = "Next";

                divNext.InnerHtml = anchor.ToString();
            }

            result.Append(divNext);
        }
    }
}