using DynamicExpresso;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NitroSystem.Dnn.BusinessEngine.Common;
using NitroSystem.Dnn.BusinessEngine.Core.Contract;
using NitroSystem.Dnn.BusinessEngine.Data.Entities.Tables;
using NitroSystem.Dnn.BusinessEngine.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NitroSystem.Dnn.BusinessEngine.Core.Infrastructure.SSR
{
    public class ServerSideRendering
    {
        private readonly IModuleData _moduleData;
        private readonly IExpressionService _expressionService;

        private List<string> IgnoreSSRList = new List<string>();
        private List<string> ParsedbForElements = new List<string>();

        public ServerSideRendering(IModuleData moduleData, IExpressionService expressionService)
        {
            this._moduleData = moduleData;
            this._expressionService = expressionService;
        }

        private const string ListPattern = @"<\w+\s*(?<FirstAttrs>.*)\s*(?<ListGroup>b-for=""\s*(?<ItemName>\w+)\s*in\s*(?<ListName>.[^""|\>]+)\s*"")\s*(?<LastAttrs>.*)\s*>[\s\S]*<\/\w+>";
        private const string AngularDataPattern = @"{{(.+)}}";
        private const string TextDataPattern = @"(<\w+\s+.*(?<Expr>bind-text=""(.[^""|\>]+)"").*>)([.|\n|\t|\s|\S][^\<]*)?(?<ParseHere>)(<\/\w+>)";
        private const string ImageDataPattern = @"<img\s+(?<FirstAttrs>.*)?\s*bind-image=""(?<ImageData>.[^""|\>]+)""\s*(options='(?<Options>.[^']*)')?\s*(no-image=""(?<NoImage>.[^""|\>]*)"")?\s*(?<LastAttrs>.*)?(/>|>)";
        private const string DateDataPattern = @"<\w+\s+(?<FirstAttrs>.*)?\s*(?<Expr>bind-date=""(?<DateData>.[^""|\>]+)"")\s*format=""(?<Format>.[^""|\>]+)""\s*(?<LastAttrs>.*)?\s*>([.|\n|\t|\s|\S][^\<]*)?(?<ParseHere>)<\/\w+>";
        private const string UrlDataPattern = @"<a\s+(?<FirstAttrs>.*)?\s*(?<Expr>bind-url=""(?<UrlData>.[^""|\>]+)"")\s*(?<LastAttrs>.*)?\s*>([.|\n|\t|\s|\S][^\<]*)?(?<ParseHere>)<\/a>";
        private const string ConditionPattern = @"b-show=""(?<ConditionExpression>.[^""|\>]+)""";

        public string Render(string template, bool ignoreUnknownType = true)
        {
            FillIgnoreSSRElements(template);

            template = ParseConditions(template);

            //1- Parse text data 
            template = ParseList(template);

            //2- Parse text data 
            template = ParseTextData(template);

            //3- Parse image data 
            template = ParseImageTag(template);

            //4- Parse date time data 
            template = ParseDateTimeTag(template);

            //5- Parse url data
            template = ParseLinkTag(template);

            //6- Parse angular data 
            template = ParseAngularData(template);

            //7- Parse SSR Elements 
            template = ParseIgnoreSSR(template);

            return template;
        }

        private void FillIgnoreSSRElements(string template)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(template);

            var htmlElements = htmlDoc.DocumentNode.SelectNodes("//*[@ignore-ssr]");
            foreach (var item in htmlElements ?? Enumerable.Empty<HtmlNode>())
            {
                this.IgnoreSSRList.Add(item.OuterHtml);
            }
        }

        private string ParseList(string template)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(template);

            var htmlElement = htmlDoc.DocumentNode.SelectSingleNode("//*[@b-for]");
            if (htmlElement != null && !ParsedbForElements.Contains(htmlElement.OuterHtml))
            {
                ParsedbForElements.Add(htmlElement.OuterHtml);

                if (htmlElement.GetAttributeValue("ignore-ssr", -1) >= 0) return ParseList(template);

                string itemHtml = htmlElement.OuterHtml;
                Match match = Regex.Match(itemHtml, ListPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    itemHtml = itemHtml.Replace(match.Groups["ListGroup"].Value, "");

                    var itemName = match.Groups["ItemName"].Value;
                    var listName = match.Groups["ListName"].Value;

                    StringBuilder items = new StringBuilder();

                    HtmlNode currentElement = htmlElement;

                    this._moduleData.AddProperty(itemName);

                    var list = this._moduleData.GetData(listName);
                    if (list != null && list.ToString().StartsWith("[")) list = JArray.Parse(list.ToString());
                    foreach (var item in (list as JArray) ?? JArray.Parse("[]"))
                    {
                        this._moduleData.SetData(itemName, item);

                        var parsedItem = ParseList(itemHtml);
                        parsedItem = ParseAngularData(parsedItem);
                        parsedItem = ParseConditions(parsedItem, true);
                        parsedItem = ParseTextData(parsedItem);
                        parsedItem = ParseImageTag(parsedItem);
                        parsedItem = ParseDateTimeTag(parsedItem);
                        parsedItem = ParseLinkTag(parsedItem);

                        HtmlNode node = HtmlNode.CreateNode(parsedItem);

                        htmlElement.ParentNode.InsertAfter(node, currentElement);

                        currentElement = node;
                    }

                    htmlElement.Remove();

                    template = htmlDoc.DocumentNode.OuterHtml;
                }

                return ParseList(template);
            }
            else
                return template;
        }

        /*
            * Parse bind-text expressions
            * Example <span bind-text="Product.ProductName"></span> ==> <span>Sumsung Galaxy Mobile</span>
        */
        private string ParseAngularData(string template)
        {
            var matches = Regex.Matches(template, AngularDataPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string propertyPath = match.Groups[1].Value;
                var propertyValue = this._expressionService.ParseExpression(propertyPath, this._moduleData, new List<object>(), true);
                if (!string.IsNullOrEmpty(propertyValue))
                    template = template.Replace(match.Value, (propertyValue ?? ""));
            }

            return template;
        }

        /*
            * Parse bind-text expressions
            * Example <span bind-text="Product.ProductName"></span> ==> <span>Sumsung Galaxy Mobile</span>
        */
        private string ParseTextData(string template)
        {
            var matches = Regex.Matches(template, TextDataPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string propertyPath = match.Groups[2].Value;
                var propertyValue = this._expressionService.ParseExpression(propertyPath, this._moduleData, new List<object>(), true);

                //var propertyValue = expre this._moduleData.GetData(propertyPath);
                //if (!string.IsNullOrEmpty(propertyPath) && propertyValue == "") propertyValue = propertyPath;

                var regex = new Regex(TextDataPattern);
                var str = regex.ReplaceGroup(match.Value, "ParseHere", propertyValue.ToString(), 1);
                str = str.Replace(match.Groups["Expr"].Value, "");
                template = template.Replace(match.Value, str);
            }

            return template;
        }

        /*
            * Parse image expressions
            * Example <img bind-image="Product.ProductName" /> ==> <img src="/portals/0/image.png" />
        */
        public string ParseImageTag(string template)
        {
            var matches = Regex.Matches(template, ImageDataPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                var noImage = match.Groups["NoImage"].Value;
                var options = match.Groups["Options"].Value;

                ImageOptions objImageOptions = new ImageOptions() { Type = ImageType.JsonValue, PropertyName = "FilePath", IsArray = true, Index = 0 };
                if (!string.IsNullOrWhiteSpace(options))
                {
                    try
                    {
                        objImageOptions = JsonConvert.DeserializeObject<ImageOptions>(options);
                    }
                    catch
                    {
                    }
                }

                var firstData = match.Groups["FirstAttrs"].Value;
                string parsedFirstData = !string.IsNullOrWhiteSpace(firstData) ? this._expressionService.ParseExpression(firstData, this._moduleData, new List<object>(),true) : firstData;

                var lastData = match.Groups["LastAttrs"].Value;
                var parsedLastData = !string.IsNullOrWhiteSpace(lastData) ? this._expressionService.ParseExpression(lastData, this._moduleData, new List<object>(),true) : lastData;

                string propertyPath = match.Groups["ImageData"].Value;
                var propertyValue = this._expressionService.ParseExpression(propertyPath, this._moduleData, new List<object>(), true);

                if (!string.IsNullOrWhiteSpace(propertyValue))
                {
                    if (objImageOptions.Type == ImageType.JsonValue && objImageOptions.IsArray && propertyValue.Trim().StartsWith("[") && propertyValue.Trim().EndsWith("]"))
                    {
                        propertyValue = JArray.Parse(propertyValue)[0][objImageOptions.PropertyName].Value<string>();
                    }

                    string imageTag = string.Format(@"<img {0} src=""{1}"" {3} />",
                        parsedFirstData,
                        propertyValue,
                        match.Groups["ImageData"].Value,
                        parsedLastData);

                    template = template.Replace(match.Value, imageTag);
                }
                else if (!string.IsNullOrWhiteSpace(noImage))
                {
                    string imageTag = string.Format(@"<img {0} src=""{1}"" {3} />",
                        parsedFirstData,
                        noImage,
                        match.Groups["ImageData"].Value,
                        parsedLastData);

                    template = template.Replace(match.Value, imageTag);
                }
            }

            return template;
        }

        /*
            * Parse date & time expressions
            * Example <span bind-date="User.CreatedOnDate" fotmat="MM/DD/YYYY"></span> ==> <span>12/30/2022</span> />
        */
        public string ParseDateTimeTag(string template)
        {
            var matches = Regex.Matches(template, DateDataPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string propertyPath = match.Groups["DateData"].Value;
                var propertyValue = this._expressionService.ParseExpression(propertyPath, this._moduleData, new List<object>(), true);
                if (!string.IsNullOrWhiteSpace(propertyValue))
                {
                    DateTime valueByFormat;
                    if (DateTime.TryParse(propertyValue, out valueByFormat))
                        propertyValue = valueByFormat.ToString(match.Groups["Format"].Value);
                    else
                        propertyValue = "";

                    var regex = new Regex(DateDataPattern);
                    var str = regex.ReplaceGroup(match.Value, "ParseHere", propertyValue, 1);
                    str = str.Replace(match.Groups["Expr"].Value, "");
                    template = template.Replace(match.Value, str);
                }
            }

            return template;
        }

        /*
            * Parse url expressions
            * Example <a bind-url="User.ProfileUrl" bind-text="User.DisplayName"></a> ==> <a href="/user/arya">Arya</a> />
        */
        public string ParseLinkTag(string template)
        {
            var matches = Regex.Matches(template, UrlDataPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                string propertyPath = match.Groups["UrlData"].Value;
                var propertyValue = this._expressionService.ParseExpression(propertyPath, this._moduleData, new List<object>(), true, "static-expression");
                if (!string.IsNullOrWhiteSpace(propertyValue))
                {
                    string url = string.Format(@"href=""{0}""", propertyValue);
                    //str = str.Replace(match.Groups["Expr"].Value, "");
                    template = template.Replace(match.Groups[3].Value, url);
                }
            }

            return template;
        }

        /*
            * Parse condition expressions
            * Example <span class="message__text" b-show="login.error">...
        */
        public string ParseConditions(string template, bool isInsideList = false)
        {
            var target = new Interpreter();

            var matches = Regex.Matches(template, ConditionPattern, RegexOptions.Multiline | RegexOptions.IgnoreCase);
            foreach (Match match in matches)
            {
                try
                {
                    string propertyPath = match.Groups["ConditionExpression"].Value;
                    var propertyValue = this._expressionService.ParseExpression(propertyPath, this._moduleData, new List<object>(), true, "parse-string-with-add-cotation-to-any-word");
                    var result = target.Eval<bool>(propertyValue.Replace("'", @""""));
                    string condition = isInsideList ? string.Empty : match.Value;
                    string attr = isInsideList ? @"data-hide-in-list=""true""" : @"data-hide-in-ssr=""true""";
                    if (!result)
                    {
                        template = template.Replace(match.Value, condition + attr);
                    }
                    else if (isInsideList)
                    {
                        template = template.Replace(match.Value, string.Empty);
                    }
                }
                catch (Exception ex)
                {

                }
            }

            return template;
        }

        private string ParseIgnoreSSR(string template)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(template);

            int index = 0;
            var htmlElements = htmlDoc.DocumentNode.SelectNodes("//*[@ignore-ssr]");
            foreach (var item in htmlElements ?? Enumerable.Empty<HtmlNode>())
            {
                if (index < this.IgnoreSSRList.Count)
                {
                    string elementHtml = this.IgnoreSSRList[index];

                    HtmlNode newNode = HtmlNode.CreateNode(elementHtml); // New way for replace html
                    newNode.Attributes.Add("ng-if", "loadedModule");
                    newNode.AddClass("ignore-ssr");
                    item.ParentNode.ReplaceChild(newNode, item);

                    //template = template.Replace(item.OuterHtml, elementHtml); -- Old way for replace html 
                }

                index++;
            }

            template = htmlDoc.DocumentNode.OuterHtml;

            return template;
        }
    }
}
