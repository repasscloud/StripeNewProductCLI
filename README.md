Open the `run.ps1`, add your API key to the first line, populate the products and look at what it's doing.

Creates products, then can be used to create payment links too. Pretty swish.


## Upload VPS Data (for middleware)

`$file --csv <path_to_book2.csv> https://x.com/api/LunaCIP <AppBearerToken>`


## Inject Stripe Product Data (from Stripe into API)

`$file <stripe_api_key> --ingest-stripe-products <url> <bearer_token>`


## Create Products in Stripe and Send To API (all-in-one service)

`$file <stripe_api_key> --csv <path_to_book1.csv> https://x.com/api/LunaCIP <AppBearerToken>`

## Create Links for All Stripe Products

`$file <stripe_api_key> --create-links`

