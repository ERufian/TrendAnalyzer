Trend Analyzer
==============

Here is another interesting interview question - 

Suppose you run a web site that shows news articles and you would like to determine what keywords are trending 
in recent news and how they are related, in order to present the news stories to your users.

The input is a collection where each item represents a news article and the keywords it contains.

The expected output is a collection of "trending groups". A trending group is a collection of keywords such that
A) All the keywords in a news article will always be grouped together
B) For any pair of keywords in a trending group it is possible to find a news article that contains both keywords
C) Trending groups are disjoint, if a keyword appears in a trend it will not appear in any other trend
D) Trending groups  are de-duplicated, every keyword in a trend appears exactly once

As an example, suppose the news articles in the input contain the following keywords:

Article A - "Spanish Soccer Team", "World Cup", "South Africa"
Article B - "World Cup", "Shakira", "South Africa"
Article C - "Yankees", "World Series"
Article D - "Derek Jeter", "Yankees"
Article E - "Pique", "Spanish Soccer Team"
Article F - "Shakira", "Concert"
Article G - "Mariano Rivera", "Yankees"

The result should contain 2 trending groups 

Trending news 1 - "Spanish Soccer Team", "World Cup", "South Africa", "Shakira", "Pique", "Concert"
Trending news 2 - "Yankees", "World Series", "Derek Jeter", "Mariano Rivera"

My solution uses a HashSet for de-duplication and Union-Find for the grouping

There are 2 potential improvements that I may add in the future
* Stream processing: Instead of requiring the entire list upfront, allow providing smaller lists in multiple calls.
* Multithreading: Once stream processing is implemented, allow multiple news providers to invoke it concurrently.