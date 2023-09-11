using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Text.Encodings.Web;
using Task_1.Models;

namespace Task_1
{
    public static class ListBuilder
    {
        public static HtmlString CreateStoreList(this IHtmlHelper html, List<Product> products)
        {
            TagBuilder divContainer = new TagBuilder("div");
            foreach (Product product in products)
            {
                TagBuilder divTop = new TagBuilder("div");
                divTop.Attributes.Add("class", "col-lg-4 col-md-6 mb-4");

                TagBuilder divMain = new TagBuilder("div");
                divMain.Attributes.Add("class", "card h-100");
                
                TagBuilder thumbnail = new TagBuilder("img");
                thumbnail.Attributes.Add("class", "card-img-top");
                thumbnail.Attributes.Add("src", product.Thumbnail);
                thumbnail.Attributes.Add("alt", product.Title);

                TagBuilder divBody = new TagBuilder("div");
                divBody.Attributes.Add("class", "card-body");

                TagBuilder title = new TagBuilder("h4");
                title.Attributes.Add("class", "card-info card-title");
                title.InnerHtml.Append(product.Title);

                TagBuilder discount = new TagBuilder("h4");
                discount.Attributes.Add("class", "card-info card-discount");
                discount.InnerHtml.Append($"Discount: {product.DiscountPercentage}%");

                TagBuilder price = new TagBuilder("h5");
                price.Attributes.Add("class", "card-info card-price");
                price.InnerHtml.Append($"Price: ${product.Price}");

                TagBuilder stock = new TagBuilder("h5");
                stock.Attributes.Add("class", "card-info card-stock");
                stock.InnerHtml.Append($"Stock: {product.Stock}");

                TagBuilder divRating = new TagBuilder("div");
                divRating.Attributes.Add("class", "rating");

                for (int i = 0; i < product.Rating; i++)
                {
                    TagBuilder star = new TagBuilder("span");
                    star.Attributes.Add("class", "star");
                    divRating.InnerHtml.AppendHtml("★");
                    divRating.InnerHtml.AppendHtml(star);
                }
                TagBuilder detailsBtn = new TagBuilder("a");
                detailsBtn.Attributes.Add("class", "btn btn-primary");
                detailsBtn.Attributes.Add("href", $"Details/{product.Id}");
                detailsBtn.InnerHtml.Append("View details");

                divBody.InnerHtml.AppendHtml(title);
                divBody.InnerHtml.AppendHtml(discount);
                divBody.InnerHtml.AppendHtml(price);
                divBody.InnerHtml.AppendHtml(stock);
                divBody.InnerHtml.AppendHtml(divRating);
                divBody.InnerHtml.AppendHtml(detailsBtn);

                divMain.InnerHtml.AppendHtml(thumbnail);
                divMain.InnerHtml.AppendHtml(divBody);

                divTop.InnerHtml.AppendHtml(divMain);

                divContainer.InnerHtml.AppendHtml(divTop);
            }
            divContainer.Attributes.Add("class", "row");
            divContainer.Attributes.Add("id", "storeItems");
            using var writer = new StringWriter();
            divContainer.WriteTo(writer, HtmlEncoder.Default);
            return new HtmlString(writer.ToString());
        }
    }
}
