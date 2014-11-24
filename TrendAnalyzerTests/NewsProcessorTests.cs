//-----------------------------------------------------------------------
// <copyright file="NewsProcessorTests.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann
// </copyright>
//-----------------------------------------------------------------------
namespace TrendAnalyzerTests
{
   using System.Collections.Generic;
   using Microsoft.VisualStudio.TestTools.UnitTesting;

   /// <summary>
   /// Static class for testing all methods in FrogJump exercises
   /// </summary>
   [TestClass]
   public class NewsProcessorTests
   {
      /// <summary>
      /// Base Test
      /// </summary>
      [TestMethod]
      public void SolutionTest1()
      {
         TrendAnalyzer.NewsProcessor sol = new TrendAnalyzer.NewsProcessor();
         List<List<string>> newsItems = new List<List<string>>
         {
            new List<string> { "Spanish Soccer Team", "World Cup", "South Africa" },
            new List<string> { "World Cup", "Shakira", "South Africa" },
            new List<string> { "Pique", "Spanish Soccer Team" },
            new List<string> { "Shakira", "Concert" },
            new List<string> { "Yankees", "World series" },
            new List<string> { "Derek Jeter", "Yankees" },
            new List<string> { "Mariano Rivera", "Yankees" }
         };

         ICollection<IEnumerable<string>> actual = sol.ProcessNews(newsItems);
         Assert.AreEqual(2, actual.Count);
      }
   }
}
