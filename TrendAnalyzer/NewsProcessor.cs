//-----------------------------------------------------------------------
// <copyright file="NewsProcessor.cs" company="Eusebio Rufian-Zilbermann">
//   Copyright (c) Eusebio Rufian-Zilbermann
// </copyright>
//-----------------------------------------------------------------------
namespace TrendAnalyzer
{
   using System;
   using System.Collections.Generic;
   using System.Linq;

   /// <summary>
   /// A news service often needs to process news articles from multiple sources and determine trends
   /// After finding the keywords, the next step for determining "trending news" is to 
   /// de-duplicate keywords and find which ones appear to be related (e.g., if the NY Yankees
   /// just won the World Series, group news about "Yankees" and "World Series" as a single Trend)
   /// </summary>
   public class NewsProcessor
   {
      /// <summary>
      /// Union-Find data:
      /// This array associates an item index with a parent index
      /// An item can be a parent of itself. These items are called root items.
      /// If two items share a common ancestor then they are in the same component
      /// A component can be identified by any of the items belonging to them
      /// </summary>
      /// <remarks>
      /// For details about Union-Find see the book Algorithms by Sedgewick and Wayne
      /// </remarks>
      private int[] parentIndex;

      /// <summary>
      /// Union-find weighted union:
      /// Store component sizes to reduce tree depth
      /// </summary>
      private int[] componentSize;

      /// <summary>
      /// Count of components in the Union-Find data
      /// </summary>
      private int componentCount;

      /// <summary>
      /// Given a list of news items and their associated keywords, generate a list of clustered stories
      /// </summary>
      /// <param name="newsItems">
      /// A list of headlines and their associated keywords
      /// </param>
      /// <returns>
      /// A de-duplicated list of topics, grouped by Trend
      /// </returns>
      /// <remarks>
      /// The following "TODO" improvements would make this more useful for real world scenarios:
      /// * Stream processing: Instead of requiring the entire list upfront, 
      ///     allow providing smaller lists in multiple calls.
      /// * Multithreading: Once stream processing is implemented, allow multiple news providers to call concurrently.
      /// </remarks>
      public ICollection<IEnumerable<string>> ProcessNews(IEnumerable<List<string>> newsItems)
      {
         if (null == newsItems)
         {
            throw new ArgumentNullException("newsItems");
         }

         HashSet<string> newsTopicsSet = new HashSet<string>();

         foreach (IEnumerable<string> newsItem in newsItems)
         {
            if (null != newsItem)
            {
               newsTopicsSet = new HashSet<string>(newsTopicsSet.Concat(new HashSet<string>(newsItem)));
            }
         }
        
         // De-duplicated topics
         string[] keywords = newsTopicsSet.ToArray();
         
         // Build Lookup table and Union-Find structure for grouping the topics
         Dictionary<string, int> keywordIndex = new Dictionary<string, int>();
         this.componentCount = keywords.Length;
         this.parentIndex = new int[keywords.Length];
         this.componentSize = new int[keywords.Length];

         for (int i = 0; keywords.Length > i; i++)
         {
            keywordIndex.Add(keywords[i], i);
            this.parentIndex[i] = i;
            this.componentSize[i] = 1;
         }

         // Run quick-union on each topic
         foreach (List<string> keywordList in newsItems)
         {
            if (null != newsItems)
            {
               for (int k = 1; keywordList.Count > k; k++)
               {
                  this.Union(keywordIndex[keywordList[k]], keywordIndex[keywordList[0]]);
               }
            }
         }

         List<string>[] results = new List<string>[this.componentCount];
         results.Initialize();

         // Fill out the results
         Dictionary<string, int> rootKeywords = new Dictionary<string, int>();
         int rootKeywordsCount = 0;
         for (int i = 0; this.parentIndex.Length > i; i++)
         {
            int rootIndex = this.Find(i);
            string rootKeyword = keywords[rootIndex];
            if (!rootKeywords.ContainsKey(rootKeyword))
            {
               rootKeywords.Add(rootKeyword, rootKeywordsCount);
               results[rootKeywordsCount] = new List<string>();
               rootKeywordsCount++;
            }

            results[rootKeywords[rootKeyword]].Add(keywords[i]);
         }
         
         return results;
      }

      /// <summary>
      /// Perform a union of 2 components
      /// </summary>
      /// <param name="component1">First component</param>
      /// <param name="component2">Second component</param>
      /// <remarks>
      /// For details about Union-Find see the book Algorithms by Sedgewick and Wayne
      /// </remarks>
      private void Union(int component1, int component2)
      {
         int root1 = this.Find(component1);
         int root2 = this.Find(component2);
         if (root1 == root2)
         {
            return;
         }

         // Weighted union
         if (this.componentSize[root1] > this.componentSize[root2])
         {
            this.parentIndex[root2] = root1;
            this.componentSize[root1] = this.componentSize[root1] + this.componentSize[root2];
         }
         else
         {
            this.parentIndex[root1] = root2;
            this.componentSize[root2] = this.componentSize[root1] + this.componentSize[root2];
         }

         this.componentCount--;
      }

      /// <summary>
      /// Find the root of a component and compress the path
      /// </summary>
      /// <param name="component">Index of a component member.</param>
      /// <returns>The root member in the component.</returns>
      /// <remarks>
      /// For details about Union-Find see the book Algorithms by Sedgewick and Wayne
      /// Original algorithm modified to perform path compression
      /// </remarks>
      private int Find(int component)
      {
         int root;

         if (component == this.parentIndex[component] || this.parentIndex[this.parentIndex[component]] == this.parentIndex[component])
         {
            root = this.parentIndex[component];
            return root;
         }

         // Save indices for path compression
         List<int> path = new List<int>();
         while (component != this.parentIndex[component])
         {
            path.Add(component);
            component = this.parentIndex[component];
         }

         root = component;

         // Perform path compression
         foreach (int node in path)
         {
            this.parentIndex[node] = root;
         }

         return root;
      }
   }
}