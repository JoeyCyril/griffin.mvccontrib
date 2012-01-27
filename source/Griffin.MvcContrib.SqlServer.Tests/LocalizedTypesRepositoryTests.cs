using System.Globalization;
using Griffin.MvcContrib.Localization.Types;
using Griffin.MvcContrib.SqlServer.Localization;
using Xunit;

namespace Griffin.MvcContrib.SqlServer.Tests
{
	public class LocalizedTypesRepositoryTests
	{
		private SqlLocalizedTypesRepository _repository = new SqlLocalizedTypesRepository(new ConnectionFactory());

		[Fact]
		public void GetNonExistant()
		{
			var p = _repository.GetPrompt(new CultureInfo(1053), new TypePromptKey(typeof (TestType), "FirstNameOrSomething"));
			Assert.Null(p);
		}

		[Fact]
		public void GetExisting()
		{
			var key = new TypePromptKey(typeof (TestType), "FirstName");
			_repository.Save(new CultureInfo(1053),  typeof(TestType), "FirstName", "F�rnamn");

			var prompt = _repository.GetPrompt(new CultureInfo(1053), key);
			Assert.NotNull(prompt);
			Assert.Equal("F�rnamn", prompt.TranslatedText);
		}

		[Fact]
		public void Update()
		{
			var key = new TypePromptKey(typeof(TestType), "FirstName");
			_repository.Update(new CultureInfo(1053), key, "F�rrenammne");
			var prompt = _repository.GetPrompt(new CultureInfo(1053), key);
			Assert.NotNull(prompt);
			Assert.Equal("F�rrenammne", prompt.TranslatedText);
		}

		[Fact]
		public void TwoLanguages()
		{
			var key = new TypePromptKey(typeof(TestType), "FirstName");
			_repository.Save(new CultureInfo(1033), typeof(TestType), "FirstName", "FirstName");
			_repository.Save(new CultureInfo(1053), typeof(TestType), "FirstName", "F�rnamn");


			var enprompt = _repository.GetPrompt(new CultureInfo(1033), key);
			var seprompt = _repository.GetPrompt(new CultureInfo(1053), key);
			Assert.NotNull(enprompt);
			Assert.NotNull(seprompt);
			Assert.NotEqual(enprompt.TranslatedText, seprompt.TranslatedText);
		}

	}
}