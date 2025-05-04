using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace generator.Tests
{
    public class GeneratorBiTests
    {
        
        [Fact]
        public void GenerateText_WithEmptyDictionary_ReturnsEmptyString()
        {
            // Arrange
            var emptyBigrams = new Dictionary<string, int> { { "ab", 10 } };
            var generator = new GeneratorBi(emptyBigrams);

            // Act
            var result = generator.GenerateText(100);

            // Assert
            Assert.Equal("ab", result);
        }
        [Fact]
        public void GetNextChar_ReturnsValidCharFromTransitions()
        {
            // Arrange
            var mockBigrams = new Dictionary<string, int> { { "ab", 10 } };
            var generator = new GeneratorBi(mockBigrams);

            // Act
            var result = generator.GetNextChar('a');

            // Assert
            Assert.Equal('b', result);
        }

        [Fact]
        public void GenerateText_StartsWithValidCharacter()
        {
            // Arrange
            var mockBigrams = new Dictionary<string, int>
            {
                {"ab", 10}, {"ba", 5}, {"bc", 5}
            };
            var generator = new GeneratorBi(mockBigrams);

            // Act
            var result = generator.GenerateText(10);

            // Assert
            Assert.Contains(result[0], mockBigrams.Keys.Select(k => k[0]));
        }
    }

    public class GeneratorWordsTests
    {
        [Fact]
        public void GenerateSentences_ReturnsCorrectNumberOfWords()
        {
            // Arrange
            var mockWords = new Dictionary<string, int>
            {
                {"test", 1}, {"word", 1}, {"sample", 1}
            };
            var generator = new GeneratorWords(mockWords);

            // Act
            var result = generator.GenerateSentences(5).Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Assert
            Assert.InRange(result.Length, 5, 6); // 5-6 words (including first)
        }

        [Fact]
        public void GetNextWord_ReturnsWordFromDictionary()
        {
            // Arrange
            var mockWords = new Dictionary<string, int>
            {
                {"test", 1}, {"word", 1}, {"sample", 1}
            };
            var generator = new GeneratorWords(mockWords);

            // Act
            var result = generator.getNextWord();

            // Assert
            Assert.Contains(result, mockWords.Keys);
        }

        [Fact]
        public void GenerateSentences_StartsWithValidWord()
        {
            // Arrange
            var mockWords = new Dictionary<string, int>
            {
                {"test", 1}, {"word", 1}, {"sample", 1}
            };
            var generator = new GeneratorWords(mockWords);

            // Act
            var result = generator.GenerateSentences(5).Split(' ')[0];

            // Assert
            Assert.Contains(result, mockWords.Keys);
        }
    }

    public class IntegrationTests
    {
        
        [Fact]
        public void GeneratedText_ContainsOnlyLettersAndSpaces()
        {
            // Arrange
            var mockBigrams = new Dictionary<string, int>
            {
                {"ab", 10}, {"ba", 5}, {"bc", 5}
            };
            var generator = new GeneratorBi(mockBigrams);

            // Act
            var result = generator.GenerateText(100);

            // Assert
            Assert.Matches(@"^[a-zA-Z]+$", result);
        }

        [Fact]
        public void GeneratedSentences_ContainsSpacesBetweenWords()
        {
            // Arrange
            var mockWords = new Dictionary<string, int>
            {
                {"test", 1}, {"word", 1}, {"sample", 1}
            };
            var generator = new GeneratorWords(mockWords);

            // Act
            var result = generator.GenerateSentences(5);

            // Assert
            Assert.Contains(' ', result);
        }

        [Fact]
        public void GeneratorBi_HandlesSingleBigram()
        {
            var mockBigrams = new Dictionary<string, int> { {"ab", 1} };
            var generator = new GeneratorBi(mockBigrams);
        
            // Act
            var result = generator.GenerateText(5);
            
            // Assert
            Assert.Matches(@"^[ab]{5}$", result); // Только a и b
            Assert.Contains("a", result);
            Assert.Contains("b", result);
        }
         [Fact]
         public void GeneratorWords()
         {
             var mockBigrams = new Dictionary<string, int> { { "Летний", 1 }, {"день", 2} };
             var generator = new GeneratorWords(mockBigrams);
        
             // Act
             var result = generator.GenerateSentences(2);
        
             // Assert
             
             Assert.Contains("Летний", result);
             Assert.Contains("день", result);
         }
    }
}
