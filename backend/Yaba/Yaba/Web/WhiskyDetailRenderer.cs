using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Yaba.Domain.Models;

namespace Yaba.Web
{
    public static class WhiskyDetailRenderer
    {
        public static string RenderDetail(
            Whisky whisky,
            IReadOnlyList<SpiritImage> images,
            Func<SpiritImage, bool, string> imageUrlBuilder)
        {
            var bottleUrl = images.Count > 0 ? imageUrlBuilder(images[0], false) : string.Empty;

            var html = new StringBuilder();
            html.Append(
                $"<article id=\"whisky-detail\" class=\"whisky-detail\" data-signals=\"{{'heroImage':'{Js(bottleUrl)}'}}\">");
            html.Append("""
                <header class="whisky-header">
                    <p class="whisky-meta"><a href="/">← Back to collection</a></p>
                    <h1>
                """);
            html.Append(H(whisky.Name));
            html.Append("</h1>");
            html.Append($"<p class=\"whisky-subtitle\">{H(whisky.Distillery)} · {H(whisky.Category.ToString())} · Campbeltown</p>");
            html.Append("</header>");

            html.Append("<section class=\"whisky-layout\">");

            if (images.Count == 0)
            {
                html.Append("<div class=\"whisky-hero\"><p class=\"muted\">No images stored yet.</p></div>");
            }
            else
            {
                html.Append("<figure class=\"whisky-hero\">");
                html.Append(
                    $"""<img data-attr:src="$heroImage" src="{H(bottleUrl)}" alt="{H(whisky.Name)}" />""");
                html.Append("</figure>");
            }

            html.Append("<dl class=\"whisky-facts\">");
            AppendFact(html, "Distillery", whisky.Distillery);
            AppendFact(html, "Category", whisky.Category.ToString());
            AppendFact(html, "Age", $"{whisky.Age} years");
            AppendFact(html, "Bottled", whisky.Bottled.ToString());
            AppendFact(html, "ABV", $"{whisky.Strength:0.0}%");
            AppendFact(html, "Size", $"{whisky.Size} cl");
            AppendFact(html, "Cask type", whisky.CaskType);
            AppendFact(html, "Series", whisky.BottlingSeries);
            AppendFact(html, "Natural colour", FormatNullableBool(whisky.NaturalColor));
            AppendFact(html, "Non chill-filtered", FormatNullableBool(whisky.NonChillFiltered));
            AppendFact(html, "Added to YABA", whisky.Created.ToString("yyyy-MM-dd"));
            html.Append("</dl>");
            html.Append("</section>");

            if (images.Count > 1)
            {
                html.Append("<div class=\"whisky-thumbs\">");
                for (var i = 0; i < images.Count; i++)
                {
                    var image = images[i];
                    var imageUrl = imageUrlBuilder(image, false);
                    html.Append("<button type=\"button\" class=\"whisky-thumb\" ");
                    html.Append($"data-on:click=\"$heroImage = '{Js(imageUrl)}'\" ");
                    html.Append($"aria-label=\"Show photo {i + 1}\">");
                    html.Append($"<img src=\"{H(imageUrlBuilder(image, true))}\" alt=\"Photo {i + 1}\" />");
                    html.Append("</button>");
                }
                html.Append("</div>");
            }

            html.Append("""
                <section class="whisky-notes">
                    <h2>Tasting profile</h2>
                    <p>Lightly peated Campbeltown single malt, double distilled and matured in 70% bourbon and 30% sherry casks. Citrus, oiliness, subtle peat smoke, toffee pudding and butterscotch on the finish.</p>
                </section>
                """);

            html.Append("</article>");
            return html.ToString();
        }

        public static string RenderCollectionRow(Whisky whisky, string detailPath)
        {
            return $"""
                <tr>
                    <td><a href="{H(detailPath)}">{H(whisky.Name)}</a></td>
                    <td>{H(whisky.Distillery)}</td>
                    <td>{whisky.Age}</td>
                    <td>{whisky.Strength:0.0}%</td>
                </tr>
                """;
        }

        private static void AppendFact(StringBuilder html, string label, string value)
        {
            html.Append("<div>");
            html.Append($"<dt>{H(label)}</dt>");
            html.Append($"<dd>{H(value)}</dd>");
            html.Append("</div>");
        }

        private static string FormatNullableBool(bool? value) =>
            value switch
            {
                true => "Yes",
                false => "No",
                _ => "Unknown"
            };

        private static string H(string value) => WebUtility.HtmlEncode(value);

        private static string Js(string value) => H(value).Replace("\\", "\\\\").Replace("'", "\\'");
    }
}
