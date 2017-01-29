# GroceryCo (Coding Exercise)
A console-based checkout system for customers of GroceryCo supermarkets.

####Third-Party Libraries Used:
  - NUnit 3.4.1 for unit testing (http://www.nunit.org/)
  - Json.NET for serialization (http://www.newtonsoft.com/json)
  
The above packages are all available on NuGet.

I also used a code snippet from a stackoverflow answer to generate tabular text on reciepts. See: http://stackoverflow.com/a/19353995/1672990

All projects in this application employ version 4.6.1 of the .NET framework.

###Running the App
  - The sample GroceryItem file (/data/GroceryItem.json) needs to be copied into the repository folder configured in the app.config application settings, since there's no way to add/remove Grocery Items in the application.

  - When the console gives multiple input options, the actions available can be selected by entering the letter or number surrounded in brackets and pressing enter.

###Assumptions Made
  - Sale types are not user defined. That is to say, new "types" of sales (a "combo" deal for example; buy one Apple and save on Oranges) will require new development.

  - A grocery item cannot have more than one promotion associated with it at any given time.

  - On the functioning of the "Additional product discount": only one subsequent item will recieve a discount after the requisite number of items have been added to the checkout. This discount will be entered as a percentage. In the case of "buy one get one free", the discount would be set to 100%.

  - "Basket" files (containing an unsorted list of item names) are assumed to be comma-separated .txt files. Sample files have been included in /data/baskets.

###Notable Decisions

#####Behavioral Decisions
  - When in "Cashier" mode, an open file dialog is opened where a "basket" can be selected to simulate a customer starting a checkout. Console applications shouldn't really use UI components, but remaining in the console isn't a strict requirement, so this was implemented for ease-of use.

  - Reciepts will be output as a plain text file, and stored in the repository folder. The resulting receipt.txt file gets overwritten with each checkout.

#####Design Decisions
  - Entities will be serialized and stored in files using JSON. One of the requirements of this project is that "the format describing regular prices and promotions [...] should be accessible for GroceryCo staff (product supply, marketing)". In other words, the layperson should be able to understand what they are looking at when they open one of the data files. In this developer's opinion, JSON strikes a good balance between functionality and plain-english readability.

  - A Repository Pattern was use for persistence. The reasoning behind this is the pattern's extensibility in the case new entity types are added to the system. However, the pattern isn't very unit testable (strictly speaking), but integration tests can be (and have been) written to ensure test coverage.

###Miscellanious Notes
  - Something that I realized too late is that an "OnSale" type of promotion is the same as an "AdditionalProduct" promotion with zero required items (ie.: a regular sale where the item is just given a new sale price is the same as saying "buy 0 required items, get next item for a sale price"). Had I picked up on this sooner it may have affected my implementation. 

  That said, it would involve some perhaps unecessary logic in setting the discounts for the "OnSale" promotions, because we would be going through the more complicated logic behind "AdditionalProduct" discount logic to the most simple type of sale.
