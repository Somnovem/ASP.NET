using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Homework_1.Pages
{
	public class Task5Model : PageModel
	{
		private readonly string randomQuoteUrl = "https://api.quotable.io/random";
		private readonly string translationUrl = "https://api.mymemory.translated.net/get";

		public Quote Quote { get; set; }

		public async Task OnGetAsync()
		{
			Quote = await GetRandomTranslatedQuoteAsync();
		}

		private async Task<Quote> GetRandomTranslatedQuoteAsync()
		{
			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(randomQuoteUrl);
				if (response.IsSuccessStatusCode)
				{
					string randomQuoteJson = await response.Content.ReadAsStringAsync();
					dynamic randomQuote = JsonConvert.DeserializeObject(randomQuoteJson);
					string author = randomQuote.author;
					string content = randomQuote.content;

					string authorTranslationUrl = $"{translationUrl}?q={author}&langpair=en|ru";
					string contentTranslationUrl = $"{translationUrl}?q={content}&langpair=en|ru";

					Task<string> authorTranslationTask = client.GetStringAsync(authorTranslationUrl);
					Task<string> contentTranslationTask = client.GetStringAsync(contentTranslationUrl);

					await Task.WhenAll(authorTranslationTask, contentTranslationTask);

					string authorTranslationJson = await authorTranslationTask;
					string contentTranslationJson = await contentTranslationTask;

					dynamic authorTranslationData = JsonConvert.DeserializeObject(authorTranslationJson);
					dynamic contentTranslationData = JsonConvert.DeserializeObject(contentTranslationJson);

					string translatedAuthor = authorTranslationData.responseData.translatedText;
					string translatedContent = contentTranslationData.responseData.translatedText;

					return new Quote
					{
						Author = translatedAuthor,
						Content = translatedContent
					};
				}
			}

			return null;
		}
	}

	public class Quote
	{
		public string Content { get; set; }
		public string Author { get; set; }
	}
}