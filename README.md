# PromotionEngine

This program handles the calculation of order cost totals, based on the defined list of producs and promotions available.

The promotions are Interfaced for easy extensibility, you can simply add another promotions class that implements the interface and customise the logic as appropriate.

The program has been developed as a class libary for easy inclusion in other projects.

# Usage

Once the class libary has been added to a project it can be configured by 
- calling the OrderProcessor.LoadProducts method to populate the latest product data (SKUs and Unit prices)
- calling the OrderProcessor.LoadPromotions method to select applicable promotion classes.
- pass an Order object into the OrderProcessor.ProcessOrder method.

You will then receive an OrderResults object with the calculated price after promotions have been applied.

# Test coverage

The unit test project tests various combinations of the promotions to ensure that the current promotions get processed correctly.
