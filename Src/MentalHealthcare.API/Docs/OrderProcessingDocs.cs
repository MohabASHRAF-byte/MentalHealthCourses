namespace MentalHealthcare.API.Docs;

public static class CartEndpointDescriptions
{
    public const string AddToCartDescription = """
                                               ### Add to Cart
                                               Adds a course to the user's cart.

                                               #### Request:
                                               - **CourseId** (Required): The ID of the course to be added to the cart.

                                               #### Response:
                                               - Returns an integer representing the cart ID.

                                               #### Edge Cases:
                                               - If the user is not authorized, an error will be thrown.
                                               - If the course is already in the cart, an error will be returned.
                                               - If the course does not exist, an error will be thrown.
                                               - If the cart has reached its item limit, an error will be thrown.

                                               #### Example Usage:
                                               To add a course to the cart:
                                               ```json
                                               POST /api/cart/add
                                               {
                                                 "courseId": 123
                                               }
                                               ```
                                               """;

    public const string DeleteFromCartDescription = """
                                                    ### Delete From Cart
                                                    Removes a specific course from the user's cart.

                                                    #### Request:
                                                    - **CourseId** (Required): The ID of the course to be removed from the cart.

                                                    #### Response:
                                                    - No content is returned upon success.

                                                    #### Edge Cases:
                                                    - If the user is not authorized, an error will be thrown.
                                                    - If the course is not found in the cart, no action will be taken.

                                                    #### Example Usage:
                                                    To remove a course from the cart:
                                                    ```json
                                                    DELETE /api/cart/delete
                                                    {
                                                      "courseId": 123
                                                    }
                                                    ```
                                                    """;

    public const string ClearCartDescription = """
                                               ### Clear Cart
                                               Clears all items from the user's cart.

                                               #### Request:
                                               - No parameters are required.

                                               #### Response:
                                               - No content is returned upon success.

                                               #### Edge Cases:
                                               - If the user is not authorized, an error will be thrown.
                                               - If the cart is already empty, no action will be taken.

                                               #### Example Usage:
                                               To clear all items from the cart:
                                               ```http
                                               DELETE /api/cart/clear
                                               ```
                                               """;

    public const string GetCartItemsDescription = """
                                                  ### Get Cart Items
                                                  Retrieves the items in the current user's cart, calculates totals, discounts, and taxes. A promo code can be optionally applied to adjust the pricing.

                                                  #### Request:
                                                  - **PromoCode** (Optional): A promo code string to apply discounts to the cart items.

                                                  #### Response:
                                                  - Returns a `CartDto` object containing:
                                                    - **Courses**: A list of items in the cart with their details.
                                                    - **NumberOfItems**: Total number of items in the cart.
                                                    - **SubTotalPrice**: The total price before applying discounts and taxes.
                                                    - **DiscountValue**: The value of the discount applied.
                                                    - **DiscountPercentage**: The discount percentage.
                                                    - **TaxesValue**: The calculated tax value (rounded to the nearest 0.5).
                                                    - **TaxesPercentage**: The tax percentage applied.
                                                    - **TotalPrice**: The final calculated total after applying discounts and taxes (rounded to the nearest 0.5).
                                                    - **Messages**: A list of messages indicating the status of the promo code or any adjustments applied. If the promo code is invalid, this field will contain a descriptive message, but the endpoint will still process the request.

                                                  #### Example Usage:
                                                  To retrieve the cart with an optional promo code:
                                                  ```http
                                                  GET /api/cart?PromoCode=SAVE10
                                                  ```

                                                  If no promo code is provided:
                                                  ```http
                                                  GET /api/cart
                                                  ```
                                                  """;
}